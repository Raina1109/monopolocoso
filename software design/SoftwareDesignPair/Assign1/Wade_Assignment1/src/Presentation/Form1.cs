using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Wade_Assignment1.Core;

namespace Wade_Assignment1.Presentation
{
    public partial class Form1 : Form
    {
        GameController gameController = new GameController();

        const string cartDisplayInitialValue = "_ _ _ _ _ _ _";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            lblShelfLetter1.Text = gameController.GetShelfLetterByPosition(1).ToString();
            lblShelfLetter2.Text = gameController.GetShelfLetterByPosition(2).ToString();
            lblShelfLetter3.Text = gameController.GetShelfLetterByPosition(3).ToString();
            lblShelfLetter4.Text = gameController.GetShelfLetterByPosition(4).ToString();
            lblShelfLetter5.Text = gameController.GetShelfLetterByPosition(5).ToString();
            lblShelfLetter6.Text = gameController.GetShelfLetterByPosition(6).ToString();

            lblCartDisplay.Text = cartDisplayInitialValue;
        }

        private void lblShelfLetter1_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblShelfLetter2_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblShelfLetter3_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblShelfLetter4_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblShelfLetter5_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblShelfLetter6_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((sender as Label), DragDropEffects.Move);
        }

        private void lblCart_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Move) != 0 && e.Data.GetDataPresent(typeof(Label)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void lblCart_DragDrop(object sender, DragEventArgs e)
        {
            Label lblShelfLetter = (Label)e.Data.GetData(typeof(Label));

            Label labelCart = (sender as Label);

            if (labelCart.Text == cartDisplayInitialValue)
            {
                labelCart.Text = String.Empty;
            }

            gameController.AddToCart(Char.Parse(lblShelfLetter.Text));

            int position = int.Parse(lblShelfLetter.Name[lblShelfLetter.Name.Length - 1].ToString());

            gameController.GetNextRandomLetterForPosition(position);

            labelCart.Text = gameController.GetCartLetters();

            lblShelfLetter.Text = gameController.GetShelfLetterByPosition(position).ToString();

            if (gameController.IsSpelledCorrect())
            {
                btnCartCollect.Enabled = true;
            }
            else
            {
                btnCartCollect.Enabled = false;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void lblTrash_DragDrop(object sender, DragEventArgs e)
        {
            Label shelfLetterLabel = (Label)e.Data.GetData(typeof(Label));

            int pointValue = gameController.GetLetterPointValue(char.Parse(shelfLetterLabel.Text));

            gameController.SubtractFromScore(pointValue);

            txtScore.Text = gameController.GetScore().ToString();

            int position = int.Parse(shelfLetterLabel.Name[shelfLetterLabel.Name.Length - 1].ToString());

            gameController.GetNextRandomLetterForPosition(position);
            shelfLetterLabel.Text = gameController.GetShelfLetterByPosition(position).ToString();
        }

        private void lblTrash_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Move) != 0 && e.Data.GetDataPresent(typeof(Label)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void btnCartCollect_Click(object sender, EventArgs e)
        {
            gameController.CollectCart();
            txtScore.Text = gameController.GetScore().ToString();

            ResetCartView();


        }

        private void ResetCartView()
        {
            string cartLetters = string.Empty;

            cartLetters = gameController.GetCartLetters();

            if (String.IsNullOrEmpty(cartLetters))
            {
                lblCartDisplay.Text = cartDisplayInitialValue;
            }

            btnCartCollect.Enabled = false;
        }

        private void btnCartDiscard_Click(object sender, EventArgs e)
        {
            gameController.DiscardCart();
            txtScore.Text = gameController.GetScore().ToString();

            ResetCartView();
        }
    }
}
