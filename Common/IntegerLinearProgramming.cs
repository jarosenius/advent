using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.Common
{
    public static class IntegerLinearProgramming
    {
        /// <summary>
        /// Solves a system of linear equations where we want to minimize the sum of all variables.
        /// </summary>
        /// <param name="coefficients">List of equations, where each equation is a list of variable indices that appear in it. [[0, 2], [1, 3]] means equation 0 has x0 + x2, equation 1 has x1 + x3</param>
        /// <param name="targets">Target values for each equation. [5, 7] means first equation = 5, second = 7</param>
        /// <param name="numVariables">Total number of variables</param>
        /// <returns>Minimum sum of variables, or -1 if no solution exists</returns>
        public static long SolveMinimizeSum(List<List<int>> coefficients, int[] targets, int numVariables)
        {
            var equations = targets.Length;
            var matrix = CreateMatrix(coefficients, targets, numVariables, equations);
            var pivotCols = RowReduce(matrix, equations, numVariables);

            var freeVars = new List<int>();
            for (var v = 0; v < numVariables; v++)
            {
                if (!pivotCols.Contains(v))
                    freeVars.Add(v);
            }

            if (freeVars.Count != 0)
                return EnumerateFreeVariables(matrix, equations, numVariables, pivotCols, freeVars, targets);

            var res = BackSubstitute(matrix, equations, numVariables, pivotCols);
            if (res != null && res.All(x => x >= 0))
                return res.Sum();
            return -1;

        }

        private static long[,] CreateMatrix(List<List<int>> coefficients, int[] targets, int numVariables, int numEquations)
        {
            /*
             * Example:
             *  coefficients = [[0, 2], [1, 3]],
             *  targets = [5, 7],
             *  numVariables = 4
             * Result:
             *  [1, 0, 1, 0 | 5]  # equation 0: x0 + x2 = 5
             *  [0, 1, 0, 1 | 7]  # equation 1: x1 + x3 = 7
             */
            var matrix = new long[numEquations, numVariables + 1];
            for (var eq = 0; eq < numEquations; eq++)
            {
                foreach (var varIdx in coefficients[eq])
                {
                    matrix[eq, varIdx] = 1;
                }
                matrix[eq, numVariables] = targets[eq];
            }

            return matrix;
        }

        private static List<int> RowReduce(long[,] matrix, int rows, int cols)
        {
            var pivotCols = new List<int>();
            var currentRow = 0;
            
            for (var col = 0; col < cols && currentRow < rows; col++)
            {
                var pivotRow = -1;
                for (var row = currentRow; row < rows; row++)
                {
                    if (matrix[row, col] == 0) 
                        continue;
                    pivotRow = row;
                    break;
                }
                
                if (pivotRow == -1)
                    continue;
                
                pivotCols.Add(col);    
                if (pivotRow != currentRow)
                {
                    for (var c = 0; c <= cols; c++)
                    {
                        (matrix[currentRow, c], matrix[pivotRow, c]) = (matrix[pivotRow, c], matrix[currentRow, c]);
                    }
                }

                for (var row = 0; row < rows; row++)
                {
                    if (row == currentRow || matrix[row, col] == 0)
                        continue;
                    
                    var factor = matrix[row, col] / matrix[currentRow, col];
                    for (var c = col; c <= cols; c++)
                    {
                        matrix[row, c] -= factor * matrix[currentRow, c];
                    }
                }    
                currentRow++;
            }
            
            return pivotCols;
        }

        private static long[] BackSubstitute(long[,] matrix, int rows, int cols, List<int> pivotCols)
        {
            /*
             * Example:
             *  with the row [0, 0, 3, 2, 0 | 17] and x3 = 1
             *  calculate x2:
             *   v = 17 - (2 × 1) = 15
             *   x2 = 15 / 3
             */
            var solution = new long[cols];

            foreach (var col in pivotCols)
            {
                for (var row = 0; row < rows; row++)
                {
                    if (matrix[row, col] == 0)
                        continue;
                    
                    var value = matrix[row, cols];
                    for (var c = 0; c < cols; c++)
                    {
                        if (c != col)
                            value -= matrix[row, c] * solution[c];
                    }
                    
                    if (value % matrix[row, col] != 0)
                        return null;
                    
                    solution[col] = value / matrix[row, col];
                    break;
                }
            }
            
            return solution;
        }

        private static long EnumerateFreeVariables(
            long[,] matrix, 
            int rows, 
            int cols, 
            List<int> pivotCols,
            List<int> freeVars,
            int[] targets)
        {
            var maxFreeValue = targets.Max();
            var minCost = long.MaxValue;
            
            var maxIterations = (int)Math.Pow(maxFreeValue + 1, freeVars.Count);
            
            if (maxIterations > 100000)
                maxFreeValue = Math.Min(maxFreeValue, 20);

            Enumerate(0, new long[cols]);
            
            return minCost == long.MaxValue ? -1 : minCost;

            void Enumerate(int freeVarIdx, long[] solution)
            {
                if (freeVarIdx == freeVars.Count)
                {
                    var tempSolution = (long[])solution.Clone();
                    
                    foreach (var col in pivotCols)
                    {
                        for (var row = 0; row < rows; row++)
                        {
                            if (matrix[row, col] == 0)
                                continue;
                            
                            var value = matrix[row, cols];
                            for (var c = 0; c < cols; c++)
                            {
                                if (c != col)
                                    value -= matrix[row, c] * tempSolution[c];
                            }
                            
                            if (matrix[row, col] == 0 || value % matrix[row, col] != 0)
                                return;
                            
                            tempSolution[col] = value / matrix[row, col];
                            break;
                        }
                    }

                    if (!tempSolution.All(x => x >= 0)) 
                        return;
                    var cost = tempSolution.Sum();
                    minCost = Math.Min(minCost, cost);

                    return;
                }
                
                var freeVar = freeVars[freeVarIdx];
                for (var value = 0; value <= maxFreeValue; value++)
                {
                    solution[freeVar] = value;
                    Enumerate(freeVarIdx + 1, solution);
                }
            }
        }
    }
}