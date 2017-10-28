using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.Interop.Events;
using osnE.StateMachine;
namespace osnE.StateMachine
{
    public class SubjectSearchState: State, IDisplay
    {
        private Verb me;
        private string verbSearchString;
        private string subjectTyped;
        private int selectedSuggestion = 0;
        private List<string> suggestions;

        public SubjectSearchState(Verb me, string verbSearchString, string typedSoFar)
        {
            this.me = me;
            this.verbSearchString = verbSearchString;
            this.subjectTyped = typedSoFar;
            this.suggestions = this.me.GetSubjectsOptions(this.subjectTyped);
            selectedSuggestion = 0;
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
            if(k is BackspaceKeyPressedEvent)
            {
                if(string.IsNullOrEmpty(this.subjectTyped) || this.subjectTyped.Length==1)
                {
                    return new InitialSubject(this.me, this.verbSearchString);
                }
                this.selectedSuggestion = 0;
                this.subjectTyped = this.subjectTyped.Substring(0, this.subjectTyped.Length - 1);
                this.suggestions = me.GetSubjectsOptions(this.subjectTyped);
                return this;
            }
            if (k is UpArrowKeyPressedEvent)
            {
                this.selectedSuggestion = Math.Max(0, selectedSuggestion - 1);
                return this;
            }
            if (k is DownArrowKeyPressedEvent)
            {
                this.selectedSuggestion = Math.Min(Math.Min(6,this.suggestions.Count-1), selectedSuggestion + 1);
                return this;
            }
            if(k is EnterKeyPressedEvent || k is TabKeyPressedEvent)
            {
                if (this.me.SubjectType == SubjectType.Arbitrary)
                {
                    me.Subject = this.subjectTyped;
                    return new ExecuteState(me);
                }

                me.Subject = this.suggestions[this.selectedSuggestion];
                if(me.PredicateType == PredicateType.None)
                {
                    return new ExecuteState(me);
                }
                return new InitialPredicateState(this.me, this.verbSearchString, this.suggestions[this.selectedSuggestion]);
            }
            if (k is SpaceKeyPressedEvent)
            {
                this.subjectTyped = this.subjectTyped + " ";
            }
            else
            {
                this.subjectTyped = this.subjectTyped + k.KeyString;
            }
            this.suggestions = this.me.GetSubjectsOptions(this.subjectTyped);
            return this;
        }
        
        public string GetTitle()
        {
            return this.me.TitleHelp;
        }

        public string GetVerbText()
        {
            if(this.subjectTyped == "")
                return this.me.Name + " " + this.me.Help;
            return this.me.Name + " " + this.subjectTyped;
        }

        public List<string> GetSuggestions()
        {
            return this.suggestions;
        }
    }
}
