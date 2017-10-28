using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osnE.StateMachine
{
    public interface IDisplay
    {
        string GetTitle();
        string GetVerbText();
        List<string> GetSuggestions();
    }
}
