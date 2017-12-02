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

namespace AbaSim.Wpf
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            Model = new Connection.ModelConnection();
            DataContext = Model;
            InitializeComponent();
        }

        public async Task setProgrammCode(string text)
        {
            if (text.Equals(""))
            {
                throw new Wpf.Exception.Exception(ExceptionId.NoProgrammCode, "Programm Code was: \"\"");
            }
            Model.setProgrammText(text);
            await startCompilation();
        }

        private void onStepValueChenged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int wantedStep = (int)StepSlider.Value;
           // StepLabel.Content = "Step: " + wantedStep;
        }

        private async Task startCompilation()
        {
            InformationLabel.Text = "Compiling";
            await Task.Run(()=>Model.startCompiling());
            InformationLabel.Text = "Running";
            await Model.startComputation();
            InformationLabel.Text = "Finished";
            reciveFromModel();
        }

        private void reciveFromModel()
        {
            StepSlider.Maximum = Model.Steps;

            if (Model.Steps>0)
            {
                StepSlider.IsEnabled = true;
                AnalysisTabItem.IsEnabled = true;
                StatisticsTabItem.IsEnabled = true;
            }
        }

        private Connection.ModelConnection Model;
    }
}
