using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class WikipediaVerb : BaseUrlOpenerVerb
    {
        public WikipediaVerb() :
            base("w", "Wikipedia", "search terms", "http://en.wikipedia.org/w/index.php?search={0}")
        {

        }
    }
}
