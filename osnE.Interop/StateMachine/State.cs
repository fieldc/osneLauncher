using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using osnE.Interop;
using osnE.Interop.Events;

namespace osnE.StateMachine
{
    abstract public class State
    {
        protected State()
        {
        }
        abstract public State Process(KeyPressedEvent k);

    }
}
