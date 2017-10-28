using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.StateMachine;

namespace osnE.Verbs
{
    public class QuitVerb : Verb
    {
        public QuitVerb() :
            base("quit", "Quit osnE", SubjectType.None, "", PredicateType.None)
        {

        }

        public override void Execute()
        {
            throw new ShutdownException();            
        }

        public override List<string> GetPredicateOptions(string search)
        {
            throw new NotImplementedException();
        }
        public override List<string> GetSubjectsOptions(string search)
        {
            throw new NotImplementedException();
        }
    }
}
