using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    
    public class GoogleVerb: BaseURLOpenerVerb
    {
        public GoogleVerb() :
            base("google", "Google", "search terms", "https://www.google.com/#q={0}")
        {

        }
    }
}
