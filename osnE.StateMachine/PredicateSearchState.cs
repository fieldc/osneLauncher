using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
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

        List<Inline> IDisplay.GetVerbText()
        {
            throw new NotImplementedException();
        }

        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            throw new NotImplementedException();
        }


        int IDisplay.GetSelectedItem()
        {
            throw new NotImplementedException();
        }
    }
}
