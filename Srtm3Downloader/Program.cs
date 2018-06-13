using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Srtm3Downloader
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var d = args[0];
			Console.WriteLine("Destination: " + d);
			Directory.CreateDirectory(d);
			using (var hc = new HttpClient())
			{
				for (var x = 1; x <= 72; ++x)
				{
					for (var y = 1; y <= 24; ++y)
					{
						var n = $"srtm_{x:00}_{y:00}.zip";
						var p = Path.Combine(d, n);
						if (File.Exists(p))
						{
							Console.WriteLine(n + " already exists");
						}
						else
						{
							Console.Write(n + "\u2026");
							try
							{
								using (var s = await hc.GetStreamAsync("http://srtm.csi.cgiar.org/SRT-ZIP/SRTM_V41/SRTM_Data_GeoTiff/" + n))
								using (var f = File.OpenWrite(p))
									await s.CopyToAsync(f);
								Console.WriteLine("\rDownloaded " + n);
							}
							catch
							{
								File.Delete(p);
								Console.WriteLine("\rFailed to download " + n);
							}
						}
					}
				}
			}
		}
	}
}
