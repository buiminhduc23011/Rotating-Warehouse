using System;
using System.Collections.Generic;
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

namespace Connect_PLC_Web_KhoXoay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string ipAddress = ipTextBox.Text;
            int port;

            if (Int32.TryParse(portTextBox.Text, out port))
            {
                // Thực hiện xử lý kết nối với IP và port đã nhập
                try
                {
                    // Sử dụng IP và port ở đây để kết nối đến server
                    // Ví dụ: var endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
                    //         // Kết nối đến endpoint và thực hiện các thao tác khác
                    //         // ...

                   // MessageBox.Show("Kết nối thành công!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Port không hợp lệ!");
            }
        }
    }
}
