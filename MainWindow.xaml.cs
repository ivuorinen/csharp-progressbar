using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace ProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Countdown = 1;

        public MainWindow()
        {
            InitializeComponent();
            Countdown = 1;
            Slider.Value = 1;
            ProgressBar.Value = 0;
            Amount.Text = "1";
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < Countdown; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(1000);
            }
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            UpdateLayout();
        }

        void Worker_DoneWorking(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Collapsed;
            ButtonDone.Visibility = Visibility.Visible;

            UpdateLayout();

            ProgressBar.Value = 0;
            Countdown = 1;
            Slider.Value = 1;
            Console.Beep();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            BtnStart.Visibility = Visibility.Collapsed;
            Slider.Visibility = Visibility.Collapsed;
            Amount.Visibility = Visibility.Collapsed;
            AmountLabel.Visibility = Visibility.Collapsed;

            UpdateLayout();

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_DoneWorking;

            worker.RunWorkerAsync();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double val = Slider.Value;
            Countdown = Convert.ToInt32(val);
            if (ProgressBar != null)
            {
                ProgressBar.Maximum = Countdown;
            }
            Amount.Text = Countdown.ToString();
        }

        private void ButtonDone_Click(object sender, RoutedEventArgs e) => Close();
    }
}
