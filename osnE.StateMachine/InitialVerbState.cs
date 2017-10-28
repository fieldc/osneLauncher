using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using osnE.Interop;
using osnE.Interop.Events;
using osnE.StateMachine;

namespace osnE.StateMachine
{
    class InitialVerbState : State,IDisplay
    {
        List<Verb> availiableVerbs;

        public InitialVerbState(List<Verb> verbs)
        {
            this.availiableVerbs = verbs;
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
            if(!(k is AlphaNumericKeyPressedEvent))
            {
                return this;
            }
            List<Verb> initialSet = VerbManager.Instance.Search(k.KeyString,6);
            if(initialSet.Count == 0)
                return this;

            return new VerbSearchState(initialSet, k.KeyString);
        }
        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            return new Dictionary<int, List<Inline>>();
        }

        string IDisplay.GetTitle()
        {
            return "Welcome to osnE! Enter a command or type \"help\" for assistance";
        }

        List<Inline> IDisplay.GetVerbText()
        {
            return new List<Inline>();
        }

        int IDisplay.GetSelectedItem()
        {
            return 0;
        }
    }
}
