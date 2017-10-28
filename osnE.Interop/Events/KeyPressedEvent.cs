using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace osnE.Interop.Events
{
    public class KeyPressedEvent
    {
        Key key;
        string keyString;

        public KeyPressedEvent(Key key, bool shiftPressed = false)
        {
            this.key = key;
            this.keyString = KeyTransformer.mapKeyPressToActualCharacter(shiftPressed, key);
        }
        public string KeyString
        {
            get { return this.keyString; }
        }
        public Key KeyPressed
        {
            get { return this.key; }
        }
    }
}
