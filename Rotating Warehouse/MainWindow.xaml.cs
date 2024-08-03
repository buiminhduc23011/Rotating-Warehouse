using Rotating_Warehouse.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Rotating_Warehouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Path path = new Path();
        BackgroundWorker worker = new BackgroundWorker();
        private DispatcherTimer Update_Status;
        private bool shouldStop = false;
        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        //
        Auto auto = new Auto();
        Report_Control Report_Control = new Report_Control();
        History_Control History_Control = new History_Control();
        Setting_Control Setting_Control = new Setting_Control();
        User_Control User_Control = new User_Control();
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            ini.init(path.Setting);
            PLC.Connect();
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
            worker.RunWorkerAsync();
            //
            Update_Status = new DispatcherTimer();
            Update_Status.Interval = TimeSpan.FromMilliseconds(1000);
            Update_Status.Tick += Update_Status_Tick;
            Update_Status.Start();
            //
            Pannel_Monitor.Children.Add(auto);
            bt_Auto.Background = new SolidColorBrush(Colors.Gray);
            bt_report.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_history.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_User.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PLC.Disconnect();
        }
        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            
            while (!shouldStop)
            {
                try
                {
                    
                    Dispatcher.Invoke(() =>
                    {

                    });
                }
                catch
                {
                }
                Thread.Sleep(100);
            }
        }
        private void Update_Status_Tick(object sender, EventArgs e)
        {
            if (PLC.Isconnect == false) PLC.Reconnect();
            Dispatcher.Invoke(() =>
            {
                Update_Screen();
            });
        }
        void Update_Screen()
        {
            float cpuUsage = cpuCounter.NextValue();
            string formattedCpuUsage = cpuUsage.ToString("F2") + "%";
            Per_CPU.Content = formattedCpuUsage;
            System.DateTime dateTime = System.DateTime.Now;
            string formattedDate = dateTime.ToString("dd/MM/yyyy");
            lb_Day.Content = formattedDate;
            string formattedtime = dateTime.ToString("HH:mm:ss");
            lb_Time.Content = formattedtime;
            //
            lb_Shifts.Content = Handel_Data.GetShifts();
        }
        private void bt_Auto_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Colors.Gray);
            bt_report.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_history.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_User.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(auto);
        }

        private void bt_report_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_report.Background = new SolidColorBrush(Colors.Gray);
            bt_history.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_User.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(Report_Control);
        }

        private void bt_history_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_report.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_history.Background = new SolidColorBrush(Colors.Gray);
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_User.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(History_Control);
        }

        private void bt_Setting_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_report.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_history.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Colors.Gray);
            bt_User.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(Setting_Control);
        }

        private void bt_User_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_report.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_history.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_User.Background = new SolidColorBrush(Colors.Gray);
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(User_Control);
        }
    }
}
