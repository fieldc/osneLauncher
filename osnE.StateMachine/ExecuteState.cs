using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;

namespace osnE.StateMachine
{
    public class ExecuteState: State,IExecute
    {
        private Verb me;

        public ExecuteState(Verb me)
        {
            this.me = me;    
        }

        public override State Process(Interop.Events.KeyPressedEvent k)
        {
            return this;
        }
        public State Execute()
        {
            this.me.Execute();
            return new ExecutedState();
        }
    }
}
