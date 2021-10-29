using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    static class Utility
    {
        private static readonly char[] POSSIBLE_READ_VALUES = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly char[] POSSIBLE_ENTER_VALUES = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Checks to see if the value read in the .txt file is a valid value (0-9).
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if value is a valid one, false if it is not.</returns>
        public static Boolean checkPossibleReadValues(char value)
        {
            foreach (char c in POSSIBLE_READ_VALUES)
            {
                if (c == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the value entered by the user is a valid enter value (1-9). 
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if value is a valid one, false if it is not.</returns>
        public static Boolean checkPossibleEnterValues(char value)
        {
            foreach (char c in POSSIBLE_ENTER_VALUES)
            {
                if (c == value)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Takes a 2-dimensional SudokuSquareGrid array and clones it.
        /// </summary>
        /// <param name="mainGrid">The 2-dimensional SudokuSquareGrid array we want to clone.</param>
        /// <returns>A brand new 2-dimensional SudokuSquareGrid array.</returns>
        internal static SudokuSquareGrid[,] cloneGrid(SudokuSquareGrid[,] mainGrid)
        {
            SudokuSquareGrid[,] rtnGrid = new SudokuSquareGrid[9,9];
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    rtnGrid[x, y] = new SudokuSquareGrid(mainGrid[x, y].ValueC, mainGrid[x, y].FixedNumB, x, y);
                }
            }
            return rtnGrid;
        }

    }
}
