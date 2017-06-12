using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yakitori.ViewModels
{
    public class SettingsWindowViewModel : BindableBase
    {
        ~SettingsWindowViewModel()
        {
            Yakitori.Properties.Settings.Default.Save();
        }

        public string Title
        {
            get
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var assembly_info = assembly.GetName();

                return $"{assembly_info.Name} {assembly_info.Version}";
            }
        }
    }
}
