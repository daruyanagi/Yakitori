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

namespace Yakitori.WPF.Views
{
	/// <summary>
	/// SettingWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class SettingWindow : Window
	{
		public SettingWindow()
		{
			InitializeComponent();
		}

		private void CancelBotton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CloseBotton_Click(object sender, RoutedEventArgs e)
		{
			SaveSettings();
			Close();
		}

		private void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
			SaveSettings();
			ApplyButton.IsEnabled = false;
		}

		private static void SaveSettings()
		{
			var settings = Yakitori.WPF.Properties.Settings.Default;

			if (settings.Count < 0) settings.Count = 5; // 不正な値だったらデフォ値に戻しておく

			settings.Save();
		}

		private void Value_Changed(object sender, RoutedEventArgs e)
		{
			if (ApplyButton != null) ApplyButton.IsEnabled = true;
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (ApplyButton != null) ApplyButton.IsEnabled = true;
		}
	}
}
