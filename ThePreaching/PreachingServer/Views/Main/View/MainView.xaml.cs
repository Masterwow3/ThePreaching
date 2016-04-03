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
            this.DataContext = new MainViewModel();
        }
    }
}
