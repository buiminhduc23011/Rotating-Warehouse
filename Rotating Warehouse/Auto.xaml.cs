using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for Auto.xaml
    /// </summary>
    public partial class Auto : UserControl
    {
        private DispatcherTimer Update_Status;
        public Auto()
        {
            InitializeComponent();
            Loaded += Auto_Loaded;
            Unloaded += Auto_Unloaded;
           
            
        }
        private void Auto_Loaded(object sender, RoutedEventArgs e)
        {
            CreateButtons_1();
            CreateButtons_2();
            Fill_Collor_Bt();
            Update_Status = new DispatcherTimer();
            Update_Status.Interval = TimeSpan.FromMilliseconds(1000);
            Update_Status.Tick += Update_Status_Tick;
            Update_Status.Start();
        }

        private void Auto_Unloaded(object sender, RoutedEventArgs e)
        {
            Update_Status.Stop();
        }
        private void Update_Status_Tick(object sender, EventArgs e)
        {

            Dispatcher.Invoke(() =>
            {
                if (PLC.Isconnect) Status_PLC.Background = new SolidColorBrush(Color.FromRgb(10,255,10));
                else Status_PLC.Background = new SolidColorBrush(Color.FromRgb(255,0, 0));

            });
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            
            clickedButton.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            foreach (StackPanel stackPanel in bt1_StackPanel.Children.OfType<StackPanel>())
            {
                foreach (Button button in stackPanel.Children.OfType<Button>())
                {
                    if (button != clickedButton)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }
            }

            foreach (StackPanel stackPanel in bt2_StackPanel.Children.OfType<StackPanel>())
            {
                foreach (Button button in stackPanel.Children.OfType<Button>())
                {
                    if (button != clickedButton)
                    {
                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }
            }
            string buttonContent = clickedButton.Content.ToString();
            int row, col;
            if (TryParseButtonContent(buttonContent, out row, out col))
            {
                PLC.Write_R_C(short.Parse(row.ToString()), short.Parse(col.ToString()));
            }
            else
            {
                MessageBox.Show("Invalid button content format");
            }
        }
         void Fill_Collor_Bt()
        {
            foreach (StackPanel stackPanel in bt1_StackPanel.Children.OfType<StackPanel>())
            {
                foreach (Button button in stackPanel.Children.OfType<Button>())
                {
                     button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                }
            }

            foreach (StackPanel stackPanel in bt2_StackPanel.Children.OfType<StackPanel>())
            {
                foreach (Button button in stackPanel.Children.OfType<Button>())
                {

                        button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }
            private void CreateButtons_1()
        {
            for (int row = 0; row < 13; row++)
            {
                StackPanel rowStackPanel = new StackPanel();
                rowStackPanel.Orientation = Orientation.Horizontal;

                for (int col = 0; col < 8; col++)
                {
                    Button button = new Button();
                    button.Content = $"R{row + 1}C{col + 1}";
                    button.Name = $"R{row + 1}C{col + 1}";
                    button.Click += Button_Click;
                    button.Width = 90;
                    button.Height = 45;
                    rowStackPanel.Children.Add(button);
                    if (col < 8)
                    {
                        button.Margin = new Thickness(10, 10,0, 0); 
                    }
                }
                bt1_StackPanel.Children.Add(rowStackPanel);
            }
        }
        private void CreateButtons_2()
        {
            for (int row = 0; row < 13; row++)
            {
                StackPanel rowStackPanel = new StackPanel();
                rowStackPanel.Orientation = Orientation.Horizontal;

                for (int col = 0; col < 8; col++)
                {
                    Button button = new Button();
                    button.Content = $"R{row + 14}C{col + 1}";
                    button.Name = $"R{row + 14}C{col + 1}";
                    button.Click += Button_Click;
                    button.Width = 90;
                    button.Height = 45;
                    rowStackPanel.Children.Add(button);
                    if (col < 8)
                    {
                        button.Margin = new Thickness(10, 10, 0, 0);
                    }
                }
                bt2_StackPanel.Children.Add(rowStackPanel);
            }
        }
        private bool TryParseButtonContent(string buttonContent, out int row, out int col)
        {
            row = col = -1;

            try
            {
                // Tách lấy số từ nội dung của nút
                string rowString = buttonContent.Substring(1, buttonContent.IndexOf('C') - 1);
                string colString = buttonContent.Substring(buttonContent.IndexOf('C') + 1);

                if (int.TryParse(rowString, out row) && int.TryParse(colString, out col))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
