﻿namespace FormRobotControlServer
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
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label_stepForward = new System.Windows.Forms.Label();
            this.checkBox_all_motors = new System.Windows.Forms.CheckBox();
            this.label_motorId = new System.Windows.Forms.Label();
            this.checkBox_disablePidController = new System.Windows.Forms.CheckBox();
            this.textBox_motorId = new System.Windows.Forms.TextBox();
            this.label_soll_angle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_soll_angle = new System.Windows.Forms.TextBox();
            this.button_saveRefPos = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox_tasks = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoMode = new System.Windows.Forms.CheckBox();
            this.label_aliveIcon_1 = new System.Windows.Forms.Label();
            this.bWorker_IndicatorLed = new System.ComponentModel.BackgroundWorker();
            this.label_aliveIcon_2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_soll_speed = new System.Windows.Forms.TextBox();
            this.groupBox_robotControl.SuspendLayout();
            this.groupBox_tasks.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.groupBox_robotControl.Controls.Add(this.label3);
            this.groupBox_robotControl.Controls.Add(this.textBox_soll_speed);
            this.groupBox_robotControl.Controls.Add(this.button2);
            this.groupBox_robotControl.Controls.Add(this.button4);
            this.groupBox_robotControl.Controls.Add(this.label_stepForward);
            this.groupBox_robotControl.Controls.Add(this.checkBox_all_motors);
            this.groupBox_robotControl.Controls.Add(this.label_motorId);
            this.groupBox_robotControl.Controls.Add(this.checkBox_disablePidController);
            this.groupBox_robotControl.Controls.Add(this.textBox_motorId);
            this.groupBox_robotControl.Controls.Add(this.label_soll_angle);
            this.groupBox_robotControl.Controls.Add(this.button1);
            this.groupBox_robotControl.Controls.Add(this.textBox_soll_angle);
            this.groupBox_robotControl.Controls.Add(this.button_saveRefPos);
            this.groupBox_robotControl.Controls.Add(this.button3);
            this.groupBox_robotControl.Location = new System.Drawing.Point(13, 13);
            this.groupBox_robotControl.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_robotControl.Name = "groupBox_robotControl";
            this.groupBox_robotControl.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_robotControl.Size = new System.Drawing.Size(387, 429);
            this.groupBox_robotControl.TabIndex = 40;
            this.groupBox_robotControl.TabStop = false;
            this.groupBox_robotControl.Text = "Actions / Modes";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(155, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(118, 34);
            this.button2.TabIndex = 8;
            this.button2.Text = "Step forward";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button_stepForward_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(7, 309);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(49, 79);
            this.button4.TabIndex = 47;
            this.button4.Text = "Save act pos";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonSaveActPos_Click);
            // 
            // label_stepForward
            // 
            this.label_stepForward.AutoSize = true;
            this.label_stepForward.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label_stepForward.Location = new System.Drawing.Point(151, 17);
            this.label_stepForward.MinimumSize = new System.Drawing.Size(127, 44);
            this.label_stepForward.Name = "label_stepForward";
            this.label_stepForward.Size = new System.Drawing.Size(127, 44);
            this.label_stepForward.TabIndex = 46;
            // 
            // checkBox_all_motors
            // 
            this.checkBox_all_motors.AutoSize = true;
            this.checkBox_all_motors.Location = new System.Drawing.Point(278, 97);
            this.checkBox_all_motors.Name = "checkBox_all_motors";
            this.checkBox_all_motors.Size = new System.Drawing.Size(45, 21);
            this.checkBox_all_motors.TabIndex = 9;
            this.checkBox_all_motors.Text = "All";
            this.checkBox_all_motors.UseVisualStyleBackColor = true;
            // 
            // label_motorId
            // 
            this.label_motorId.AutoSize = true;
            this.label_motorId.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_motorId.Location = new System.Drawing.Point(6, 22);
            this.label_motorId.Name = "label_motorId";
            this.label_motorId.Size = new System.Drawing.Size(67, 17);
            this.label_motorId.TabIndex = 7;
            this.label_motorId.Text = "Motor id";
            // 
            // checkBox_disablePidController
            // 
            this.checkBox_disablePidController.AutoSize = true;
            this.checkBox_disablePidController.Location = new System.Drawing.Point(9, 282);
            this.checkBox_disablePidController.Name = "checkBox_disablePidController";
            this.checkBox_disablePidController.Size = new System.Drawing.Size(168, 21);
            this.checkBox_disablePidController.TabIndex = 3;
            this.checkBox_disablePidController.Text = "Disable PID Controller";
            this.checkBox_disablePidController.UseVisualStyleBackColor = true;
            this.checkBox_disablePidController.CheckedChanged += new System.EventHandler(this.checkChanged_disablePidController);
            // 
            // textBox_motorId
            // 
            this.textBox_motorId.Location = new System.Drawing.Point(7, 42);
            this.textBox_motorId.Name = "textBox_motorId";
            this.textBox_motorId.Size = new System.Drawing.Size(100, 22);
            this.textBox_motorId.TabIndex = 6;
            // 
            // label_soll_angle
            // 
            this.label_soll_angle.AutoSize = true;
            this.label_soll_angle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_soll_angle.Location = new System.Drawing.Point(6, 69);
            this.label_soll_angle.Name = "label_soll_angle";
            this.label_soll_angle.Size = new System.Drawing.Size(80, 17);
            this.label_soll_angle.TabIndex = 5;
            this.label_soll_angle.Text = "Soll angle";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 129);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 34);
            this.button1.TabIndex = 3;
            this.button1.Text = "Stopp";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_Stopp_Click);
            // 
            // textBox_soll_angle
            // 
            this.textBox_soll_angle.Location = new System.Drawing.Point(7, 89);
            this.textBox_soll_angle.Name = "textBox_soll_angle";
            this.textBox_soll_angle.Size = new System.Drawing.Size(100, 22);
            this.textBox_soll_angle.TabIndex = 4;
            // 
            // button_saveRefPos
            // 
            this.button_saveRefPos.Location = new System.Drawing.Point(62, 309);
            this.button_saveRefPos.Name = "button_saveRefPos";
            this.button_saveRefPos.Size = new System.Drawing.Size(49, 79);
            this.button_saveRefPos.TabIndex = 0;
            this.button_saveRefPos.Text = "Save ref pos";
            this.button_saveRefPos.UseVisualStyleBackColor = true;
            this.button_saveRefPos.Click += new System.EventHandler(this.buttonSaveRefPos_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(154, 89);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 34);
            this.button3.TabIndex = 2;
            this.button3.Text = "Move to";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button_MoveTo_Clicked);
            // 
            // groupBox_tasks
            // 
            this.groupBox_tasks.Controls.Add(this.checkBox_AutoMode);
            this.groupBox_tasks.Location = new System.Drawing.Point(408, 13);
            this.groupBox_tasks.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_tasks.Name = "groupBox_tasks";
            this.groupBox_tasks.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_tasks.Size = new System.Drawing.Size(223, 429);
            this.groupBox_tasks.TabIndex = 41;
            this.groupBox_tasks.TabStop = false;
            this.groupBox_tasks.Text = "Settings";
            // 
            // checkBox_AutoMode
            // 
            this.checkBox_AutoMode.AutoSize = true;
            this.checkBox_AutoMode.Location = new System.Drawing.Point(7, 24);
            this.checkBox_AutoMode.Name = "checkBox_AutoMode";
            this.checkBox_AutoMode.Size = new System.Drawing.Size(98, 21);
            this.checkBox_AutoMode.TabIndex = 4;
            this.checkBox_AutoMode.Text = "Auto mode";
            this.checkBox_AutoMode.UseVisualStyleBackColor = true;
            this.checkBox_AutoMode.CheckedChanged += new System.EventHandler(this.checkChanged_autoMode);
            // 
            // label_aliveIcon_1
            // 
            this.label_aliveIcon_1.AutoSize = true;
            this.label_aliveIcon_1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label_aliveIcon_1.Location = new System.Drawing.Point(69, 35);
            this.label_aliveIcon_1.MinimumSize = new System.Drawing.Size(40, 0);
            this.label_aliveIcon_1.Name = "label_aliveIcon_1";
            this.label_aliveIcon_1.Size = new System.Drawing.Size(40, 17);
            this.label_aliveIcon_1.TabIndex = 4;
            // 
            // label_aliveIcon_2
            // 
            this.label_aliveIcon_2.AutoSize = true;
            this.label_aliveIcon_2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.label_aliveIcon_2.Location = new System.Drawing.Point(69, 63);
            this.label_aliveIcon_2.MinimumSize = new System.Drawing.Size(40, 0);
            this.label_aliveIcon_2.Name = "label_aliveIcon_2";
            this.label_aliveIcon_2.Size = new System.Drawing.Size(40, 17);
            this.label_aliveIcon_2.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label_aliveIcon_2);
            this.groupBox2.Controls.Add(this.label_aliveIcon_1);
            this.groupBox2.Location = new System.Drawing.Point(639, 13);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(130, 429);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Motor States";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 45;
            this.label2.Text = "Motor 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 44;
            this.label1.Text = "Motor 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 49;
            this.label3.Text = "Soll speed";
            // 
            // textBox_soll_speed
            // 
            this.textBox_soll_speed.Location = new System.Drawing.Point(8, 137);
            this.textBox_soll_speed.Name = "textBox_soll_speed";
            this.textBox_soll_speed.Size = new System.Drawing.Size(100, 22);
            this.textBox_soll_speed.TabIndex = 48;
            // 
            // FormRobotControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453);
            this.Controls.Add(this.groupBox2);
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
            this.groupBox_robotControl.PerformLayout();
            this.groupBox_tasks.ResumeLayout(false);
            this.groupBox_tasks.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label_aliveIcon_1;
        private System.ComponentModel.BackgroundWorker bWorker_IndicatorLed;
        private System.Windows.Forms.Label label_aliveIcon_2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_motorId;
        private System.Windows.Forms.TextBox textBox_motorId;
        private System.Windows.Forms.CheckBox checkBox_AutoMode;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_all_motors;
        private System.Windows.Forms.Label label_stepForward;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_soll_speed;
    }
}