using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/********************************************************************************
 *                        Assignment Problem Solver
 *                    -------------------------------
 * Use to solve a certain subset of the assignment problem: Have several people give     
 * preferences and/or dislikes, make this software assign the appropriate costs and     
 * calculate the best overall assignment for them.
 * The classes Matrix, MatrixElement, MatrixMath and Program can be used on their
 * own to solve any sort of assignment problem.
 *      
 ********************************************************************************      
 *      
 * MIT License
 *
 * Copyright (c) 2016 Dennis Schwarz  ( DennisSchwarzDev@gmx.de )
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * *******************************************************************************/
namespace AssignmentProblem
{
	public class Agent : NamedEntity
	{
	    private readonly List<Task> _preferredTasks = new List<Task>();
        private readonly List<Task> _impossibleTasks = new List<Task>();
	    public int RoleMultiplier { get ; set; }

	    public List<Task> PreferredTasks
	    {
	        get { return _preferredTasks; }
	    }

	    public List<Task> ImpossibleTasks
	    {
	        get { return _impossibleTasks; }
	    }

	    public Agent(string pName = "")
	    {
	        RoleMultiplier = 1;
			Name = pName;
		}

	    public Agent()
	    {
	        RoleMultiplier = 1;
	    }

	    public void AddPreferredTask(Task pPreferred)
		{
			PreferredTasks.Add(pPreferred);
		}

		public void AddImpossibleTask(Task pImpossible)
		{
			ImpossibleTasks.Add(pImpossible);
		}

		/// <summary>
		/// Remove Task from all lists of agent
		/// </summary>
		/// <param name="pTaskToRemove"></param>
		public void RemoveTask(Task pTaskToRemove)
		{
			int position = PreferredTasks.IndexOf(pTaskToRemove);
			if (position != -1)
			{
				PreferredTasks.RemoveAt(position);
			}

			position = ImpossibleTasks.IndexOf(pTaskToRemove);
			if (position != -1)
			{
				ImpossibleTasks.RemoveAt(position);
			}
		}
	}
}
