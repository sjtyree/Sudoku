using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    static class SudokuSolver
    {
        /// <summary>
        /// The initial function that is called to solve the Sudoku puzzle. It checks to see if the puzzle is solvable, then attempts to solve it.
        /// </summary>
        /// <param name="puzzle">The puzzle to be solved.</param>
        /// <returns>True if the puzzle is solvable and was solved. False if the puzzle is not solvable, or could not be solved.</returns>
        public static bool solvePuzzle(SudokuSquareGrid [,] puzzle)
        {
            return checkPuzzle(puzzle) && solvePuzzle(puzzle, 0, 0);
        }

        /// <summary>
        /// Checks to make sure the initial puzzle is not unsolvable (such as having two numbers in the same row).
        /// </summary>
        /// <param name="puzzle">The puzzle to check</param>
        /// <returns>True if the puzzle can be solved, false if it can not be solved.</returns>
        private static bool checkPuzzle(SudokuSquareGrid[,] puzzle)
        {
            for (int x = 0; x < 9; x++)
            {
                if (!checkrow(puzzle, x))
                    return false;
            }
            for (int y = 0; y < 9; y++)
            {
                if (!checkcolumn(puzzle, y))
                    return false;
            }
            for (int x = 0; x < 9; x += 3)
            {
                for (int y = 0; y < 9; y += 3)
                {
                    if (!checksquare(puzzle, x, y))
                        return false;
                }

            }

            return true;
        }

        /// <summary>
        /// The main recursive function that is used to solve the Sudoku puzzle.
        /// </summary>
        /// <param name="puzzle">The sudoku puzzle to be solved.</param>
        /// <param name="x">The puzzle's x coordinate that we are checking.</param>
        /// <param name="y">The puzzle's y coordinate that we are checking.</param>
        /// <returns>True if we have successfully reached the end of the solved puzzle. False if we have tried every number for the current square, and need to move back one square to try another number. 
        /// Also returns false if the puzzle cannot be solved for some reason.</returns>
        private static bool solvePuzzle(SudokuSquareGrid[,] puzzle, int x, int y)
        {
            //base case, reached the end of the puzzle
            if (x == 9 && y == 0)
            {
                return true;
            }
            SudokuSquareGrid square = puzzle[x, y];
            int nextX, nextY;//the next x and y coordinates we will go to after this call
            if (y == 8) //if we're at the end of the row, move onto the next row
            {
                nextX = x + 1;
                nextY = 0;
            }
            else //we are not at the end of the row, keep moving right 
            {
                nextX = x;
                nextY = y + 1;
            }
            if (!square.FixedNumB) //if number is not fixed, try to put a number in.
            {
                for (char prospect = '1'; prospect <= '9'; prospect++) //iterate through the numbers 1-9 to determine which number goes there
                {
                    if (checkrow(puzzle, x, y, prospect) && checkcolumn(puzzle, x, y, prospect) && checksquare(puzzle, x, y, prospect))
                    {//if number is not in the row or column or square, put the number there and move on to the next square
                        square.ValueC = prospect;
                        puzzle[x,y] = square;
                        if (solvePuzzle(puzzle, nextX, nextY)) //return true and break the loop if puzzle is complete. otherwise, continue looping
                            return true;
                    }
                }
                square.ValueC = '0'; //set value back to 0 if we can't find a prospective number
                puzzle[x,y] = square;
                return false; //return false and try a different number on the previous square
            }
            else //if number is fixed, move on to the next square
            {
                return solvePuzzle(puzzle, nextX, nextY);
            }
        }

        /// <summary>
        /// Checks the row of the x-y coordinate to see if any other squares in the row contain the same number. This version checks each square against a prospect number that is sent in.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="prospect">The prospect number</param>
        /// <returns>True if the coordinate's row does not contain the prospect number. False if the row does contain the prospect number.</returns>
        private static bool checkrow(SudokuSquareGrid[,] puzzle, int x, int y, char prospect)
        {
            for (int num = 0; num < 9; num++)
            {
                if (y != num)
                    if (puzzle[x,num].ValueC == prospect)
                        return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the row of the x coordinate to see if there are two squares in the row that contain the same number.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="x">The x coordinate, AKA the row number.</param>
        /// <returns>True if the row does not contain 2 of the same number. False if the row does contain 2 of the same number.</returns>
        public static bool checkrow(SudokuSquareGrid[,] puzzle, int x)
        {
            for (int a = 0; a < 9; a++)
            {
                //skip '0' (empty) squares
                if (puzzle[x,a].ValueC == '0')
                    continue;
                for (int b = a + 1; b < 9; b++)
                {
                    //skip '0' (empty) squares
                    if (puzzle[x,b].ValueC == '0')
                        continue;
                    if (puzzle[x,a].ValueC == puzzle[x,b].ValueC)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks the column of the x-y coordinate to see if any other squares in the column contain the same number.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="prospect">The prospect number.</param>
        /// <returns>True if the coordinate's column does not contain the prospect number. False if the column does contain the prospect number.</returns>
        private static bool checkcolumn(SudokuSquareGrid[,] puzzle, int x, int y, char prospect)
        {
            for (int num = 0; num < 9; num++)
            {
                if (x != num)
                    if (puzzle[num,y].ValueC == prospect)
                        return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the column of the y coordinate to see if there are two squares in the column that contain the same number.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>True if the column does not contain two of the same number. False if the column does contain two of the same number.</returns>
        public static bool checkcolumn(SudokuSquareGrid[,] puzzle, int y)
        {
            for (int a = 0; a < 9; a++)
            {
                //skip '0' (empty) squares
                if (puzzle[a,y].ValueC == '0')
                    continue;
                for (int b = a + 1; b < 9; b++)
                {
                    //skip '0' (empty) squares
                    if (puzzle[b,y].ValueC == '0')
                        continue;
                    if (puzzle[a,y].ValueC == puzzle[b,y].ValueC)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks the 9x9 square of the x-y coordinate to see if any other squares in the current 9x9 square contain the same number.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="prospect">The prospect number.</param>
        /// <returns>True if the coordinate's square does not contain the prospect number. False if the square does contain the prospect number.</returns>
        private static bool checksquare(SudokuSquareGrid[,] puzzle, int x, int y, char prospect)
        {
            if (x % 3 == 0 && y % 3 == 0) //we're at the top left of the square
            {
                //check squares
                return puzzle[x + 1,y + 1].ValueC != prospect &&
                        puzzle[x + 1,y + 2].ValueC != prospect &&
                        puzzle[x + 2,y + 1].ValueC != prospect &&
                        puzzle[x + 2,y + 2].ValueC != prospect;

            }
            else if (x % 3 == 0 && y % 3 == 1) //we're at the top middle of the square
            {
                //check squares
                return puzzle[x + 1,y - 1].ValueC != prospect &&
                        puzzle[x + 1,y + 1].ValueC != prospect &&
                        puzzle[x + 2,y - 1].ValueC != prospect &&
                        puzzle[x + 2,y + 1].ValueC != prospect;
            }
            else if (x % 3 == 0 && y % 3 == 2) //we're at the top right of the square
            {
                //check squares\
                return puzzle[x + 1,y - 2].ValueC != prospect &&
                        puzzle[x + 1,y - 1].ValueC != prospect &&
                        puzzle[x + 2,y - 2].ValueC != prospect &&
                        puzzle[x + 2,y - 1].ValueC != prospect;
            }
            else if (x % 3 == 1 && y % 3 == 0) //we're at the middle left of the square
            {
                //check squares 
                return puzzle[x - 1,y + 1].ValueC != prospect &&
                        puzzle[x - 1,y + 2].ValueC != prospect &&
                        puzzle[x + 1,y + 1].ValueC != prospect &&
                        puzzle[x + 1,y + 2].ValueC != prospect;
            }
            else if (x % 3 == 1 && y % 3 == 1) //we're at the middle middle of the square
            {
                //check squares 
                return puzzle[x - 1,y - 1].ValueC != prospect &&
                        puzzle[x - 1,y + 1].ValueC != prospect &&
                        puzzle[x + 1,y - 1].ValueC != prospect &&
                        puzzle[x + 1,y + 1].ValueC != prospect;
            }
            else if (x % 3 == 1 && y % 3 == 2) //we're at the middle right of the square
            {
                //check squares 
                return puzzle[x - 1,y - 2].ValueC != prospect &&
                        puzzle[x - 1,y - 1].ValueC != prospect &&
                        puzzle[x + 1,y - 2].ValueC != prospect &&
                        puzzle[x + 1,y - 1].ValueC != prospect;
            }
            else if (x % 3 == 2 && y % 3 == 0) //we're at the bottom left of the square
            {
                //check squares 
                return puzzle[x - 2,y + 1].ValueC != prospect &&
                        puzzle[x - 2,y + 2].ValueC != prospect &&
                        puzzle[x - 1,y + 1].ValueC != prospect &&
                        puzzle[x - 1,y + 2].ValueC != prospect;
            }
            else if (x % 3 == 2 && y % 3 == 1) //we're at the bottom middle of the square
            {
                //check squares 
                return puzzle[x - 2,y - 1].ValueC != prospect &&
                        puzzle[x - 2,y + 1].ValueC != prospect &&
                        puzzle[x - 1,y - 1].ValueC != prospect &&
                        puzzle[x - 1,y + 1].ValueC != prospect;
            }
            else if (x % 3 == 2 && y % 3 == 2) //we're at the bottom right of the square
            {
                //check squares 
                return puzzle[x - 2,y - 2].ValueC != prospect &&
                        puzzle[x - 2,y - 1].ValueC != prospect &&
                        puzzle[x - 1,y - 2].ValueC != prospect &&
                        puzzle[x - 1,y - 1].ValueC != prospect;
            }
            return false; //return false if no conditions above are met
        }

        /// <summary>
        /// Checks the 9x9 square of the x-y coordinate to see if there are two squares in the current 9x9 square that contain the same number.
        /// </summary>
        /// <param name="puzzle">The Sudoku puzzle to check.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>True if the coordinate's square does not contain two of the same number. False if the square does contain two of the same number.</returns>
        public static bool checksquare(SudokuSquareGrid[,] puzzle, int x, int y)
        {
            //first, convert the 9x9 square to a one dimensional array so that it's easier to check 
            SudokuSquareGrid[] grid = convertSquareToArray(puzzle, x, y);
            //then, use the created array to check for 2 of the same number 
            for (int a = 0; a < 9; a++)
            {
                //skip '0' (empty) squares
                if (grid[a].ValueC == '0')
                    continue;
                for (int b = a + 1; b < 9; b++)
                {
                    //skip '0' (empty) squares
                    if (grid[b].ValueC == '0')
                        continue;
                    if (grid[a].ValueC == grid[b].ValueC)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Takes the 3x3 square from the puzzle that contains the x-y coordinate and converts it to a 9 one-dimensional element array. The purpose is to make it easier to iterate over the elements of the 3x3 square.
        /// </summary>
        /// <param name="puzzle">The sudoku puzzle.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A SudokuSquareGrid array that contains all of the squares from the 3x3 square.</returns>
        public static SudokuSquareGrid[] convertSquareToArray(SudokuSquareGrid[,] puzzle, int x, int y)
        {
            SudokuSquareGrid[] grid = new SudokuSquareGrid[9];
            if (x % 3 == 0 && y % 3 == 0) //we're at the top left of the square
            {
                grid[0] = puzzle[x, y];
                grid[1] = puzzle[x, y + 1];
                grid[2] = puzzle[x, y + 2];
                grid[3] = puzzle[x + 1, y];
                grid[4] = puzzle[x + 1, y + 1];
                grid[5] = puzzle[x + 1, y + 2];
                grid[6] = puzzle[x + 2, y];
                grid[7] = puzzle[x + 2, y + 1];
                grid[8] = puzzle[x + 2, y + 2];
            }
            else if (x % 3 == 0 && y % 3 == 1) //we're at the top middle of the square
            {
                grid[0] = puzzle[x, y - 1];
                grid[1] = puzzle[x, y];
                grid[2] = puzzle[x, y + 1];
                grid[3] = puzzle[x + 1, y - 1];
                grid[4] = puzzle[x + 1, y];
                grid[5] = puzzle[x + 1, y + 1];
                grid[6] = puzzle[x + 2, y - 1];
                grid[7] = puzzle[x + 2, y];
                grid[8] = puzzle[x + 2, y + 1];
            }
            else if (x % 3 == 0 && y % 3 == 2) //we're at the top right of the square
            {
                grid[0] = puzzle[x, y - 2];
                grid[1] = puzzle[x, y - 1];
                grid[2] = puzzle[x, y];
                grid[3] = puzzle[x + 1, y - 2];
                grid[4] = puzzle[x + 1, y - 1];
                grid[5] = puzzle[x + 1, y];
                grid[6] = puzzle[x + 2, y - 2];
                grid[7] = puzzle[x + 2, y - 1];
                grid[8] = puzzle[x + 2, y];
            }
            else if (x % 3 == 1 && y % 3 == 0) //we're at the middle left of the square
            {
                grid[0] = puzzle[x - 1, y];
                grid[1] = puzzle[x - 1, y + 1];
                grid[2] = puzzle[x - 1, y + 2];
                grid[3] = puzzle[x, y];
                grid[4] = puzzle[x, y + 1];
                grid[5] = puzzle[x, y + 2];
                grid[6] = puzzle[x + 1, y];
                grid[7] = puzzle[x + 1, y + 1];
                grid[8] = puzzle[x + 1, y + 2];
            }
            else if (x % 3 == 1 && y % 3 == 1) //we're at the middle middle of the square
            {
                grid[0] = puzzle[x - 1, y - 1];
                grid[1] = puzzle[x - 1, y];
                grid[2] = puzzle[x - 1, y + 1];
                grid[3] = puzzle[x, y - 1];
                grid[4] = puzzle[x, y];
                grid[5] = puzzle[x, y + 1];
                grid[6] = puzzle[x + 1, y - 1];
                grid[7] = puzzle[x + 1, y];
                grid[8] = puzzle[x + 1, y + 1];
            }
            else if (x % 3 == 1 && y % 3 == 2) //we're at the middle right of the square
            {
                grid[0] = puzzle[x - 1, y - 2];
                grid[1] = puzzle[x - 1, y - 1];
                grid[2] = puzzle[x - 1, y];
                grid[3] = puzzle[x, y - 2];
                grid[4] = puzzle[x, y - 1];
                grid[5] = puzzle[x, y];
                grid[6] = puzzle[x + 1, y - 2];
                grid[7] = puzzle[x + 1, y - 1];
                grid[8] = puzzle[x + 1, y];
            }
            else if (x % 3 == 2 && y % 3 == 0) //we're at the bottom left of the square
            {
                grid[0] = puzzle[x - 2, y];
                grid[1] = puzzle[x - 2, y + 1];
                grid[2] = puzzle[x - 2, y + 2];
                grid[3] = puzzle[x - 1, y];
                grid[4] = puzzle[x - 1, y + 1];
                grid[5] = puzzle[x - 1, y + 2];
                grid[6] = puzzle[x, y];
                grid[7] = puzzle[x, y + 1];
                grid[8] = puzzle[x, y + 2];
            }
            else if (x % 3 == 2 && y % 3 == 1) //we're at the bottom middle of the square
            {
                grid[0] = puzzle[x - 2, y - 1];
                grid[1] = puzzle[x - 2, y];
                grid[2] = puzzle[x - 2, y + 1];
                grid[3] = puzzle[x - 1, y - 1];
                grid[4] = puzzle[x - 1, y];
                grid[5] = puzzle[x - 1, y + 1];
                grid[6] = puzzle[x, y - 1];
                grid[7] = puzzle[x, y];
                grid[8] = puzzle[x, y + 1];
            }
            else if (x % 3 == 2 && y % 3 == 2) //we're at the bottom right of the square
            {
                grid[0] = puzzle[x - 2, y - 2];
                grid[1] = puzzle[x - 2, y - 1];
                grid[2] = puzzle[x - 2, y];
                grid[3] = puzzle[x - 1, y - 2];
                grid[4] = puzzle[x - 1, y - 1];
                grid[5] = puzzle[x - 1, y];
                grid[6] = puzzle[x, y - 2];
                grid[7] = puzzle[x, y - 1];
                grid[8] = puzzle[x, y];
            }

            return grid;
        }

        /// <summary>
        /// Prints the puzzle to the console.
        /// </summary>
        /// <param name="puzzle">The puzzle to print.</param>
        public static void printPuzzle(SudokuSquareGrid[,] puzzle)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    Console.Write(puzzle[x, y].ValueC);
                }
                Console.WriteLine();
            }
        }
    }

}
