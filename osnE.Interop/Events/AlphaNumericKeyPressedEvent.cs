using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Interop.Events
{
    public class AlphaNumericKeyPressedEvent : KeyPressedEvent 
    {
         public AlphaNumericKeyPressedEvent (System.Windows.Input.Key input) : base(input)
        {

        }
    }
}
