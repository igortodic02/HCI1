using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using Mysqlx.Crud;

namespace Projekat1
{
    public partial class BuyTicket : Window
    {
        private string Usrnm { get; set; }
        private string Naziv { get; set; }
        private bool is3D { get; set; }
        public int TerminId { get; set; }
        public string TimeHall { get; set; }
        public string DateStr { get; set; }

        public ObservableCollection<ShowtimeItem> Showtimes { get; } = new ObservableCollection<ShowtimeItem>();
        public ObservableCollection<string> Dates { get; } = new ObservableCollection<string>();
        private HashSet<string> selectedSeats = new HashSet<string>();
        public int theme = -1;

        public BuyTicket(string naziv, bool is3D, string usrnm, int theme)
        {
            InitializeComponent();
            this.Naziv = naziv;
            this.is3D = is3D;
            Usrnm = usrnm;
            this.theme = theme;
            setTheme();
            DataContext = this;
            if (MainWindow.isEnglish)
                translate();
            var tmp = new List<ShowtimeItem>();

            MySqlConnection conn1 = MainWindow.OpenConnection();
            MySqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandText = "SELECT * FROM hci.termin where nazivFilm = \"" + naziv + "\";";
            MySqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                int idTermina = reader1.GetInt32(0);
                int brSale = reader1.GetInt32(2);
                TimeSpan tm = reader1.GetTimeSpan(3);

                tmp.Add(new ShowtimeItem
                {
                    IdTermina = idTermina,
                    Time = tm,
                    DisplayTime = tm.ToString().Substring(0, 5) + (MainWindow.isEnglish?" in hall ":" u sali ") + brSale
                });
            }
            reader1.Close();
            conn1.Close();

            foreach (var item in tmp.OrderBy(x => x.Time))
                Showtimes.Add(item);

            for (int i = 0; i < 7; i++)
            {
                Dates.Add(DateTime.Today.AddDays(i).ToString("yyyy-MM-dd"));
            }
            
        }


        public class ShowtimeItem
        {
            public int IdTermina { get; set; }
            public TimeSpan Time { get; set; }
            public string DisplayTime { get; set; }
        }

        private void pickTime_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (pickTime.SelectedItem is ShowtimeItem item)
            {
                this.TerminId = item.IdTermina;
                this.TimeHall = item.DisplayTime;
            }
        }

        private void pickDate_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (pickDate.SelectedItem is string item)
            {
                this.DateStr = item;
            }
        }

        private void ChooseSeatsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pickDate.SelectedItem == null || pickTime.SelectedItem == null)
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
            Grid1.Visibility = Visibility.Collapsed;
            SeatsGrid.Visibility = Visibility.Visible;
            BuySeatsBtn.Visibility = Visibility.Visible;
            PriceLbl.Visibility = Visibility.Visible;

            string dateStr = pickDate.SelectedItem as string;

            ShowtimeItem item = pickTime.SelectedItem as ShowtimeItem;
            int terminId = item.IdTermina;

            int hallNumber = int.Parse(item.DisplayTime.Substring(item.DisplayTime.Length - 1));

            int rows = 0, cols = 0;
            MySqlConnection conn = MainWindow.OpenConnection();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT * FROM hci.sala WHERE broj = '"+hallNumber+"';";

            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            rows = reader.GetInt32(1);
            cols = reader.GetInt32(2);
            reader.Close();
            conn.Close();

            SeatsGrid.Children.Clear();
            SeatsGrid.RowDefinitions.Clear();
            SeatsGrid.ColumnDefinitions.Clear();
            selectedSeats.Clear();

            SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            for (int c = 0; c < cols; c++)
                SeatsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            for (int r = 0; r < rows; r++)
                SeatsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            for (int r = 0; r < rows; r++)
            {
                var lbl = new Label
                {
                    Content = (r + 1).ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(5, 5, 0, 0),
                    Foreground = Brushes.White,
                    Height = 30,
                    Width = 30,
                };
                changeTheme(lbl);
                Grid.SetRow(lbl, r);
                Grid.SetColumn(lbl, 0);
                SeatsGrid.Children.Add(lbl);

                for (int c = 0; c < cols; c++)
                {
                    string tag = (r + 1) + "," + (c + 1);
                    var btn = new Button
                    {
                        Content = (c + 1).ToString(),
                        Tag = tag,
                        Style = (Style)FindResource("PurpleButton"),
                        Height = 30,
                        Width = 30,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        BorderThickness = new Thickness(0),
                        Margin = new Thickness(5, 5, 0, 0)
                    };
                    changeTheme(btn);
                    btn.MouseEnter += (s, e) =>
                    {
                        var t = (string)Tag;
                        if (selectedSeats.Contains(t))
                            btn.Background = Brushes.PaleGreen;
                    };

                    btn.Click += (s, ev) =>
                    {
                        var b = (Button)s;
                        if (!b.IsEnabled) 
                            return;

                        var t = (string)b.Tag;
                        if (selectedSeats.Contains(t))
                        {
                            selectedSeats.Remove(t);
                            b.ClearValue(Button.BackgroundProperty);

                        }
                        else
                        {
                            selectedSeats.Add(t);
                            b.Background = Brushes.Lime;
                        }
                        PriceLbl.Content = MainWindow.isEnglish ?
                        ("Total amount: " + selectedSeats.Count + " x " + (this.is3D ? 6 : 5) + " = " + (selectedSeats.Count * (this.is3D ? 6 : 5)) + ",00 KM")
                        :
                        ("Ukupan iznos: " + selectedSeats.Count + " x " + (this.is3D ? 6 : 5) + " = " + (selectedSeats.Count * (this.is3D ? 6 : 5)) + ",00 KM");
                    };

                    Grid.SetRow(btn, r);
                    Grid.SetColumn(btn, c + 1);
                    SeatsGrid.Children.Add(btn);
                }
            }

            var boughtSet = new HashSet<string>();
            MySqlConnection conn1 = MainWindow.OpenConnection();
            MySqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandText = "SELECT * FROM hci.kupovina WHERE terminId = '"+terminId+"' AND datum = \""+dateStr+"\";";
            MySqlDataReader reader1 = cmd1.ExecuteReader();

            while (reader1.Read())
            {
                string seatsStr = reader1.GetString(1);
                foreach (string seat in seatsStr.Split(" "))
                    boughtSet.Add(seat);
            }
            reader1.Close();
            conn1.Close();

            foreach (var child in SeatsGrid.Children.OfType<Button>())
            {
                if (child.Tag is string tag && boughtSet.Contains(tag))
                {
                    child.IsEnabled = false;
                    child.Background = Brushes.LightGray;
                }
            }

            BuySeatsBtn.Click += (s,e)=> BuySeatsBtn_Click(s,e);
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid1.Visibility = Visibility.Visible;
            SeatsGrid.Visibility = Visibility.Collapsed;
            BuySeatsBtn.Visibility = Visibility.Collapsed;
            PriceLbl.Visibility = Visibility.Collapsed;
        }

        private void BuySeatsBtn_Click(object sender, RoutedEventArgs e)
        {
            if(selectedSeats.Count == 0)
            {
                ErrorWindow err;
                if (MainWindow.isEnglish)
                {
                    err = new ErrorWindow("Choose at least one seat!", "Error");
                }
                else
                {
                    err = new ErrorWindow("Izaberite bar jedno sjedište!", "Greška");
                }
                
                err.Owner = this;
                err.ShowDialog();
                return;
            }
            string sjediste = string.Join(" ", selectedSeats);
            int terminId = this.TerminId;
            MySqlConnection conn1 = MainWindow.OpenConnection();
            MySqlCommand cmd1 = conn1.CreateCommand();
            cmd1.CommandText = "INSERT INTO `hci`.`kupovina` (`sjediste`, `cijena`, `datum`, `terminId`, `korisnickoIme`) VALUES('"+sjediste+"', '"+ (selectedSeats.Count * (this.is3D ? 6 : 5)) + "', '"+this.DateStr+"', '"+terminId+"', '"+this.Usrnm+"');";
            cmd1.ExecuteNonQuery();
            conn1.Close();
            this.Close();
        }

        private void translate()
        {
            ChooseSeatsBtn.Content = "Choose seats";
            BuySeatsBtn.Content = "Buy tickets for chosen seats";
            chooseShowtimeLbl.Content = "Choose showtime";
            chooseTimeAndHall.Content = "Choose time and hall:";
            chooseDateLbl.Content = "Choose date:";
            this.Title = "Buy ticket";
        }

        private void setTheme()
        {
            SolidColorBrush scb = null;
            FontFamily ff = null;
            switch (this.theme)
            {
                case 0:
                    {
                        darkPic.Visibility = Visibility.Visible;
                        brightPic.Visibility = Visibility.Collapsed;
                        colorPic.Visibility = Visibility.Collapsed;
                        scb = Brushes.White;
                        ff = new FontFamily("Segoe UI");
                        break;
                    }

                case 1:
                    {
                        darkPic.Visibility = Visibility.Collapsed;
                        brightPic.Visibility = Visibility.Visible;
                        colorPic.Visibility = Visibility.Collapsed;
                        scb = Brushes.Black;
                        ff = new FontFamily("Segoe UI");
                        break;
                    }

                case 2:
                    {
                        darkPic.Visibility = Visibility.Collapsed;
                        brightPic.Visibility = Visibility.Collapsed;
                        colorPic.Visibility = Visibility.Visible;
                        scb = Brushes.Black;
                        ff = new FontFamily("Comic Sans MS");
                        break;
                    }
            }
            ChooseSeatsBtn.Foreground = scb;
            ChooseSeatsBtn.FontFamily = ff;
            BuySeatsBtn.Foreground = scb;
            BuySeatsBtn.FontFamily = ff;
            chooseDateLbl.Foreground = scb;
            chooseDateLbl.FontFamily = ff;
            chooseShowtimeLbl.Foreground = scb;
            chooseShowtimeLbl.FontFamily = ff;
            chooseTimeAndHall.Foreground = scb;
            chooseTimeAndHall.FontFamily = ff;

            pickTime.Foreground = scb;
            pickTime.FontFamily = ff;

            pickDate.Foreground = scb;
            pickDate.FontFamily = ff;

            arwBack.Foreground = scb;
            arwBack.FontFamily = ff;

            PriceLbl.Foreground = scb;
            PriceLbl.FontFamily = ff;

        }

        public void changeTheme(object i)
        {
            Control control = null;
            TextBlock tb = null;
            if (i is Control)
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

            if (i is TextBlock)
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
    }
}