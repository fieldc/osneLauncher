using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.Interop.Events;

namespace osnE.StateMachine
{
    public class VerbSearchState : State, IDisplay
    {
        private int selectedVerb = 0;
        private List<Verb> verbs;
        protected string typed = "";

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
                Verb v = VerbManager.Instance.Get(this.typed);
                if (v != null)
                    return new InitialSubject(v, this.typed);


            }
            if (char.IsLetterOrDigit((char)KeyInterop.VirtualKeyFromKey(k.KeyPressed)))
            {
                //this.typed = this.typed + k.KeyPressed.ToString().ToLower();
                this.typed = this.typed + k.KeyString;
            }
            
            this.selectedVerb = 0;
            lock (this.verbs)
            {
                this.verbs = VerbManager.Instance.Search(this.typed, 6);
            }

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

        List<Inline> IDisplay.GetVerbText()
        {
            //return "<Run DefaultStyleKey=\"VerbHighLight\">" + this.verbs[selectedVerb].Name + "</Run> <Run DefaultStyleKey=\"InlineHelpHighLight\"> " + this.verbs[this.selectedVerb].Help + "</Run>";
            lock (this.verbs)
            {
                if (this.verbs.Count == 0)
                {
                    return new List<Inline>();
                }
                int searchStart = this.verbs[selectedVerb].Name.IndexOf(this.typed, StringComparison.OrdinalIgnoreCase);
                string front = "";
                string highlight = "";
                string back = "";

                if (searchStart < 0)
                {
                    return new List<Inline>() {
                        new Run(this.verbs[selectedVerb].Name) { FontWeight = FontWeights.Bold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))} ,
                        new Run(" "+this.verbs[this.selectedVerb].Help) { FontWeight = FontWeights.DemiBold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF999999"))} 
                    };
                }
                if (searchStart > 0)
                {
                    front = this.verbs[selectedVerb].Name.Substring(0, searchStart);
                }
                highlight = this.verbs[selectedVerb].Name.Substring(searchStart, this.typed.Length);

                if (searchStart + this.typed.Length < this.verbs[selectedVerb].Name.Length)
                    back = this.verbs[selectedVerb].Name.Substring(searchStart + this.typed.Length);

                return new List<Inline>() {
                    new Run(front) { FontWeight = FontWeights.Bold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))} ,
                    new Run(highlight) { FontWeight = FontWeights.DemiBold,  Foreground = new SolidColorBrush(Colors.White)},
                    new Run(back) { FontWeight = FontWeights.Bold,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))},
                    new Run(" "+this.verbs[this.selectedVerb].Help) { FontWeight = FontWeights.DemiBold, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF999999"))} 
                };
            } 
        }

        Dictionary<int,List<Inline>> IDisplay.GetSuggestions()
        {
            Dictionary<int, List<Inline>> t = new Dictionary<int, List<Inline>>();
            int counter=0;
            lock (this.verbs)
            {
                foreach (string verbName in (from v in this.verbs where v != this.verbs[this.selectedVerb] select v.Name).ToList<string>())
                {
                    int searchStart = verbName.IndexOf(this.typed, StringComparison.OrdinalIgnoreCase);
                    string front = "";
                    string highlight = "";
                    string back = "";

                    if (searchStart > 0)
                    {
                        front = verbName.Substring(0, searchStart);
                    }
                    highlight = verbName.Substring(searchStart, this.typed.Length);

                    if (searchStart + this.typed.Length < verbName.Length)
                        back = verbName.Substring(searchStart + this.typed.Length);

                    List<Inline> r = new List<Inline>() {
                        new Run(front) { FontWeight = FontWeights.Normal, Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))} ,
                        new Run(highlight) { FontWeight = FontWeights.DemiBold,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF9FBE57"))},
                        new Run(back) { FontWeight = FontWeights.Normal,  Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF7F9845"))}};

                    t.Add(counter, r);
                    counter = counter + 1;
                }
            }
            return t;
        }

        int IDisplay.GetSelectedItem()
        {
            return this.selectedVerb;
        }
    }
}
