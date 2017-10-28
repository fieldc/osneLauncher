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
    class PredicateSearchState : State, IDisplay
    {
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
