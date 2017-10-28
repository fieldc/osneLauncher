using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using System.Diagnostics;
using System.IO;
using Common.Logging;
using System.Configuration;

namespace osnE.Verbs
{
    public class UnlearnVerb : Verb, IDisposable
    {

        DateTime subjectRefreshed;
        private List<OpenerItem> subjects = new List<OpenerItem>();
        FileSystemWatcher commands;

        public UnlearnVerb() :
            base("unlearn", "Continue typing to unlearn an application or document", SubjectType.Bounded, "target", PredicateType.None, null, null)
        {
            this.titleHelp = "Continue typing to open an application or document";
            this.RefreshSubjects();

            this.commands = new FileSystemWatcher(ConfigurationManager.AppSettings["CommandDirectory"]);
            this.commands.Created += Item_Created;
            this.commands.Deleted += Item_Deleted;
            this.commands.EnableRaisingEvents = true;
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

            OpenerItem o = new OpenerItem(fi.LastAccessTime, i, e.FullPath);
            if( ! this.subjects.Contains(o))
                this.subjects.Add(o);
        }

        private void RefreshSubjects()
        {
   
            try
            {
                foreach (string f in Directory.GetFiles(ConfigurationManager.AppSettings["CommandDirectory"]))
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

            this.subjectRefreshed = DateTime.Now;
        }
        public override void Execute()
        {
            OpenerItem o = (from toOpen in this.subjects where toOpen.FileName == this.subject.Trim() select toOpen).FirstOrDefault();
            if (o != null)
            {
                try
                {
                    File.Delete(o.ExePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in unlearn ex=" + ex.Message);
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

            t = (from s in this.subjects where s.StartsWith(search.ToLower()) orderby s select s.FileName).Take(count).ToList<string>();
            if(t!=null && t.Count>0)
                count=6-t.Count;

            if(count>0)
                t.AddRange((from s in this.subjects where s.Contains(search.ToLower()) orderby s select s.FileName).Take(count).ToList<string>());

            t = t.Distinct().ToList<string>();
            return t;
        }

        void IDisposable.Dispose()
        {
            this.commands.EnableRaisingEvents = false;
            this.commands = null;
        }
    }
   
}
