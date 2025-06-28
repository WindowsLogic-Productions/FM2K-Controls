using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Controls
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            this.KeyPreview = true;
            // saveToolStripMenuItem.Text = "Save As..."; //Default name of command
        }

        string filename;
        bool input_key = false;
        Button targetButton;
        //Const

        // List of Keys for buttons on form
        readonly string[] ButtonKeys = {
            null, "LMB", "RMB", "Control-Break", "MMB", "X1MB", "X2MB", null, "BckSpc", "Tab", null, null, "Clear", "Enter", null, null,
            "SHIFT", "CTRL", "ALT", "PAUSE", "Caps lock", null, null, null, null, null, null, "ESC", null, null, null,null,
            "Space", "PgUp", "PgDn", "End", "Home", "Left", "Up", "Right", "Down", "Select", "Print", "Execute", "PrntScrn", "Insert", "Delete", "Help",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", null, null, null, null,null,null,
            null, "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
            "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Left Win", "Right Win", "Apps Key", null, "Sleep",
            "Num0", "Num1", "Num2", "Num3", "Num4", "Num5", "Num6", "Num7", "Num8", "Num9", "*", "+", "Separator", "-", "Decimal", "/",
            "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "F16",
            "F17", "F18", "F19", "F20", "F21", "F22", "F23", "F24", null, null, null, null, null, null, null, null,
            "NumLock", "ScrollLock", null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            "L SHIFT", "R SHIFT", "L CTRL", "R CTRL", "L ALT", "R ALT", "Back", "Forward", "Refresh", "Stop", "Search", "Favourites", "Br Home", "VolMute", "Vol -", "Vol +",
            "NextTrack", "PrevTrack", "Stop", "Play/Pause", "StartMail", "SelectMedia", "SrartApp1", "StartApp2", null, null, ";", "+", ",", "-", ".", "/",
            "~", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, null, null, null, null, null, "[", "\\", "]", "'", "<others>",
            null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            null, null, null, null, null, null, "Attn", "CrSel", "ExSel", "EraseEOF", "Play", "Zoom", null, "PA1", "Clear", "<others>",

        };
        readonly List<byte[]> commandConstants = new List<byte[]>()
        {

            // As default those bytes reads as ?? and because I used that templates
            new byte[] { 143, 227}, // UP
            new byte[] { 137, 69 }, // LEFT
            new byte[] { 137, 186}, // DOWN
            new byte[] { 141, 182}, // RIGHT
            new byte[] { 130, 96 }, // A
            new byte[] { 130, 97 }, // B
            new byte[] { 130, 98 }, // C
            new byte[] { 130, 99 }, // D
            new byte[] { 130, 100}, // E
            new byte[] { 130, 101},  // F
            new byte[] { 83, 69} // Start
        };

        // Array of integers with KeyCodes
        int[] buttons = { 
          /*0  1  2  3  4  5  6  7  8  9  10 //P1
           *11 12 13 14 15 16 17 18 19 20 21 //P2
           *            22 23 24 25 26 27 28 //P1 JOY
           *            29 30 31 32 33 34 35 //P2 JOY
            */
          //U  D  L  R  S  A  B  C  D  E  F
            0, 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0, //P1
            0, 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0, //P2
                        0, 0, 0, 0, 0, 0, 0, //P1 JOY
                        0, 0, 0, 0, 0, 0, 0  //P2 JOY
        };

        byte[] iniContainer;



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filename != "") // If you didn't open the file, it will save it
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.FileName = "game.ini";
                sf.Filter = "INI files| *.ini";
                
                if (sf.ShowDialog() == DialogResult.OK) filename = sf.FileName;
                else return;
            }
            string BaseP1 = "Player1 KEY ";
            string BaseP2 = "Player2 KEY ";
            string BaseP1j = "Player1 JOY ";
            string BaseP2j = "Player2 JOY ";
            List<byte> outputText = new List<byte>();

            // Prepare byte array for writing.
            // P1 KEY
            foreach (byte b in Encoding.ASCII.GetBytes("[GamePlay]\n")) outputText.Add(b);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[0]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[0].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[2]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[1].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[1]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[2].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[3]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[3].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in Encoding.ASCII.GetBytes("PAUSE")) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[4].ToString())) outputText.Add(b); outputText.Add(13);

            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[4]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[5].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[5]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[6].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[6]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[7].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[7]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[8].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[8]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[9].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[9]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[10].ToString())) outputText.Add(b); outputText.Add(13);

            // P2 KEY
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[0]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[11].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[2]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[12].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[1]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[13].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[3]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[14].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in Encoding.ASCII.GetBytes("PAUSE")) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[15].ToString())) outputText.Add(b); outputText.Add(13);

            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[4]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[16].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[5]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[17].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[6]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[18].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[7]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[19].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[8]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[20].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2 + "")) outputText.Add(b);
            foreach (byte b in commandConstants[9]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[21].ToString())) outputText.Add(b); outputText.Add(13);

            //P1 JOY
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[4]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[23].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[5]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[24].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[6]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[25].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[7]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[26].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[8]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[27].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[9]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[28].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP1j + "")) outputText.Add(b);
            foreach (byte b in Encoding.ASCII.GetBytes("PAUSE")) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[22].ToString())) outputText.Add(b); outputText.Add(13);

            //P2 JOY
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[4]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[30].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[5]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[31].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[6]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[32].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[7]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[33].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[8]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[34].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in commandConstants[9]) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[35].ToString())) outputText.Add(b); outputText.Add(13);
            foreach (byte b in Encoding.ASCII.GetBytes(BaseP2j + "")) outputText.Add(b);
            foreach (byte b in Encoding.ASCII.GetBytes("PAUSE")) outputText.Add(b); outputText.Add(61); foreach (byte b in Encoding.ASCII.GetBytes(buttons[29].ToString())) outputText.Add(b); outputText.Add(13);

            foreach (byte b in Encoding.ASCII.GetBytes($"GameWindowPoint_x = {(Screen.PrimaryScreen.Bounds.X + 640) / 2}\nGameWindowPoint_y = {(Screen.PrimaryScreen.Bounds.Y + 480) / 2}\nGameWindowSize_x = 640\nGameWindowSize_y = 480\nGameScreenMode = 0")) outputText.Add(b);

            // Delete original
            File.Delete(filename);

            //Write file
            using (FileStream fstream = new FileStream($"{filename}", FileMode.OpenOrCreate))
            {
                // Write byte array to file
                fstream.Write(outputText.ToArray(), 0, outputText.ToArray().Length);
                Console.WriteLine("Text writed into file");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open game ini file.
            OpenFileDialog file = new OpenFileDialog
            {
                Filter = "INI files|*.ini"
            };
            if (file.ShowDialog() == DialogResult.OK)
            {
                filename = file.FileName;

                // Load all keybindings from ini.
                iniContainer = File.ReadAllBytes(filename);

                for (var i = 0; i < iniContainer.Length; i++)
                {
                    if (iniContainer[i] == 61)
                    {
                        byte[] compareByteArray = new byte[] { iniContainer[i - 2], iniContainer[i - 1] };

                        // ========================================
                        // P1
                        // ========================================
                        //START
                        if (iniContainer[i - 11] == 49)
                        {
                            // P1 Key
                            if (iniContainer[i - 9] == 75)
                            {
                                if (compare2arrays(compareByteArray, commandConstants[10])) GetKeyCode(4, P1ButtonStart, i);
                            }
                            if (iniContainer[i - 9] == 74) // P1 JOY 
                            {
                                if (compare2arrays(compareByteArray, commandConstants[10])) GetKeyCode(22, P1JoyStart, i, true);
                            }
                        }
                        if (iniContainer[i - 8] == 49 && iniContainer[i - 6] == 75) // P1 KEY
                        {
                            //UP
                            if (compare2arrays(compareByteArray, commandConstants[0])) GetKeyCode(0, P1ButtonUp, i);
                            //LEFT
                            if (compare2arrays(compareByteArray, commandConstants[1])) GetKeyCode(2, P1ButtonLeft, i);
                            //DOWN
                            if (compare2arrays(compareByteArray, commandConstants[2])) GetKeyCode(1, P1ButtonDown, i);
                            //RIGHT
                            if (compare2arrays(compareByteArray, commandConstants[3])) GetKeyCode(3, P1ButtonRight, i);
                            //A
                            if (compare2arrays(compareByteArray, commandConstants[4])) GetKeyCode(5, P1ButtonA, i);
                            //B
                            if (compare2arrays(compareByteArray, commandConstants[5])) GetKeyCode(6, P1ButtonB, i);
                            //C
                            if (compare2arrays(compareByteArray, commandConstants[6])) GetKeyCode(7, P1ButtonC, i);
                            //D
                            if (compare2arrays(compareByteArray, commandConstants[7])) GetKeyCode(8, P1ButtonD, i);
                            //E
                            if (compare2arrays(compareByteArray, commandConstants[8])) GetKeyCode(9, P1ButtonE, i);
                            //F
                            if (compare2arrays(compareByteArray, commandConstants[9])) GetKeyCode(10, P1ButtonF, i);

                        }
                        else if (iniContainer[i - 8] == 49 && iniContainer[i - 6] == 74) // P1 JOY 
                        {
                            // Joy A
                            if (compare2arrays(compareByteArray, commandConstants[4])) GetKeyCode(23, P1JoyA, i, true);
                            // Joy B
                            if (compare2arrays(compareByteArray, commandConstants[5])) GetKeyCode(24, P1JoyB, i, true);
                            // Joy C
                            if (compare2arrays(compareByteArray, commandConstants[6])) GetKeyCode(25, P1JoyC, i, true);
                            // Joy D
                            if (compare2arrays(compareByteArray, commandConstants[7])) GetKeyCode(26, P1JoyD, i, true);
                            // Joy E
                            if (compare2arrays(compareByteArray, commandConstants[8])) GetKeyCode(27, P1JoyE, i, true);
                            // Joy F
                            if (compare2arrays(compareByteArray, commandConstants[9])) GetKeyCode(28, P1JoyF, i, true);
                        }
                        // ========================================
                        // P2
                        // ========================================
                        //START
                        if (iniContainer[i - 11] == 50)
                        {
                            if (iniContainer[i - 9] == 75) // P2 KEY
                            {
                                if (compare2arrays(compareByteArray, commandConstants[10])) GetKeyCode(4, P2ButtonStart, i);
                            }
                            if (iniContainer[i - 9] == 74) // P2 JOY 
                            {
                                if (compare2arrays(compareByteArray, commandConstants[10])) GetKeyCode(22, P2JoyStart, i, true);
                            }
                        }

                        if (iniContainer[i - 8] == 50 && iniContainer[i - 6] == 75) // P2 KEY
                        {

                            //UP
                            if (compare2arrays(compareByteArray, commandConstants[0])) GetKeyCode(11, P2ButtonUp, i);
                            //LEFT
                            if (compare2arrays(compareByteArray, commandConstants[1])) GetKeyCode(13, P2ButtonLeft, i);
                            //DOWN
                            if (compare2arrays(compareByteArray, commandConstants[2])) GetKeyCode(12, P2ButtonDown, i);
                            //RIGHT
                            if (compare2arrays(compareByteArray, commandConstants[3])) GetKeyCode(14, P2ButtonRight, i);
                            //A
                            if (compare2arrays(compareByteArray, commandConstants[4])) GetKeyCode(16, P2ButtonA, i);
                            //B
                            if (compare2arrays(compareByteArray, commandConstants[5])) GetKeyCode(17, P2ButtonB, i);
                            //C
                            if (compare2arrays(compareByteArray, commandConstants[6])) GetKeyCode(18, P2ButtonC, i);
                            //D
                            if (compare2arrays(compareByteArray, commandConstants[7])) GetKeyCode(19, P2ButtonD, i);
                            //E
                            if (compare2arrays(compareByteArray, commandConstants[8])) GetKeyCode(20, P2ButtonE, i);
                            //F
                            if (compare2arrays(compareByteArray, commandConstants[9])) GetKeyCode(21, P2ButtonF, i);
                        }
                        else if (iniContainer[i - 8] == 50 && iniContainer[i - 6] == 74) // P2 JOY 
                        {
                            // Joy A
                            if (compare2arrays(compareByteArray, commandConstants[4])) GetKeyCode(29, P2JoyA, i, true);
                            // Joy B
                            if (compare2arrays(compareByteArray, commandConstants[5])) GetKeyCode(30, P2JoyB, i, true);
                            // Joy C
                            if (compare2arrays(compareByteArray, commandConstants[6])) GetKeyCode(31, P2JoyC, i, true);
                            // Joy D
                            if (compare2arrays(compareByteArray, commandConstants[7])) GetKeyCode(32, P2JoyD, i, true);
                            // Joy E
                            if (compare2arrays(compareByteArray, commandConstants[8])) GetKeyCode(33, P2JoyE, i, true);
                            // Joy F
                            if (compare2arrays(compareByteArray, commandConstants[9])) GetKeyCode(34, P2JoyF, i, true);
                        }
                    }
                }
                saveToolStripMenuItem.Text = "Save";
            }



        }

        // This function used for shorten code
        public void GetKeyCode(int target1, Button target2, int index, bool joy = false)
        {
            var str = "";
            for (int j = 1; j < 10; j++)
            {
                if (iniContainer[index + j] != 13) str += Encoding.ASCII.GetString(new byte[] { iniContainer[index + j] });
                else break;
            }
            //Console.WriteLine(str + " " + ButtonKeys[Convert.ToInt32(str)]);
            buttons[target1] = (byte)Convert.ToInt32(str);
            if (joy) target2.Text = "Joy" + Convert.ToInt32(str);
            else target2.Text = ButtonKeys[Convert.ToInt32(str)];
        }

        // Since C# can't use byte[] == byte[] 
        public bool compare2arrays(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) return false;
            }
            return true;
        }

        // Any button on form pressed
        private void P1Button_Click(object sender, EventArgs e) {
            labelStatus.Text = "Waiting input...";
            targetButton = (Button)sender;
            input_key = true;
        }

        // Read input from keyboard
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            labelStatus.Text = "";
            if (input_key)
            {
                input_key = !input_key;
                targetButton.Text = ButtonKeys[e.KeyValue];
                buttons[Convert.ToInt32(targetButton.Tag)] = e.KeyValue;
                Console.WriteLine(e.KeyValue);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                targetButton.Text = ButtonKeys[e.KeyValue];
                buttons[Convert.ToInt32(targetButton.Tag)] = e.KeyValue;
                Console.WriteLine(e.KeyValue);
                e.Handled = true;
            }
        }

        // Prevent default behavior to read arrow keys as navigation keys
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (input_key && (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down))
            {
                input_key = false;
                int keyCode = (int)keyData;
                targetButton.Text = ButtonKeys[keyCode];
                buttons[Convert.ToInt32(targetButton.Tag)] = keyCode;
                Console.WriteLine(keyCode);
                return true; // Suppress default behavior
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }
    }
}
