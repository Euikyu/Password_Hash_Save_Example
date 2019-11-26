using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
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

namespace Password_Hash_Save_Example
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SALT = "HASH_SALT";

        public MainWindow()
        {
            InitializeComponent();

            //first start (execute when empty info)
            if (Properties.Settings.Default.id == null || Properties.Settings.Default.id == "")
            {
                //id = testID
                Properties.Settings.Default.id = "testID";

                //password = test1234
                var hash = Encoding.ASCII.GetBytes(SALT + "test1234");
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1hash = sha1.ComputeHash(hash);
                var hashedPassword = Encoding.ASCII.GetString(sha1hash);
                Properties.Settings.Default.passwd = hashedPassword;

                //save
                Properties.Settings.Default.Save();
            }
        }
        
        //Save button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //bring text in Textbox
            var id = id_TextBox.Text;
            var passwd = passwd_PasswordBox.Password;

            //checking right value
            if (CheckID(id) && CheckPassword(SALT,passwd))
            {
                MessageBox.Show("Logged in successfully.");
            }
            else
            {
                var msg = CheckID(id) ? "Wrong password." : "Wrong ID.";
                MessageBox.Show(msg);
            }
        }
        //Change Password
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow w = new ChangePasswordWindow(SALT);
            w.ShowDialog();
            Properties.Settings.Default.Reload();
        }

        private bool CheckID(string id)
        {
            return (Properties.Settings.Default.id == id);
        }
        private bool CheckPassword(string salt, string password)
        {
            var hash = Encoding.ASCII.GetBytes(salt + password);
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1hash = sha1.ComputeHash(hash);
            var hashedPassword = Encoding.ASCII.GetString(sha1hash);

            return (Properties.Settings.Default.passwd == hashedPassword);
        }

    }
}
