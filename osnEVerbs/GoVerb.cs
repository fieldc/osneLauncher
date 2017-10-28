using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using osnE.Interop;
using System.Diagnostics;
using osnE.WindowsHooks;
using Common.Logging;

namespace osnE.Verbs
{
    public class GoVerb: Verb
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();
        private Thread processWatcher;
        private List<WindowInfo> subjects;
        private bool running = true;

        public GoVerb() :
            base("go", "switch to a program", SubjectType.Bounded, "window name", PredicateType.None)
        {

            ThreadStart t = new ThreadStart(this.RefreshSubjects);
            this.processWatcher = new Thread(t);
            this.processWatcher.Start();

        }

        private void RefreshSubjects()
        {
            while(running)
            {
                this.GetProcesses();
                Thread.Sleep(10000);
            }
        }

        private void GetProcesses()
        {
            List<WindowInfo> g = new List<WindowInfo>();
            g.AddRange(WindowEnumerator.EnumerateWindows());
            this.subjects = g;
            foreach(WindowInfo w in g)
            {
                logger.DebugFormat("Found Window name={0}", w.WindowName);
            }
        }

        public override void Execute()
        {
            IntPtr i = this.subjects.Where(window => window.WindowName == this.subject).Select(window => window.hwnd).FirstOrDefault();
            if (i != null)
            {
                WindowEnumerator.ForceWindowIntoForeground(i);
            }
        }
        public override List<string> GetPredicateOptions(string search)
        {
            throw new NotImplementedException();
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            int count = 6;
            List<string> s = new List<string>();

            s = (from window in this.subjects where window.StartsWith(search) orderby window.WindowName select window.WindowName).Take(count).ToList<string>();
            if (s != null && s.Count>0)
                count = 6 - s.Count;

            if (count > 0)
                s.AddRange((from window in this.subjects where window.Contains(search) orderby window.WindowName select window.WindowName).Take(count).ToList<string>());
            return s;
        }
       
    }
}
