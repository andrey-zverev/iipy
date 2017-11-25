using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IIPU_lab3
{
    public partial class Form1 : Form
    {
        private static readonly Work work = new Work();
        private static readonly Regex timeExpression = new Regex(@"^\d+$");
        public Form1()
        {
            InitializeComponent();
        }

        private void Updating()
        {
            ChargeLable.Text = work.Percent;
            Charge.Value = Convert.ToInt32(work.Percent.TrimEnd('%'), 10);
            SleepTime.Text = work.CurrentTime.ToString();
            Status.Text = work.PowerStatus;
            EnableInput((work.PowerStatus == "Online") ? false : true);
            TimeLeft.Text = work.Remaining;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Updating();
            Timer.Enabled = true;
        }

        private void EnableInput(bool enable)
        {
            NewTime.Enabled = enable;
            SetTime.Enabled = enable;
        }

        private void UpdateWork(bool timeChaged = false)
        {
            work.UpdateInfo(timeChaged);
            Updating();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void SleepTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void ChargeLable_Click(object sender, EventArgs e)
        {

        }

        private void SetTime_Click(object sender, EventArgs e)
        {
            if (timeExpression.Match(NewTime.Text).Value != String.Empty)
            {
                work.SetNewTime(Convert.ToInt32(NewTime.Text));
                UpdateWork(true);
            }
            NewTime.Text = String.Empty;
        }

        private void Charge_Click(object sender, EventArgs e)
        {

        }

        private void Timer_Tick_1(object sender, EventArgs e)
        {
            UpdateWork();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            work.SetNewTime(work.PreviousTime);
        }
    }
}
