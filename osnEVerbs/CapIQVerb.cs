using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class CapIQVerb :BaseUrlOpenerVerb
    {
        public CapIQVerb() :
            base("ciq", "Opens cap iq page", "search string", "https://www.capitaliq.com/ciqdotnet/search/searchprofiles.aspx?SearchText={0}&SearchText1={0}")
        {

        }
    }
}
