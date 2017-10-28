using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.StateMachine;
using osnE.Interop.Events;

namespace osnE.StateMachine
{
    class InitialSubject : State,IDisplay
    {
        Verb me;
        string verbSearchString;
        public InitialSubject(Verb selectedVerb, string verbSearchString)
        {
            this.me = selectedVerb;
            this.verbSearchString = verbSearchString;
        }

        public override State Process(KeyPressedEvent k)
        {
            if( k is  BackspaceKeyPressedEvent)
            {
                return new VerbSearchState(VerbManager.Instance.Search(this.verbSearchString, 6), this.verbSearchString);
            }
           
            if(k is SpaceKeyPressedEvent || k is TabKeyPressedEvent || k is UpArrowKeyPressedEvent || k is DownArrowKeyPressedEvent)
            {
                return this;
            }
            if (k is EnterKeyPressedEvent)
            {
                if (this.me.SubjectType == SubjectType.None)
                    return new ExecuteState(this.me);

                return this;
            }
            return new SubjectSearchState(this.me, this.verbSearchString, k.KeyString);
        }

        string IDisplay.GetTitle()
        {
            return me.Help;
        }

        string IDisplay.GetVerbText()
        {
            return me.Name;
        }

        List<string> IDisplay.GetSuggestions()
        {
            if (this.me.SubjectType == SubjectType.Arbitrary || this.me.SubjectType == SubjectType.ArbitraryWithSuggestions)
                return new List<string>();

            return me.GetSubjectsOptions("");
        }
    }
}
