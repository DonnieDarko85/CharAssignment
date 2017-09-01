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

		public Task(string pName)
		{
		    Name = string.IsNullOrEmpty(pName) ? "PG " + _lastUsedIndex++ : pName;
		}
    }
}
