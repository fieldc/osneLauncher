using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace osnE.Interop
{
    public interface IKeyboardCallback
    {
        void CapsActivated();
        void EscDectivated();
        void CapturedKey(Key k);
    }
}
