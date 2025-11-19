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
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using Mysqlx.Crud;
using Org.BouncyCastle.Ocsp;
using System.Collections.ObjectModel;

namespace Projekat1
{
    public partial class MainWindow : Window
    {
        static Boolean login = false;
        public static Boolean isEnglish = false;
        
        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            
        }
        public MainWindow(Boolean isEn)
        {
            InitializeComponent();
            if (isEn)
            {
                translateEng();
            }
        }

        public static MySqlConnection OpenConnection()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            string str = config.GetConnectionString("db");
            MySqlConnection conn = new MySqlConnection(str);
            conn.Open();
            return conn;
        }
        void LoginClick(object sender, RoutedEventArgs e)
        {
            login = true;
            SetVisibility();
        }

        void RegisterClick(object sender, RoutedEventArgs e)
        {
            login = false;
            SetVisibility();
        }

        void SetVisibility()
        {
            registerBtn.Visibility = Visibility.Collapsed;
            loginBtn.Visibility = Visibility.Collapsed;
            urnmLbl.Visibility = Visibility.Visible;
            urnmTb.Visibility = Visibility.Visible;
            pswdLbl.Visibility = Visibility.Visible;
            pswdTb.Visibility = Visibility.Visible;
            submitBtn.Visibility = Visibility.Visible;
            back.Visibility = Visibility.Visible;
        }

        void SubmitClick(object sender, RoutedEventArgs e)
        {
            Boolean success = false;
            string usrnm = urnmTb.Text;
            string pswd = pswdTb.Password;
            string theme = "";

            if(usrnm.Length<5 || pswd.Length < 5)
            {
                ErrorWindow dlg;
                if (isEnglish)
                {
                    dlg = new ErrorWindow("Username and password must be at least 5 charachters long!", "Error");
                }
                else
                {
                    dlg = new ErrorWindow("Korisničko ime i lozinka moraju imati minimalno 5 karaktera!", "Greška");
                }
                
                dlg.Owner = this;
                dlg.ShowDialog();
            }
            else
            {
                MySqlConnection conn = OpenConnection();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM hci.korisnik;";
                MySqlDataReader reader = cmd.ExecuteReader();
                if (login)
                {
                    while (reader.Read())
                    {
                        if (usrnm.Equals(reader.GetString(0)) && pswd.Equals(reader.GetString(1)))
                        {
                            theme = reader.GetString(2);
                            success = true;
                            break;
                        }
                    }
                    reader.Close();
                    if (!success)
                    {
                        ErrorWindow dlg;
                        if (isEnglish)
                        {
                            dlg = new ErrorWindow("Wrong username or password!", "Error");
                        }
                        else
                        {
                            dlg = new ErrorWindow("Pogrešno korisničko ime ili lozinka!", "Greška");
                        }
                        dlg.Owner = this;
                        dlg.ShowDialog();
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        if (usrnm.Equals(reader.GetString(0)))
                        {
                            ErrorWindow dlg;
                            if (isEnglish)
                            {
                                dlg = new ErrorWindow("Username already exists!", "Error");
                            }
                            else
                            {
                                dlg = new ErrorWindow("Korisničko ime već postoji!", "Greška");
                            }
                            
                            dlg.Owner = this;
                            dlg.ShowDialog();
                            reader.Close();
                            conn.Close();
                            return;
                        }
                    }
                    reader.Close();
                    MySqlCommand cmd1 = conn.CreateCommand();
                    cmd1.CommandText = "INSERT INTO `hci`.`korisnik` (`korisnickoIme`, `lozinka`, `tema`) VALUES('" + usrnm+"', '"+pswd+ "', 'dark');";
                    cmd1.ExecuteNonQuery();
                    success = true;
                    theme = "dark";
                }
                conn.Close();
            }
            if (success)
            {
                new HomeWindow(usrnm, theme).Show();
                this.Close();
            }
        }
        void BackClick(object sender, RoutedEventArgs e)
        {
            registerBtn.Visibility = Visibility.Visible;
            loginBtn.Visibility = Visibility.Visible;
            urnmLbl.Visibility = Visibility.Collapsed;
            urnmTb.Visibility = Visibility.Collapsed;
            pswdLbl.Visibility = Visibility.Collapsed;
            pswdTb.Visibility = Visibility.Collapsed;
            submitBtn.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Collapsed;
            urnmTb.Text = "";
            pswdTb.Password = "";
        }

        public void translate_Click(object sender, RoutedEventArgs e)
        {
            if (isEnglish)
            {
                isEnglish = false;
                translate.Content = "EN";

                registerBtn.Content = "Registruj se";
                loginBtn.Content = "Prijavi se";
                urnmLbl.Content = "Korisničko ime:";
                pswdLbl.Content = "Lozinka:";
                submitBtn.Content = "Pošalji";

                
            }
            else
            {
                translateEng();
            }
        }

        public void translateEng()
        {
            isEnglish = true;
            translate.Content = "SRB";

            registerBtn.Content = "Register";
            loginBtn.Content = "Log in";
            urnmLbl.Content = "Username:";
            pswdLbl.Content = "Password:";
            submitBtn.Content = "Submit";

            
        }

    }
}
