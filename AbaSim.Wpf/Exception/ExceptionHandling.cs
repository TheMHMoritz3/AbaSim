using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AbaSim.Wpf.Exception
{
    class ExceptionHandling
    {
        public static void catchException(System.Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        public static void catchException(Exception ex)
        {
            switch(ex.Id)
            {
                case ExceptionId.NoProgrammCode:
                    MessageBox.Show("No programmcode is given.\n"+ex.Message,"AbaSim",MessageBoxButton.OK,MessageBoxImage.Error,MessageBoxResult.OK);
                    break;
            }
        }
    }
}
