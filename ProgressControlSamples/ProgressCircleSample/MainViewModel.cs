using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProgressCircleSample
{
    public class MainViewModel : NotifyPropertyBase
    {

        private double percent;
        public double Percent
        {
            get => percent;
            set => SetProperty(ref percent, value);
        }

        public ICommand Reset
        {
            get
            {
                return new RelayCommand((x) => Percent = 0);
            }
        }

        public ICommand TestProgress
        {
            get
            {
                return new RelayCommand(async (x) =>
               {
                   for(int i = 0; i < 101; i++)
                   {
                       Percent = i;
                       await Task.Delay(100);
                   }
               });
            }
        }
    }
}
