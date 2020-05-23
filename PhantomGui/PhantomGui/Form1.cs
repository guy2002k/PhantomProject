using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PhantomGui
{
    public partial class Phantom : Form
    {
        private const int MULTIPLY_PANEL = 6;
        private const int ADDER = 10;

        //draggble constants
        private const int WM_NCLBUTTONDOWN = 0Xa1;
        private const int HT_CAPTION = 0x2;

        private string name;
        private float current_brightness = 0;
        private bool isMaximized = false;


        private struct RGBColors
        {
            public static Color yellow = Color.FromArgb(224, 186, 21);
            public static Color green = Color.FromArgb(135, 163, 120);
            public static Color blue = Color.FromArgb(24, 161, 251);
            public static Color red = Color.FromArgb(191, 26, 8);
            public static Color purple = Color.FromArgb(172, 126, 241);
        }

        private Process client;

        private static Dictionary<string,int> counter_Clicks;
        private static List<JsonRequest> items;
        private Color clickBackColor;
        private IconButton currentBtn;
        private Panel leftPanelButtons;


        public Phantom()
        {
            InitializeComponent();

            counter_Clicks = new Dictionary<string, int>();

            leftPanelButtons = new Panel();
            leftPanelButtons.Size = new Size(6, 78);

            SwitchPanel((object)this.home);
            ActivateButton((object)this.home, RGBColors.red);

            client = new Process();
            client.StartInfo.FileName = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\")) + "ExeFiles\\SC.exe";

            RunClient();

        }


        private void UpdateReplies(bool take_data)
        {
            string up = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\"));
            string path = up + "Reply";

            string name,fileName = "";

            string[] files = Directory.GetFiles(path);

            if (take_data)
            {
                listBox.Items.Clear();

                foreach (string file in files)
                {
                    fileName = file.Substring(path.Length + 1);
                    try
                    {
                        name = GetListFileName(fileName);
                        listBox.Items.Add(name);
                    }

                    catch {}
                }

 
            }

            else
            {
                string[] replies = this.GetRepliesAsArray();

                DeleteIrrelevantFiles(replies);

            }
        }

        private string[] GetRepliesAsArray()
        {
            string[] replies=new string[listBox.Items.Count];
            for(int i=0; i<this.listBox.Items.Count; i++)
            {
                replies[i] = (string) this.listBox.Items[i];
            }
            return replies;
        }

        private void DeleteIrrelevantFiles(string[] replies)
        {
            string up = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\"));
            string path = up + "Reply";
            string[] files = Directory.GetFiles(path);
            string s;

            bool isRelevant = false;

            foreach(string file in files)
            {
                isRelevant = false;

                foreach (string reply in replies)
                {
                    s = file.Substring(path.Length + 1);
                    if (this.GetFileName(reply) == file.Substring(path.Length + 1))
                    {
                        isRelevant = true;
                        break;
                    }
                }

                if (!isRelevant)
                {
                    File.Delete(file);
                }
            }
        }







        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;

                currentBtn.ForeColor = color;
                currentBtn.IconColor = color;

                if (currentBtn.Name == "menu")
                    currentBtn.Controls.Add(leftPanelButtons);

                else
                    panelMenu.Controls.Add(leftPanelButtons);

                //Left border button
                leftPanelButtons.BackColor = color;
                leftPanelButtons.Location = new Point(0, currentBtn.Location.Y);
                leftPanelButtons.Visible = true;
                leftPanelButtons.BringToFront();

                /*
                //Current Child Form Icon
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
                */
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                if (currentBtn.Name=="menu")
                    currentBtn.Controls.Remove(leftPanelButtons);
                else
                    panelMenu.Controls.Remove(leftPanelButtons);

                currentBtn.BackColor = this.panelMenu.BackColor;
                currentBtn.ForeColor = Color.Gainsboro;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gainsboro;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }


        private void BordeMainPane(bool toOpen)
        {
            int limit;
            if (toOpen)
            {
                limit = panelMenu.Size.Width * MULTIPLY_PANEL;

                while (panelMenu.Size.Width != limit)
                {
                    panelMenu.Size = new Size(panelMenu.Size.Width + ADDER, panelMenu.Size.Height);
                }
            }

            else
            {
                limit = panelMenu.Size.Width / MULTIPLY_PANEL;

                while (panelMenu.Size.Width != limit)
                {
                    panelMenu.Size = new Size(panelMenu.Size.Width - ADDER, panelMenu.Size.Height);
                }
            }

            panelMenu.BringToFront();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            name = "Menu_Button";
            if (!counter_Clicks.ContainsKey(name))
                counter_Clicks.Add(name, 0);

            if (counter_Clicks[name] % 2 == 0)
            {
                ActivateButton(sender, RGBColors.green);
                this.menu.IconChar = FontAwesome.Sharp.IconChar.CaretRight;
                BordeMainPane(true);

                this.recv.Text = "Recieve";
                this.send.Text = "Send";
            }

            else
            {
                DisableButton();
                BordeMainPane(false);
                this.menu.IconChar = FontAwesome.Sharp.IconChar.CaretDown;

                this.recv.Text = "";
                this.send.Text = "";
            }

            counter_Clicks[name]++;

        }


        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int ReleaseCapture();



        private void PhantomDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

                try
                {
                    if (counter_Clicks["Window_Button"] % 2 != 0)
                        WindowBtn_Click(sender, e);
                }

                catch {}
            }
        }


        private void Scale_Form(int multiplier)
        { 

            this.phantomGhost.Size = !isMaximized ? new Size(this.phantomGhost.Width * multiplier, this.phantomGhost.Height * multiplier) : new Size(this.phantomGhost.Width / multiplier, this.phantomGhost.Height / multiplier);

            this.mainPanel.Size = new Size(this.Size.Width, this.Size.Height - this.Minus.Height);
            this.panelMenu.Size = new Size(this.panelMenu.Width, this.Size.Height - this.Minus.Height);

            this.Exit.Location = new Point(this.Size.Width - this.Exit.Width, this.Exit.Location.Y);
            this.Window.Location = new Point(this.Exit.Location.X - this.Window.Width, this.Window.Location.Y);
            this.Minus.Location = new Point(this.Window.Location.X - this.Minus.Width, this.Minus.Location.Y);
        }


        public void SwitchPanel(object sender)
        {

            switch (((IconButton)sender).Name)
            {

                case "send":
                {
                        //visible
                        this.select_prot.Visible = true;
                          //lbl
                        this.ip_label.Visible = true;
                        this.port_label.Visible = true;
                        this.request_label.Visible = true;
                         //tbx
                        this.ip_textBox.Visible = true;
                        this.port_textBox.Visible = true;
                        this.request_textBox.Visible = true;
                         //btn
                        this.to_send_btn.Visible = true;


                        //disvible
                        
                        //home
                        this.phantomGhost.Visible = false;
                        this.massageLabel.Visible = false;
                        
                        //recv
                        this.inbox_lbl.Visible = false;
                        this.inbox_icon.Visible = false;
                           //lsrbtx
                        this.listBox.Visible = false;
                           //btn
                        this.delete_item.Visible = false;
                        this.open_with.Visible = false;




                        break;
                }

                case "recv":
                {
                        //visble
                        //lsrbtx
                        this.listBox.Visible = true;
                        this.delete_item.Visible = true;
                        this.open_with.Visible = true;
                        //lbl+icon
                        this.inbox_lbl.Visible = true;
                        this.inbox_icon.Visible = true;


                        //disvisble
                        this.select_prot.Visible = false;
                          //lbl
                        this.ip_label.Visible = false;
                        this.port_label.Visible = false;
                        this.request_label.Visible = false;
                          //tbx
                        this.ip_textBox.Visible = false;
                        this.port_textBox.Visible = false;
                        request_textBox.Visible = false;
                          //btn
                        this.to_send_btn.Visible = false;
                          //png
                        this.phantomGhost.Visible = false;
                        this.massageLabel.Visible = false;


                        break;
                }

                case "settings":
                {


                      break;
                }

                case "home":
                {
                        //visible
                        this.phantomGhost.Visible = true;
                        this.massageLabel.Visible = true;


                        //disvible
                        this.select_prot.Visible = false;
                          //lbl
                          //send
                        this.ip_label.Visible = false;
                        this.port_label.Visible = false;
                        this.request_label.Visible = false;

                          //recv
                        this.inbox_lbl.Visible = false;
                        this.inbox_icon.Visible = false;

                          //tbx
                        this.ip_textBox.Visible = false;
                        this.port_textBox.Visible = false;
                        this.request_textBox.Visible = false;
                        //send
                          //btn
                        this.to_send_btn.Visible = false;
                        //recv
                        this.delete_item.Visible = false;
                        this.open_with.Visible = false;
                        //lsrbtx
                        this.listBox.Visible = false;

                        break;
                }
            }


        }







        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.
            Brush myBrush = Brushes.Black;

            // Determine the color of the brush to draw each item based 
            // on the index of the item to draw.
            switch (e.Index)
            {
                case 0:
                    myBrush = Brushes.Gainsboro;
                    break;
                case 1:
                    myBrush = Brushes.Orange;
                    break;
                case 2:
                    myBrush = Brushes.Purple;
                    break;
            }

            // Draw the current item text based on the current Font 
            // and the custom brush settings.
            try
            {
                e.Graphics.DrawString(listBox.Items[e.Index].ToString(),
                    e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
            }

            catch { }
            // If the ListBox has focus, draw a focus rectangle around the selected item.

        }

        internal string GetSystemDefaultBrowser()
        {
            string name = string.Empty;
            RegistryKey regKey = null;

            try
            {
                //set the registry key we want to open
                regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

                //get rid of the enclosing quotes
                name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

            }
            catch (Exception ex)
            {
                name = string.Format("ERROR: An exception of type: {0} occurred in method: {1} in the following module: {2}", ex.GetType(), ex.TargetSite, this.GetType());
            }
            finally
            {
                //check and see if the key is still open, if so
                //then close it
                if (regKey != null)
                    regKey.Close();
            }
            //return the value
            //return name.Split("\\")[];
            //"c:\\program files\\internet explorer\\iexplore.exe %1"
            string fin_name = "";

            int index = name.IndexOf(".exe")+3;

            while (name[index]!=Convert.ToChar(@"\"))
            {
                fin_name += name[index];
                index--;
            }


            return new string(fin_name.Reverse().ToArray());
        }

        private string GetValidIpPort(string st)
        {
            return st.Substring(st.IndexOf(": ") + 2);
        }

        public string GetFileName(string listName)
        {
            string name="";
            string str="";

            foreach(string st in listName.Split(','))
            {
                str = GetValidIpPort(st);
                if (str == "txt" || str == "html")
                {
                    name = name.Substring(0, name.Length - 1);
                    name += "." + str;
                }
                else
                {
                    name += str + " ";
                }
            }

            return name;
        }

        private string GetListFileName(string fileName)
        {
            string[] prop = fileName.Split(' ');
            string[] more_prop = prop[2].Split('.');


            try
            {

                var type = Uri.CheckHostName(prop[0]);
                if (type != UriHostNameType.IPv4)
                {
                    Dns.Resolve(ip_textBox.Text);
                }

                Convert.ToInt32(prop[1]);
            }

            catch
            {
                throw new Exception("File not valid");
            }


            return "From: " + prop[0] + ", Port: " + prop[1] + ", Reply Number: "+ more_prop[0] + ", Type: " + more_prop[1];
        }

        private void SendToServer()
        {
            string path = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\")) + "Jsons\\ClientRequest.json";

            StreamReader reader = new StreamReader(path);

            List<JsonRequest> data = JsonConvert.DeserializeObject<List<JsonRequest>>(reader.ReadToEnd());

            reader.Close();


            data[0].Ip.Add(this.ip_textBox.Text);
            data[0].Port.Add(Convert.ToInt32(this.port_textBox.Text));
            data[0].Request.Add(this.request_textBox.Text);
            

            File.WriteAllText(path, JsonConvert.SerializeObject(data.ToArray()));

        }
        
        private void RunClient()
        {
            client.Start();

            Console.WriteLine("Littttttllllllleeeeeeee frikimngggg bitch");
        }


        public async void CloseClient()
        {
            string path = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\"));

            string clientNamePath = path + "Jsons\\ClientName.json";
            string certificationPath = path + "Jsons\\Tor-Certificate.json";

            StreamReader reader = new StreamReader(clientNamePath);

            List<JsonClientName> data = JsonConvert.DeserializeObject<List<JsonClientName>>(reader.ReadToEnd());

            reader.Close();

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", certificationPath);

            FirestoreDb db = FirestoreDb.Create("tor-servers");

            try
            {
                DocumentReference delete = db.Collection("Clients").Document(data[0].Name);
                delete.DeleteAsync();
            }

            catch { }


            data[0].Name = "";
            File.WriteAllText(clientNamePath, JsonConvert.SerializeObject(data.ToArray()));
        }

        private static void KillProcessAndChildrens(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }
































































        private void Phantom_Load(object sender, EventArgs e)
        {

        }

        private void ExitBtn_MouseHover(object sender, EventArgs e)
        {
            this.Exit.IconColor = System.Drawing.Color.Red;
        }

        private void ExitBtn_MouseLeave(object sender, EventArgs e)
        {
            this.Exit.IconColor = System.Drawing.Color.Gainsboro;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            CloseClient();
            KillProcessAndChildrens(client.Id);
            this.Close();
        }

        private void WindowBtn_Click(object sender, EventArgs e)
        {
            int multiplier=this.Size.Width;
            name = "Window_Button";
            if (!counter_Clicks.ContainsKey(name))
                counter_Clicks.Add(name, 0);

            if (counter_Clicks[name] % 2 == 0)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Window.IconChar= FontAwesome.Sharp.IconChar.WindowRestore;

                multiplier = this.Size.Width / multiplier;
            }

            else
            {
                this.WindowState = FormWindowState.Normal;
                this.Window.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;

                multiplier = multiplier / this.Size.Width;
            }

            Scale_Form(multiplier);

            isMaximized ^= true;

            counter_Clicks[name]++;
        }

        private void MinusBtn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void phantomGhostBtn_MouseHover(object sender, EventArgs e)
        {
            /*
                current_brightness -= 0.5f;
                this.phantomGhost.Image = ImageBrigness.Lighten((Bitmap)this.phantomGhost.Image as Bitmap,-10);
                */
        }

        private void send_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.yellow);
            SwitchPanel(sender);
        }

        private void recv_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.blue);
            UpdateReplies(true);
            SwitchPanel(sender);

        }

        private void home_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.red);
            SwitchPanel(sender);
        }

        private void ip_textBox_Validating(object sender, CancelEventArgs e)
        {
            var type = Uri.CheckHostName(ip_textBox.Text);
            if (type!=UriHostNameType.IPv4)
            {
                try
                {
                    Dns.Resolve(ip_textBox.Text);
                }
                catch
                {
                    ip_textBox.Text = "";
                    MessageBox.Show("Ip/Dns is not Valid, Please correct it!", "Ip/Dns Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void port_textBox_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int port = Convert.ToInt32(port_textBox.Text);

                if (port < 0 || port > 65535)
                    throw new Exception();
            }
            catch
            {
                port_textBox.Text = "";
                MessageBox.Show("Port is not Valid, Please correct it!", "Port Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void to_send_btn_Click(object sender, EventArgs e)
        {
            if(this.ip_textBox.Text=="" || this.port_textBox.Text == "" || request_textBox.Text=="")
                MessageBox.Show("Not all your fields are filled, Please fill them!", "Send Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);

            SendToServer();

            this.ip_textBox.Text = "";
            this.port_textBox.Text = "";
            this.request_textBox.Text = "";
        }

        private void delete_item_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection selectedIndexes = this.listBox.SelectedIndices;
            int j = 0, length = selectedIndexes.Count;

            for (int i=0; i<length;i++)
            {
                this.listBox.Items.RemoveAt(selectedIndexes[0]);
            }

            UpdateReplies(false);

            //removes from json
        }

        private void open_with_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();

            string up = Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, @"..\..\..\..\"))+"Reply\\";
            string name_current,str = "";

            ListBox.SelectedIndexCollection selectedIndexes = this.listBox.SelectedIndices;
            int j = 0, length = selectedIndexes.Count;


            for (int i = 0; i < length; i++)
            {
                name_current = GetFileName(Convert.ToString(this.listBox.Items[selectedIndexes[i]]));

                names.Add(name_current);
            }

            foreach (string name in names)
            {
                name_current = up + name;
                Process.Start(name_current);
            }


        }
    }
}

/*
*/
