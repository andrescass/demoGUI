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
using System.Windows.Threading;

namespace demoGUI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private bool launching;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += Timer_Tick;
            launching = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(launching)
            {
                uploadBut.Background = Brushes.Green;
                launching = false;
            }
            else
            {
                uploadBut.Background = Brushes.LightGray;
                timer.Stop();
            }
            
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void uploadBut_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            launching = true;
        }
    }
}
