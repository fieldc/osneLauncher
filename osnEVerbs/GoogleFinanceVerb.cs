using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class GoogleFinanceVerb : BaseURLOpenerVerb
    {
        public GoogleFinanceVerb():
            base("gf", "Google Finance", "ticker", "http://finance.google.com/finance?q={0}")
        {

        }
    }
}
