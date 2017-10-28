using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace osnE.WindowsHooks
{
    public class KeystrokeCapturedEventArgs : EventArgs
    {
        private Key key;
        private ModifierKeys modifierKeys;
        public KeystrokeCapturedEventArgs(Key k, ModifierKeys modifierKeys)
        {
            this.key = k;
            this.modifierKeys = modifierKeys;
        }
        public bool ShiftPressed
        {
            get { return (this.modifierKeys == ModifierKeys.Shift); }
        }
        public Key Keystroke
        {
            get { return this.key; }
        }
    }
}
