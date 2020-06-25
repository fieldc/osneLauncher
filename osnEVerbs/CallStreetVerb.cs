using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class CallStreetVerb : BaseUrlOpenerVerb
    {
        public CallStreetVerb():
            base("cs", "Open Call Street Site", "search string", "https://callstreet.factset.com/eventView.jsp?itemValue={0}-US")
        {

        }
    }
}
