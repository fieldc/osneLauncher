using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Threading.Tasks;
using osnE.Interop.Events;
using osnE.Interop;


namespace osnE.StateMachine
{
    public class InvalidSubjectState : State, IDisplay
    {
        private Verb me;
        private string verbSearchString;
        private string subjectTyped;
        
        public InvalidSubjectState(Verb selectedVerb, string verbSearchString,string subjectSearchString)
        {
            this.me = selectedVerb;
            this.verbSearchString = verbSearchString;
            this.subjectTyped = subjectSearchString;
        }

        public override State Process(KeyPressedEvent k)
        {
            if (k is BackspaceKeyPressedEvent)
            {
                if (string.IsNullOrEmpty(this.subjectTyped) || this.subjectTyped.Length == 1)
                {
                    return new InitialSubject(this.me, this.verbSearchString);
                }

                this.subjectTyped =this.subjectTyped.Substring(0, this.subjectTyped.Length - 1);
                //this should be un-necessary
                if(this.me.GetSubjectsOptions(this.subjectTyped).Count>0)
                    return new SubjectSearchState(this.me, this.verbSearchString, this.subjectTyped);

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
            return new List<Inline>()
                {
                    new Run(this.me.Name+" "+ this.subjectTyped) {FontWeight = FontWeights.Bold, Foreground = new SolidColorBrush(Colors.White)}, 
                };
        }

        Dictionary<int, List<Inline>> IDisplay.GetSuggestions()
        {
            return new Dictionary<int,List<Inline>>();
        }
        int IDisplay.GetSelectedItem()
        {
            return 0;
        }
    }
}
