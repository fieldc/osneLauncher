using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

            return new VerbSearchState(VerbManager.Instance.Search(k.KeyString,6),k.KeyString);
        }
        public string GetSelectedText()
        {
            return "";
        }
        public string GetTitle()
        {
            return "Welcome to osnE! Enter a command or type \"help\" for assistance";
        }
        public List<string> GetSuggestions()
        {
            return new List<string>();
        }
        public string GetVerbText()
        {
            return "";
        }
    }
}
