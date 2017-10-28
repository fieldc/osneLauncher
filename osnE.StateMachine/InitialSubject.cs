using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
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
        int selectedSuggestion ;
        List<string> suggestions;

        public InitialSubject(Verb selectedVerb, string verbSearchString)
        {
            this.me = selectedVerb;
            this.verbSearchString = verbSearchString;
            this.selectedSuggestion = 0;
            this.suggestions = this.me.GetSubjectsOptions("");
        }

        public override State Process(KeyPressedEvent k)
        {
            if( k is  BackspaceKeyPressedEvent)
            {
                return new VerbSearchState(VerbManager.Instance.Search(this.verbSearchString, 6), this.verbSearchString);
            }
           
            if(k is SpaceKeyPressedEvent )
            {
                return this;
            }
            if (k is UpArrowKeyPressedEvent)
            {
                this.selectedSuggestion = Math.Max(0, selectedSuggestion - 1);
                Console.WriteLine("selectedSuggestion=" + selectedSuggestion.ToString());
                return this;
            }
            if (k is DownArrowKeyPressedEvent)
            {
                this.selectedSuggestion = Math.Min(Math.Min(6, this.suggestions.Count - 1), selectedSuggestion + 1);
                Console.WriteLine("selectedSuggestion=" + selectedSuggestion.ToString());
                return this;
            }
            if (k is TabKeyPressedEvent)
            {
                if (this.suggestions.Count > 0)
                {
                    string subjectTyped = "";
                    if(this.selectedSuggestion<this.suggestions.Count)
                        subjectTyped=this.suggestions[this.selectedSuggestion];

                    if(!string.IsNullOrEmpty(subjectTyped))
                        return new SubjectSearchState(this.me, this.verbSearchString, subjectTyped);
                }
                return this;
            }
            if (k is EnterKeyPressedEvent)
            {
                if (this.me.SubjectType == SubjectType.None)
                    return new ExecuteState(this.me);

                return this;
            }
            if (k.KeyString == null)
                return this;

            return new SubjectSearchState(this.me, this.verbSearchString, k.KeyString);
        }

        string IDisplay.GetTitle()
        {
            return me.Help;
        }

        List<Inline> IDisplay.GetVerbText()
        {

            return new List<Inline>()
                {
                    new Run(this.me.Name) {FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White)}
                };
        }

        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            Dictionary<int, List<Inline>> t = new Dictionary<int, List<Inline>>();
            int counter = 0;
            foreach (string subjectName in this.suggestions)
            {
                List<Inline> r ;
                if (this.selectedSuggestion > 0 && this.selectedSuggestion == counter)
                {
                    r = new List<Inline>() {
                        new Run(subjectName) { FontWeight = FontWeights.Bold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))} 
                    };
                }
                else
                {
                    r = new List<Inline>() {
                        new Run(subjectName) { FontWeight = FontWeights.Normal, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))} 
                    };
                }
                t.Add(counter, r);
                counter = counter + 1;
            }
            return t;
        }


        int IDisplay.GetSelectedItem()
        {
            return this.selectedSuggestion;
        }
    }
}
