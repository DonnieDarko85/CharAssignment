using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentProblem
{
	public class Task: NamedEntity
    {
        private static int _lastUsedIndex;

        public string PgNumber { get; set; }

		public Task(string pName)
		{
		    Name = string.IsNullOrEmpty(pName) ? "PG " + _lastUsedIndex++ : pName;
		}

        public string GetNumberAndName()
        {
            return PgNumber + " | " + Name;
        }

        public override bool Equals(object obj)
        {
            return Name.Trim().Equals(((Task)obj).Name.Trim());
        }
    }
}
