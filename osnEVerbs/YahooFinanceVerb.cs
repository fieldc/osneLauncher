using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class YahooFinanceVerb : BaseUrlOpenerVerb
    {
        public YahooFinanceVerb() :
            base("yf", "Yahoo Finance", "ticker", "http://finance.yahoo.com/q?s={0}&ql=1")
        {

        }
    }
}
