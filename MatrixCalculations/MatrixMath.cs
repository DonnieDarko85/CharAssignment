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

namespace MatrixCalculations
{
	public class MatrixMath
    {
		/// <summary>
		/// The matrix calculations are done on
		/// </summary>
        private Matrix m = null;

		/// <summary>
		/// Matrixsize n  (Matrix is of size n x n)
		/// </summary>
        private int m_size = 0;

		/// <summary>
		/// Minimum column value
		/// </summary>
        private int m_minColID = -1;

		/// <summary>
		/// Minimum row value
		/// </summary>
        private int m_minRowID = -1;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p_size"></param>
		/// <param name="p_costs"></param>
        public MatrixMath(int p_size, List<int> p_costs)
        {
            m = new Matrix(p_size, p_costs);
            m_size = p_size;
        }

        /// <summary>
        /// Calculate a valid, optimal assignment for the matrix
        /// Source: http://csclab.murraystate.edu/bob.pilgrim/445/munkres.html
        /// </summary>
        /// <returns>Optimal assignment (indexed by rowID)</returns>
        public List<int> Calculate()
        {
            m.PrintMatrix("(0) Original Matrix");

            SubMinFromRows();
            m.PrintMatrix("(1) After substracting row minimums");

            FindAndStarZeroes();
            m.PrintMatrix("(2) After finding/staring zeroes");

            while (!CoverColumnsAndFindAssignment())
            {
                m.PrintMatrix("(3) After covering zeroes");

                while (!PrimeNoncoveredZero())
                {
                    m.PrintMatrix("(4) After priming zeroes");

                    SubstractSmallestValue();
                    m.PrintMatrix("(6) After substracting smallest value");
                }
                m.PrintMatrix("(4) After priming zeroes");

                AugmentPath();
                m.PrintMatrix("(5) After augmenting path");
            }

            m.PrintMatrix("(7) Final Matrix");

            return m.GetOptimalAssignmentByRow();
        }

        /// <summary>
        /// Substract the minimum value of each row from each element of the row
        /// </summary>
        private void SubMinFromRows()
        {
            // Iterate over rows to find minimum element
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                int rowMinimum = m[rowID, 0].Value;
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (m[rowID, colID].Value < rowMinimum)
                    {
                        rowMinimum = m[rowID, colID].Value;
                    }
                }
                for (int colID = 0; colID < m_size; colID++)
                {
                    m[rowID, colID].Value -= rowMinimum;
                }
            }
        }

        /// <summary>
        /// Find Zero in matrix. If there is no starred zero in it's row or column, star it. Repeat for each element
        /// </summary>
        private void FindAndStarZeroes()
        {
            int rowID = 0;
            int colID = 0;

            // Iterate over rows and colums to find (and star) zeroes
            while (true)
            {
                m.FindUncoveredZero(ref rowID, ref colID);

                if (rowID == -1) break;

                m[rowID, colID].IsStar = true;
                m.CoverRow(rowID);
                m.CoverColumn(colID);
            }

            // Remove cover from matrix again
            m.Uncover();
        }

        /// <summary>
        /// Cover all columns containing starred zeroes.
        /// If all Columns are covered, complete assignment was found
        /// </summary>
        /// <returns>TRUE is assignment was found</returns>
        private bool CoverColumnsAndFindAssignment()
        {
            // Iterate over rows and colums to find starred zeroes
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (m[rowID, colID].IsStar)
                    {
                        m.CoverColumn(colID);
                    }
                }
            }

            // Count number of covered columns
            int columnCount = 0;
            for (int colID = 0; colID < m_size; colID++)
            {
                if (m.ColCovered(colID))
                {
                    columnCount++;
                }
            }

            // If all columns covered, assignment was found
            if (columnCount >= m_size)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Finds noncovered zero and primes it
        /// </summary>
        /// <returns>FALSE if no zero to prime</returns>
        private bool PrimeNoncoveredZero()
        {
            int rowID = -1;
            int colID = -1;

            while (true)
            {
                m.FindUncoveredZero(ref rowID, ref colID);
                if (rowID == -1)
                {
                    return false;
                }
                else
                {
                    m[rowID, colID].IsPrime = true;
                    if (m.IsStarInRow(rowID))
                    {
                        colID = m.FindStarInRow(rowID);
                        m.CoverRow(rowID);
                        m.CoverColumn(colID, false);
                    }
                    else
                    {
                        m_minRowID = rowID;
                        m_minColID = colID;
                        return true;
                    }
                }
            }
        }

        private void AugmentPath()
        {
            bool done = false;
            int rowID = -1;
            int colID = -1;

            List<int> rowPath = new List<int>();
            List<int> colPath = new List<int>();

            rowPath.Add(m_minRowID);
            colPath.Add(m_minColID);

            while (!done)
            {
                rowID = m.FindStarInColumn(colPath[colPath.Count - 1]);
                if (rowID > -1)
                {
                    rowPath.Add(rowID);
                    colPath.Add(colPath[colPath.Count - 1]);
                }
                else
                {
                    done = true;
                }

                if (!done)
                {
                    colID = m.FindPrimeInRow(rowPath[rowPath.Count - 1]);
                    rowPath.Add(rowPath[rowPath.Count - 1]);
                    colPath.Add(colID);
                }
            }

            m.AugmentPath(rowPath, colPath);
            m.Uncover();
            m.RemovePrimes();
        }


        /// <summary>
        /// Add smallest uncovered value to each element of covered rows,
        /// then substract it from each element in an uncovered column
        /// </summary>
        public void SubstractSmallestValue()
        {
            int minValue = m.FindSmallestUncoveredValue();

            for (int rowID = 0; rowID < m_size; rowID++)
            {
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (m.RowCovered(rowID))
                    {
                        m[rowID, colID].Value += minValue;
                    }
                    if (!m.ColCovered(colID))
                    {
                        m[rowID, colID].Value -= minValue;
                    }
                }
            }
        }
    }
}
