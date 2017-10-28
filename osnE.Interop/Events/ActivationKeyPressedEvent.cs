using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace osnE.Interop.Events
{
    public class ActivationKeyPressedEvent : KeyPressedEvent
    {
        public ActivationKeyPressedEvent(System.Windows.Input.Key pressed): base(pressed)
        {

        }
    }
}
