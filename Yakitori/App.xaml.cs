using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Yakitori
{
    using System.Xml;
    using Windows.UI.Notifications;
    using WindowsInput;
    using WindowsInput.Native;

    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var args = Environment.GetCommandLineArgs();

            var option = args.Length == 1 ? string.Empty : args[1];
            var simulator = new InputSimulator();
            var keybord = simulator.Keyboard;

            switch (option)
            {
                case "/d": /* デスクトップ全体を撮影 */
                    ShowCountDown(() =>
                    {
                        keybord.KeyPress(VirtualKeyCode.SNAPSHOT);
                        ShowToast("Capture is completed, and saved to clipboard.", "Desktop");
                        App.Current.Shutdown();
                    });
                    break;
                case "/f": /* デスクトップ全体を撮影してファイルへ保存 */
                    ShowCountDown(() =>
                    {
                        keybord.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.SNAPSHOT);
                        ShowToast(@"Capture is completed, and saved to ""Pictures/Screenshots"" folder.", "Desktop");
                        App.Current.Shutdown();
                    });
                    break;
                case "/a": /* アクティブウィンドウを撮影 */
                    ShowCountDown(() =>
                    {
                        keybord.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SNAPSHOT);
                        ShowToast("Capture is completed, and saved to clipboard.", "Active Window");
                        App.Current.Shutdown();
                    });
                    break;
                case "/r": /* デスクトップの選択領域を撮影 */
                    ShowCountDown(() =>
                    {
                        var modefier_keys = new VirtualKeyCode[]
                        {
                            VirtualKeyCode.LWIN,
                            VirtualKeyCode.SHIFT,
                        };
                        keybord.ModifiedKeyStroke(modefier_keys, VirtualKeyCode.VK_S);
                        App.Current.Shutdown();
                    });
                    break;
                case "/s": /* クリップボードの画像をスクリーンショットフォルダーへ保存 */
                    try
                    {
                        var image = Clipboard.GetImage();

                        if (image != null)
                        {
                            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
                            var path = System.IO.Path.Combine(
                                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                                "Screenshots",
                                DateTime.Now.ToString("yyyyMMdd-hhmmss"));
                            path = System.IO.Path.ChangeExtension(path, "png");

                            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                            {
                                enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));
                                enc.Save(stream);
                            }

                            ShowToast("The image is saved.", "Clipboard", path, (n, o) => 
                            {
                                System.Diagnostics.Process.Start("explorer.exe", $@"/select,""{path}""");
                            });
                        }
                        else
                        {
                            ShowToast("There are no images.", "Clipboard");
                        }
                    }
                    catch (Exception exception)
                    {
                        ShowToast(exception.Message, exception.GetType().Name);
                    }
                    finally
                    {
                        App.Current.Shutdown();
                    }
                    break;
                case "/o":
                    var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    folder = System.IO.Path.Combine(folder, "Screenshots");
                    System.Diagnostics.Process.Start("explorer.exe", folder);
                    App.Current.Shutdown();
                    break;
                default:
                    MainWindow = new Views.SettingsWindow();
                    MainWindow.Show();
                    break;
            }
        }

        public static void ShowToast(string message, string title, string image_path = "", 
            Windows.Foundation.TypedEventHandler<ToastNotification, object> handler = null)
        {
            if (!Yakitori.Properties.Settings.Default.IsNotificationEnabled) return;

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var app_name = assembly.GetName().Name;
            var app_path = System.IO.Path.GetDirectoryName(assembly.Location);
            
            image_path = string.IsNullOrEmpty(image_path)
                ? "file:///" + System.IO.Path.Combine(app_path, "app.png")
                : "file:///" + image_path;

            // Get a toast XML template
            var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Text
            var text_elements = xml.GetElementsByTagName("text");
            text_elements[0].AppendChild(xml.CreateTextNode(title));
            text_elements[1].AppendChild(xml.CreateTextNode(message));

            // Image
            var image_elements = xml.GetElementsByTagName("image");
            image_elements[0].Attributes.GetNamedItem("src").NodeValue = image_path;

            // Audio
            var audio_element = xml.CreateElement("audio");
            audio_element.SetAttribute("src", "ms-winsoundevent:Notification.Default");
            audio_element.SetAttribute("loop", "false");
            xml.DocumentElement.AppendChild(audio_element);

            var toast = new ToastNotification(xml);

            if (handler != null) toast.Activated += handler;
            
            ToastNotificationManager.CreateToastNotifier(app_name).Show(toast);
        }

        public static void ShowCountDown(Action action)
        {
            if (!Yakitori.Properties.Settings.Default.IsCountDownEnabled)
            {
                action();

                return;
            }

            var count = Yakitori.Properties.Settings.Default.Count;
            var window = new Views.CountDownWindow();
            var timer = new System.Windows.Threading.DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (sender, args) =>
            {
                window.CountDownLabel.Text = count.ToString();

                count = count - 1;

                if (count < 0)
                {
                    window.Close();

                    action();
                }
            };

            window.Loaded += (sender, args) => timer.Start();

            window.Show();
        }
    }
}
