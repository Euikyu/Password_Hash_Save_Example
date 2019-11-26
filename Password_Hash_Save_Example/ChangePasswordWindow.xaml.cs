using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace Password_Hash_Save_Example
{
    /// <summary>
    /// ChangePasswordWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly string SALT;
        public ChangePasswordWindow(string salt)
        {
            InitializeComponent();
            //load main salt
            this.SALT = salt;
        }

        //confirm changing password
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var id = id_TextBox.Text;
            var passwd = currentPasswd_PasswordBox.Password;

            //checking right id n password
            if (CheckID(id) && CheckPassword(SALT, passwd))
            {
                var changePasswd = changePasswd_PasswordBox.Password;
                var confirm = confirmPasswd_PasswordBox.Password;

                //checking integrity
                if(confirm == changePasswd)
                {
                    ChangePassword(SALT, changePasswd);
                    MessageBox.Show("Change password successfully.");
                    Close();
                }
                else
                {
                    MessageBox.Show("Different password between changing one and confirm.");
                }
            }
            else
            {
                var msg = CheckID(id) ? "Wrong password." : "Wrong ID.";
                MessageBox.Show(msg);
            }

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
        private void ChangePassword(string salt, string password)
        {
            var hash = Encoding.ASCII.GetBytes(SALT + password);
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1hash = sha1.ComputeHash(hash);
            var hashedPassword = Encoding.ASCII.GetString(sha1hash);
            Properties.Settings.Default.passwd = hashedPassword;

            Properties.Settings.Default.Save();
        }
    }
}
