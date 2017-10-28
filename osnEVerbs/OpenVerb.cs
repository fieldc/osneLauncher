using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using System.Threading;
using System.Diagnostics;
using System.IO;
using osnE.WindowsHooks;
using Common.Logging;


namespace osnE.Verbs
{
   public class OpenVerb : Verb, IDisposable
    {
        static readonly ILog logger = LogManager.GetCurrentClassLogger();
        DateTime subjectRefreshed;
        private List<OpenerItem> subjects = new List<OpenerItem>();
        FileSystemWatcher commands;
        FileSystemWatcher programs;

        public OpenVerb() :
            base("open", "Continue typing to open an application or document", SubjectType.Bounded, "target", PredicateType.None, null, null)
        {
            this.titleHelp = "Continue typing to open an application or document";
            this.RefreshSubjects();

            this.commands = new FileSystemWatcher(ConfigurationManager.AppSettings["CommandDirectory"]);
            
            
            this.commands.Created += Item_Created;
            this.commands.Deleted += Item_Deleted;
            this.commands.EnableRaisingEvents = true;

            this.programs = new FileSystemWatcher(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs", "*.lnk");
            this.programs.Created += Item_Created;
            this.programs.Deleted += Item_Deleted;
            this.programs.EnableRaisingEvents = true;
        }

        void Item_Deleted(object sender, FileSystemEventArgs e)
        {
            lock (this.subjects)
            {
                this.subjects.RemoveAll(toOpen => toOpen.ExePath.ToLower() == e.FullPath.ToLower());
            }
        }

        void Item_Created(object sender, FileSystemEventArgs e)
        {
            FileInfo fi = new FileInfo(e.FullPath);
            string i = Path.GetFileName(e.FullPath).ToLower();
            if (i.EndsWith(".lnk") || i.EndsWith(".url"))
                i = Path.GetFileNameWithoutExtension(i);

            OpenerItem o = new OpenerItem(fi.LastAccessTime, i, e.FullPath);
            lock (this.subjects)
            {
                if (!this.subjects.Contains(o))
                    this.subjects.Add(o);
            }
        }

        private void RefreshSubjects()
        {
            try
            {
                string chromePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Google/Chrome/Application/chrome.exe";
                if (File.Exists(chromePath))
                {
                    FileInfo fi = new FileInfo(chromePath);
                    string i = Path.GetFileName(chromePath).ToLower();
                    lock (this.subjects)
                    {
                        this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, "chrome"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("caught exception ex=" + ex.Message);
            }

            try
            {
                //if (!ConfigurationManager.AppSettings.AllKeys.Contains("CommandDirectory"))

                string path = ConfigurationManager.AppSettings["CommandDirectory"];
                
                foreach (string f in Directory.GetFiles(path))
                {
                    FileInfo fi = new FileInfo(f);
                    string i = Path.GetFileName(f).ToLower();
                    if (i.EndsWith(".lnk") || i.EndsWith(".url"))
                        i = Path.GetFileNameWithoutExtension(i);
                    lock (this.subjects)
                    {
                        this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, f));
                    }
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

                    lock (this.subjects)
                    {
                        this.subjects.Add(new OpenerItem(fi.LastAccessTime, i, f));
                    }
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
            OpenerItem o=null;
            lock (this.subjects)
            {
                o = (from toOpen in this.subjects where toOpen.FileName == this.subject.Trim() select toOpen).FirstOrDefault();
            }
            if (o != null && File.Exists(o.ExePath))
            {
                o.LastUsed = DateTime.Now;
                try
                {
                    File.SetLastAccessTime(o.ExePath, DateTime.Now);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception updating last access time ex=" + ex.Message);
                }
                ProcessStartInfo p = new ProcessStartInfo();
                p.UseShellExecute = true;
                p.FileName = o.ExePath;
                Process.Start(p);
                Thread.Sleep(500);
                if(o.ExePath.EndsWith(".lnk"))
                {
                    ShellShortcut s = new ShellShortcut(o.ExePath);
                    if (s.Path.EndsWith(".xls") || s.Path.EndsWith(".xlsx") || s.Path.EndsWith(".csv"))
                    {
                        FileInfo fi = new FileInfo(s.Path);
                        //if its an excel file need to force the window forward
                        List<WindowInfo> g = new List<WindowInfo>();
                        g.AddRange(WindowEnumerator.EnumerateWindows());
                        IntPtr i = g.Where(window => window.WindowName == fi.Name).Select(window => window.hwnd).FirstOrDefault();
                        if (i != null)
                        {
                            WindowEnumerator.ForceWindowIntoForeground(i);
                        }
                    }
                }
            }
        }
    
        public override List<string> GetPredicateOptions(string search)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetSubjectsOptions(string search)
        {
            int count=6;
            List<string> t = new List<string>();
            lock (this.subjects)
            {
                t = (from s in this.subjects where s.StartsWith(search.ToLower()) orderby s select s.FileName).Take(count).ToList<string>();
                if (t != null && t.Count > 0)
                    count = 6 - t.Count;

                if (count > 0)
                    t.AddRange((from s in this.subjects where s.Contains(search.ToLower()) orderby s select s.FileName).Take(count).ToList<string>());

            }
            t = t.Distinct().ToList<string>();
            return t;
        }

        void IDisposable.Dispose()
        {
            this.commands.EnableRaisingEvents = false;
            this.programs.EnableRaisingEvents = false;
            this.commands = null;
            this.programs = null;
        }
    }
   public class OpenerItem : IComparable<OpenerItem>
   {
       public static readonly ILog logger = LogManager.GetCurrentClassLogger();
       public DateTime LastUsed { get; set; }
       public string FileName { get; set; }
       public string ExePath { get; set; }

       public OpenerItem(DateTime used, string fileName, string exePath)
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
           return this.FileName.StartsWith(s, StringComparison.OrdinalIgnoreCase);
       }
       public bool Contains(string s)
       {
           return this.FileName.Contains(s);
       }
   }
}
