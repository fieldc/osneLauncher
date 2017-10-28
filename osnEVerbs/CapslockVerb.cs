using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.StateMachine;

namespace osnE.Verbs
{
    public class CapslockVerb : Verb
    {
        public CapslockVerb(): base("capslock","Toggle capslock key",SubjectType.Bounded,"Turn on or off",PredicateType.None)
        {

        }

        public override List<string> GetPredicateOptions(string search)
        {
            throw new NotImplementedException();
        }
        public override void Execute()
        {
            if (this.subject == "on")
            {
                StateManager.Instance.KbdManager.CapslockOn();
            }
            else
            {
                StateManager.Instance.KbdManager.CapslockOff();
            }
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            return new List<string>() { "on", "off" }.Where(a => a.StartsWith(search)).ToList<string>();
        }
    }
}
