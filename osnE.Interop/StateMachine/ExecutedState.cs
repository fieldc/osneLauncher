using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.StateMachine
{
    public class ExecutedState : State
    {

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
            return new InitialVerbState(VerbManager.Instance.Verbs());
        }
    }
}
