using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace osnE.Interop
{

    public abstract class Verb : IComparable<Verb>
    {
        protected string name;
        protected SubjectType subjectType;
        protected string subject;

        protected PredicateType predicateType;
        protected string predicate;
        protected List<string> predicateList;
        protected string predicateSeperator;

        protected string description;
        protected string subjectHelp;
        protected string predicateHelp;

        protected string titleHelp;

        public Verb(string name, string description, SubjectType subjectType, string subjectHelp, PredicateType predicateType, string predicateHelp = "", string predicateSeperator = "", List<string> predicateList = null)
        {
            if (name == null)
                throw new ArgumentNullException("command");

            /*
			if (postfix == null && (postfixType == SubjectType.Arbitrary || postfixType == SubjectType.Bounded))
				throw new ArgumentNullException("postfix");
            */
            if (description == null)
                throw new ArgumentNullException("description");

            if (subjectHelp == null)
                throw new ArgumentNullException("subjectHelp");

            this.name = name;
            this.description = description;
            this.subjectHelp = subjectHelp;
            this.subjectType = subjectType;
            this.predicateType = predicateType;
            if (predicateSeperator != null)
            {
                this.predicateSeperator = predicateSeperator;
            }
            else
            {
                this.predicateSeperator = "";
            }

            if (predicateList != null)
            {
                this.predicateList = predicateList;
            }
            else
            {
                this.predicateList = new List<string>();
            }
        }

        public string Name
        {
            get { return name; }
        }
        public string Subject
        {
            get { return subject; }
            set { this.subject = value; }
        }
        public string Predicate
        {
            get { return predicate; }
        }
        public string Description
        {
            get { return description; }
        }
        public SubjectType SubjectType
        {
            get { return subjectType; }
        }
        public PredicateType PredicateType
        {
            get { return this.predicateType; }
        }
        public override string ToString()
        {
            return name.ToLower();
            /*
            if (subjectType == SubjectType.None)
                return name.ToLower();
            else
                return String.Format("{0} {{{1}}}", name.ToLower(), subject.ToLower());
            */
        }

        public override bool Equals(Object obj)
        {
            if (Object.ReferenceEquals(this, obj))
                return true;
            Verb other = obj as Verb;
            if (other == null)
                return false;
            return String.Equals(ToString(), other.ToString());
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region IComparable<Verb> Members

        int IComparable<Verb>.CompareTo(Verb other)
        {
            return String.CompareOrdinal(ToString(), other.ToString());
        }

        #endregion
        
        public abstract void Execute();
        public abstract List<string> GetSubjectsOptions(string search);
        public abstract List<string> GetPredicateOptions(string search);
        
        public bool Contains(string s)
        {
            return this.name.ToLower().Contains(s.ToLower());
        }
        public bool StartsWith(string s)
        {
            return this.name.ToLower().StartsWith(s.ToLower());
        }

        public string TitleHelp
        {
            get { return this.description; }
        }
        public string Help
        {
            get
            {
                return this.subjectHelp;
            }
        }
    }
}
