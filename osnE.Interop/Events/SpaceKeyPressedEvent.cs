using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Interop.Events
{
    public class SpaceKeyPressedEvent : KeyPressedEvent
    {
        public SpaceKeyPressedEvent(System.Windows.Input.Key pressed)
            : base(pressed)
        {

        }
    }
}
