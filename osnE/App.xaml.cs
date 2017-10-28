using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using osnE.Interop;
using osnE.Interop.Events;
using osnE.WindowsHooks;
using osnE.StateMachine;

namespace osnE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        KeyboardManager keyboard;
        StateManager state;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.keyboard = new KeyboardManager();
            this.state = StateManager.Instance;
            this.state.RegisterKeyboardManager(this.keyboard);
            this.keyboard.Activated += this.state.Keyboard_Activated;
            this.keyboard.Deactivated += this.state.Keyboard_Deactivated;
            this.keyboard.KeystrokeCaptured += this.state.Keyboard_KeystrokeCaptured;
            
        }

        

        
    }
}
