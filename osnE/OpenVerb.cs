using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using System.Diagnostics;
using System.IO;
using Common.Logging;


namespace osnE
{
    class OpenerItem : IComparable<OpenerItem>
    {
        public static readonly ILog logger = LogManager.GetCurrentClassLogger();
        public DateTime LastUsed { get; set; }
        public string FileName { get; set; }
        public string ExePath { get; set; }
        
        public OpenerItem(DateTime used,string fileName, string exePath)
        {
            this.LastUsed = used;
            this.FileName = fileName;
            this.ExePath = exePath;
            logger.DebugFormat("Creating Opener Item fileName={0} lastUsed={1} exePath={2}", this.FileName, this.LastUsed.ToFileTime(), this.ExePath);
        }


        int IComparable<OpenerItem>.CompareTo(OpenerItem other)
        {
            if (this.LastUsed > other.LastUsed)
                return -1;
            else if (this.LastUsed < other.LastUsed)
                return 1;

            return this.FileName.CompareTo(other.FileName);
        }
        public bool StartsWith(string s)
        {
            return this.FileName.StartsWith(s,StringComparison.OrdinalIgnoreCase);
        }
        public bool Contains(string s)
        {
            return this.FileName.Contains(s);
        }
    }
    class OpenVerb : Verb
    {

        DateTime subjectRefreshed;
        private List<OpenerItem> subjects = new List<OpenerItem>();
        FileSystemWatcher commands;
        FileSystemWatcher programs;

        public OpenVerb() :
            base("open","","open a shortcut","opens a shortcut",SubjectType.Bounded,PredicateType.None,null,null)
        {
            this.RefreshSubjects();
            this.commands = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + "Enso's Learn As Open Commands/");
            this.commands.Created += Item_Created;
            this.commands.Deleted += Item_Deleted;

            this.programs = new FileSystemWatcher(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs", "*.lnk");

            this.programs.Created += Item_Created;
            this.programs.Deleted += Item_Deleted;
        }

        void Item_Deleted(object sender, FileSystemEventArgs e)
        {
            this.subjects.RemoveAll(toOpen => toOpen.ExePath.ToLower() == e.FullPath.ToLower());
        }

        void Item_Created(object sender, FileSystemEventArgs e)
        {
            FileInfo fi = new FileInfo(e.FullPath);
            string i = Path.GetFileName(e.FullPath).ToLower();
            if (i.EndsWith(".lnk") || i.EndsWith(".url"))
                i = Path.GetFileNameWithoutExtension(i);

            this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, e.FullPath));
        }

        private void RefreshSubjects()
        {
            try
            {
                foreach (string f in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + "Enso's Learn As Open Commands/"))
                {
                    FileInfo fi = new FileInfo(f);
                    string i = Path.GetFileName(f).ToLower();
                    if (i.EndsWith(".lnk") || i.EndsWith(".url"))
                        i = Path.GetFileNameWithoutExtension(i);

                    this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, f));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("caught exception ex=" + ex.Message);
            }

            try
            {
                Stopwatch s = new Stopwatch();
                s.Start();
                foreach (string f in Directory.GetFiles(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs", "*.lnk", SearchOption.AllDirectories))
                {
                    FileInfo fi = new FileInfo(f);
                    string i = Path.GetFileName(f).ToLower();
                    if (i.EndsWith(".lnk") || i.EndsWith(".url"))
                        i = Path.GetFileNameWithoutExtension(i);

                    this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, f));
                }
                s.Stop();
                Console.WriteLine("Got Programs Time=" + s.ElapsedMilliseconds.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Caught exception enumerating programs ex=" + ex.Message);
            }
            this.subjectRefreshed = DateTime.Now;
        }
        public override void Execute()
        {
            OpenerItem o = (from toOpen in this.subjects where toOpen.FileName == this.subject.Trim() select toOpen).FirstOrDefault();
            if (o != null)
            {
                ProcessStartInfo p = new ProcessStartInfo();
                p.UseShellExecute = true;
                p.FileName = o.ExePath;
                Process.Start(p);
            }
        }

        protected override List<string> GetSubjectSuggesitons()
        {
            throw new NotImplementedException();
        }
        protected override List<string> GetPredicateOptions()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSubjectsOptions()
        {
            List<string> t = (from s in this.subjects where s.Contains(this.subject.ToLower()) orderby s select s.FileName).Take(6).ToList<string>();
            return t;
        }
    }
}
