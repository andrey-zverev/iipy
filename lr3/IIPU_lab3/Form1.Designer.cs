namespace IIPU_lab3
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Charge = new System.Windows.Forms.ProgressBar();
            this.SleepLable = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SleepTime = new System.Windows.Forms.TextBox();
            this.Status = new System.Windows.Forms.TextBox();
            this.TimeLeft = new System.Windows.Forms.TextBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.NewTime = new System.Windows.Forms.TextBox();
            this.SetTime = new System.Windows.Forms.Button();
            this.ChargeLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Charge
            // 
            this.Charge.Location = new System.Drawing.Point(18, 12);
            this.Charge.Name = "Charge";
            this.Charge.Size = new System.Drawing.Size(223, 23);
            this.Charge.TabIndex = 1;
            this.Charge.Click += new System.EventHandler(this.Charge_Click);
            // 
            // SleepLable
            // 
            this.SleepLable.AutoSize = true;
            this.SleepLable.Location = new System.Drawing.Point(9, 62);
            this.SleepLable.Name = "SleepLable";
            this.SleepLable.Size = new System.Drawing.Size(88, 13);
            this.SleepLable.TabIndex = 2;
            this.SleepLable.Text = " Sleep mode time";
            this.SleepLable.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Power status";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time left";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // SleepTime
            // 
            this.SleepTime.Enabled = false;
            this.SleepTime.Location = new System.Drawing.Point(138, 62);
            this.SleepTime.Name = "SleepTime";
            this.SleepTime.Size = new System.Drawing.Size(100, 20);
            this.SleepTime.TabIndex = 5;
            this.SleepTime.TextChanged += new System.EventHandler(this.SleepTime_TextChanged);
            // 
            // Status
            // 
            this.Status.Enabled = false;
            this.Status.Location = new System.Drawing.Point(138, 88);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(100, 20);
            this.Status.TabIndex = 6;
            // 
            // TimeLeft
            // 
            this.TimeLeft.Enabled = false;
            this.TimeLeft.Location = new System.Drawing.Point(138, 114);
            this.TimeLeft.Name = "TimeLeft";
            this.TimeLeft.Size = new System.Drawing.Size(100, 20);
            this.TimeLeft.TabIndex = 7;
            // 
            // Timer
            // 
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick_1);
            // 
            // NewTime
            // 
            this.NewTime.Location = new System.Drawing.Point(12, 158);
            this.NewTime.Name = "NewTime";
            this.NewTime.Size = new System.Drawing.Size(100, 20);
            this.NewTime.TabIndex = 8;
            // 
            // SetTime
            // 
            this.SetTime.Location = new System.Drawing.Point(138, 158);
            this.SetTime.Name = "SetTime";
            this.SetTime.Size = new System.Drawing.Size(100, 23);
            this.SetTime.TabIndex = 9;
            this.SetTime.Text = "Set";
            this.SetTime.UseVisualStyleBackColor = true;
            this.SetTime.Click += new System.EventHandler(this.SetTime_Click);
            // 
            // ChargeLable
            // 
            this.ChargeLable.AutoSize = true;
            this.ChargeLable.Location = new System.Drawing.Point(125, 16);
            this.ChargeLable.Name = "ChargeLable";
            this.ChargeLable.Size = new System.Drawing.Size(0, 13);
            this.ChargeLable.TabIndex = 10;
            this.ChargeLable.Click += new System.EventHandler(this.ChargeLable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 207);
            this.Controls.Add(this.ChargeLable);
            this.Controls.Add(this.SetTime);
            this.Controls.Add(this.NewTime);
            this.Controls.Add(this.TimeLeft);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.SleepTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SleepLable);
            this.Controls.Add(this.Charge);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar Charge;
        private System.Windows.Forms.Label SleepLable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SleepTime;
        private System.Windows.Forms.TextBox Status;
        private System.Windows.Forms.TextBox TimeLeft;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.TextBox NewTime;
        private System.Windows.Forms.Button SetTime;
        private System.Windows.Forms.Label ChargeLable;
    }
}

