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
    /// Interaction logic for SelectPuzzleWindow.xaml
    /// </summary>
    public partial class SelectPuzzleWindow : Window
    {
        private String selectedPuzzle;
        private String[] puzzles;
        public string SelectedPuzzle { get => selectedPuzzle; }

        public SelectPuzzleWindow()
        {
            InitializeComponent();
        }

        public SelectPuzzleWindow(int difficulty)
        {
            InitializeComponent();
            fillComboBox(difficulty);
        }


        private void fillComboBox(int difficulty)
        {
            //int puzzNum = 2; // TODO either select a number randomly or let the player select one or increment by one every time
            String directory = "..\\..\\Puzzles\\";
            //String filename = "Puzzle" + puzzNum + ".txt";
            String difficultyS = "";
            switch (difficulty)
            {
                case (0):
                    difficultyS = "Easy\\";
                    break;
                case (1):
                    difficultyS = "Medium\\";
                    break;
                case (2):
                    difficultyS = "Hard\\";
                    break;
            }
            puzzles = Directory.GetFiles(directory + difficultyS);
            foreach (String puzzle in puzzles)
            {
                //only add .txt files to the combo box 
                PuzzleSelectComboBox.Items.Add(puzzle.Split('\\')[4]);
            }
        }

        private void OnOkClickEvent(object sender, RoutedEventArgs e)
        {
            if (PuzzleSelectComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a puzzle");
            }
            else if (!puzzles[PuzzleSelectComboBox.SelectedIndex].EndsWith(".txt"))
            {
                MessageBox.Show("Invalid file; can only read from .txt files.");
            }
            else
            {
                selectedPuzzle = puzzles[PuzzleSelectComboBox.SelectedIndex];// PuzzleSelectComboBox.SelectedItem.ToString();
                this.Close();
            }
        }

        private void OnCancelClickEvent(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
