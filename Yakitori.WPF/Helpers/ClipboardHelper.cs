using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class ClipboardHelper
{
	public static void SaveImageToFile(string path)
	{
		try
		{
			var image = System.Windows.Clipboard.GetImage(); if (image == null) return;
			var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();

			using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
			{
				enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));
				enc.Save(stream);
			}
		}
		catch (Exception exception)
		{
			System.Diagnostics.Debug.WriteLine($"Clipboard.SaveImageToFile() failed: {exception.Message}");
		}
	}
}
