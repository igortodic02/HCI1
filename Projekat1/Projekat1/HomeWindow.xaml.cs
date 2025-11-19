using MaterialDesignColors.ColorManipulation;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekat1
{
    public partial class HomeWindow : Window
    {
        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Hours { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Minutes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Halls { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> D { get; } = new ObservableCollection<string>();
        
        public string  Usrnm { get; set; } = "";
        public string Admin { get; set; } = "";
        public string IsAdmin { get; set; } = "Collapsed";
        public string IsKorisnik { get; set; } = "Collapsed";
        public string Hour { get; set; } = "";
        public string Minute { get; set; } = "";
        public string Hall { get; set; } = "";
        public string path { get; set; } = "";
        public string HourUpdate { get; set; } = "";
        public string MinuteUpdate{ get; set; } = "";
        public string HallUpdate { get; set; } = "";
        public string pathUpdate { get; set; } = "";
        public Border bd2a;
        public Border bd2b;
        public int theme = -1;

        public ObservableCollection<string> Themes { get; } = new ObservableCollection<string>();
        public ObservableCollection<string> Filters { get; } = new ObservableCollection<string>();
        public List<string> Zanrovi = new List<string>();
        public ObservableCollection<string> Sorts { get; } = new ObservableCollection<string>();

        public Boolean pathUpdated { get; set; } = false;
        public HomeWindow(string u, string theme)
        {
            InitializeComponent();
            
            DataContext = this;

            if (MainWindow.isEnglish)
            {
                translate();
                Themes.Add("Dark mode");
                Themes.Add("Bright mode");
                Themes.Add("Colorful mode");

                Items.Add("Action");
                Items.Add("Animated");
                Items.Add("Adventure");
                Items.Add("Biography");
                Items.Add("Drama");
                Items.Add("Documentary");
                Items.Add("Fantasy");
                Items.Add("Horror");
                Items.Add("Historical");
                Items.Add("Comedy");
                Items.Add("Crime");
                Items.Add("Mystery");
                Items.Add("Musical");
                Items.Add("Science Fiction");
                Items.Add("Family");
                Items.Add("War");
                Items.Add("Romance");
                Items.Add("Sports");
                Items.Add("Thriller");
                Items.Add("Western");

                Filters.Add("No filter");
                foreach (var item in Items)
                {
                    Filters.Add(item);
                }
                Filters.Add("2D");
                Filters.Add("3D");

                Sorts.Add("By lenght");
                Sorts.Add("By default");
            }
            else
            {
                Themes.Add("Tamni režim");
                Themes.Add("Svijetli režim");
                Themes.Add("Šareni režim");

                Items.Add("Akcija");
                Items.Add("Animirani");
                Items.Add("Avantura");
                Items.Add("Biografija");
                Items.Add("Drama");
                Items.Add("Dokumentarac");
                Items.Add("Fantazija");
                Items.Add("Horor");
                Items.Add("Istorijski");
                Items.Add("Komedija");
                Items.Add("Krimi");
                Items.Add("Misterija");
                Items.Add("Mjuzikl");
                Items.Add("Naučna fantastika");
                Items.Add("Porodični");
                Items.Add("Ratni");
                Items.Add("Romatnika");
                Items.Add("Sport");
                Items.Add("Triler");
                Items.Add("Vestern");

                Filters.Add("Bez filtera");
                foreach (var item in Items)
                {
                    Filters.Add(item);
                }

                Filters.Add("2D");
                Filters.Add("3D");

                Sorts.Add("Po trajanju");
                Sorts.Add("Podrazumijevano");
            }
            Zanrovi.Add("Akcija");
            Zanrovi.Add("Animirani");
            Zanrovi.Add("Avantura");
            Zanrovi.Add("Biografija");
            Zanrovi.Add("Drama");
            Zanrovi.Add("Dokumentarac");
            Zanrovi.Add("Fantazija");
            Zanrovi.Add("Horor");
            Zanrovi.Add("Istorijski");
            Zanrovi.Add("Komedija");
            Zanrovi.Add("Krimi");
            Zanrovi.Add("Misterija");
            Zanrovi.Add("Mjuzikl");
            Zanrovi.Add("Naučna fantastika");
            Zanrovi.Add("Porodični");
            Zanrovi.Add("Ratni");
            Zanrovi.Add("Romatnika");
            Zanrovi.Add("Sport");
            Zanrovi.Add("Triler");
            Zanrovi.Add("Vestern");
            if (theme == "dark")
            {
                themesLb.SelectedIndex = 0;
                this.theme = 0;
            }
            if (theme == "bright")
            {
                themesLb.SelectedIndex = 1;
                this.theme = 1;
            }         
            if (theme == "color")
            {
                themesLb.SelectedIndex = 2;
                this.theme = 2;
            }



            for (int i = 9; i < 24; i++)
            {
                Hours.Add(i.ToString());
            }
            for (int i = 0; i < 60; i+=5)
            {
                string s = i.ToString();
                if(s.Length < 2)
                    s= "0" + s;
                Minutes.Add(s);
            }

            D.Add("2D");
            D.Add("3D");

            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM hci.sala;";
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Halls.Add(reader.GetInt64(0).ToString());
            }    
            reader.Close();
            conn.Close();

            Usrnm = u;
            usrnmLbl.Content = u;
            if (u == "admin")
            {
                if (MainWindow.isEnglish)
                {
                    Admin = "Create a new movie";
                }
                else
                {
                    Admin = "Kreiraj novi film";
                }
                
                IsAdmin= "Visible";
            }

            else
            {
                if (MainWindow.isEnglish)
                {
                    Admin = "My transactions";
                }
                else
                {
                    Admin = "Moje transakcije";
                }
                
                IsKorisnik = "Visible";
            }
            LoadMoviesFromDb();
        }

        private void TransactionCreateClick(object sender, RoutedEventArgs e)
        {
            MainPageBtn.IsEnabled = true;
            TransactionCreateBtn.IsEnabled = false;
            if ("admin".Equals(Usrnm))
            {
                HomePageScroll.Visibility = Visibility.Collapsed;
                CreateMovieScroll.Visibility = Visibility.Visible;
                PickedMovieScroll.Visibility = Visibility.Collapsed;

            }
            else
            {
                HomePageScroll.Visibility = Visibility.Collapsed;
                PickedMovieScroll.Visibility = Visibility.Collapsed;
                ViewTransactionsScroll.Visibility = Visibility.Visible;

                ViewTransactionsPanel.Children.Clear();
                MySqlConnection conn = MainWindow.OpenConnection();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM hci.kupovina where korisnickoIme = '" + Usrnm + "';";
                MySqlConnection connDeleted = MainWindow.OpenConnection();
                MySqlCommand deleted = connDeleted.CreateCommand();
                deleted.CommandText = "SELECT * FROM hci.kupovinadeleted where korisnickoIme = '" + Usrnm + "';";
                MySqlDataReader reader = cmd.ExecuteReader();
                MySqlDataReader readerDeleted = deleted.ExecuteReader();
                if (!reader.HasRows && !readerDeleted.HasRows)
                {
                    Label lb = new Label();
                    if (MainWindow.isEnglish)
                    {
                        lb.Content = "No transactions have been made";
                    }
                    else
                    {
                        lb.Content = "Nije izvršena nijedna transakcija";
                    }
                    

                    lb.FontSize = 30;
                    lb.FontWeight = FontWeights.Bold;
                    lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                    lb.HorizontalAlignment = HorizontalAlignment.Center;
                    lb.Height = 100;
                    lb.Margin = new Thickness(0, 300, 0, 0);
                    lb.VerticalAlignment = VerticalAlignment.Top;
                    lb.VerticalContentAlignment = VerticalAlignment.Center;
                    lb.Foreground = Brushes.White;
                    changeTheme(lb);
                    ViewTransactionsPanel.Children.Add(lb);
                    return;
                }
                var sp = new StackPanel
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 1465,
                    Orientation = Orientation.Vertical, 
                    Margin= new Thickness(0,30,0,30)
                };
                var sp0 = new StackPanel
                {
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 1465,
                    VerticalAlignment = VerticalAlignment.Center,
                    Orientation= Orientation.Horizontal
                };
                List<string> list = new List<string>();
                if (MainWindow.isEnglish)
                {
                    list.AddRange(new[] { "Movie", "Date", " Time", " Hall number", "  Seats", "            Price" });
                }
                else
                {
                    list.AddRange(new[] { "Film", "Datum", "Vrijeme", "Broj sale", "  Sjedista", "          Cijena" });
                }
               
                foreach (string s in list)
                {
                    Label lb1 = new Label()
                    {
                        Content = s,
                        FontSize = 18,
                        Width = this.theme==2?((s == "  Sjedista" || s== "  Seats") ? 420: ((s == "            Price" || s == "          Cijena")?180:203)):((s == "  Sjedista" || s== "  Seats") ? 420: 203),
                        FontWeight = FontWeights.Bold,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Top,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Foreground = Brushes.White
                    };
                    changeTheme(lb1);
                    sp0.Children.Add(lb1);
                }

                var bd1 = new Border
                {
                    Width = 1465,
                    Padding = new Thickness(15),
                    CornerRadius = new CornerRadius(8, 8, 0, 0),
                    Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = sp0
                };
                sp.Children.Add(bd1);

                while (reader.Read())
                {
                    string sjedista = reader.GetString(1);
                    int cijena = reader.GetInt32(2);
                    DateTime datum = reader.GetDateTime(3);
                    int terminId = reader.GetInt32(4);

                    MySqlConnection conn1 = MainWindow.OpenConnection();
                    MySqlCommand cmd1 = conn1.CreateCommand();
                    cmd1.CommandText = "SELECT * FROM hci.termin where terminId = "+terminId+";";
                    MySqlDataReader reader1 = cmd1.ExecuteReader();

                    reader1.Read();
                    string nazivFilma = reader1.GetString(1);
                    int brSale = reader1.GetInt32(2);
                    string tm = reader1.GetTimeSpan(3).ToString().Substring(0, 5); 
                    reader1.Close();
                    conn1.Close();

                    List<string> cells = new List<string>();
                    cells.AddRange(new[] { nazivFilma, datum.ToString("yyyy-MM-dd"), tm, brSale.ToString(), sjedista, cijena.ToString()+",00 KM" });

                    var spRow = new StackPanel
                    {
                        Width= 1465,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Orientation = Orientation.Horizontal,

                    };

                    foreach (string cell in cells)
                    {
                        var bd2 = new Border
                        {
                            Height = 80,                   
                            Background = Brushes.Transparent,
                            BorderBrush = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                            BorderThickness = cell == cells[cells.Count - 1] ? new Thickness(5, 0, 5, 5) : new Thickness(5, 0, 0, 5),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };
                        
                        if (cell == cells[4])
                        {
                            var lbl1 = new Label { Content = MainWindow.isEnglish ? "Row" : "Red", Foreground = Brushes.White, Width = 60, Margin = new Thickness(0, 5, 0, 0) };
                            var lbl2 = new Label { Content = MainWindow.isEnglish ? "Number" : "Broj", Foreground = Brushes.White, Width = 60, Margin = new Thickness(0, 5, 0, 0) };
                            var row = new StackPanel()
                            {
                                
                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment= HorizontalAlignment.Left,
                                Children = {lbl1 }
                            };
                            var num = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Children = {lbl2}
                            };

                            changeTheme(lbl1);
                            changeTheme(lbl2);


                            StackPanel seats = new StackPanel()
                            {
                                Orientation = Orientation.Vertical,
                                Children =
                                {
                                    row,num
                                }
                            };
                            var rowScroller = new ScrollViewer
                            {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                                Content = seats
                            };
                            List<string> seatCoord = cell.Split(' ').ToList();
                            foreach(string coord in seatCoord)
                            {
                                string rw = coord.Split(",")[0];
                                string nm = coord.Split(",")[1];
                                var l1 = new Label { Content = rw, Foreground = Brushes.White, Width = 25, Margin = new Thickness(5, 5, 0, 0), Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                                var l2 = new Label { Content = nm, Foreground = Brushes.White, Width = 25, Margin = new Thickness(5, 5, 0, 0), Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };

                                changeTheme(l1);
                                changeTheme(l2);
                                row.Children.Add(l1);
                                num.Children.Add(l2);
                            }
                            bd2.Child = rowScroller;
                            bd2.Width = 445;
                        }
                        else
                        {
                            var lbl = new Label
                            {
                                Width = 198,
                                Content = cell,
                                Foreground = Brushes.White,
                                VerticalAlignment = VerticalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                            };
                            changeTheme(lbl);
                            bd2.Child = lbl;
                        }
                        spRow.Children.Add(bd2);
                    }
                    sp.Children.Add(spRow);
                }
                while (readerDeleted.Read())
                {
                    string sjedista = readerDeleted.GetString(1);
                    int cijena = readerDeleted.GetInt32(2);
                    string nazivFilma = readerDeleted.GetString(3);
                    string tm = readerDeleted.GetTimeSpan(4).ToString().Substring(0, 5);
                    DateTime datum = readerDeleted.GetDateTime(5);
                    int brSale = readerDeleted.GetInt32(7);

                    List<string> cells = new List<string>();
                    cells.AddRange(new[] { nazivFilma, datum.ToString("yyyy-MM-dd"), tm, brSale.ToString(), sjedista, cijena.ToString() + ",00 KM" });

                    var spRow = new StackPanel
                    {
                        Width = 1465,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Orientation = Orientation.Horizontal,

                    };

                    foreach (string cell in cells)
                    {
                        var bd2 = new Border
                        {
                            Height = 80,
                            Background = Brushes.Transparent,
                            BorderBrush = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                            BorderThickness = cell == cells[cells.Count - 1] ? new Thickness(5, 0, 5, 5) : new Thickness(5, 0, 0, 5),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                        };

                        if (cell == cells[4])
                        {
                            var lbl1 = new Label { Content = MainWindow.isEnglish ? "Row" : "Red", Foreground = Brushes.White, Width = 60, Margin = new Thickness(0, 5, 0, 0) };
                            var lbl2 = new Label { Content = MainWindow.isEnglish ? "Number" : "Broj", Foreground = Brushes.White, Width = 60, Margin = new Thickness(0, 5, 0, 0) };
                            var row = new StackPanel()
                            {

                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Children = { lbl1 }
                            };
                            var num = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Children = { lbl2 }
                            };

                            changeTheme(lbl1);
                            changeTheme(lbl2);


                            StackPanel seats = new StackPanel()
                            {
                                Orientation = Orientation.Vertical,
                                Children =
                                {
                                    row,num
                                }
                            };
                            var rowScroller = new ScrollViewer
                            {
                                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled,
                                Content = seats
                            };
                            List<string> seatCoord = cell.Split(' ').ToList();
                            foreach (string coord in seatCoord)
                            {
                                string rw = coord.Split(",")[0];
                                string nm = coord.Split(",")[1];
                                var l1 = new Label { Content = rw, Foreground = Brushes.White, Width = 25, Margin = new Thickness(5, 5, 0, 0), Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };
                                var l2 = new Label { Content = nm, Foreground = Brushes.White, Width = 25, Margin = new Thickness(5, 5, 0, 0), Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"), HorizontalContentAlignment = HorizontalAlignment.Center, VerticalContentAlignment = VerticalAlignment.Center };

                                changeTheme(l1);
                                changeTheme(l2);
                                row.Children.Add(l1);
                                num.Children.Add(l2);
                            }
                            bd2.Child = rowScroller;
                            bd2.Width = 445;
                        }
                        else
                        {
                            var lbl = new Label
                            {
                                Width = 198,
                                Content = cell,
                                Foreground = Brushes.White,
                                VerticalAlignment = VerticalAlignment.Center,
                                VerticalContentAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                            };
                            changeTheme(lbl);
                            bd2.Child = lbl;
                        }
                        spRow.Children.Add(bd2);
                    }
                    sp.Children.Add(spRow);
                }
                readerDeleted.Close();
                reader.Close();
                conn.Close();
                connDeleted.Close();
                ViewTransactionsPanel.Children.Add(sp);
            }
        }

        private void MainPageBtnClick(object sender, RoutedEventArgs e)
        {
            LoadMoviesFromDb();
            MainPageBtn.IsEnabled = false;
            TransactionCreateBtn.IsEnabled = true;
            HomePageScroll.Visibility = Visibility.Visible;
            CreateMovieScroll.Visibility = Visibility.Collapsed;
            PickedMovieScroll.Visibility =Visibility.Collapsed;
            UpdateMovieScroll.Visibility = Visibility.Collapsed;
            ViewTransactionsScroll.Visibility = Visibility.Collapsed;
        }

        private void selectHours(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                if(listBox.Name.Equals("hoursLb"))
                    this.Hour = listBox.SelectedItem.ToString();
                else
                    this.HourUpdate = listBox.SelectedItem.ToString();
            }
        }

        private void selectMinutes(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                if (listBox.Name.Equals("minutesLb"))
                    this.Minute = listBox.SelectedItem.ToString();
                else
                    this.MinuteUpdate = listBox.SelectedItem.ToString();
            }
        }
        private void selectHalls(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                if (listBox.Name.Equals("hallLb"))
                    this.Hall = listBox.SelectedItem.ToString();
                else
                    this.HallUpdate = listBox.SelectedItem.ToString();
            }
        }

        private void addShowtimeClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b.Name.Equals("addShowtimeBtn"))
            {
                string s = " (" + Hour + ":" + Minute + "," + Hall + ")";
                if (s.Length < 10)
                {
                    ErrorWindow dlg;
                    if (MainWindow.isEnglish)
                    {
                        dlg = new ErrorWindow("Enter all fields!", "Error");
                    }
                    else
                    {
                        dlg = new ErrorWindow("Unesite sva polja!", "Greška");
                    }
                    
                    dlg.Owner = this;
                    dlg.ShowDialog();
                    return;
                }
                if (terminLbl.Content.ToString().Contains(s))
                {
                    ErrorWindow err;
                    if (MainWindow.isEnglish)
                    {
                        err = new ErrorWindow("Showtime already exists!", "Error");
                    }
                    else
                    {
                        err = new ErrorWindow("Termin već postoji!", "Greška");
                    }
                    err.Owner = this;
                    err.ShowDialog();
                    return;
                }
                terminLbl.Content = terminLbl.Content + s;
            }
            else
            {
                string s = " (" + HourUpdate + ":" + MinuteUpdate + "," + HallUpdate + ")";
                if (s.Length < 10)
                {

                    ErrorWindow dlg;
                    if (MainWindow.isEnglish)
                    {
                        dlg = new ErrorWindow("Enter all fields!", "Error");
                    }
                    else
                    {
                        dlg = new ErrorWindow("Unesite sva polja!", "Greška");
                    }
                    dlg.Owner = this;
                    dlg.ShowDialog();
                    return;
                }
                if (terminLblUpdate.Content.ToString().Contains(s))
                {
                    ErrorWindow err;
                    if (MainWindow.isEnglish)
                    {
                        err = new ErrorWindow("Showtime already exists!", "Error");
                    }
                    else
                    {
                        err = new ErrorWindow("Termin već postoji!", "Greška");
                    }
                    
                    err.Owner = this;
                    err.ShowDialog();
                    return;
                }
                terminLblUpdate.Content = terminLblUpdate.Content + s;
            }
        }
        private void BackspaceClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (b.Name.Equals("BackspaceBtn"))
            {
                if (terminLbl.Content.ToString().Length > 10)
                    terminLbl.Content = terminLbl.Content.ToString().Substring(0, terminLbl.Content.ToString().Length - 10);
            }
            else
            {

                if (terminLblUpdate.Content.ToString().Length > 10)
                    terminLblUpdate.Content = terminLblUpdate.Content.ToString().Substring(0, terminLblUpdate.Content.ToString().Length - 10);
            }
        }

        private void lengthTbOnChange(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if ((!int.TryParse(tb.Text, out int i) && tb.Text.Length > 0)||(tb.Text.Length > 3))
            {
                tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
                tb.CaretIndex = tb.Text.Length;
            }
        }

        private void LoadImageClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var dlg = new OpenFileDialog();
            dlg.ShowDialog();
            string ext = "";
            if (!b.Name.EndsWith("Update"))
            {
                path = dlg.FileName;
                ext = Path.GetExtension(path);
            }
            else
            {
                pathUpdate = dlg.FileName;
                pathUpdated = true;
                ext = Path.GetExtension(pathUpdate);
            }
           

            if (ext != ".png" && ext != ".jpg" && ext != ".jpeg")
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose a picture!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Izaberite sliku!", "Greška");
                }

                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if(!b.Name.EndsWith("Update"))
                img.Background = new ImageBrush(ImgNoLock(path)) { Stretch = Stretch.UniformToFill };
            else
                imgUpdate.Background = new ImageBrush(ImgNoLock(pathUpdate)) { Stretch = Stretch.UniformToFill };
        }
        
        BitmapImage ImgNoLock(string path)
        {
            var b = new BitmapImage();
            using var s = File.OpenRead(path);                   
            b.BeginInit();
            b.CacheOption = BitmapCacheOption.OnLoad;             
            b.StreamSource = s;                                    
            b.EndInit();
            b.Freeze();
            return b;
        }

        private void CreateMovie(object sender, RoutedEventArgs e)
        {
            if (nameTb.Text.Length == 0) 
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Add movie name!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Dodajte ime filma!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (descriptionTb.Text.Length == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Add movie description!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Dodajte opis filma!", "Greška");
                }

                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (genreTb.SelectedItems.Count == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose at least one genre!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite bar jedan žanr!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (terminLbl.Content.ToString().Length <= 10)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose at least one showtime for the movie!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite bar jedan termin za film!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (dLb.SelectedItems.Count == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose if the movie is 2D or 3D!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite da li je film 2D ili 3D!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (lengthTb.Text.Length == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Enter the movie lenght in minutes!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Unesite trajanje filma u minutama!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (img.Background == null)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Add movie picture!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Dodajte sliku filma!", "Greška"); 
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            MySqlConnection conn0 = MainWindow.OpenConnection();
            MySqlCommand cmd0 = conn0.CreateCommand();
            cmd0.CommandText = "SELECT * FROM hci.film;";
            MySqlDataReader reader0 = cmd0.ExecuteReader();
            while (reader0.Read()) 
            {
                string naziv = reader0.GetString(0);
                if (nameTb.Text.Equals(naziv))
                {
                    ErrorWindow err;
                    if (MainWindow.isEnglish)
                    {
                        err = new ErrorWindow("Movie with the same name already exists!", "Error");
                    }
                    else
                    {
                        err = new ErrorWindow("Film sa istim nazivom već postoji!", "Greška");
                    }
                    
                    err.Owner = this;
                    err.ShowDialog();
                    return;
                }
            }

            string dest = "../../../assets/movieImages/" + nameTb.Text + Path.GetExtension(path);
            File.Copy(path, dest);
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();
            int d = 5;
            if (dLb.SelectedItem.ToString().Equals("2D"))
                d = 0;
            else
                if(dLb.SelectedItem.ToString().Equals("3D"))
                    d = 1;
            List<string> selectedGenres = new List<string>();
            foreach (var item in genreTb.SelectedItems)
            {
                int idx = genreTb.Items.IndexOf(item);
                selectedGenres.Add(Zanrovi[idx]);
            }
            cmd.CommandText = "INSERT INTO `hci`.`film` (`naziv`, `opis`, `zanr`, `trajanje`, `3D`, `slika`) VALUES('"+ nameTb.Text+"', '"+ descriptionTb.Text+"', '"+ string.Join(", ", selectedGenres) +"', "+ lengthTb.Text + ", '"+d+"', '"+dest+"');";
            cmd.ExecuteNonQuery();
            List<string> list= terminLbl.Content.ToString().Split("(").ToList();
            for(int i = 1; i < list.Count; i++)
            {
                string time= list[i].Substring(0,5);
                string sala = list[i].Substring(6,1);
                cmd.CommandText = "INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES('" + nameTb.Text + "', '"+sala+"', '"+time+"');";
                cmd.ExecuteNonQuery();
            }
            conn.Close();

            nameTb.Text = "";
            descriptionTb.Text = "";
            genreTb.UnselectAll();
            terminLbl.Content= "Termini:";
            if(MainWindow.isEnglish)
                terminLbl.Content = "Showtimes:";
            dLb.UnselectAll();
            lengthTb.Text = "";
            img.Background = null;
            Hour  = "";
            Minute = "";
            Hall = "";
            path = "";

            MainPageBtnClick(null,null);

        }

        private void LoadMoviesFromDb()
        {
            MoviesWrapPanel.Children.Clear();
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();
            if(filterLb.SelectedIndex<1)
                cmd.CommandText = "SELECT * FROM hci.film;";
            if (filterLb.SelectedIndex > 20)
                cmd.CommandText = "SELECT * FROM hci.film where 3D = "+(filterLb.SelectedItem.ToString()=="3D"? 1:0)+";";
            if (filterLb.SelectedIndex < 21 && filterLb.SelectedIndex > 0)
                cmd.CommandText = "SELECT * FROM hci.film WHERE zanr LIKE '%" + Zanrovi[filterLb.SelectedIndex-1] +"%';";
            MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                Label lb = new Label();
                lb.Content = "Nema dostupnih filmova";
                if(MainWindow.isEnglish)
                    lb.Content = "No movies available";
                lb.FontSize = 30;
                lb.FontWeight = FontWeights.Bold;
                lb.HorizontalContentAlignment = HorizontalAlignment.Center;
                lb.HorizontalAlignment = HorizontalAlignment.Center;
                lb.Height = 100;
                lb.Margin = new Thickness(0, 300, 0, 0);
                lb.VerticalAlignment = VerticalAlignment.Top;
                lb.VerticalContentAlignment = VerticalAlignment.Center;
                lb.Foreground = Brushes.White;
                changeTheme(lb);
                MoviesWrapPanel.Children.Add(lb);
                return;
            }
            List<(string naziv, string opis, string zanr, int trajanje, string slika)> movies = new List<(string, string, string, int, string)>();
            while (reader.Read())
            {
                var movie = (
                    naziv: reader.GetString(0),
                    opis: reader.GetString(1),
                    zanr: reader.GetString(2),
                    trajanje: reader.GetInt32(3),
                    slika: reader.GetString(5)
                );
                movies.Add(movie);
            }
            if (sortLb.SelectedIndex == 0)
            {
                movies.Sort((x, y) => x.trajanje.CompareTo(y.trajanje));
            }
            foreach( var movie in movies)
            {
                MySqlConnection conn1 = MainWindow.OpenConnection();
                MySqlCommand cmd1 = conn1.CreateCommand();
                cmd1.CommandText = "SELECT * FROM hci.termin where nazivFilm = \""+movie.naziv+"\";";
                MySqlDataReader reader1 = cmd1.ExecuteReader();
                List<TimeSpan> times = new List<TimeSpan>();
                while (reader1.Read())
                {
                    times.Add(reader1.GetTimeSpan(3));
                }
                times.Sort();
                reader1.Close();
                conn1.Close();
                List<string> timesStr = new List<string>();
                foreach (TimeSpan t in times)
                {
                    timesStr.Add(t.ToString().Substring(0,5));
                }
                var label = new Label()
                {
                    Content = movie.naziv,
                    FontSize = 14,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(5)
                };
                changeTheme(label);
                    var bd1 = new Border
                {
                    Width = 300,
                    Height = 50,
                    CornerRadius = new CornerRadius(8, 8, 0, 0),
                    Margin = new Thickness(20, 20, 20, 0),
                    Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Child = label
                };
                var bd2 = new Border
                {
                    Name = "bd2",
                    Width = 300,
                    Height = 450,
                    Margin = new Thickness(20,0,20,0),
                    Background = new ImageBrush(ImgNoLock(movie.slika)) { Stretch = Stretch.UniformToFill }
                };
                bd2a = bd2;
                var lbl = new Label()
                {
                    FocusVisualStyle = null,
                    Content = string.Join(", ", timesStr),
                    FontSize = 12,
                    Foreground = Brushes.White,
                    Padding = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                changeTheme(lbl);
                var bd3 = new Border
                {
                    Padding = new Thickness(5),
                    Width = 300,
                    Height = 50,
                    CornerRadius = new CornerRadius(0, 0, 8, 8),
                    Margin = new Thickness(20, 0, 20, 20),
                    Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FocusVisualStyle = null,
                    Child = new ScrollViewer
                    {
                        FocusVisualStyle = null,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment= VerticalAlignment.Center,
                        Content = lbl
                    }
                };
                var sp = new StackPanel { Orientation = Orientation.Vertical};
                sp.Children.Add(bd1);
                sp.Children.Add(bd2);
                sp.Children.Add(bd3);
                sp.Cursor = Cursors.Hand;
                sp.MouseLeftButtonUp += (s, e) => ClickMovie(s,e,movie.naziv);
                
                MoviesWrapPanel.Children.Add(sp);
                
            }
            reader.Close();
            conn.Close();
        }

        private void ClickMovie(object sender, MouseButtonEventArgs e, string naziv)
        {
            MainPageBtn.IsEnabled = true;
            TransactionCreateBtn.IsEnabled = true;
            HomePageScroll.Visibility = Visibility.Collapsed;
            CreateMovieScroll.Visibility = Visibility.Collapsed;
            PickedMovieScroll.Visibility = Visibility.Visible;
            UpdateMovieScroll.Visibility = Visibility.Collapsed;
            PickedMovieGrid.Children.Clear();
            

            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM hci.film where naziv = \"" + naziv + "\";";
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            string opis = reader.GetString(1);
            string zanr = reader.GetString(2);
            int trajanje = reader.GetInt32(3);
            bool is3D = reader.GetBoolean(4);
            string slika = reader.GetString(5);
            reader.Close();
            conn.Close() ;

            MySqlConnection conn1 = MainWindow.OpenConnection();
            MySqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandText = "SELECT * FROM hci.termin where nazivFilm = \"" + naziv + "\";";
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            List<TimeSpan> times = new List<TimeSpan>();
            string terminiSale = "";
            while (reader1.Read())
            {
                int brSale = reader1.GetInt32(2);
                TimeSpan tm = reader1.GetTimeSpan(3);
                times.Add(tm);
                terminiSale += " (" + tm.ToString().Substring(0, 5) + "," + brSale + ")";
            }
            times.Sort();
            reader1.Close();
            conn1.Close();
            List<string> timesStr = new List<string>();
            foreach (TimeSpan t in times)
            {
                timesStr.Add(t.ToString().Substring(0, 5));
            }
            string termini = string.Join(", ", timesStr);
            var lbl0 = new Label()
            {
                Content = naziv,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5)
            };
            changeTheme(lbl0);
            var bd1 = new Border
            {
                Width = 300,
                Height = 50,
                CornerRadius = new CornerRadius(8, 8, 0, 0),
                Margin = new Thickness(20, 20, 20, 0),
                Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Child = lbl0
            };
            var bd2 = new Border
            {
                Width = 300,
                Height = 450,
                Margin = new Thickness(20, 0, 20, 0),
                Background = new ImageBrush(ImgNoLock(slika)) { Stretch = Stretch.UniformToFill }
            };
            bd2b = bd2;
            var lbl1 = new Label()
            {
                FocusVisualStyle = null,
                Content = termini,
                FontSize = 12,
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            changeTheme(lbl1);
            var bd3 = new Border
            {
                Padding = new Thickness(5),
                Width = 300,
                Height = 50,
                CornerRadius = new CornerRadius(0, 0, 8, 8),
                Margin = new Thickness(20, 0, 20, 20),
                Background = (Brush)new BrushConverter().ConvertFromString("#7331c4"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FocusVisualStyle = null,
                Child = new ScrollViewer
                {
                    FocusVisualStyle = null,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = lbl1
                }
            };
            var sp1 = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(100, 35, 100, 0)
            };
            sp1.Children.Add(bd1);
            sp1.Children.Add(bd2);
            sp1.Children.Add(bd3);
            if (Usrnm.Equals("admin"))
            {
                var updateBtn = new Button
                {
                    Content = MainWindow.isEnglish ?"Update":"Ažuriraj",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 51,
                    Width = 120,
                    Margin = new Thickness(0, 10, 10, 0),
                    Style = (Style)FindResource("PurpleButton")
                };
                changeTheme(updateBtn);
                var deleteBtn = new Button
                {
                    Content = MainWindow.isEnglish ? "Delete" : "Obriši",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 51,
                    Width = 120,
                    Margin = new Thickness(10, 10, 0, 0),
                    Style = (Style)FindResource("PurpleButton")
                };
                changeTheme(deleteBtn);
                var sp = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                sp.Children.Add (updateBtn);
                sp.Children.Add (deleteBtn);
                sp1.Children.Add(sp);

                updateBtn.Click += (s, e) => UpdateClick(s, e, naziv, opis, zanr, trajanje, is3D, slika, terminiSale);
                deleteBtn.Click += (s, e) => deleteBtn_Click(s, e, naziv);
            }
            else
            {
                var buyBtn = new Button
                {
                    Content = MainWindow.isEnglish ? "Buy ticket" : "Kupi kartu",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 51,
                    Width = 120,
                    Margin = new Thickness(0, 10, 10, 0),
                    Style = (Style)FindResource("PurpleButton")
                };
                changeTheme(buyBtn);
                sp1.Children.Add(buyBtn);
                buyBtn.Click += (s, e) => buyBtn_Click(s, e, naziv, is3D);
            }
            Grid.SetColumn(sp1, 0);

            Label nazivLb = new Label()
            {
                FocusVisualStyle = null,
                Content = naziv,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                Margin = new Thickness(10,50,10,10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
            };
            changeTheme(nazivLb);
            Label zanrLb = new Label()
            {
                FocusVisualStyle = null,
                Content = zanr,
                FontSize = 14,
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            changeTheme(zanrLb);
            Label trajanjeLb = new Label()
            {
                FocusVisualStyle = null,
                Content = MainWindow.isEnglish ? ("Lenght: " + trajanje + "min    " + (is3D ? "3D" : "2D")) : ("Trajanje: " +trajanje+"min    "+(is3D?"3D":"2D")),
                FontSize = 14,
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            changeTheme(trajanjeLb);
            TextBlock opisLb = new TextBlock()
            {
                FocusVisualStyle = null,
                Text = opis,
                FontSize = 12,
                Foreground = Brushes.White,
                Padding = new Thickness(5),
                Margin = new Thickness(50, 100, 50, 50),
                Width = 800,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.Wrap
            };
            changeTheme(opisLb);

            var sp2 = new StackPanel { Orientation = Orientation.Vertical };
            sp2.Children.Add(nazivLb);
            sp2.Children.Add(zanrLb);
            sp2.Children.Add(trajanjeLb);
            sp2.Children.Add(opisLb);
            Grid.SetColumn(sp2, 1);

            PickedMovieGrid.Children.Add(sp1);
            PickedMovieGrid.Children.Add(sp2 );
        }

        private void UpdateClick(object sender, RoutedEventArgs e,string naziv, string opis, string zanr, int trajanje, bool is3D, string slika, string terminiSale)
        {
            PickedMovieScroll.Visibility = Visibility.Collapsed;
            UpdateMovieScroll.Visibility= Visibility.Visible;
            nameTbUpdate.Text = naziv;
            descriptionTbUpdate.Text = opis;
            string[] zanrovi = zanr.Split(", ");
            for (int i = 0; i < zanrovi.Length; i++)
            {
                for(int j = 0;j<Zanrovi.Count;j++) 
                {
                    if (zanrovi[i].Equals(Zanrovi[j]))
                    {
                        genreTbUpdate.SelectedItems.Add(genreTbUpdate.Items[j]);
                    }
                }
            }
            terminLblUpdate.Content += terminiSale;
            lengthTbUpdate.Text = trajanje.ToString();
            pathUpdated = false;
            imgUpdate.Background = new ImageBrush(ImgNoLock(slika)) { Stretch = Stretch.UniformToFill };
            pathUpdate = slika;

            if (is3D)
            {
                dLbUpdate.SelectedIndex = 1;
            }
            else
            {
                dLbUpdate.SelectedIndex = 0;
            }
        }

        private void UpdateMovie(object sender, RoutedEventArgs e)
        {
            if (descriptionTbUpdate.Text.Length == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Add movie description!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Dodajte opis filma!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (genreTbUpdate.SelectedItems.Count == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose at least one genre!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite bar jedan žanr!", "Greška");
                }
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (terminLblUpdate.Content.ToString().Length <= 10)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose at least one showtime for the movie!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite bar jedan termin za film!", "Greška");
                }
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (dLbUpdate.SelectedItems.Count == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose if the movie is 2D or 3D!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Odaberite da li je film 2D ili 3D!", "Greška");
                }
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (lengthTbUpdate.Text.Length == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Enter the movie lenght in minutes!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Unesite trajanje filma u minutama!", "Greška");
                }
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            if (imgUpdate.Background == null)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Add a movie picture!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Dodajte sliku filma!", "Greška");
                }

                err.Owner = this;
                err.ShowDialog();
                return;
            }
            string dest;
            if (pathUpdated)
            {
                imgUpdate.Background = null;
                img.Background = null;
                bd2a.Background = null;
                bd2b.Background = null;
                dest = "../../../assets/movieImages/" + nameTbUpdate.Text + Path.GetExtension(pathUpdate);
                File.Delete(dest);
                File.Copy(pathUpdate, dest);
                imgUpdate.Background = new ImageBrush(ImgNoLock(dest)) { Stretch = Stretch.UniformToFill };
            }
            
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();
            int d = 5;
            if (dLbUpdate.SelectedItem.ToString().Equals("2D"))
                d = 0;
            else
                if (dLbUpdate.SelectedItem.ToString().Equals("3D"))
                d = 1;
            List<string> selectedGenres = new List<string>();
            foreach (var item in genreTbUpdate.SelectedItems)
            {
                int idx = genreTbUpdate.Items.IndexOf(item);
                selectedGenres.Add(Zanrovi[idx]);
            }
            
            cmd.CommandText = "UPDATE `hci`.`film` SET `opis` = '" + descriptionTbUpdate.Text + "', `zanr` = '" + string.Join(", ", selectedGenres) + "', `trajanje` = '" + lengthTbUpdate.Text + "', `3D` = '" + d + "' WHERE (`naziv` = '" + nameTbUpdate.Text + "');";
            cmd.ExecuteNonQuery();
            List<string> list = terminLblUpdate.Content.ToString().Split("(").ToList();

            MySqlCommand cmd2 = conn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM hci.termin where nazivFilm = \"" + nameTbUpdate.Text + "\";";
            MySqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                Boolean exists = false;
                int id = reader2.GetInt32(0);
                int brSale = reader2.GetInt32(2);
                string tm = reader2.GetTimeSpan(3).ToString().Substring(0, 5);

                for (int i = 1; i < list.Count; i++)
                {
                    string time = list[i].Substring(0, 5);
                    string sala = list[i].Substring(6, 1);

                    if (time.Equals(tm) && (sala.Equals(brSale.ToString())))
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    DeleteShowtime(id);
                }
            }
            reader2.Close();

            for (int i = 1; i < list.Count; i++)
            {
                string time = list[i].Substring(0, 5);
                string sala = list[i].Substring(6, 1);

                MySqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandText = "SELECT * FROM hci.termin where nazivFilm = \"" + nameTbUpdate.Text + "\";";
                MySqlDataReader reader1 = cmd1.ExecuteReader();
                Boolean exists0 = false;
                while (reader1.Read())
                {
                    int brSale = reader1.GetInt32(2);
                    string tm = reader1.GetTimeSpan(3).ToString().Substring(0, 5);
                    if(time.Equals(tm) && (sala.Equals(brSale.ToString())))
                    {
                        exists0 = true;
                    }    
                }
                reader1.Close();
                if (!exists0)
                {
                    MySqlCommand insertTermin = conn.CreateCommand();
                    insertTermin.CommandText = "INSERT INTO `hci`.`termin` (`nazivFilm`, `brojSala`, `vrijeme`) VALUES('" + nameTbUpdate.Text + "', '" + sala + "', '" + time + "');";
                    insertTermin.ExecuteNonQuery();
                }
            }
            conn.Close();

            string nm = nameTbUpdate.Text;

            nameTbUpdate.Text = "";
            descriptionTbUpdate.Text = "";
            genreTbUpdate.UnselectAll();
            terminLblUpdate.Content = "Termini:";
            if (MainWindow.isEnglish)
                terminLblUpdate.Content = "Showtimes:";
            dLbUpdate.UnselectAll();
            lengthTbUpdate.Text = "";
            imgUpdate.Background = null;
            HourUpdate = "";
            MinuteUpdate = "";
            HallUpdate = "";

            ClickMovie(null, null, nm);

        }

        public void DeleteMovie(string name)
        {
            MySqlConnection conn0 = MainWindow.OpenConnection();
            MySqlCommand cmd0 = conn0.CreateCommand();
            cmd0.CommandText = "SELECT * FROM hci.termin where nazivFilm = \"" + name + "\";";
            MySqlDataReader reader0 = cmd0.ExecuteReader();
            while (reader0.Read())
            {
                DeleteShowtime(reader0.GetInt32(0));
            }
            reader0.Close();
            MySqlCommand cmd1 = conn0.CreateCommand();
            cmd1.CommandText = "DELETE FROM `hci`.`film` WHERE (`naziv` = '" + name + "');";
            cmd1.ExecuteNonQuery();
        }

        public void DeleteShowtime(int id)
        {
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlConnection conn1 = MainWindow.OpenConnection();
            MySqlCommand cmd0 = conn.CreateCommand();
            cmd0.CommandText = "SELECT * FROM hci.termin where terminId=" + id + ";";
            MySqlDataReader reader0 = cmd0.ExecuteReader();
            reader0.Read();
            string naziv = reader0.GetString(1);
            int brSale = reader0.GetInt32(2);
            TimeSpan tm = reader0.GetTimeSpan(3);
            reader0.Close();

            MySqlCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = "SELECT * FROM hci.kupovina where terminId="+id+";";
            MySqlDataReader reader1 = cmd1.ExecuteReader();

            while (reader1.Read())
            {
                int kupovinaId = reader1.GetInt32(0);
                string sjedista = reader1.GetString(1);
                int cijena = reader1.GetInt32(2);
                DateTime datum = reader1.GetDateTime(3);
                string username = reader1.GetString(5);
                MySqlCommand cmd2 = conn1.CreateCommand();
                cmd2.CommandText = "DELETE FROM `hci`.`kupovina` WHERE (`idKupovina` = '"+kupovinaId+"');";
                cmd2.ExecuteNonQuery();
                MySqlCommand cmd3 = conn1.CreateCommand();
                cmd3.CommandText = "INSERT INTO `hci`.`kupovinadeleted` (`sjediste`, `cijena`, `NazivFIlm`, `vrijeme`, `datum`, `korisnickoIme`, `brojSale`) " +
                    "VALUES ('"+ sjedista + "', '"+ cijena + "', '"+ naziv+ "', '"+tm.ToString()+"', '"+ datum.ToString("yyyy-MM-dd") + "', '"+username+"', '"+brSale+"');";
                cmd3.ExecuteNonQuery();
            }
            reader1.Close();
            conn1.Close();
            MySqlCommand delete = conn.CreateCommand();
            delete.CommandText = "DELETE FROM `hci`.`termin` WHERE (`terminId` = '" + id + "');";
            delete.ExecuteNonQuery();
            conn.Close();
        }

        private void backUpdate(object sender, RoutedEventArgs e)
        {
            UpdateMovieScroll.Visibility = Visibility.Collapsed;
            PickedMovieScroll.Visibility = Visibility.Visible;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e, string naziv)
        {
            DeleteMovie(naziv);
            MainPageBtnClick(null,null);
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(MainWindow.isEnglish);
            mw.Show();
            
            this.Close();
        }

        private void buyBtn_Click(object s, RoutedEventArgs e, string naziv, bool is3D)
        {
            var buy = new BuyTicket(naziv, is3D, this.Usrnm, this.theme);
            buy.Owner = this;
            buy.ShowDialog();
        }

        private void translate()
        {
            LogOutLbl.Content = "Log out";
            addShowtimeBtn.Content = "Add showtime";
            addImgBtn.Content = "Add picture";
            createBtn.Content = "Create movie";
            addShowtimeBtnUpdate.Content= "Add showtime";
            addImgBtnUpdate.Content = "Add picture";
            createBtnUpdate.Content = "Update movie";

            MainPageBtn.Content = "Main page";

            nameLbl.Content = "Movie name:";
            descriptionLbl.Content = "Movie description:";
            genreLbl.Content = "Genre:";
            terminLbl.Content = "Showtimes:";
            hoursLbl.Content = "Hours:";
            minutesLbl.Content = "Minutes:";
            hallLbl.Content = "Hall:";
            lb.Content = "Movie lenght (minutes):";

            nameLblUpdate.Content = "Movie name:";
            descriptionLblUpdate.Content = "Movie description:";
            genreLblUpdate.Content = "Genre:";
            terminLblUpdate.Content = "Showtimes:";
            hoursLblUpdate.Content = "Hours:";
            minutesLblUpdate.Content = "Minutes:";
            hallLblUpdate.Content = "Hall:";
            lbUpdate.Content = "Movie lenght (minutes):";

            sortLbl.Content = "Sort";
            filterLbl.Content = "Filter";
        }

        private void themesLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();

            

            if (listbox.SelectedIndex == 0)
            {
                this.theme = 0;
                cmd.CommandText = "UPDATE `hci`.`korisnik` SET `tema` = 'dark' WHERE(`korisnickoIme` = '"+Usrnm+"');";
                cmd.ExecuteNonQuery();
                darkPic.Visibility = Visibility.Visible;
                brightPic.Visibility = Visibility.Collapsed;
                colorPic.Visibility = Visibility.Collapsed;

                setTheme(Brushes.White, "Segoe UI");
                LoadMoviesFromDb();
                return;
            }
            if (listbox.SelectedIndex == 1)
            {
                this.theme = 1;
                cmd.CommandText = "UPDATE `hci`.`korisnik` SET `tema` = 'bright' WHERE(`korisnickoIme` = '" + Usrnm + "');";
                cmd.ExecuteNonQuery();

                darkPic.Visibility = Visibility.Collapsed;
                brightPic.Visibility = Visibility.Visible;
                colorPic.Visibility = Visibility.Collapsed;

                setTheme(Brushes.Black, "Segoe UI");

                LoadMoviesFromDb();
                return;
            }
            if (listbox.SelectedIndex == 2)
            {
                this.theme = 2;
                cmd.CommandText = "UPDATE `hci`.`korisnik` SET `tema` = 'color' WHERE(`korisnickoIme` = '" + Usrnm + "');";
                cmd.ExecuteNonQuery();

                darkPic.Visibility = Visibility.Collapsed;
                brightPic.Visibility = Visibility.Collapsed;
                colorPic.Visibility = Visibility.Visible;

                setTheme(Brushes.Black, "Comic Sans MS");
                LoadMoviesFromDb();
                return;
            }
        }

        public void setTheme(SolidColorBrush scb, string type )
        {
            themesLb.Foreground = scb;
            themesLb.FontFamily = new FontFamily(type);

            usrnmLbl.Foreground = Brushes.Black;
            usrnmLbl.FontFamily = new FontFamily(type);

            TransactionCreateLbl.FontFamily = new FontFamily(type);

            genreTb.Foreground = scb;
            genreTb.FontFamily = new FontFamily(type);

            hoursLb.Foreground = scb;
            hoursLb.FontFamily = new FontFamily(type);

            minutesLb.Foreground = scb;
            minutesLb.FontFamily = new FontFamily(type);

            hallLb.Foreground = scb;
            hallLb.FontFamily = new FontFamily(type);

            dLb.Foreground = scb;
            dLb.FontFamily = new FontFamily(type);

            genreTbUpdate.Foreground = scb;
            genreTbUpdate.FontFamily = new FontFamily(type);

            hoursLbUpdate.Foreground = scb;
            hoursLbUpdate.FontFamily = new FontFamily(type);

            minutesLbUpdate.Foreground = scb;
            minutesLbUpdate.FontFamily = new FontFamily(type);

            hallLbUpdate.Foreground = scb;
            hallLbUpdate.FontFamily = new FontFamily(type);

            dLbUpdate.Foreground = scb;
            dLbUpdate.FontFamily = new FontFamily(type);

            LogOutLbl.Foreground = Brushes.Black;
            LogOutLbl.FontFamily = new FontFamily(type);

            addShowtimeBtn.Foreground = scb;
            addShowtimeBtn.FontFamily = new FontFamily(type);

            addImgBtn.Foreground = scb;
            addImgBtn.FontFamily = new FontFamily(type  );

            createBtn.Foreground = scb;
            createBtn.FontFamily = new FontFamily(type);

            addShowtimeBtnUpdate.Foreground = scb;
            addShowtimeBtnUpdate.FontFamily = new FontFamily(type);

            addImgBtnUpdate.Foreground = scb;
            addImgBtnUpdate.FontFamily = new FontFamily(type);

            createBtnUpdate.Foreground = scb;
            createBtnUpdate.FontFamily = new FontFamily(type);

            MainPageBtn.Foreground = Brushes.Black;
            MainPageBtn.FontFamily = new FontFamily(type);

            nameLbl.Foreground = scb;
            nameLbl.FontFamily = new FontFamily(type);

            descriptionLbl.Foreground = scb;
            descriptionLbl.FontFamily = new FontFamily(type);

            genreLbl.Foreground = scb;
            genreLbl.FontFamily = new FontFamily(type);

            terminLbl.Foreground = scb;
            terminLbl.FontFamily = new FontFamily(type);

            hoursLbl.Foreground = scb;
            hoursLbl.FontFamily = new FontFamily(type);

            minutesLbl.Foreground = scb;
            minutesLbl.FontFamily = new FontFamily(type);

            hallLbl.Foreground = scb;
            hallLbl.FontFamily = new FontFamily(type);

            lb.Foreground = scb;
            lb.FontFamily = new FontFamily(type);

            nameLblUpdate.Foreground = scb;
            nameLblUpdate.FontFamily = new FontFamily(type);

            descriptionLblUpdate.Foreground = scb;
            descriptionLblUpdate.FontFamily = new FontFamily(type);

            genreLblUpdate.Foreground = scb;
            genreLblUpdate.FontFamily = new FontFamily(type);

            terminLblUpdate.Foreground = scb;
            terminLblUpdate.FontFamily = new FontFamily(type);

            hoursLblUpdate.Foreground = scb;
            hoursLblUpdate.FontFamily = new FontFamily(type);

            minutesLblUpdate.Foreground = scb;
            minutesLblUpdate.FontFamily = new FontFamily(type);

            hallLblUpdate.Foreground = scb;
            hallLblUpdate.FontFamily = new FontFamily(type);

            BackspaceBtn.Foreground = scb;
            BackspaceBtn.FontFamily = new FontFamily(type);

            sortLbl.Foreground = scb;
            sortLbl.FontFamily = new FontFamily(type);

            filterLbl.Foreground = scb;
            filterLbl.FontFamily = new FontFamily(type);

            arrowLeft.Foreground = scb;
            arrowLeft.FontFamily = new FontFamily(type);

            arrowRight.Foreground = scb;
            arrowRight.FontFamily = new FontFamily(type);

            sortLb.Foreground = scb;
            sortLb.FontFamily = new FontFamily(type);

            filterLb.Foreground = scb;
            filterLb.FontFamily = new FontFamily(type);
        }

        public void changeTheme(object i)
        {
            Control control = null;
            TextBlock tb = null;
            if(i is Control)
            {
                control = i as Control;
                switch (this.theme)
                {
                    case 0:
                        {
                            control.Foreground = Brushes.White;
                            control.FontFamily = new FontFamily("Segoe UI");
                            break;
                        }

                    case 1:
                        {
                            control.Foreground = Brushes.Black;
                            control.FontFamily = new FontFamily("Segoe UI");
                            break;
                        }

                    case 2:
                        {
                            control.Foreground = Brushes.Black;
                            control.FontFamily = new FontFamily("Comic Sans MS");
                            break;
                        }
                }
            }
                
            if(i is TextBlock)
            {
                tb = i as TextBlock;
                switch (this.theme)
                {
                    case 0:
                        {
                            tb.Foreground = Brushes.White;
                            tb.FontFamily = new FontFamily("Segoe UI");
                            break;
                        }

                    case 1:
                        {
                            tb.Foreground = Brushes.Black;
                            tb.FontFamily = new FontFamily("Segoe UI");
                            break;
                        }

                    case 2:
                        {
                            tb.Foreground = Brushes.Black;
                            tb.FontFamily = new FontFamily("Comic Sans MS");
                            break;
                        }
                }
            } 
        }

        private void filterLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMoviesFromDb();
        }

        private void sortLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMoviesFromDb();
        }
    }
}
