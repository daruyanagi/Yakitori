using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yakitori.ViewModels
{
    public class CountDownWindowViewModel : BindableBase
    {
        private int count_down = 5;

        public int CountDown
        {
            get { return count_down; }
            set { SetProperty(ref count_down, value); }
        }
    }
}
