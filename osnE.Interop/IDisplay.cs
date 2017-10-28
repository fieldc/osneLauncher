using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace osnE.Interop
{
    public interface IDisplay
    {
        string GetTitle();
        List<Inline> GetVerbText();
        Dictionary<int,List<Inline>> GetSuggestions();
        int GetSelectedItem();
    }
}
