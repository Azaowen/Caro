using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_co_caro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int cellNums = 0;

        private int currentPlayer = 1;

        private int maxLengthLine;

        Player player1 = new Player("X", Image.FromFile(Application.StartupPath + @"\Resources\X.png"));
        Player player2 = new Player("O", Image.FromFile(Application.StartupPath + @"\Resources\O.png"));
        int[,] process;


        #region FORM LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
            pnLinh.Size = new Size(others.broadSize, others.broadSize);// new Size for board
            int newX = (panel3.Width - pnLinh.Width) / 2;
            int newY = (panel3.Height - pnLinh.Height) / 2;
            pnLinh.Location = new Point(newX, newY);// set location to the center relative to its parents
            process = new int[cellNums, cellNums];// set size for array
            maxLengthLine = cellNums;
            makeCells(); // start creating cell on it
        }
        #endregion

        


        public void makeCells()
        {
            Button pve = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < cellNums; i++)
            {
                for (int j = 0; j < cellNums; j++)
                {
                    Button button = new Button()
                    {
                        Width = others.cellSize,
                        Height = others.cellSize,
                        Location = new Point(pve.Location.X + pve.Width, pve.Location.Y),
                        BackgroundImage = Image.FromFile(Application.StartupPath + @"\Resources\none.png"),
                        BackgroundImageLayout = ImageLayout.Stretch
                    };
                    button.Tag = new Point(i, j); // save location of button
                    button.Click += button_Click;
                    pnLinh.Controls.Add(button);
                    pve = button;
                }
                pve = new Button()
                {
                    Width = 0,
                    Location = new Point(0, pve.Location.Y + others.cellSize)
                };
            }

        }

        #region EVENT OF BUTTON (Player 1)
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button; // Casting

            Image currentImage = btn.BackgroundImage;// Create an image to assign image from current button

            bool hasImage = currentImage == player1.Avatar || currentImage == player2.Avatar;// Check if that button contains X or O pic

            if (hasImage) return;// cancel if button do have X or O pic

            // otherwise, change image of it
            if(currentPlayer==1)
                btn.BackgroundImage = player1.Avatar;
            else btn.BackgroundImage = player2.Avatar;

            Point cellPosition = (Point)btn.Tag; // get location from tag
            UpdateArray(cellPosition.X, cellPosition.Y);// update array
        }
        #endregion

        #region UPDATE ARRAY WHEN SELECTED
        private void UpdateArray(int row, int column)
        {
            process[row, column] = currentPlayer == 1 ? 1 : 2 ;// change value
            if(winner(row, column))
            {
                MessageBox.Show($"{currentPlayer} won");
                pnLinh.Enabled = false; // stop playing when the game is completed
            }else if (tie())
            {
                MessageBox.Show("Tie");
            }
            changeUnderline(); // change under to the next player
        }
        #endregion

        #region UNDERLINE BELOW AVATAR
        private void changeUnderline()
        {
            if (currentPlayer == 1)
            {
                label1.BackColor = Color.Red; // underline 
                label2.BackColor = Color.Empty;// clear other underline
                currentPlayer = 2;// reset player
            }
            else
            {
                label2.BackColor = Color.Red;
                label1.BackColor = Color.Empty;
                currentPlayer = 1;
            }
        }
        #endregion

        #region HANDLE WINNER
        private bool winner(int row, int column)
        {
            int sum = 0; // sum 
            int columnAlpha, rowAlpha; // clone

            #region HORIZONTAL
            columnAlpha = column - maxLengthLine; // starting at the current location from left to right with length limited
            while (true)
            {
                if (columnAlpha < 0) 
                {
                    columnAlpha = 0; 
                }
                else if (columnAlpha > process.GetLength(1) - 1)
                {
                    break; // stop checking if it goes to the end of the line (in the Right)
                }
                else if (process[row, columnAlpha] != currentPlayer)
                {
                    sum = 0; columnAlpha++; // reset SUM if it see anything not of current player, Including empty cell
                }
                else
                {
                    sum += 1; columnAlpha++; 
                }
                if (sum == maxLengthLine)
                    return true; // whenever it equal to set length limited. Show Winner
            }
            #endregion

            #region VERTICAL
            rowAlpha = row - maxLengthLine;
            sum = 0;
            while (true)
            {
                if (rowAlpha < 0)
                {
                    rowAlpha++;
                }
                else if (rowAlpha > process.GetLength(0) - 1)
                {
                    break;
                }
                else if (process[rowAlpha, column] != currentPlayer)
                {
                    sum = 0; rowAlpha++;
                }
                else
                {
                    sum += 1; rowAlpha++;
                }
                if (sum == maxLengthLine)
                    return true;
            }
            #endregion

            #region MAIN DIAGONAL
            sum = 0;
            columnAlpha = column - maxLengthLine;
            rowAlpha = row - maxLengthLine;
            while (true)
            {
                if (rowAlpha < 0 || columnAlpha<0)
                {
                    rowAlpha++;
                    columnAlpha++;
                }
                else if (rowAlpha > process.GetLength(0) - 1 || columnAlpha > process.GetLength(1) - 1)
                {
                    break;
                }
                else if (process[rowAlpha, columnAlpha] != currentPlayer)
                {
                    sum = 0; 
                    rowAlpha++;
                    columnAlpha++;
                }
                else
                {
                    sum += 1;
                    rowAlpha++;
                    columnAlpha++;
                }
                if (sum == maxLengthLine)
                    return true;
            }
            #endregion

            #region SECONDARY DIAGONAL
            columnAlpha = column + maxLengthLine;
            rowAlpha = row - maxLengthLine;
            sum = 0;
            while (true)
            {
                if (rowAlpha < 0 || columnAlpha > process.GetLength(1) - 1)
                {
                    rowAlpha++;
                    columnAlpha--;
                }
                else if (rowAlpha > process.GetLength(0) - 1 || columnAlpha < 0)
                {
                    break;
                }
                else if (process[rowAlpha, columnAlpha] != currentPlayer)
                {
                    sum = 0;
                    rowAlpha++;
                    columnAlpha--;
                }
                else
                {
                    sum += 1;
                    rowAlpha++;
                    columnAlpha--;
                }
                if (sum == maxLengthLine)
                    return true;
                
            }
            #endregion
            
            return false;
        }
        #endregion

        #region HANDLE TIE
        private bool tie()
        {
            foreach (int a in process)
                if (a == 0)
                    return false;
            return true;
        }
        #endregion

        #region PICTURE BOX, IMAGES AND ANIMATION
        private void timer1_Tick(object sender, EventArgs e)
        {
            movecoud(1);

        }
        void movecoud(int x)
        {
            if (pictureBox1.Left >= Width + pictureBox1.Width)
            {
                pictureBox1.Left = -pictureBox1.Width;
            }
            else
            {
                pictureBox1.Left += x;
            }

            if (pictureBox6.Left >= Width + pictureBox6.Width)
            {
                pictureBox6.Left = -pictureBox6.Width;
            }
            else
            {
                pictureBox6.Left += x;
            }


            if (pictureBox2.Left >= Width + pictureBox2.Width)
            {
                pictureBox2.Left = -pictureBox2.Width;
            }
            else
            {
                pictureBox2.Left += x;
            }
            panel1.Invalidate();
        }

        #endregion
            
        #region CLOSE WINDOW
        private void btnReply_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if Home form is already opened, then open it
            Home homeForm = Application.OpenForms.OfType<Home>().FirstOrDefault();
            if (homeForm != null)
            {
                homeForm.Show();
            }else
            {
                DialogResult r =  MessageBox.Show(
                    "Chương Trình có vẻ đang chạy lỗi! Vui lòng khởi động lại","Thông Báo",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning
                    );
                if (r == DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion


    }
}
