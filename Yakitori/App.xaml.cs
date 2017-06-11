using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Yakitori
{
    using WindowsInput;
    using WindowsInput.Native;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            var args = Environment.GetCommandLineArgs();

            var option = args.Length == 1 ? string.Empty : args[1];
            var simulator = new InputSimulator();
            var keybord = simulator.Keyboard;

            switch (option)
            {
                case "/d":
                    keybord.KeyPress(VirtualKeyCode.SNAPSHOT);
                    break;
                case "/a":
                    var window = new Views.CoundDownWindow();

                    window.Show(5, () => 
                    {
                        keybord.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SNAPSHOT);
                    });
                    break;
                case "/r":
                    var modefier_keys = new VirtualKeyCode[] 
                    {
                        VirtualKeyCode.LWIN,
                        VirtualKeyCode.SHIFT,
                    };
                    keybord.ModifiedKeyStroke(modefier_keys, VirtualKeyCode.VK_S);
                    break;
                default:
                    var app = new App();
                    app.InitializeComponent();
                    app.Run();
                    break;
            }
        }
    }
}
