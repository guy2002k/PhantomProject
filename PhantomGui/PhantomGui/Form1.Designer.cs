using System;
using System.Threading;
namespace PhantomGui
{
    partial class Phantom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Phantom));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.inbox_icon = new FontAwesome.Sharp.IconPictureBox();
            this.inbox_lbl = new System.Windows.Forms.Label();
            this.open_with = new FontAwesome.Sharp.IconButton();
            this.delete_item = new FontAwesome.Sharp.IconButton();
            this.listBox = new System.Windows.Forms.ListBox();
            this.to_send_btn = new FontAwesome.Sharp.IconButton();
            this.request_textBox = new System.Windows.Forms.TextBox();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.ip_textBox = new System.Windows.Forms.TextBox();
            this.request_label = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.ip_label = new System.Windows.Forms.Label();
            this.select_prot = new System.Windows.Forms.Label();
            this.massageLabel = new System.Windows.Forms.Label();
            this.phantomGhost = new System.Windows.Forms.PictureBox();
            this.menu = new FontAwesome.Sharp.IconButton();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.home = new FontAwesome.Sharp.IconButton();
            this.recv = new FontAwesome.Sharp.IconButton();
            this.send = new FontAwesome.Sharp.IconButton();
            this.Exit = new FontAwesome.Sharp.IconButton();
            this.Window = new FontAwesome.Sharp.IconButton();
            this.Minus = new FontAwesome.Sharp.IconButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inbox_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phantomGhost)).BeginInit();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(13, -14);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(170, 64);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.mainPanel.Controls.Add(this.inbox_icon);
            this.mainPanel.Controls.Add(this.inbox_lbl);
            this.mainPanel.Controls.Add(this.open_with);
            this.mainPanel.Controls.Add(this.delete_item);
            this.mainPanel.Controls.Add(this.listBox);
            this.mainPanel.Controls.Add(this.to_send_btn);
            this.mainPanel.Controls.Add(this.request_textBox);
            this.mainPanel.Controls.Add(this.port_textBox);
            this.mainPanel.Controls.Add(this.ip_textBox);
            this.mainPanel.Controls.Add(this.request_label);
            this.mainPanel.Controls.Add(this.port_label);
            this.mainPanel.Controls.Add(this.ip_label);
            this.mainPanel.Controls.Add(this.select_prot);
            this.mainPanel.Controls.Add(this.massageLabel);
            this.mainPanel.Controls.Add(this.phantomGhost);
            this.mainPanel.Controls.Add(this.menu);
            this.mainPanel.Controls.Add(this.panelMenu);
            this.mainPanel.Location = new System.Drawing.Point(0, 38);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(4);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1301, 713);
            this.mainPanel.TabIndex = 2;
            // 
            // inbox_icon
            // 
            this.inbox_icon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.inbox_icon.IconChar = FontAwesome.Sharp.IconChar.EnvelopeOpenText;
            this.inbox_icon.IconColor = System.Drawing.Color.White;
            this.inbox_icon.IconSize = 43;
            this.inbox_icon.Location = new System.Drawing.Point(127, 22);
            this.inbox_icon.Name = "inbox_icon";
            this.inbox_icon.Size = new System.Drawing.Size(46, 43);
            this.inbox_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.inbox_icon.TabIndex = 17;
            this.inbox_icon.TabStop = false;
            // 
            // inbox_lbl
            // 
            this.inbox_lbl.AutoSize = true;
            this.inbox_lbl.Font = new System.Drawing.Font("Franklin Gothic Demi", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inbox_lbl.ForeColor = System.Drawing.Color.White;
            this.inbox_lbl.Location = new System.Drawing.Point(175, 16);
            this.inbox_lbl.Name = "inbox_lbl";
            this.inbox_lbl.Size = new System.Drawing.Size(145, 49);
            this.inbox_lbl.TabIndex = 16;
            this.inbox_lbl.Text = "Inbox: ";
            // 
            // open_with
            // 
            this.open_with.FlatAppearance.BorderSize = 2;
            this.open_with.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_with.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.open_with.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_with.ForeColor = System.Drawing.Color.White;
            this.open_with.IconChar = FontAwesome.Sharp.IconChar.FolderOpen;
            this.open_with.IconColor = System.Drawing.Color.White;
            this.open_with.IconSize = 35;
            this.open_with.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.open_with.Location = new System.Drawing.Point(1027, 620);
            this.open_with.Name = "open_with";
            this.open_with.Rotation = 0D;
            this.open_with.Size = new System.Drawing.Size(221, 68);
            this.open_with.TabIndex = 15;
            this.open_with.Text = "Open";
            this.open_with.UseVisualStyleBackColor = true;
            this.open_with.Click += new System.EventHandler(this.open_with_Click);
            // 
            // delete_item
            // 
            this.delete_item.FlatAppearance.BorderSize = 2;
            this.delete_item.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_item.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.delete_item.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete_item.ForeColor = System.Drawing.Color.White;
            this.delete_item.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            this.delete_item.IconColor = System.Drawing.Color.White;
            this.delete_item.IconSize = 35;
            this.delete_item.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete_item.Location = new System.Drawing.Point(800, 620);
            this.delete_item.Name = "delete_item";
            this.delete_item.Rotation = 0D;
            this.delete_item.Size = new System.Drawing.Size(221, 68);
            this.delete_item.TabIndex = 14;
            this.delete_item.Text = "Delete";
            this.delete_item.UseVisualStyleBackColor = true;
            this.delete_item.Click += new System.EventHandler(this.delete_item_Click);
            // 
            // listBox
            // 
            this.listBox.BackColor = this.mainPanel.BackColor;
            this.listBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox.Font = new System.Drawing.Font("Franklin Gothic Demi", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 38;
            this.listBox.Items.AddRange(new object[] {
            "From: Google.com, Port: 13456, Type: html",
            "From: Google.com, Port: 13456, Type: html",
            "From: Google.com, Port: 13456, Type: txt",
            "xaa",
            "xaxax",
            "axax",
            "xax",
            "ASF",
            "DSDA",
            "DSA",
            "SDAD",
            "DASD",
            "ADAS",
            "ADSA",
            "ADASD",
            "ADSD"});
            this.listBox.Location = new System.Drawing.Point(127, 77);
            this.listBox.Name = "listBox";
            this.listBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox.Size = new System.Drawing.Size(1121, 534);
            this.listBox.TabIndex = 13;
            this.listBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBox1_DrawItem);
            // 
            // to_send_btn
            // 
            this.to_send_btn.FlatAppearance.BorderSize = 2;
            this.to_send_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.to_send_btn.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.to_send_btn.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.to_send_btn.IconChar = FontAwesome.Sharp.IconChar.PaperPlane;
            this.to_send_btn.IconColor = System.Drawing.Color.Black;
            this.to_send_btn.IconSize = 35;
            this.to_send_btn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.to_send_btn.Location = new System.Drawing.Point(604, 600);
            this.to_send_btn.Name = "to_send_btn";
            this.to_send_btn.Rotation = 0D;
            this.to_send_btn.Size = new System.Drawing.Size(206, 61);
            this.to_send_btn.TabIndex = 12;
            this.to_send_btn.Text = "send";
            this.to_send_btn.UseVisualStyleBackColor = true;
            this.to_send_btn.Click += new System.EventHandler(this.to_send_btn_Click);
            // 
            // request_textBox
            // 
            this.request_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.request_textBox.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.request_textBox.Location = new System.Drawing.Point(318, 270);
            this.request_textBox.Multiline = true;
            this.request_textBox.Name = "request_textBox";
            this.request_textBox.Size = new System.Drawing.Size(807, 246);
            this.request_textBox.TabIndex = 11;
            // 
            // port_textBox
            // 
            this.port_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.port_textBox.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port_textBox.Location = new System.Drawing.Point(939, 159);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(186, 29);
            this.port_textBox.TabIndex = 10;
            this.port_textBox.Validating += new System.ComponentModel.CancelEventHandler(this.port_textBox_Validating);
            // 
            // ip_textBox
            // 
            this.ip_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ip_textBox.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ip_textBox.Location = new System.Drawing.Point(286, 156);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Size = new System.Drawing.Size(484, 29);
            this.ip_textBox.TabIndex = 9;
            this.ip_textBox.Validating += new System.ComponentModel.CancelEventHandler(this.ip_textBox_Validating);
            // 
            // request_label
            // 
            this.request_label.AutoSize = true;
            this.request_label.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.request_label.Location = new System.Drawing.Point(178, 270);
            this.request_label.Name = "request_label";
            this.request_label.Size = new System.Drawing.Size(118, 32);
            this.request_label.TabIndex = 8;
            this.request_label.Text = "Request: ";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port_label.Location = new System.Drawing.Point(860, 156);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(73, 32);
            this.port_label.TabIndex = 7;
            this.port_label.Text = "Port: ";
            // 
            // ip_label
            // 
            this.ip_label.AutoSize = true;
            this.ip_label.Font = new System.Drawing.Font("Franklin Gothic Demi", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ip_label.Location = new System.Drawing.Point(178, 153);
            this.ip_label.Name = "ip_label";
            this.ip_label.Size = new System.Drawing.Size(102, 32);
            this.ip_label.TabIndex = 6;
            this.ip_label.Text = "Ip/Dns: ";
            // 
            // select_prot
            // 
            this.select_prot.AutoSize = true;
            this.select_prot.Font = new System.Drawing.Font("Franklin Gothic Demi", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.select_prot.Location = new System.Drawing.Point(561, 9);
            this.select_prot.Name = "select_prot";
            this.select_prot.Size = new System.Drawing.Size(335, 43);
            this.select_prot.TabIndex = 5;
            this.select_prot.Text = "Choose properties: ";
            // 
            // massageLabel
            // 
            this.massageLabel.AutoSize = true;
            this.massageLabel.Font = new System.Drawing.Font("Franklin Gothic Heavy", 36F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.massageLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.massageLabel.Location = new System.Drawing.Point(375, 537);
            this.massageLabel.Name = "massageLabel";
            this.massageLabel.Size = new System.Drawing.Size(695, 75);
            this.massageLabel.TabIndex = 4;
            this.massageLabel.Text = "Unseen Like A Phantom";
            // 
            // phantomGhost
            // 
            this.phantomGhost.BackColor = this.mainPanel.BackColor;
            this.phantomGhost.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.phantomGhost.Image = ((System.Drawing.Image)(resources.GetObject("phantomGhost.Image")));
            this.phantomGhost.InitialImage = null;
            this.phantomGhost.Location = new System.Drawing.Point(506, 48);
            this.phantomGhost.Name = "phantomGhost";
            this.phantomGhost.Size = new System.Drawing.Size(451, 468);
            this.phantomGhost.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.phantomGhost.TabIndex = 3;
            this.phantomGhost.TabStop = false;
            this.phantomGhost.MouseHover += new System.EventHandler(this.phantomGhostBtn_MouseHover);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.menu.FlatAppearance.BorderSize = 0;
            this.menu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menu.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.menu.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
            this.menu.IconColor = System.Drawing.Color.Gainsboro;
            this.menu.IconSize = 50;
            this.menu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Rotation = 0D;
            this.menu.Size = new System.Drawing.Size(62, 78);
            this.menu.TabIndex = 0;
            this.menu.UseVisualStyleBackColor = false;
            this.menu.Click += new System.EventHandler(this.Settings_Click);
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.panelMenu.Controls.Add(this.home);
            this.panelMenu.Controls.Add(this.recv);
            this.panelMenu.Controls.Add(this.send);
            this.panelMenu.Location = new System.Drawing.Point(0, 78);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(4);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(62, 646);
            this.panelMenu.TabIndex = 2;
            // 
            // home
            // 
            this.home.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.home.FlatAppearance.BorderSize = 0;
            this.home.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.home.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.home.IconChar = FontAwesome.Sharp.IconChar.Home;
            this.home.IconColor = System.Drawing.Color.Gainsboro;
            this.home.IconSize = 50;
            this.home.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.home.Location = new System.Drawing.Point(0, 542);
            this.home.Name = "home";
            this.home.Rotation = 0D;
            this.home.Size = new System.Drawing.Size(62, 78);
            this.home.TabIndex = 5;
            this.home.UseVisualStyleBackColor = false;
            this.home.Click += new System.EventHandler(this.home_Click);
            // 
            // recv
            // 
            this.recv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.recv.Dock = System.Windows.Forms.DockStyle.Top;
            this.recv.FlatAppearance.BorderSize = 0;
            this.recv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.recv.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.recv.Font = new System.Drawing.Font("Franklin Gothic Demi", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recv.ForeColor = System.Drawing.Color.Gainsboro;
            this.recv.IconChar = FontAwesome.Sharp.IconChar.MailBulk;
            this.recv.IconColor = System.Drawing.Color.Gainsboro;
            this.recv.IconSize = 50;
            this.recv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.recv.Location = new System.Drawing.Point(0, 78);
            this.recv.Name = "recv";
            this.recv.Rotation = 0D;
            this.recv.Size = new System.Drawing.Size(62, 78);
            this.recv.TabIndex = 6;
            this.recv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.recv.UseVisualStyleBackColor = false;
            this.recv.Click += new System.EventHandler(this.recv_Click);
            // 
            // send
            // 
            this.send.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.send.Dock = System.Windows.Forms.DockStyle.Top;
            this.send.FlatAppearance.BorderSize = 0;
            this.send.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.send.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.send.Font = new System.Drawing.Font("Franklin Gothic Demi", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.send.ForeColor = System.Drawing.Color.Gainsboro;
            this.send.IconChar = FontAwesome.Sharp.IconChar.PaperPlane;
            this.send.IconColor = System.Drawing.Color.Gainsboro;
            this.send.IconSize = 50;
            this.send.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.send.Location = new System.Drawing.Point(0, 0);
            this.send.Name = "send";
            this.send.Rotation = 0D;
            this.send.Size = new System.Drawing.Size(62, 78);
            this.send.TabIndex = 5;
            this.send.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.send.UseVisualStyleBackColor = false;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // Exit
            // 
            this.Exit.FlatAppearance.BorderSize = 0;
            this.Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Exit.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.Exit.IconChar = FontAwesome.Sharp.IconChar.TimesCircle;
            this.Exit.IconColor = System.Drawing.Color.Gainsboro;
            this.Exit.IconSize = 32;
            this.Exit.Location = new System.Drawing.Point(1254, 1);
            this.Exit.Name = "Exit";
            this.Exit.Rotation = 0D;
            this.Exit.Size = new System.Drawing.Size(47, 49);
            this.Exit.TabIndex = 3;
            this.Exit.UseVisualStyleBackColor = true;
            this.Exit.Click += new System.EventHandler(this.ExitBtn_Click);
            this.Exit.MouseLeave += new System.EventHandler(this.ExitBtn_MouseLeave);
            this.Exit.MouseHover += new System.EventHandler(this.ExitBtn_MouseHover);
            // 
            // Window
            // 
            this.Window.FlatAppearance.BorderSize = 0;
            this.Window.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Window.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.Window.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            this.Window.IconColor = System.Drawing.Color.Gainsboro;
            this.Window.IconSize = 32;
            this.Window.Location = new System.Drawing.Point(1201, 1);
            this.Window.Name = "Window";
            this.Window.Rotation = 0D;
            this.Window.Size = new System.Drawing.Size(47, 49);
            this.Window.TabIndex = 4;
            this.Window.UseVisualStyleBackColor = true;
            this.Window.Click += new System.EventHandler(this.WindowBtn_Click);
            // 
            // Minus
            // 
            this.Minus.FlatAppearance.BorderSize = 0;
            this.Minus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minus.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.Minus.ForeColor = System.Drawing.Color.DimGray;
            this.Minus.IconChar = FontAwesome.Sharp.IconChar.WindowMinimize;
            this.Minus.IconColor = System.Drawing.Color.Gainsboro;
            this.Minus.IconSize = 32;
            this.Minus.Location = new System.Drawing.Point(1148, 1);
            this.Minus.Name = "Minus";
            this.Minus.Rotation = 0D;
            this.Minus.Size = new System.Drawing.Size(47, 35);
            this.Minus.TabIndex = 5;
            this.Minus.UseVisualStyleBackColor = true;
            this.Minus.Click += new System.EventHandler(this.MinusBtn_Click);
            // 
            // Phantom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1301, 737);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.Minus);
            this.Controls.Add(this.Window);
            this.Controls.Add(this.Exit);
            this.Controls.Add(this.pictureBox2);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Phantom";
            this.Load += new System.EventHandler(this.Phantom_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PhantomDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inbox_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phantomGhost)).EndInit();
            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelMenu;
        private FontAwesome.Sharp.IconButton menu;
        private FontAwesome.Sharp.IconButton Exit;
        private FontAwesome.Sharp.IconButton Window;
        private FontAwesome.Sharp.IconButton Minus;
        private System.Windows.Forms.PictureBox phantomGhost;
        private System.Windows.Forms.Label massageLabel;
        private FontAwesome.Sharp.IconButton recv;
        private FontAwesome.Sharp.IconButton send;
        private FontAwesome.Sharp.IconButton home;
        private System.Windows.Forms.Label select_prot;
        private System.Windows.Forms.Label request_label;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label ip_label;
        private System.Windows.Forms.TextBox request_textBox;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.TextBox ip_textBox;
        private FontAwesome.Sharp.IconButton to_send_btn;
        private System.Windows.Forms.ListBox listBox;
        private FontAwesome.Sharp.IconButton open_with;
        private FontAwesome.Sharp.IconButton delete_item;
        private System.Windows.Forms.Label inbox_lbl;
        private FontAwesome.Sharp.IconPictureBox inbox_icon;
    }
}

