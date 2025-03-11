using BAITIEULUAN.DataContext;
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

namespace BAITIEULUAN
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

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            SignUp signup = new SignUp();
            this.Hide();
            signup.ShowDialog();
            this.Show();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var table = DataProvider.Instance.ExecuteQuery("SELECT * FROM Account WHERE TENTK = @username AND MATKHAU = @password", new object[] { txtUsername.Text, txtPassword.Password });
            if(table.Rows.Count > 0)
            {
                var row = table.Rows[0];
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Home home = new Home(row["ID"].ToString());
                this.Hide();
                home.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
