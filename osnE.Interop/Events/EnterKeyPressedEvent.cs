using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Interop.Events
{
    public class EnterKeyPressedEvent : KeyPressedEvent
    {
        public EnterKeyPressedEvent(System.Windows.Input.Key pressed)
            : base(pressed)
        {

        }
    }
}
