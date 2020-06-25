using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.Verbs
{
    public class CapIQSearch : BaseUrlOpenerVerb
    {
        public CapIQSearch():
            base("ciqs", "Opens cap iq search page", "search string", "https://www.capitaliq.com/ciqdotnet/search/searchprofiles.aspx?SearchText={0}&SearchText1={0}")
        {

        }

        public override void Execute()
        {
            this.subject = this.subject.Trim();
            if (this.subject.Length == 4)
            {
                this.subject="NasdaqGM:" + this.subject;
            }
            else if (this.subject.Length < 4)
            {
                this.subject = "NYSE:" + this.subject;
            }
            base.Execute();
        }
    }
}
