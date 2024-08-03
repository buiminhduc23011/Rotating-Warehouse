using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;
using System.IO.Ports;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Rotating_Warehouse
{
    /// <summary>
    /// Interaction logic for Setting_Control.xaml
    /// </summary>
    public partial class Setting_Control : UserControl
    {
        Path Path = new Path();
        public Setting_Control()
        {
            InitializeComponent();
        }

        private void comPortComboBox_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();

            comPortComboBox.Items.Clear();
            foreach (string portName in portNames)
            {
                comPortComboBox.Items.Add(portName);
            }
        }
        private void bt_Save_Click(object sender, RoutedEventArgs e)
        {
            if (comPortComboBox.Text != "" )
            {
                object data_Setting = new
                {
                    Port_PLC = comPortComboBox.Text,
                };
                string json = JsonSerializer.Serialize(data_Setting);
                File.WriteAllText(Path.Setting, json);
                MessageBox.Show("Vui Lòng Thoát App Để Áp Dụng Cài Đặt");
            }
            else
            {
                MessageBox.Show("Vui Lòng điền đủ thông tin cài đặt");
            }
        }
    }
}
