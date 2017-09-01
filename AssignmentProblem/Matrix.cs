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
    class Matrix
    {
        /// <summary>
        /// Array with all elements of the matrix
        /// </summary>
        private MatrixElement[] m_elements = null;
        
        /// <summary>
        /// If TRUE, then column is covered
        /// </summary>
        private bool[] m_rowCover = null;

        /// <summary>
        /// If TRUE, then column is covered
        /// </summary>
        private bool[] m_colCover = null;

        /// <summary>
        /// Size n of the matrix
        /// </summary>
        private int m_size = 0;

        /// <summary>
        /// Returns matrix elements by index
        /// </summary>
        /// <param name="key">index of the matrix element</param>
        /// <returns></returns>
        public MatrixElement this[int row, int column]
        {
            get
            {
                int index = column + row * m_size;

                if (index >= 0 && index < m_size * m_size)
                {
                    return m_elements[index];
                }
                return null;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p_size">Size n of thes n x n Matrix</param>
        /// <param name="p_costs">Cost values for the matrix</param>
        public Matrix(int p_size, List<int> p_costs)
        {
            m_size = p_size;

            m_elements = new MatrixElement[m_size * m_size];
            for (int i = 0; i < m_size * m_size; i++)
            {
                m_elements[i] = new MatrixElement(p_costs[i]);
            }

            m_colCover = new bool[m_size];
            m_rowCover = new bool[m_size];
            Uncover();
        }


        /// <summary>
        /// Outputs current matrix to console
        /// </summary>
        public void PrintMatrix(string heading)
        {
            Console.WriteLine("------------------------------------------\n" + heading + ":");
            
            // Iterate over rows
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                Console.Write("| ");

                for (int colID = 0; colID < m_size; colID++)
                {
                    if (RowCovered(rowID))
                    {
                        Console.Write("███");
                    }
                    else if (ColCovered(colID))
                    {
                        Console.Write(" █ ");
                    }
                    else
                    {
                        Console.Write("   ");
                    }
                    Console.Write("{0,3}", this[rowID, colID].Value);
                    Console.Write(this[rowID, colID].IsPrime ? "'" : " ");
                    Console.Write(this[rowID, colID].IsStar ? "*" : " ");
                    Console.Write(" ");
                }

                Console.WriteLine("|");
            }
        }

        /// <summary>
        /// Returns optimal assignment
        /// </summary>
        /// <returns></returns>
        public List<int> GetOptimalAssignmentByRow()
        {
			List<int> assignment = new List<int>();

            // Iterate over rows and colums to find optimal assignment
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (this[rowID, colID].IsStar)
                    {
                        assignment.Add(colID);
                    }
                }
            }

            return assignment;
        }

        /// <summary>
        /// Is column covered?
        /// </summary>
        /// <param name="index">column ID</param>
        /// <returns>TRUE is covered</returns>
        public bool ColCovered(int index)
        {
            if (index >= 0 && index < m_size)
            {
                return m_colCover[index];
            }
            return false;
        }

        /// <summary>
        /// Is Row Covered?
        /// </summary>
        /// <param name="index">row ID</param>
        /// <returns>TRUE is covered</returns>
        public bool RowCovered(int index)
        {
            if (index >= 0 && index < m_size)
            {
                return m_rowCover[index];
            }
            return false;
        }

        /// <summary>
        /// Covers a column
        /// </summary>
        /// <param name="index">column id</param>
        /// <param name="cover">if false, uncover</param>
        public void CoverColumn(int index, bool cover = true)
        {
            if (index >= 0 && index < m_size)
            {
                m_colCover[index] = cover;
            }
        }

        /// <summary>
        /// Covers a row
        /// </summary>
        /// <param name="index">row ID</param>
        /// <param name="cover">if false, uncover</param>
        public void CoverRow(int index, bool cover = true)
        {
            if (index >= 0 && index < m_size)
            {
                m_rowCover[index] = cover;
            }
        }

        /// <summary>
        /// Removes covers from matrix
        /// </summary>
        public void Uncover()
        {
            for (int i = 0; i < m_size; i++)
            {
                CoverColumn(i, false);
                CoverRow(i, false);
            }
        }


        /// <summary>
        /// Remove primes from matrix
        /// </summary>
        public void RemovePrimes()
        {
            for (int i = 0; i < m_size * m_size; i++)
            {
                m_elements[i].IsPrime = false;
            }
        }

        /// <summary>
        /// Find uncovered zero
        /// </summary>
        /// <param name="p_rowID"></param>
        /// <param name="p_colID"></param>
        public void FindUncoveredZero(ref int p_rowID, ref int p_colID)
        {
            // Iterate over rows and colums to find zeroes
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (this[rowID, colID].Value == 0 && !RowCovered(rowID) && !ColCovered(colID))
                    {
                        p_rowID = rowID;
                        p_colID = colID;
                        return;
                    }
                }
            }

            p_rowID = -1;
            p_colID = -1;
        }

        /// <summary>
        /// Has row a star?
        /// </summary>
        /// <param name="rowID">row to search</param>
        /// <returns>TRUE is star in row</returns>
        public bool IsStarInRow(int rowID)
        {
            return (FindStarInRow(rowID) != -1);
        }

        /// <summary>
        /// Get Position of Star in row
        /// </summary>
        /// <param name="rowID">row to search</param>
        /// <returns>-1 if no star, column id otherwise</returns>
        public int FindStarInRow(int rowID)
        {
            for (int colID = 0; colID < m_size; colID++)
            {
                if (this[rowID, colID].IsStar)
                {
                    return colID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get Position of Star in column
        /// </summary>
        /// <param name="colID">column to search</param>
        /// <returns>-1 if no star, row id otherwise</returns>
        public int FindStarInColumn(int colID)
        {
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                if (this[rowID, colID].IsStar)
                {
                    return rowID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get Position of Prime in row
        /// </summary>
        /// <param name="rowID">row to search</param>
        /// <returns>-1 if no prime, column id otherwise</returns>
        public int FindPrimeInRow(int rowID)
        {
            for (int colID = 0; colID < m_size; colID++)
            {
                if (this[rowID, colID].IsPrime)
                {
                    return colID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Finds the smallest uncovered value in the matrix
        /// </summary>
        /// <returns>Smallest uncovered value</returns>
        public int FindSmallestUncoveredValue()
        {
            int minValue = int.MaxValue;
            for (int rowID = 0; rowID < m_size; rowID++)
            {
                for (int colID = 0; colID < m_size; colID++)
                {
                    if (!RowCovered(rowID) && !ColCovered(colID))
                    {
                        if (minValue > this[rowID, colID].Value)
                        {
                            minValue = this[rowID, colID].Value;
                        }
                    }
                }
            }

            return minValue;
        }


        public void AugmentPath(List<int> rowPath, List<int> colPath)
        {
            for (int i = 0; i < rowPath.Count; i++)
            {
                if (this[rowPath[i], colPath[i]].IsStar)
                {
                    this[rowPath[i], colPath[i]].IsStar = false;
                }
                else
                {
                    this[rowPath[i], colPath[i]].IsStar = true;
                }
            }
        }

    }
}
