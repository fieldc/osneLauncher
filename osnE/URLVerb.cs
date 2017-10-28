using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using osnE.Interop;

namespace osnE
{


    public class URLVerb : Verb
    {
        //new Verb("url","","suggestionList a url","suggestionList a url",SubjectType.Arbitrary,PredicateType.List,"|",new List<string>(){"firefox","chrome","ie"})
        public URLVerb ():
            base("url","","suggestionList a url","suggestionList a url",SubjectType.ArbitraryWithSuggestions,PredicateType.None,"|",new List<string>(){"firefox","chrome","ie"})
        {
            ;
        }
        protected override List<string> GetSubjectsOptions()
        {
            throw new NotImplementedException();
        }
        protected override List<string> GetSubjectSuggesitons()
        {
            List<string> t = new List<string>() { "www.cnn.com", "www.google.com", "www.reddit.com" };
            return (from s in t where s.Contains(this.subject.ToLower()) select s).Take(6).ToList<string>();
        }
        protected override List<string> GetPredicateOptions()
        {
            return (from v in this.predicateList where v.StartsWith(this.predicate.ToLower()) select v).Take(6).ToList<string>();
        }
        public override void Execute()
        {

            Console.WriteLine("executing");
            ProcessStartInfo p = new ProcessStartInfo();
            p.UseShellExecute = true;

            if (!this.subject.Contains("."))
                this.subject = "www." + this.subject.Trim() + ".com";
            this.subject = "http://" + this.subject.Trim();

            p.FileName = Uri.EscapeUriString(this.subject);
            Process.Start(p);
        }
    }
}
