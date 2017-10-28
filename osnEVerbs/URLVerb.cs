using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using osnE.Interop;

namespace osnE.Verbs
{
    public class URLVerb : Verb
    {
        public URLVerb ():
            base("url", "opens a url", SubjectType.ArbitraryWithSuggestions, "url", PredicateType.None, "chose a browser", "|", new List<string>() { "firefox", "chrome", "ie" })
        {
            ;
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            List<string> t = new List<string>() { "www.cnn.com", "www.google.com", "www.reddit.com" };
            return (from s in t where s.Contains(search.ToLower()) select s).Take(6).ToList<string>();
        }
        public override List<string> GetPredicateOptions(string search)
        {
            return (from v in this.predicateList where v.StartsWith(search.ToLower()) select v).Take(6).ToList<string>();
        }
        public override void Execute()
        {
            Console.WriteLine("executing");
            ProcessStartInfo p = new ProcessStartInfo();
            p.UseShellExecute = true;

            if (!this.subject.Contains("."))
                this.subject = "www." + this.subject.Trim() + ".com";
            
            //this.subject = "http://" + this.subject.Trim();
            this.subject = this.subject.Trim();
            p.FileName = Uri.EscapeUriString(this.subject);
            Process.Start(p);
        }
    }
}
