using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;

namespace osnE.Interop.Events
{
    public class KeyPressedEventFactory
    {
        public static KeyPressedEvent Create(Key input,bool shiftPressed)
        {
            switch (input)
            {
                default:
                    return new KeyPressedEvent(input, shiftPressed);
                case Key.CapsLock:
                    return new ActivationKeyPressedEvent(input);
                case Key.Back:
                    return new BackspaceKeyPressedEvent(input);
                case Key.Delete:
                    return new BackspaceKeyPressedEvent(input);
                case Key.Escape:
                    return new DeactivationKeyPressedEvent(input);
                case Key.Down:
                    return new DownArrowKeyPressedEvent(input);
                case Key.Enter: //Also maps Key.Return
                    return new EnterKeyPressedEvent(input);
                case Key.Space:
                    return new SpaceKeyPressedEvent(input);
                case Key.Tab:
                    return new TabKeyPressedEvent(input);
                case Key.Up:
                    return new UpArrowKeyPressedEvent(input);
                case Key.A:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.B:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.C:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.E:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.F:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.G:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.H:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.I:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.J:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.K:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.L:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.M:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.N:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.O:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.P:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.Q:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.R:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.S:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.T:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.U:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.V:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.W:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.X:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.Y:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.Z:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D0:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D1:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D2:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D3:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D4:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D5:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D6:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D7:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D8:
                    return new AlphaNumericKeyPressedEvent(input);
                case Key.D9:
                    return new AlphaNumericKeyPressedEvent(input);
                
            }
        }
    }
}
