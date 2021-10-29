using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            SelectPuzzleWindow selectPuzzleWindow;
            SudokuWindow sudokuWindow;
            int difficulty = SudokuWindow.EASY;//set to easy by default
            Button b = (Button)sender;
            String btnName = b.Name;

            if (btnName.Equals(EasyButton.Name))
            {
                difficulty = SudokuWindow.EASY;
            }
            if (btnName.Equals(MediumButton.Name))
            {
                difficulty = SudokuWindow.MEDIUM;
            }
            if (btnName.Equals(HardButton.Name))
            {
                difficulty = SudokuWindow.HARD;
            }

            selectPuzzleWindow = new SelectPuzzleWindow(difficulty);
            selectPuzzleWindow.ShowDialog();
            String puzzle = selectPuzzleWindow.SelectedPuzzle;
            if (puzzle != null)
            {
                sudokuWindow = new SudokuWindow(difficulty, puzzle);
                sudokuWindow.Show();
            }
        }
    }
}
