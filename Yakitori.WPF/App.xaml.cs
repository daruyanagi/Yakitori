using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace Yakitori.WPF
{
	/// <summary>
	/// App.xaml の相互作用ロジック
	/// </summary>
	public partial class App : Application
	{
		private static IKeyboardSimulator simulator = new InputSimulator().Keyboard;
		private static string[] args;

		public App()
		{
			args = Environment.GetCommandLineArgs();

			// 必須オプションを処理
			if (ProcessNecessaryOption())
			{

			}
			else // 必須オプションが指定されていない場合
			{
				// 設定画面を表示する
				ShowSettingWindow();
			}

			App.Current.Shutdown(); // メッセージポンプが残ってるかもなので、シャットダウンしておく
		}

		private static string GetScreenShotsFolder() => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Screenshots");
		private static string GetScreenShotsFile() => System.IO.Path.ChangeExtension(System.IO.Path.Combine(GetScreenShotsFolder(), DateTime.Now.ToString("yyyyMMdd-hhmmss")), "png");
		private static void OpenScreenShotsFolder() => System.Diagnostics.Process.Start("explorer.exe", GetScreenShotsFolder());

		private void ShowSettingWindow()
		{
			MainWindow = new Views.SettingWindow();
			MainWindow.ShowDialog();
		}

		private bool ProcessNecessaryOption()
		{
			if (args.Contains("/DESKTOP") || args.Contains("/D")) // 大文字は保存あり
			{
				WaitFor(() => simulator.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.SNAPSHOT));
				// ClipboardHelper.SaveImageToFile(GetScreenShotsFile()); 標準で保存される
				Console.WriteLine($"/DESKTOP is processed");
			}
			else if (args.Contains("/desktop") || args.Contains("/d")) // 小文字は保存しない
			{
				WaitFor(() => simulator.KeyPress(VirtualKeyCode.SNAPSHOT));
				Console.WriteLine($"/desktop is processed");
			}
			else if (args.Contains("/ACTIVE") || args.Contains("/A"))
			{
				WaitFor(() => simulator.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SNAPSHOT));
				// ClipboardHelper.SaveImageToFile(GetScreenShotsFile()); うまく保存されない
				Console.WriteLine($"/ACTIVE is processed");
			}
			else if (args.Contains("/active") || args.Contains("/a"))
			{
				WaitFor(() => simulator.ModifiedKeyStroke(VirtualKeyCode.MENU, VirtualKeyCode.SNAPSHOT));
				Console.WriteLine($"/active is processed");
			}
			else if (args.Contains("/RECT") || args.Contains("/R"))
			{
				WaitFor(() => simulator.ModifiedKeyStroke(new VirtualKeyCode[] { VirtualKeyCode.LWIN, VirtualKeyCode.SHIFT, }, VirtualKeyCode.VK_S));
				// ClipboardHelper.SaveImageToFile(GetScreenShotsFile()); ユーザーインタラクションがあるためうまく動かない
				Console.WriteLine($"/RECT is processed");
			}
			else if (args.Contains("/rect") || args.Contains("/r"))
			{
				WaitFor(() => simulator.ModifiedKeyStroke(new VirtualKeyCode[] { VirtualKeyCode.LWIN, VirtualKeyCode.SHIFT, }, VirtualKeyCode.VK_S));
				Console.WriteLine($"/rect is processed");
			}
			else if (args.Contains("/save") || args.Contains("/s"))
			{
				ClipboardHelper.SaveImageToFile(GetScreenShotsFile());
				Console.WriteLine($"/save is processed as necessary option");
			}
			else if (args.Contains("/open") || args.Contains("/o"))
			{
				OpenScreenShotsFolder();
				Console.WriteLine($"/open is processed as necessary option");
			}
			else
			{
				return false; // 必須オプションがなかった
			}

			return true; // 必須オプションが処理された
		}

		private static void WaitFor(Action action)
		{
			if (!Yakitori.WPF.Properties.Settings.Default.IsCountDownEnabled)
			{
				action();
				PlaySound();

				return;
			}

			var count = Yakitori.WPF.Properties.Settings.Default.Count;
			var window = new Views.CountDownWindow();
			var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };

			timer.Tick += (sender, args) =>
			{
				window.CountDownLabel.Text = count.ToString();
				if (--count < 0)
				{
					timer.Stop();
					window.Close();

					action();
					PlaySound();
				}
			};

			window.Loaded += (sender, args) => timer.Start();
			window.ShowDialog();
		}

		private static void PlaySound()
		{
			if (!Yakitori.WPF.Properties.Settings.Default.IsSoundEffectEnabled) return;

			var player = new System.Media.SoundPlayer(@".\Media\ScreenShot.wav");
			player.PlaySync();
		}

		private static void ProcessAdditionalOption()
		{
			// * /S(AVE)：スクリーンショットフォルダーにクリップボードのイメージを保存
			if (args.Contains("/SAVE") || args.Contains("/S"))
			{
				ClipboardHelper.SaveImageToFile(GetScreenShotsFile());
				Console.WriteLine($"/SAVE is processed as additional option");
			}

			// * /O(PEN)：保存フォルダーを開く
			if (args.Contains("/OPEN") || args.Contains("/O"))
			{
				OpenScreenShotsFolder();
				Console.WriteLine($"/OPEN is processed as additional option");
			}
		}
	}
}
