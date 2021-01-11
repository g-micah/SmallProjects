using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class FormMain : Form
    {
        private bool vsAI;
        private bool xTurn;
        private Button[] board;
        private bool[] enabled;

        public FormMain()
        {
            InitializeComponent();
            board = new Button[] { button1, button2, button3, button4, button5, button6, button7, button8, button9 };
            enabled = new bool[] { true, true, true, true, true, true, true, true, true };
            VsAI = true;
            ResetGame();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void ResetGame()
        {
            XTurn = true;
            for (int i = 0; i < 9; i++)
            {
                board[i].Text = "";
                board[i].Cursor = Cursors.Hand;
                board[i].BackColor = Color.Black;
                enabled[i] = true;
            }
        }

        public bool VsAI
        {
            set
            {
                if (value && value != vsAI)
                {
                    ResetGame();
                    vsAI = true;
                    playerVsPlayerToolStripMenuItem.Checked = false;
                    playerVsAIToolStripMenuItem.Checked = true;
                    labelMode.Text = "Player vs AI";
                }
                else if (!value && value != vsAI)
                {
                    ResetGame();
                    vsAI = false;
                    playerVsPlayerToolStripMenuItem.Checked = true;
                    playerVsAIToolStripMenuItem.Checked = false;
                    labelMode.Text = "Player vs Player";
                }
            }
            get
            {
                return vsAI;
            }
        }

        public bool XTurn
        {
            set
            {
                if (value)
                {
                    xTurn = true;
                    labelTurn.Text = "X's Turn";
                }
                else
                {
                    xTurn = false;
                    labelTurn.Text = "O's Turn";
                }
            }
            get
            {
                return xTurn;
            }
        }

        private void PlayTurn(int btnInput)
        {
            int btn = btnInput-1;
            if (enabled[btn])
            {
                if (XTurn)
                {
                    board[btn].Text = "X";
                }
                else
                {
                    board[btn].Text = "O";
                }
                XTurn = !XTurn;

                board[btn].Cursor = Cursors.Default;
                enabled[btn] = false;
            }
            if (HasGameEnded())
            {
                if(XTurn)
                {
                    if (VsAI)
                    {
                        labelTurn.Text = "You lost! Better luck next time.";
                    }
                    else
                    {
                        labelTurn.Text = "O has won the game!";
                    }
                }
                else
                {
                    if (VsAI)
                    {
                        labelTurn.Text = "You Won! Congratulations.";
                    }
                    else
                    {
                        labelTurn.Text = "X has won the game!";
                    }
                }
                for (int i = 0; i < 9; i++)
                {
                    board[i].Cursor = Cursors.Default;
                    enabled[i] = false;
                }
            }
            else if (IsGameDraw())
            {
                labelTurn.Text = "Game is a draw!";
            }
            else if (VsAI && !XTurn)
            {
                labelTurn.Focus();
                Application.UseWaitCursor = true;
                labelTurn.Text = "O's Turn .";
                Wait(300);
                labelTurn.Text = "O's Turn . .";
                Wait(300);
                labelTurn.Text = "O's Turn . . .";
                Wait(300);

                AiPlayTurn();

                Application.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void Wait(int time)
        {
            Thread thread = new Thread(delegate ()
            {
                System.Threading.Thread.Sleep(time);
            });
            thread.Start();
            while (thread.IsAlive)
                Application.DoEvents();
        }

        private void AiPlayTurn()
        {
            //Ai Brain
            int canWin = aiCanWin();
            int canBlock = aiCanBlock();

            if (canWin != -1) // If can win in one move, win.
            {
                PlayTurn(canWin + 1);
            }
            else if (canBlock != -1) // Else if can block opponent from winning, block.
            {
                PlayTurn(canBlock + 1);
            }
            else if (board[4].Text == "")
            {
                PlayTurn(5);
            }
            else // Else play at random other space.
            {
                
                bool played = false;
                Random random = new Random();
                int playSpace = random.Next(1, 9);
                while (!played)
                {
                    if (enabled[playSpace - 1])
                    {
                        PlayTurn(playSpace);
                        played = true;
                    }
                    else
                    {
                        playSpace = random.Next(1, 9);
                    }
                }
            }
        }

        private void playerVsPlayerToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            VsAI = false;
        }

        private void playerVsAIToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            VsAI = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(3);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(4);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(5);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(6);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(7);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(8);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (!(VsAI && !XTurn))
            {
                PlayTurn(9);
            }
        }

        private void restartGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool IsGameDraw()
        {
            if (!(enabled[0] || enabled[1] || enabled[2] || enabled[3] || enabled[4] || enabled[5] || enabled[6] || enabled[7] || enabled[8]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool HasGameEnded()
        {
            if (board[0].Text == board[1].Text && board[0].Text == board[2].Text && board[0].Text != "")
            {
                board[0].BackColor = Color.FromArgb(32, 32, 32);
                board[1].BackColor = Color.FromArgb(32, 32, 32);
                board[2].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[3].Text == board[4].Text && board[3].Text == board[5].Text && board[3].Text != "")
            {
                board[3].BackColor = Color.FromArgb(32, 32, 32);
                board[4].BackColor = Color.FromArgb(32, 32, 32);
                board[5].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[6].Text == board[7].Text && board[6].Text == board[8].Text && board[6].Text != "")
            {
                board[6].BackColor = Color.FromArgb(32, 32, 32);
                board[7].BackColor = Color.FromArgb(32, 32, 32);
                board[8].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[0].Text == board[3].Text && board[0].Text == board[6].Text && board[0].Text != "")
            {
                board[0].BackColor = Color.FromArgb(32, 32, 32);
                board[3].BackColor = Color.FromArgb(32, 32, 32);
                board[6].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[1].Text == board[4].Text && board[1].Text == board[7].Text && board[1].Text != "")
            {
                board[1].BackColor = Color.FromArgb(32, 32, 32);
                board[4].BackColor = Color.FromArgb(32, 32, 32);
                board[7].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[2].Text == board[5].Text && board[2].Text == board[8].Text && board[2].Text != "")
            {
                board[2].BackColor = Color.FromArgb(32, 32, 32);
                board[5].BackColor = Color.FromArgb(32, 32, 32);
                board[8].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[0].Text == board[4].Text && board[0].Text == board[8].Text && board[0].Text != "")
            {
                board[0].BackColor = Color.FromArgb(32, 32, 32);
                board[4].BackColor = Color.FromArgb(32, 32, 32);
                board[8].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else if (board[2].Text == board[4].Text && board[2].Text == board[6].Text && board[2].Text != "")
            {
                board[2].BackColor = Color.FromArgb(32, 32, 32);
                board[4].BackColor = Color.FromArgb(32, 32, 32);
                board[6].BackColor = Color.FromArgb(32, 32, 32);
                return true;
            }
            else
            {
                return false;
            }
        }

        private int aiCanWin()
        {
            if (board[0].Text == "O" && board[1].Text == "O" && board[2].Text != "X")
            {
                return 2;
            }
            else if(board[0].Text == "O" && board[2].Text == "O" && board[1].Text != "X")
            {
                return 1;
            }
            else if (board[1].Text == "O" && board[2].Text == "O" && board[0].Text != "X")
            {
                return 0;
            }
            else if (board[3].Text == "O" && board[4].Text == "O" && board[5].Text != "X")
            {
                return 5;
            }
            else if (board[4].Text == "O" && board[5].Text == "O" && board[3].Text != "X")
            {
                return 3;
            }
            else if (board[3].Text == "O" && board[5].Text == "O" && board[4].Text != "X")
            {
                return 4;
            }
            else if (board[6].Text == "O" && board[7].Text == "O" && board[8].Text != "X")
            {
                return 8;
            }
            else if (board[7].Text == "O" && board[8].Text == "O" && board[6].Text != "X")
            {
                return 6;
            }
            else if (board[6].Text == "O" && board[8].Text == "O" && board[7].Text != "X")
            {
                return 7;
            }
            else if (board[0].Text == "O" && board[3].Text == "O" && board[6].Text != "X")
            {
                return 6;
            }
            else if (board[3].Text == "O" && board[6].Text == "O" && board[0].Text != "X")
            {
                return 0;
            }
            else if (board[0].Text == "O" && board[6].Text == "O" && board[3].Text != "X")
            {
                return 3;
            }
            else if (board[1].Text == "O" && board[4].Text == "O" && board[7].Text != "X")
            {
                return 7;
            }
            else if (board[1].Text == "O" && board[7].Text == "O" && board[4].Text != "X")
            {
                return 4;
            }
            else if (board[4].Text == "O" && board[7].Text == "O" && board[1].Text != "X")
            {
                return 1;
            }
            else if (board[2].Text == "O" && board[5].Text == "O" && board[8].Text != "X")
            {
                return 8;
            }
            else if (board[5].Text == "O" && board[8].Text == "O" && board[2].Text != "X")
            {
                return 2;
            }
            else if (board[2].Text == "O" && board[8].Text == "O" && board[5].Text != "X")
            {
                return 5;
            }
            else if (board[0].Text == "O" && board[4].Text == "O" && board[8].Text != "X")
            {
                return 8;
            }
            else if (board[4].Text == "O" && board[8].Text == "O" && board[0].Text != "X")
            {
                return 0;
            }
            else if (board[0].Text == "O" && board[8].Text == "O" && board[4].Text != "X")
            {
                return 4;
            }
            else if (board[2].Text == "O" && board[4].Text == "O" && board[6].Text != "X")
            {
                return 6;
            }
            else if (board[4].Text == "O" && board[6].Text == "O" && board[2].Text != "X")
            {
                return 2;
            }
            else if (board[2].Text == "O" && board[6].Text == "O" && board[4].Text != "X")
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }

        private int aiCanBlock()
        {
            if (board[0].Text == "X" && board[1].Text == "X" && board[2].Text != "O")
            {
                return 2;
            }
            else if (board[0].Text == "X" && board[2].Text == "X" && board[1].Text != "O")
            {
                return 1;
            }
            else if (board[1].Text == "X" && board[2].Text == "X" && board[0].Text != "O")
            {
                return 0;
            }
            else if (board[3].Text == "X" && board[4].Text == "X" && board[5].Text != "O")
            {
                return 5;
            }
            else if (board[4].Text == "X" && board[5].Text == "X" && board[3].Text != "O")
            {
                return 3;
            }
            else if (board[3].Text == "X" && board[5].Text == "X" && board[4].Text != "O")
            {
                return 4;
            }
            else if (board[6].Text == "X" && board[7].Text == "X" && board[8].Text != "O")
            {
                return 8;
            }
            else if (board[7].Text == "X" && board[8].Text == "X" && board[6].Text != "O")
            {
                return 6;
            }
            else if (board[6].Text == "X" && board[8].Text == "X" && board[7].Text != "O")
            {
                return 7;
            }
            else if (board[0].Text == "X" && board[3].Text == "X" && board[6].Text != "O")
            {
                return 6;
            }
            else if (board[3].Text == "X" && board[6].Text == "X" && board[0].Text != "O")
            {
                return 0;
            }
            else if (board[0].Text == "X" && board[6].Text == "X" && board[3].Text != "O")
            {
                return 3;
            }
            else if (board[1].Text == "X" && board[4].Text == "X" && board[7].Text != "O")
            {
                return 7;
            }
            else if (board[1].Text == "X" && board[7].Text == "X" && board[4].Text != "O")
            {
                return 4;
            }
            else if (board[4].Text == "X" && board[7].Text == "X" && board[1].Text != "O")
            {
                return 1;
            }
            else if (board[2].Text == "X" && board[5].Text == "X" && board[8].Text != "O")
            {
                return 8;
            }
            else if (board[5].Text == "X" && board[8].Text == "X" && board[2].Text != "O")
            {
                return 2;
            }
            else if (board[2].Text == "X" && board[8].Text == "X" && board[5].Text != "O")
            {
                return 5;
            }
            else if (board[0].Text == "X" && board[4].Text == "X" && board[8].Text != "O")
            {
                return 8;
            }
            else if (board[4].Text == "X" && board[8].Text == "X" && board[0].Text != "O")
            {
                return 0;
            }
            else if (board[0].Text == "X" && board[8].Text == "X" && board[4].Text != "O")
            {
                return 4;
            }
            else if (board[2].Text == "X" && board[4].Text == "X" && board[6].Text != "O")
            {
                return 6;
            }
            else if (board[4].Text == "X" && board[6].Text == "X" && board[2].Text != "O")
            {
                return 2;
            }
            else if (board[2].Text == "X" && board[6].Text == "X" && board[4].Text != "O")
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }
    }
}



