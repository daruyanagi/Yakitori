using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Yakitori.WPF.ViewModels
{
	public class SettingViewModel:BindableBase
	{
		public RelayCommand DesktopCaptureCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/d"));
		public RelayCommand DesktopCaptureAndSaveCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/D"));
		public RelayCommand ActiveWindowCaptureCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/a"));
		public RelayCommand SelectedRectCaptureCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/r"));
		public RelayCommand SaveToFileCaptureCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/s"));
		public RelayCommand OpenScreenshotFolderCommand { get; } = new RelayCommand(() => System.Diagnostics.Process.Start("yakitori.wpf.exe", "/o"));

		public SettingViewModel()
		{

		}

		~SettingViewModel()
		{
			Yakitori.WPF.Properties.Settings.Default.Save();
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
