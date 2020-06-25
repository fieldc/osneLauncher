using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using osnE.Interop;

namespace osnE.Verbs
{
    public abstract class BaseUrlOpenerVerb : Verb
    {
        private readonly string _format;

        protected BaseUrlOpenerVerb(string name,string description,string subjectHelp,string urlFormat):
            base(name, description, SubjectType.Arbitrary, subjectHelp, PredicateType.None)
        {
            this._format = urlFormat;
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            return new List<string>();
        }
        public override List<string> GetPredicateOptions(string search)
        {
            return new List<string>();
        }
        public override void Execute()
        {
            if(String.IsNullOrEmpty(this.subject)){
                return;
            }
            this.subject = this.subject.Trim();
            string url = String.Format(this._format, this.subject);

            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) +"/Google/Chrome/Application/chrome.exe";
            p.Arguments = String.Format("--new-window \"{0}\"",Uri.EscapeUriString(url));
            Process.Start(p);
        }
    }
}
