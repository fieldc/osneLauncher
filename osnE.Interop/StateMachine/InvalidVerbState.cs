using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop.Events;
using osnE.Interop;

namespace osnE.StateMachine
{
    public class InvalidVerbState : State, IDisplay
    {
        private string typed;

        public InvalidVerbState(string typed)
        {
            this.typed = typed;
        }

        public string GetTitle()
        {
            return "There is no matching command. Use backspace to delete characters";
        }

        public string GetVerbText()
        {
            return this.typed;
        }

        public List<string> GetSuggestions()
        {
            return new List<string>();
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
           if (k is BackspaceKeyPressedEvent)
            {
                if(this.typed.Length == 1)
                {
                    return new InitialVerbState(VerbManager.Instance.Verbs());
                }
                this.typed = this.typed.Substring(0, this.typed.Length - 1);
                List<Verb> verbs = VerbManager.Instance.Search(this.typed, 6);
                if( verbs.Count>0)
                {
                    return new VerbSearchState(verbs, this.typed);
                }
            }
            return this;
        }
    }
}
