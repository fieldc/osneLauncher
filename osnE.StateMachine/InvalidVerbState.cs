using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using osnE.Interop.Events;
using osnE.Interop;

namespace osnE.StateMachine
{
    public class InvalidVerbState : State, IDisplay
    {
        private string typed = "";
        public InvalidVerbState(string typed)
        {
            this.typed = typed;
        }

        string IDisplay.GetTitle()
        {
            return "There is no matching command. Use backspace to delete characters";
        }

        List<Inline> IDisplay.GetVerbText()
        {
            return new List<Inline>()
                {
                    new Run(this.typed) {FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White)}, 
                };
        }

        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            return new Dictionary<int, List<Inline>>();
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
           if (k is BackspaceKeyPressedEvent)
            {
                if (this.typed.Length == 1)
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


        int IDisplay.GetSelectedItem()
        {
            return 0;
        }
    }
}
