using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace osnE.Interop
{
    public interface IVerb
    {
        VerbKeyResponse HandleKey(Key k);
        bool StartsWith (string s);
        bool Contains(string s);
        void Execute();
        void Reset();
        //public Keys PredicateSeparator();
    }
}
