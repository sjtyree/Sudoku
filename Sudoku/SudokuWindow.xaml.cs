using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class SudokuWindow : Window
    {
        public static readonly int EASY = 0;
        public static readonly int MEDIUM = 1;
        public static readonly int HARD = 2;
        private int difficulty;
        private String puzzleName;
        private bool dontOpenScreen = false;
        private bool puzzleCreated = false; 
        private SudokuSquareGrid[,] mainGrid;
        private SudokuSquareGrid[,] completePuzzle;

        public SudokuWindow()
        {
            //InitializeComponent();
        }

        public SudokuWindow(int difficulty, String puzzle)
        {
            this.difficulty = difficulty;
            this.puzzleName = puzzle;
            //InitializeComponent();

            initializeWindow();
            createPuzzle();
        }

        private void initializeWindow()
        {
            Grid gr = new Grid();
            gr.HorizontalAlignment = HorizontalAlignment.Stretch;
            gr.VerticalAlignment = VerticalAlignment.Stretch;
            //gr.ShowGridLines = true;
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            ColumnDefinition colDef6 = new ColumnDefinition();
            ColumnDefinition colDef7 = new ColumnDefinition();
            ColumnDefinition colDef8 = new ColumnDefinition();
            ColumnDefinition colDef9 = new ColumnDefinition();
            gr.ColumnDefinitions.Add(colDef1);
            gr.ColumnDefinitions.Add(colDef2);
            gr.ColumnDefinitions.Add(colDef3);
            gr.ColumnDefinitions.Add(colDef4);
            gr.ColumnDefinitions.Add(colDef5);
            gr.ColumnDefinitions.Add(colDef6);
            gr.ColumnDefinitions.Add(colDef7);
            gr.ColumnDefinitions.Add(colDef8);
            gr.ColumnDefinitions.Add(colDef9);
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            RowDefinition rowDef6 = new RowDefinition();
            RowDefinition rowDef7 = new RowDefinition();
            RowDefinition rowDef8 = new RowDefinition();
            RowDefinition rowDef9 = new RowDefinition();
            gr.RowDefinitions.Add(rowDef1);
            gr.RowDefinitions.Add(rowDef2);
            gr.RowDefinitions.Add(rowDef3);
            gr.RowDefinitions.Add(rowDef4);
            gr.RowDefinitions.Add(rowDef5);
            gr.RowDefinitions.Add(rowDef6);
            gr.RowDefinitions.Add(rowDef7);
            gr.RowDefinitions.Add(rowDef8);
            gr.RowDefinitions.Add(rowDef9);
            mainGrid = new SudokuSquareGrid[9, 9];
            
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    mainGrid[x, y] = new SudokuSquareGrid(x, y);
                    mainGrid[x, y].Name = "square" + x + y;
                    //mainGrid[x, y].KeyDown += SudokuWindow_KeyDown;
                    mainGrid[x, y].ValueChanged += SudokuWindow_valueChanged;
                    Grid.SetRow(mainGrid[x, y], x);
                    Grid.SetColumn(mainGrid[x, y], y);
                    gr.Children.Add(mainGrid[x, y]);
                }
            }
            setBorders(gr);
            this.Content = gr;
            this.WindowState = WindowState.Maximized;
            this.Title = "Sudoku";
        }

        private void createPuzzle()
        {
            bool solvedPuzzle = false;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(puzzleName);
                if (lines.Length != 9)
                    throw new Exception("Incorrect number of rows. Number of rows must be 9.");
                int x = 0;
                foreach (String Line in lines)
                {
                    if (Line.Length != 9)
                        throw new Exception("Incorrect number of columns. Number of columns must be 9.");
                    for (int y = 0; y < Line.Length; y++)
                    {
                        char c = Line[y];
                        if (!Utility.checkPossibleReadValues(c))
                            throw new Exception("Invalid value: " + c + ". Only the numbers 0-9 are allowed.");
                        if (c != '0')
                        {
                            SudokuSquareGrid square = mainGrid[x, y];
                            square.FixedNumB = true;
                            square.ActualValueC = c;
                            square.ValueC = c;
                            square.X = x;
                            square.Y = y;
                        }
                    }
                    x++;
                }
                SudokuSolver.printPuzzle(mainGrid);
                completePuzzle = Utility.cloneGrid(mainGrid);
                solvedPuzzle = SudokuSolver.solvePuzzle(completePuzzle);
                if (solvedPuzzle)
                {
                    SudokuSolver.printPuzzle(completePuzzle);
                    puzzleCreated = true;
                }
                else
                {
                    throw new Exception("Puzzle is unsolvable.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Cannot continue for the following reason: " + e.Message +" Please try selecting a different Puzzle.");
                dontOpenScreen = true;
                puzzleCreated = false;
            }
        }

        /// <summary>
        /// Event that is called anytime a value in one of the squares is modified. It first sets any incorrect rows, columns, or squares to red, then checks to see if the puzzle has been complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SudokuWindow_valueChanged(object sender, EventArgs e)
        {
            if (!puzzleCreated) //don't do anything if the puzzle hasn't been created yet
                return;
            SudokuSquareGrid changedSquare = (SudokuSquareGrid)sender;
            //check row, column, square
            if (SudokuSolver.checkrow(mainGrid, changedSquare.X))
                setRowColor(changedSquare.X, Brushes.White, false);
            else
                setRowColor(changedSquare.X, Brushes.Red, true);

            if (SudokuSolver.checkcolumn(mainGrid, changedSquare.Y))
                setColumnColor(changedSquare.Y, Brushes.White, false);
            else
                setColumnColor(changedSquare.Y, Brushes.Red, true);

            if (SudokuSolver.checksquare(mainGrid, changedSquare.X, changedSquare.Y))
                setSquareColor(changedSquare.X, changedSquare.Y, Brushes.White, false);
            else
                setSquareColor(changedSquare.X, changedSquare.Y, Brushes.Red, true);


            //check if puzzle is complete
            if (puzzleComplete())
            {
                MessageBox.Show("Puzzle Complete!\nGreat job!", "Complete!");
                try
                {
                    //rename the puzzle filename to mark it as complete
                    if (!puzzleName.Contains("_COMPLETE"))
                        System.IO.File.Move(puzzleName, puzzleName.Substring(0, puzzleName.Length - 4) + "_COMPLETE" + puzzleName.Substring(puzzleName.Length - 4));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not rename puzzle file for the following reason: " + ex.Message);
                }
                this.Close();
            }
        }

        /// <summary>
        /// Compares the values of the main puzzle grid to the values of the calculated complete puzzle grid to see if the puzzle has successfully been completed.
        /// </summary>
        /// <returns>True if the puzzle is complete, false if it is not.</returns>
        private bool puzzleComplete()
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (mainGrid[x, y].ValueC != completePuzzle[x, y].ValueC)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// creates the thick borders between each 3x3 square of the puzzle.
        /// </summary>
        /// <param name="gr">The grid that contains the sudoku puzzle.</param>
        private void setBorders(Grid gr)
        {
            Border b; 
            
            for (int x = 0; x < 9; x += 3)
            {
                for (int y = 0; y < 9; y += 3)
                {
                    b = new Border();
                    b.BorderBrush = Brushes.Black;
                    b.BorderThickness = new Thickness(3);
                    Grid.SetRowSpan(b, 3);
                    Grid.SetColumnSpan(b, 3);
                    Grid.SetRow(b, x);
                    Grid.SetColumn(b, y);
                    gr.Children.Add(b);
                }
            }
        }

        /// <summary>
        /// Overwritten Show method from base Window class. Does not open the window if there was an issue loading the puzzle (invalid puzzle, puzzle is unsolvable, etc)
        /// </summary>
        public new void Show()
        {
            if (!dontOpenScreen)
            {
                base.Show();
            }
        }

        /// <summary>
        /// Sets the color of row x.
        /// </summary>
        /// <param name="x">The row number of the puzzle.</param>
        /// <param name="color">The color to set the row to.</param>
        /// <param name="redRow">True if we are setting this row to red, false otherwise.</param>
        private void setRowColor(int x, Brush color, bool redRow)
        {
            if (x < 0 || x > 8)
                return;
            for (int y = 0; y < 9; y++)
            {
                SudokuSquareGrid row = mainGrid[x, y];
                row.IsRowRed = redRow;
                if (!row.IsColumnRed && !row.IsSquareRed)
                    row.setColor(color);
            }
        }

        /// <summary>
        /// Sets the color of column y.
        /// </summary>
        /// <param name="y">The column number of the puzzle.</param>
        /// <param name="color">The color to set the column to.</param>
        /// <param name="redColumn">True if we are setting this column to red, false otherwise.</param>
        private void setColumnColor(int y, Brush color, bool redColumn)
        {
            if (y < 0 || y > 8)
                return;
            for (int x = 0; x < 9; x++)
            {
                SudokuSquareGrid column = mainGrid[x, y];
                column.IsColumnRed = redColumn;
                if (!column.IsRowRed && !column.IsSquareRed)
                    column.setColor(color);
            }
        }

        /// <summary>
        /// Sets the color of the 3x3 square that contains the x-y coordinate.
        /// </summary>
        /// <param name="x">X coordinate of a square that is contained in the 3x3 square we want to color.</param>
        /// <param name="y">Y coordinate of a square that is contained in the 3x3 square we want to color.</param>
        /// <param name="color">The color to set the 3x3 square to.</param>
        /// <param name="redSquare">True if we are setting this 3x3 square to red, false otherwise.</param>
        private void setSquareColor(int x, int y, Brush color, bool redSquare)
        {
            if (y < 0 || y > 8 || x < 0 || x > 8)
                return;
            SudokuSquareGrid[] tempGrid = SudokuSolver.convertSquareToArray(mainGrid, x, y);
            for (int a = 0; a < tempGrid.Length; a++)
            {
                SudokuSquareGrid square = tempGrid[a];
                square.IsSquareRed = redSquare;
                if (!square.IsRowRed && !square.IsColumnRed)
                    square.setColor(color);
            }
        }
    }
}
