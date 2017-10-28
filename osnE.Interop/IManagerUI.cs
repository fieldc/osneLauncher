using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;

namespace osnE.Interop
{
    public interface IManagerUI
    {
        //this.m.ActivateWindow(); this.m.UpdateInterface((IDisplay)this.CurrentState); 
        void ActivateAndUpdate(IDisplay state);
        void Update(IDisplay state);
        void Deactivate();
        void Shutdown();
    }
}
