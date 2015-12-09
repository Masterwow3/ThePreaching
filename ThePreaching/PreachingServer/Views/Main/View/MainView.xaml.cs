using System.Windows;
using System.Windows.Controls;
using PreachingServer.Views.Main.ViewModel;

namespace PreachingServer.Views.Main.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = MainViewModel= new MainViewModel();
        }

        public MainViewModel MainViewModel { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel.Settings.SaveSettings();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainViewModel.Settings.LoadSettings();
        }
    }
}
