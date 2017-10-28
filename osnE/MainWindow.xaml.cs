using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Markup;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using osnE.WindowsHooks;
using osnE.Interop;
using osnE.Verbs;
using osnE.Interop.Events;
using osnE.StateMachine;
using Common.Logging;

namespace osnE
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window , IManagerUI
    {
        private List<ListItem> uiElements;
        static readonly ILog logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.uiElements = new List<ListItem>(7);
            for (int i =0 ; i<7; i++)
            {
                this.uiElements.Add(new ListItem(i + 1));
            }
            StateManager.Instance.RegisterWindow(this);
            List<Verb> l = new List<Verb>() {
                new URLVerb(),
                new OpenVerb(),
                new GoVerb(),
                new SECVerb(),
                new GoogleVerb(),
                new GoogleFinanceVerb(),
                new YahooFinanceVerb(),
                new CallStreetVerb(),
                new WikipediaVerb(),
                new LearnVerb(),
                new CapslockVerb(),
                new UnlearnVerb(),
                new QuitVerb()
            };
            foreach (Verb v in l)
            {
                VerbManager.Instance.RegisterVerb(v);
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.ErrorFormat("Exception caught ex={0}", e.ExceptionObject);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.CloseUiElements();
            base.OnClosing(e);
        }

        private void CloseUiElements()
        {
            foreach (ListItem l in this.uiElements)
            {
                l.ClearTextAndHide();
            }
        }
        public void UpdateInterface(IDisplay state)
        {
            string titleText = state.GetTitle();
            if (this.titleRow.Text != titleText)
                this.titleRow.Text = titleText;
            
            List<Inline> verbText = state.GetVerbText();
            if (verbText!=null && verbText.Count>0)
            {
                this.uiElements[0].SetTextAndShow(verbText);
                Dictionary<int,List<Inline>> suggestions = state.GetSuggestions();
                int highlightSuggestion = state.GetSelectedItem();
                int display = Math.Min(6, suggestions.Count);
                for(int i=0;i<display;i++)
                {
                    this.uiElements[i+1].SetTextAndShow(suggestions[i]);
                    if((i+1)==highlightSuggestion)
                    {
                        this.uiElements[i + 1].txtSearchString.FontWeight = FontWeights.Bold;
                    }
                }
                for(int i=display+1;i<this.uiElements.Count;i++)
                {
                    if (this.uiElements[i] != null)
                        this.uiElements[i].ClearTextAndHide();
                }
            }
            else
            {
                foreach (ListItem li in this.uiElements)
                {
                    li.ClearTextAndHide();
                }
            }
        }
        
        public void ActivateWindow()
        {
            this.Activate();
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void DeactivateWindow()
        {
            this.CloseUiElements();
            this.Visibility = System.Windows.Visibility.Hidden;
        }


        void IManagerUI.ActivateAndUpdate(IDisplay state)
        {
            this.Dispatcher.BeginInvoke((Action)(() => { this.ActivateWindow(); this.UpdateInterface(state); }));
        }

        void IManagerUI.Update(IDisplay state)
        {
            this.Dispatcher.BeginInvoke((Action)(() => { this.UpdateInterface(state); }));
        }

        void IManagerUI.Deactivate()
        {
            this.Dispatcher.BeginInvoke((Action)(() => { this.DeactivateWindow(); }));
        }
        
        void IManagerUI.Shutdown()
        {
            this.Dispatcher.BeginInvoke((Action)(() => 
            { 
                foreach(ListItem li in this.uiElements)
                {
                    li.Close();
                }
                this.Close();
                Application.Current.Shutdown(); 
            }));
            Environment.Exit(0);
        }
    }
}
