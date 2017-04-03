namespace FormRobotControlServer
{
    partial class FormRobotControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRobotControl));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox_robotControl = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_saveRefPos = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox_tasks = new System.Windows.Forms.GroupBox();
            this.checkBox_disablePidController = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label_soll_angle = new System.Windows.Forms.Label();
            this.textBox_soll_angle = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_aliveIcon = new System.Windows.Forms.Label();
            this.bWorker_IndicatorLed = new System.ComponentModel.BackgroundWorker();
            this.groupBox_robotControl.SuspendLayout();
            this.groupBox_tasks.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(67, 4);
            // 
            // groupBox_robotControl
            // 
            this.groupBox_robotControl.Controls.Add(this.button1);
            this.groupBox_robotControl.Controls.Add(this.button_saveRefPos);
            this.groupBox_robotControl.Controls.Add(this.button3);
            this.groupBox_robotControl.Location = new System.Drawing.Point(13, 13);
            this.groupBox_robotControl.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_robotControl.Name = "groupBox_robotControl";
            this.groupBox_robotControl.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_robotControl.Size = new System.Drawing.Size(370, 265);
            this.groupBox_robotControl.TabIndex = 40;
            this.groupBox_robotControl.TabStop = false;
            this.groupBox_robotControl.Text = "Actions";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "Stopp";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_Stopp_Click);
            // 
            // button_saveRefPos
            // 
            this.button_saveRefPos.Location = new System.Drawing.Point(10, 24);
            this.button_saveRefPos.Name = "button_saveRefPos";
            this.button_saveRefPos.Size = new System.Drawing.Size(118, 35);
            this.button_saveRefPos.TabIndex = 0;
            this.button_saveRefPos.Text = "Save ref pos";
            this.button_saveRefPos.UseVisualStyleBackColor = true;
            this.button_saveRefPos.Click += new System.EventHandler(this.buttonSaveRefPos_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(10, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 34);
            this.button3.TabIndex = 2;
            this.button3.Text = "Move to";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button_MoveTo_Clicked);
            // 
            // groupBox_tasks
            // 
            this.groupBox_tasks.Controls.Add(this.checkBox_disablePidController);
            this.groupBox_tasks.Location = new System.Drawing.Point(399, 13);
            this.groupBox_tasks.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_tasks.Name = "groupBox_tasks";
            this.groupBox_tasks.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_tasks.Size = new System.Drawing.Size(370, 265);
            this.groupBox_tasks.TabIndex = 41;
            this.groupBox_tasks.TabStop = false;
            this.groupBox_tasks.Text = "Modes";
            // 
            // checkBox_disablePidController
            // 
            this.checkBox_disablePidController.AutoSize = true;
            this.checkBox_disablePidController.Location = new System.Drawing.Point(7, 24);
            this.checkBox_disablePidController.Name = "checkBox_disablePidController";
            this.checkBox_disablePidController.Size = new System.Drawing.Size(168, 21);
            this.checkBox_disablePidController.TabIndex = 3;
            this.checkBox_disablePidController.Text = "Disable PID Controller";
            this.checkBox_disablePidController.UseVisualStyleBackColor = true;
            this.checkBox_disablePidController.CheckedChanged += new System.EventHandler(this.checkChanged_disablePidController);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(189, 23);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 36);
            this.button4.TabIndex = 6;
            this.button4.Text = "Ok";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button_Ok_Click);
            // 
            // label_soll_angle
            // 
            this.label_soll_angle.AutoSize = true;
            this.label_soll_angle.Location = new System.Drawing.Point(7, 33);
            this.label_soll_angle.Name = "label_soll_angle";
            this.label_soll_angle.Size = new System.Drawing.Size(70, 17);
            this.label_soll_angle.TabIndex = 5;
            this.label_soll_angle.Text = "Soll angle";
            // 
            // textBox_soll_angle
            // 
            this.textBox_soll_angle.Location = new System.Drawing.Point(83, 30);
            this.textBox_soll_angle.Name = "textBox_soll_angle";
            this.textBox_soll_angle.Size = new System.Drawing.Size(100, 22);
            this.textBox_soll_angle.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_aliveIcon);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.label_soll_angle);
            this.groupBox1.Controls.Add(this.textBox_soll_angle);
            this.groupBox1.Location = new System.Drawing.Point(13, 286);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(756, 154);
            this.groupBox1.TabIndex = 42;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TEST";
            // 
            // label_aliveIcon
            // 
            this.label_aliveIcon.AutoSize = true;
            this.label_aliveIcon.Location = new System.Drawing.Point(7, 71);
            this.label_aliveIcon.MinimumSize = new System.Drawing.Size(40, 0);
            this.label_aliveIcon.Name = "label_aliveIcon";
            this.label_aliveIcon.Size = new System.Drawing.Size(40, 17);
            this.label_aliveIcon.TabIndex = 4;
            // 
            // FormRobotControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_tasks);
            this.Controls.Add(this.groupBox_robotControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(800, 500);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormRobotControl";
            this.Text = "RobotControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabase_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDatabase_Closed);
            this.Load += new System.EventHandler(this.FormDatabase_Load);
            this.groupBox_robotControl.ResumeLayout(false);
            this.groupBox_tasks.ResumeLayout(false);
            this.groupBox_tasks.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBox_robotControl;
        private System.Windows.Forms.GroupBox groupBox_tasks;
        private System.Windows.Forms.Button button_saveRefPos;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox_disablePidController;
        private System.Windows.Forms.Label label_soll_angle;
        private System.Windows.Forms.TextBox textBox_soll_angle;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_aliveIcon;
        private System.ComponentModel.BackgroundWorker bWorker_IndicatorLed;
    }
}