using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cross_word_puzzle
{
    public partial class Form1 : Form
    {
        private List<TextBox> textBoxes;
        private string[,] grid;
        private int gridSize = 15;  // Updated grid size to 15x15
        private ListBox wordListBox;

        private readonly Dictionary<int, List<string>> words = new Dictionary<int, List<string>>()
        {
            { 2, new List<string> { "su", "on" } },
            { 3, new List<string> { "bir", "iki", "üç" } },
            { 4, new List<string> { "ters", "kulp" } },
            { 5, new List<string> { "kitap" } },
            { 6, new List<string> { "okumak", "konsol" } },
            { 7, new List<string> { "karamel" } },
            { 8, new List<string> { "kitaplýk" } }
        };

        public Form1()
        {
            InitializeComponent();
            InitializeCrosswordForm();
        }

        private void InitializeCrosswordForm()
        {
            this.textBoxes = new List<TextBox>();
            this.grid = new string[gridSize, gridSize];

            Label label = new Label();
            label.Text = "Crossword Puzzle";
            label.Location = new Point(10, 10);
            this.Controls.Add(label);

            Button createButton = new Button();
            createButton.Text = "Create Puzzle";
            createButton.Location = new Point(10, 35);
            createButton.Click += (sender, e) => CreatePuzzle();
            this.Controls.Add(createButton);

            // Create a ListBox to display the words
            wordListBox = new ListBox();
            wordListBox.Location = new Point(500, 10);  // Adjusted for larger grid
            wordListBox.Size = new Size(100, 200);
            this.Controls.Add(wordListBox);

            // Populate the ListBox with the words
            foreach (var wordList in words)
            {
                foreach (var word in wordList.Value)
                {
                    wordListBox.Items.Add(word);
                }
            }
            Button checkButton = new Button();
            checkButton.Text = "Check Words";
            checkButton.Location = new Point(10, 60); // Konumu ayarlayýn
            checkButton.Click += CheckButton_Click; // Týklama olayýný ele alýn
            this.Controls.Add(checkButton); // Butonu forma ekleyin
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            CheckWordValidity();
        }

        private void CheckWordValidity()
        {
            foreach (var textBox in textBoxes)
            {
                var point = (Point)textBox.Tag;
                if (!string.IsNullOrEmpty(grid[point.X, point.Y]) && textBox.Text.ToLower() == grid[point.X, point.Y])
                {
                    textBox.BackColor = Color.Green; // Eðer kelime doðruysa arka plan rengini yeþil yapýn
                }
            }
        }

        private void CreatePuzzle()
        {
            // Clear existing text boxes and grid
            foreach (var textBox in textBoxes)
            {
                this.Controls.Remove(textBox);
            }
            textBoxes.Clear();
            Array.Clear(grid, 0, grid.Length);

            int startX = 10;
            int startY = 70;
            int textBoxSize = 30;

            // Create the grid with TextBoxes
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Width = textBoxSize;
                    textBox.Height = textBoxSize;
                    textBox.Location = new Point(startX + j * textBoxSize, startY + i * textBoxSize);
                    textBox.TextAlign = HorizontalAlignment.Center;
                    textBox.Font = new Font("Arial", 14);
                    textBox.ReadOnly = true;
                    textBox.BackColor = Color.White;
                    textBox.Tag = new Point(i, j);
                    this.Controls.Add(textBox);
                    textBoxes.Add(textBox);
                }
            }

            // Place words on the grid
            foreach (var wordList in words)
            {
                foreach (var word in wordList.Value)
                {
                    PlaceWord(word);
                }
            }

            // Update TextBoxes with the letters
            foreach (var textBox in textBoxes)
            {
                var point = (Point)textBox.Tag;
                if (!string.IsNullOrEmpty(grid[point.X, point.Y]))
                {
                    textBox.Text = grid[point.X, point.Y];
                }
            }
        }
        

        private void PlaceWord(string word)
        {
            Random rand = new Random();
            bool placed = false;

            while (!placed)
            {
                int row = rand.Next(gridSize);
                int col = rand.Next(gridSize);
                bool horizontal = rand.Next(2) == 0;

                if (horizontal)
                {
                    if (col + word.Length <= gridSize && CanPlaceWord(word, row, col, horizontal))
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            grid[row, col + i] = word[i].ToString();
                        }
                        placed = true;
                    }
                }
                else
                {
                    if (row + word.Length <= gridSize && CanPlaceWord(word, row, col, horizontal))
                    {
                        for (int i = 0; i < word.Length; i++)
                        {
                            grid[row + i, col] = word[i].ToString();
                        }
                        placed = true;
                    }
                }
            }
        }

        private bool CanPlaceWord(string word, int row, int col, bool horizontal)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (horizontal)
                {
                    if (!string.IsNullOrEmpty(grid[row, col + i]) && grid[row, col + i] != word[i].ToString())
                    {
                        return false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(grid[row + i, col]) && grid[row + i, col] != word[i].ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
