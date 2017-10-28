using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
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
        private string verbSearchString="";
        private string subjectTyped="";
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
                Console.WriteLine("selectedSuggestion=" + selectedSuggestion.ToString());
                return this;
            }
            if (k is DownArrowKeyPressedEvent)
            {
                this.selectedSuggestion = Math.Min(Math.Min(6,this.suggestions.Count-1), selectedSuggestion + 1);
                Console.WriteLine("selectedSuggestion=" + selectedSuggestion.ToString());
                return this;
            }
            if (k is TabKeyPressedEvent)
            {
                this.subjectTyped = this.suggestions[this.selectedSuggestion];
                this.selectedSuggestion = 0;
                this.suggestions = this.me.GetSubjectsOptions(this.subjectTyped);
                return this;
            }
            if(k is EnterKeyPressedEvent )
            {
                if (this.me.SubjectType == SubjectType.Arbitrary)
                {
                    if (this.subjectTyped.Length > 0)
                    {
                        me.Subject = this.subjectTyped;
                        return new ExecuteState(me);
                    }
                    return this;
                }
                if (this.suggestions.Count == 0)
                {
                    //should never happen
                    return new InvalidSubjectState(this.me, this.verbSearchString, this.subjectTyped);
                }
                if(this.selectedSuggestion>=this.suggestions.Count)
                {
                    this.selectedSuggestion = 0;
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

            if(this.suggestions.Count==0 && (this.me.SubjectType == SubjectType.ArbitraryWithSuggestions||this.me.SubjectType == SubjectType.Bounded) )
            {
                return new InvalidSubjectState(this.me, this.verbSearchString, this.subjectTyped);
            }
            return this;
        }

        string IDisplay.GetTitle()
        {
            return this.me.TitleHelp;
        }

        List<Inline> IDisplay.GetVerbText()
        {
            if (this.subjectTyped == "")
            {
                return new List<Inline>()
                {
                    new Run(this.me.Name+" ") {FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White)},
                    new Run(this.me.Help) {FontWeight = FontWeights.DemiBold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF999999"))}
                };
            }
            return  new List<Inline>()
                {
                    new Run(this.me.Name+" "+ this.subjectTyped) {FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White)}, 
                };
        }

        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            Dictionary<int, List<Inline>> t = new Dictionary<int, List<Inline>>();
            int counter = 0;
            foreach (string verbName in this.suggestions)
            {
                int searchStart = verbName.IndexOf(this.subjectTyped,StringComparison.OrdinalIgnoreCase);
                string front = "";
                string highlight = "";
                string back = "";

                if (searchStart > 0)
                {
                    front = verbName.Substring(0, searchStart);
                    highlight = verbName.Substring(searchStart, this.subjectTyped.Length);

                    if (searchStart + this.subjectTyped.Length < verbName.Length)
                        back = verbName.Substring(searchStart + this.subjectTyped.Length);
                }
                else
                {
                    front = verbName;
                }
                List<Inline> r ;
                if (this.selectedSuggestion > 0 && this.selectedSuggestion == counter)
                {
                    r = new List<Inline>() {
                        new Run(front) { FontWeight = FontWeights.DemiBold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))} ,
                        new Run(highlight) { FontWeight = FontWeights.Bold,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))},
                        new Run(back) { FontWeight = FontWeights.DemiBold,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))}};
                }
                else
                {
                    r = new List<Inline>() {
                        new Run(front) { FontWeight = FontWeights.Normal, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))} ,
                        new Run(highlight) { FontWeight = FontWeights.DemiBold,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))},
                        new Run(back) { FontWeight = FontWeights.Normal,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))}};
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
