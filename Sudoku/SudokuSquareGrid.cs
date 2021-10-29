using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sudoku
{
    class SudokuSquareGrid : System.Windows.Controls.Grid
    {
        private static readonly String INPUT_ERROR = "Invalid value";
        Grid sudokuGrid;

        TextBox valueTB; 

        TextBox possValNW;
        TextBox possValN;
        TextBox possValNE;
        TextBox possValW;
        TextBox possValE;
        TextBox possValSW;
        TextBox possValS;
        TextBox possValSE;

        public event EventHandler ValueChanged;

        private bool fixedNumB;
        private bool isRowRed;
        private bool isColumnRed;
        private bool isSquareRed;
        private char actualValueC;
        private char valueC;
        private int x;
        private int y;

        public bool FixedNumB { get => fixedNumB; set { fixedNumB = value; enableDisableEdit(); } }
        public bool IsRowRed { get => isRowRed; set => isRowRed = value; }
        public bool IsColumnRed { get => isColumnRed; set => isColumnRed = value; }
        public bool IsSquareRed { get => isSquareRed; set => isSquareRed = value; }
        public char ActualValueC { get => actualValueC; set { actualValueC = value; valueC = value; valueTB.Text = value.ToString(); } }
        public char ValueC { get => valueC; set => valueC = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public SudokuSquareGrid(int x, int y)
        {
            this.valueC = '0';
            this.fixedNumB = false;
            isRowRed = false;
            isColumnRed = false;
            isSquareRed = false;
            this.x = x;
            this.y = y;
            initializeGrid();
        }

        public SudokuSquareGrid(char value, Boolean fixedNum, int x, int y)
        {
            this.valueC = value;
            this.fixedNumB = fixedNum;
            isRowRed = false;
            isColumnRed = false;
            isSquareRed = false;
            this.x = x;
            this.y = y;
            initializeGrid();
        }

        private void initializeGrid()
        {
            sudokuGrid = new Grid();

            Border b = new Border();
            b.BorderBrush = Brushes.Black;
            b.BorderThickness = new Thickness(1);
            Grid.SetRowSpan(b, 3);
            Grid.SetColumnSpan(b, 3);
            sudokuGrid.Children.Add(b);

            ColumnDefinition colDef1 = new ColumnDefinition();
            colDef1.Width = new GridLength(0.2, GridUnitType.Star);
            ColumnDefinition colDef2 = new ColumnDefinition();
            colDef2.Width = new GridLength(0.6, GridUnitType.Star);
            ColumnDefinition colDef3 = new ColumnDefinition();
            colDef3.Width = new GridLength(0.2, GridUnitType.Star);
            sudokuGrid.ColumnDefinitions.Add(colDef1);
            sudokuGrid.ColumnDefinitions.Add(colDef2);
            sudokuGrid.ColumnDefinitions.Add(colDef3);

            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(0.2, GridUnitType.Star);
            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(0.6, GridUnitType.Star);
            RowDefinition rowDef3 = new RowDefinition();
            rowDef3.Height = new GridLength(0.2, GridUnitType.Star);
            sudokuGrid.RowDefinitions.Add(rowDef1);
            sudokuGrid.RowDefinitions.Add(rowDef2);
            sudokuGrid.RowDefinitions.Add(rowDef3);

            possValNW = createPossValueTextBox(possValNW);
            Grid.SetColumn(possValNW, 0);
            Grid.SetRow(possValNW, 0);
            sudokuGrid.Children.Add(possValNW);

            possValN = createPossValueTextBox(possValN);
            Grid.SetColumn(possValN, 1);
            Grid.SetRow(possValN, 0);
            sudokuGrid.Children.Add(possValN);

            possValNE = createPossValueTextBox(possValNE);
            Grid.SetColumn(possValNE, 2);
            Grid.SetRow(possValNE, 0);
            sudokuGrid.Children.Add(possValNE);

            possValW = createPossValueTextBox(possValW);
            Grid.SetColumn(possValW, 0);
            Grid.SetRow(possValW, 1);
            sudokuGrid.Children.Add(possValW);

            valueTB = createValueTextBox(valueTB);
            Grid.SetColumn(valueTB, 1);
            Grid.SetRow(valueTB, 1);
            sudokuGrid.Children.Add(valueTB);

            possValE = createPossValueTextBox(possValE);
            Grid.SetColumn(possValE, 2);
            Grid.SetRow(possValE, 1);
            sudokuGrid.Children.Add(possValE);

            possValSW = createPossValueTextBox(possValSW);
            Grid.SetColumn(possValSW, 0);
            Grid.SetRow(possValSW, 2);
            sudokuGrid.Children.Add(possValSW);

            possValS = createPossValueTextBox(possValS);
            Grid.SetColumn(possValS, 1);
            Grid.SetRow(possValS, 2);
            sudokuGrid.Children.Add(possValS);

            possValSE = createPossValueTextBox(possValSE);
            Grid.SetColumn(possValSE, 2);
            Grid.SetRow(possValSE, 2);
            sudokuGrid.Children.Add(possValSE);

            sudokuGrid.GotFocus += TBGotFocusEvent;
            sudokuGrid.LostFocus += TBLostFocusEvent;

            this.Children.Add(sudokuGrid);
            
        }

        private TextBox createPossValueTextBox(TextBox tb)
        {
            tb = new TextBox();
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontSize = 12;
            tb.MaxLength = 1;
            tb.PreviewTextInput += TextBoxPreviewTextEvent;
            tb.TextChanged += TextBoxTextChangedEvent;
            tb.BorderBrush = null;
            tb.TextAlignment = TextAlignment.Center;
            tb.IsEnabled = false;
            return tb;
        }

        private TextBox createValueTextBox(TextBox tb)
        {
            tb = new TextBox();
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.Text = "";
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontSize = 36;
            tb.MaxLength = 1;
            tb.PreviewTextInput += TextBoxPreviewTextEvent;
            tb.TextChanged += TextBoxTextChangedEvent;
            tb.BorderBrush = null;
            tb.TextAlignment = TextAlignment.Center;

            return tb;
        }

        /// <summary>
        /// Enables or disables editing on all textfields, depending on the value of FixedNumB. 
        /// This is to ensure that the user doesn't change the number of a square that was part of the original puzzle. 
        /// </summary>
        private void enableDisableEdit()
        {
            if (fixedNumB)
            {
                valueTB.IsEnabled = false;

                //possValNW.IsEnabled = false;
                //possValN.IsEnabled = false;
                //possValNE.IsEnabled = false;
                //possValW.IsEnabled = false;
                //possValE.IsEnabled = false;
                //possValSW.IsEnabled = false;
                //possValS.IsEnabled = false;
                //possValSE.IsEnabled = false;
            }
            else
            {
                valueTB.IsEnabled = true;

                //possValNW.IsEnabled = true;
                //possValN.IsEnabled = true;
                //possValNE.IsEnabled = true;
                //possValW.IsEnabled = true;
                //possValE.IsEnabled = true;
                //possValSW.IsEnabled = true;
                //possValS.IsEnabled = true;
                //possValSE.IsEnabled = true;
            }
        }

        internal void setColor(Brush color)
        {
            sudokuGrid.Background = color;
            possValNW.Background = color;
            possValN.Background = color;
            possValNE.Background = color;
            possValW.Background = color;
            valueTB.Background = color;
            possValE.Background = color;
            possValSW.Background = color;
            possValS.Background = color;
            possValSE.Background = color;
        }

        private void TBGotFocusEvent(object sender, RoutedEventArgs e)
        {
            possValNW.IsEnabled = true;
            possValN.IsEnabled = true;
            possValNE.IsEnabled = true;
            possValW.IsEnabled = true;
            possValE.IsEnabled = true;
            possValSW.IsEnabled = true;
            possValS.IsEnabled = true;
            possValSE.IsEnabled = true;
        }

        private void TBLostFocusEvent(object sender, RoutedEventArgs e)
        {
            possValNW.IsEnabled = false;
            possValN.IsEnabled = false;
            possValNE.IsEnabled = false;
            possValW.IsEnabled = false;
            possValE.IsEnabled = false;
            possValSW.IsEnabled = false;
            possValS.IsEnabled = false;
            possValSE.IsEnabled = false;
        }

        private void TextBoxPreviewTextEvent(object sender, TextCompositionEventArgs e)
        {
            if (!Utility.checkPossibleEnterValues(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void TextBoxTextChangedEvent(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
                if (Utility.checkPossibleEnterValues(tb.Text[0]))
                {
                    valueC = tb.Text[0];
                    //call value changed event
                    ValueChanged?.Invoke(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show(INPUT_ERROR);
                    tb.Text = "";
                }
            }
            else
            {
                valueC = '0';
                //call the value changed event 
                ValueChanged?.Invoke(this, new EventArgs());
            }

        }
    }
}
