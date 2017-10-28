using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Interop
{
    public class ShutdownException:Exception
    {
        public ShutdownException():base("shutting down")
        {
            ;
        }
    }
}
