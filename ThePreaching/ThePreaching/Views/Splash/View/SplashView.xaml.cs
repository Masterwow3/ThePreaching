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
using System.Windows.Shapes;
using ThePreaching.Views.Main.View;
using ThePreaching.Views.Splash.ViewModel;

namespace ThePreaching.Views.Splash.View
{
    /// <summary>
    /// Interaction logic for SplashView.xaml
    /// </summary>
    public partial class SplashView : Window
    {
        public SplashView()
        {
            InitializeComponent();
            this.DataContext = ViewModel= new SplashViewModel();
            Loading();
        }

        private async Task Loading()
        {
           await  ViewModel.Initializing();
            new MainWindow().Show();
            this.Close();
        }

        public SplashViewModel ViewModel { get; set; }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
