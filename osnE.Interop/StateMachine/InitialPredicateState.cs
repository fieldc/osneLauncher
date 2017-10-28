using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.StateMachine;
using osnE.Interop;
using osnE.Interop.Events;

namespace osnE.StateMachine
{
    class InitialPredicateState : State, IDisplay
    {
        private Verb me;
        private string verbSearch;
        private string subjectSearch;

        public InitialPredicateState(Verb verb,string verbSearch,string subjectSearch)
        {
            this.me = verb;
            this.verbSearch = verbSearch;
            this.subjectSearch = subjectSearch;
        }
        public override State Process(KeyPressedEvent k)
        {
            throw new NotImplementedException();
        }

        string IDisplay.GetTitle()
        {
            throw new NotImplementedException();
        }

        string IDisplay.GetVerbText()
        {
            throw new NotImplementedException();
        }

        List<string> IDisplay.GetSuggestions()
        {
            throw new NotImplementedException();
        }
    }
}
