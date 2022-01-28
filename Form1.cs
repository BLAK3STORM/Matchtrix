using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Card_Matching_Game
{
    public partial class Form1 : Form
    {
        // Instantiation
        int time = 0; // Initial time for the time label timer
        Random random = new Random();
        private SoundPlayer clickSound;
        private SoundPlayer unmatchedSound;
        private SoundPlayer matchedSound;
        private SoundPlayer winSound;
        private SoundPlayer startButtonClick;

        // List to contain letters which reffers to icons in 'Webdings' font
        List<string> icons = new List<string>()
        {
            "!","!","N","N",",",",","k","k","L","L","j","j",
            "b","b","v","v","w","w","o","o","O","O","l","l",
            "m","m","%","%","p","p","U","U","$","$","q","q"
        };

        List<string> icons1 = new List<string>()
        {
            "!","!","N","N",",",",","k","k","L","L","j","j",
            "b","b","v","v","w","w","o","o","O","O","l","l",
            "m","m","%","%","p","p","U","U","$","$","q","q"
        };

        Label firstClicked = null; // Points to the first label that is clicked. Initially null
        Label secondClicked = null; // Points to the second label that is clicked. Initially null
        bool isGameStarted = false; // Game will not start unless the button is clicked

        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquare(); // Calling the method
            timeLabel.Text = "Not Started Yet!"; // Initial time label text

            // Sounds
            clickSound = new SoundPlayer("Resource/click.wav");
            unmatchedSound = new SoundPlayer("Resource/mismatch.wav");
            matchedSound = new SoundPlayer("Resource/match.wav");
            winSound = new SoundPlayer("Resource/win.wav");
            startButtonClick = new SoundPlayer("Resource/startbuttonclick.wav");
        }

        // Timer to countup
        private void timer2_Tick(object sender, EventArgs e)
        {
            time = time + 1;
            timeLabel.Text = time + " second(s)";
        }

        private void AssignIconsToSquare()
        {
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if(iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count); // Random number from 0 to 16
                    iconLabel.Text = icons[randomNumber]; // Randomly assigns an icon to the label
                    iconLabel.ForeColor = iconLabel.BackColor; // Hides the icons
                    icons.RemoveAt(randomNumber); // Removes the added icon from the list
                }
            }
        }

        private void label_Click(object sender, EventArgs e)
        {

            if (isGameStarted == true) // Makes the tiles 'click'able
            {
                // Ignores all clicks while the timer is running by returning nothing
                if (timer1.Enabled == true)
                    return;

                clickSound.Play(); // Plays sound on every click

                Label clickLabel = sender as Label;

                if (clickLabel != null)
                {
                    // If the player clicks on the revealed icon label, this event ignores the click
                    // by returning nothing
                    if (clickLabel.ForeColor == Color.Black)
                        return;

                    if (firstClicked == null)
                    {
                        firstClicked = clickLabel; // Assigns the first clicked label to the variable
                        firstClicked.ForeColor = Color.Black; // Reveals the icon upon click

                        return; // Returns nothing
                    }

                    secondClicked = clickLabel; // Assigns the second clicked label to the variable
                    secondClicked.ForeColor = Color.Black; // Reveals the icon upon click

                    unmatchedSound.Play(); // Plays sound on every mismatch

                    checkForWinner(); // Checking everytime after two cards are turned

                    // If two icons are matched, they stay visible and the variables are set to 'null'
                    // so that they can hold next inputs
                    if (firstClicked.Text == secondClicked.Text)
                    {
                        // Changes the color of the matched tiles
                        firstClicked.BackColor = Color.DodgerBlue;
                        secondClicked.BackColor = Color.DodgerBlue;
                        firstClicked.ForeColor = Color.MidnightBlue;
                        secondClicked.ForeColor = Color.MidnightBlue;
                        firstClicked = null;
                        secondClicked = null;
                        matchedSound.Play(); // Plays sound on every match
                        return;
                    }

                    // Starting the timer after two icons are visible
                    timer1.Start();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Resetting both variables to take input again
            firstClicked = null;
            secondClicked = null;
            
        }

        // Will reset everything after the game completes
        private void reset()
        {
            // Filling the list everytime as this method will remove items from the list
            icons1 = new List<string>{ "!","!","N","N",",",",","k","k","L","L","j","j",
                                       "b","b","v","v","w","w","o","o","O","O","l","l",
                                       "m","m","%","%","p","p","U","U","$","$","q","q"
                                      };

            
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label label = control as Label;

                if (label != null)
                {
                    label.Text = null; // Removes all the texts from the label to assign new
                    int randomNumber = random.Next(icons1.Count); // Random number from 0 to 36
                    label.Text = icons1[randomNumber]; // Randomly assigns an icon to the label
                    label.BackColor = Color.CornflowerBlue; // Resets the background color
                    label.ForeColor = label.BackColor; // Hides the icons
                    icons1.RemoveAt(randomNumber); // Removes the added icon from the list
                }

            }
            
        }

        private void checkForWinner()
        {
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label label = control as Label;

                if(label != null)
                {
                    if(label.ForeColor == label.BackColor)
                        return;
                }
            }

            // Changes the last matched tiles
            firstClicked.ForeColor = Color.MidnightBlue;
            secondClicked.ForeColor = Color.MidnightBlue;
            firstClicked.BackColor = Color.DodgerBlue;
            secondClicked.BackColor = Color.DodgerBlue;

            // Stoping the label timer
            timer2.Stop();
            time = 0; // Resetting the timer to start from 0

            winSound.Play(); // Plays sound on winning
            MessageBox.Show("You Have Matched All The Cards!!!" +
                          "\n      Press 'OK' to reset the tiles", "Congratutalions!");
            reset(); // Resetting for a new game
            isGameStarted = false; // Making the game hold till the button is pressed
            start_button.Enabled = true; // Sets the button 'press'able again
            timeLabel.Text = "Not Started Yet!"; // Resetting the time label text after each game
        }

        // Button click will start the game
        private void start_button_Click(object sender, EventArgs e)
        {
            startButtonClick.Play(); // Plays sound upon start button click
            isGameStarted = true; // Starts the game
            timer2.Start(); // Starts the label timer
            start_button.Enabled = false; // Sets the button un'press'able till a reset
        }
    }
}
