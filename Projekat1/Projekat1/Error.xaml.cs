using System.Windows;

namespace Projekat1
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow(string message, string title = "Error")
        {
            InitializeComponent();
            this.Title = title;
            MessageTextBlock.Text = message;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
