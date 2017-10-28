using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using osnE.Interop;
using osnE.Interop.Events;
using osnE.WindowsHooks;


namespace osnE.StateMachine
{
    class StateManager
    {
        private static StateManager _instance;
        private static volatile object _synclock = new object();
        public static StateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synclock)
                    {
                        System.Threading.Thread.MemoryBarrier();
                        if (_instance == null)
                        {
                            _instance = new StateManager();
                        }
                    }
                }
                return _instance;
            }
        }
        private State CurrentState;
        private MainWindow m;
        private KeyboardManager k;

        protected StateManager()
        {
            this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
        }

        public void RegisterWindow(MainWindow m)
        {
            this.m = m;
        }
        public void RegisterKeyboardManager(KeyboardManager k)
        {
            this.k = k;
        }
        public State GetState()
        {
            if (this.CurrentState == null)
                this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
            
            return this.CurrentState;
        }

        public KeyboardManager KbdManager
        {
            get { return this.k;  }
        }

        #region Low Level Keyboard Integration Hooks
        public void Keyboard_Activated(object sender, EventArgs e)
        {
            if (this.m != null)
            {
                this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
                this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.ActivateWindow(); this.m.UpdateInterface((IDisplay)this.CurrentState); }));
            }
            else
            {
                this.k.Deactivate();
            }
        }

        public void Keyboard_KeystrokeCaptured(object sender, KeystrokeCapturedEventArgs k)
        {
            KeyPressedEvent kevt = KeyPressedEventFactory.Create(k.Keystroke, k.ShiftPressed);
            if (kevt is DeactivationKeyPressedEvent)
            {
                if(this.m!=null)
                    this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.DeactivateWindow(); }));

                this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
                return;
            }
            if(kevt is ActivationKeyPressedEvent)
            {
                this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
                if (this.m != null)
                {
                    this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.ActivateWindow(); this.m.UpdateInterface((IDisplay)this.CurrentState); }));
                }
                else
                {
                    this.k.Deactivate();
                }
                
                return;
            }

            this.CurrentState = this.CurrentState.Process(kevt);
            if(this.CurrentState is IDisplay)
            {
                this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.UpdateInterface((IDisplay)this.CurrentState); }));
            }
            else if (this.CurrentState is IExecute)
            {
                ((IExecute)this.CurrentState).Execute();
                this.k.Deactivate();
                this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.DeactivateWindow(); }));
                this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
            }

        }

        public void Keyboard_Deactivated(object sender, EventArgs e)
        {
            this.CurrentState = new InitialVerbState(VerbManager.Instance.Verbs());
            if (this.m != null)
            {
                this.m.Dispatcher.BeginInvoke((Action)(() => { this.m.DeactivateWindow(); }));
                this.k.Deactivate();
            }
        }
        #endregion

    }
}
