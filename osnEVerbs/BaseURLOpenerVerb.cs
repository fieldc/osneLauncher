using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using osnE.Interop;

namespace osnE.Verbs
{
    public abstract class BaseURLOpenerVerb : Verb
    {
        protected string format;

        public BaseURLOpenerVerb(string name,string description,string subjectHelp,string urlFormat):
            base(name, description, SubjectType.Arbitrary, subjectHelp, PredicateType.None)
        {
            this.format = urlFormat;
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
            this.subject = this.subject.Trim();
            string url = String.Format(this.format, this.subject);
            ProcessStartInfo p = new ProcessStartInfo();
            p.UseShellExecute = true;
            p.FileName = Uri.EscapeUriString(url);
            Process.Start(p);
        }
    }
}
