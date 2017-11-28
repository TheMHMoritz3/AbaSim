using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AbaSim.Wpf
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void onCompileClicked(object sender, RoutedEventArgs e)
        {
            startSimulationWindowThread();
        }

        private async void startSimulationWindowThread()
        {
            Window1 simulationWindow = new Window1();
            simulationWindow.Show();
            await simulationWindow.setProgrammCode(ProgrammCodeTextBox.Text);
        }
    }
}
