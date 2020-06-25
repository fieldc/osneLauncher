using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class SECVerb : BaseUrlOpenerVerb
    {
        public SECVerb() :
            base("sec", "SEC Ticker Search", "ticker", "http://www.sec.gov/cgi-bin/browse-edgar?CIK={0}&Find=Search&owner=exclude&action=getcompany")
        {

        }
    }
}
