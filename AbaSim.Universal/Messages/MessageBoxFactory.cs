using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AbaSim.Universal.Messages
{
    class MessageBoxFactory
    {
        private static MessageDialog CalculationDialog = null;
        public static MessageDialog getMessageBoxCalculation()
        {
            if (CalculationDialog == null)
            {
                CalculationDialog = new MessageDialog("until the Calculation finished.");
                CalculationDialog.Title = "Stay Tuned";
                //CalculationDialog = MessageDialogOptions.None;
                //CalculationDialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            }
                //var res = await dialog.ShowAsync();

            //if ((int)res.Id == 0)
            //{ *** }
            return CalculationDialog;
        }
    }
}
