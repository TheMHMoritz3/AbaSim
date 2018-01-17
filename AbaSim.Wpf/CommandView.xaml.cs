using AbaSim.Wpf.Connection;
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

namespace AbaSim.Wpf.Widgets
{
    /// <summary>
    /// Interaktionslogik für CommandView.xaml
    /// </summary>
    public partial class CommandView : UserControl
    {
        public CommandView()
        {
            InitializeComponent();
        }

        public void setDataBinding(List<Command> data)
        {
            System.Diagnostics.Trace.WriteLine(data.Count);
            CommandViewList.DataContext = data;
        }
    }
}
