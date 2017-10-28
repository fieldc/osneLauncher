using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.Interop.Events;

namespace osnE.StateMachine
{
    public class VerbSearchState : State, IDisplay
    {
        private int selectedVerb = 0;
        List<Verb> verbs;
        string typed = "";

        public VerbSearchState(List<Verb> verbs,string typed)
        {
            this.verbs =  verbs;
            this.typed = typed.Trim();
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
            if( k is TabKeyPressedEvent)
            {
                if (verbs[selectedVerb].SubjectType == SubjectType.None)
                {
                    return new ExecuteState(verbs[selectedVerb]);
                }
                return new InitialSubject(verbs[selectedVerb], this.typed);
            }
            if (k is BackspaceKeyPressedEvent)
            {
                if(string.IsNullOrEmpty(this.typed) || this.typed.Length == 1)
                {
                    return new InitialVerbState(VerbManager.Instance.Verbs());
                }
                this.typed = this.typed.Substring(0, this.typed.Length - 1);
            }
            if(k is UpArrowKeyPressedEvent)
            {
                this.selectedVerb = Math.Max(0, selectedVerb - 1);
                return this;
            }
            if(k is DownArrowKeyPressedEvent)
            {
                this.selectedVerb = Math.Min(Math.Min(6, this.verbs.Count - 1), selectedVerb + 1);
                return this;
            }
            if(k is EnterKeyPressedEvent)
            {
                return new ExecuteState(this.verbs[selectedVerb]);
            }
            if(k is SpaceKeyPressedEvent)
            { 
                if(this.verbs.Count==1)
                {
                    return new InitialSubject(verbs[selectedVerb], this.typed);
                }
            }
            if (char.IsLetterOrDigit((char)KeyInterop.VirtualKeyFromKey(k.KeyPressed)))
            {
                this.typed = this.typed + k.KeyPressed.ToString().ToLower();
            }
            
            this.selectedVerb = 0;
            this.verbs = VerbManager.Instance.Search(this.typed, 6);
            if (this.verbs == null || this.verbs.Count == 0)
            {
                return new InvalidVerbState(this.typed);
            }
            else if(this.verbs.Count == 1)
            {
                if (verbs[selectedVerb].SubjectType == SubjectType.None)
                {
                    return new ExecuteState(verbs[selectedVerb]);
                }
                //return new InitialSubject(verbs[selectedVerb], this.typed);
            }
            
            
            return this;
        }

        string IDisplay.GetTitle()
        {
            return "Continue typing to open an application or document";
        }

        string IDisplay.GetVerbText()
        {
            return this.verbs[selectedVerb].Name + " " + this.verbs[this.selectedVerb].Help;
        }

        List<string> IDisplay.GetSuggestions()
        {
            return (from v in this.verbs select v.Name).ToList<string>();
        }
    }
}
