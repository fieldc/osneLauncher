using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osnE.Interop;

namespace osnE.Interop
{
    public class VerbManager
    {
        private static VerbManager _instance;
        private static volatile object _synclock = new object();
        public static VerbManager Instance
        {
            get {
                if (_instance == null) {
                    lock (_synclock) {
                        System.Threading.Thread.MemoryBarrier();
                        if (_instance == null) {
                            _instance = new VerbManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private List<Verb> verbs;

        protected VerbManager()
        {
            this.verbs = new List<Verb>();
        }

        public bool RegisterVerb(Verb v)
        {
            this.verbs.Add(v);
            return true;
        }

        public List<Verb> Verbs()
        {
            return new List<Verb>(this.verbs);
        }

        public Verb Get(string search)
        {
            Verb verb = (from v in this.verbs where v.Name.ToLower() == search.ToLower() select v).FirstOrDefault();
            return verb;
        }
        public List<Verb> Search(string search,int take)
        {
            if(search == null)
            {
                return new List<Verb>();
            }
            List<Verb> l = (from v in this.verbs where v.StartsWith(search.ToLower()) select v).Take(take).ToList<Verb>();

            if (take == 1 && l == null)
                return new List<Verb>();
            
            if (l!=null && l.Count == take)
                return l;

            if(l==null){
                l = new List<Verb>();
            }

            l.AddRange((from v in this.verbs where v.Contains(search.ToLower()) select v).Take(take - l.Count).ToList<Verb>());
            l = l.Distinct().ToList<Verb>();
            return l;
        }
    }
}
