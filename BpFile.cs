using Microsoft.Extensions.Logging;
using System.IO;
using System.Net.Cache;
using System.Runtime.CompilerServices;

namespace WrathBlueprintTree;

public class LastFilePath()
{   
    string lastPath = "C:\\Users";
    public void SetPath (string req){
        if (req !=""){
            this.lastPath = req;
        }
    }
    public string GetPath (){
        return lastPath;
    }
}
public partial class BpFile{
    public static (string,string) Open(object sender, EventArgs e)
	{
		FileResult myFileResult = null;
		string fileData = "";
		FilePickerFileType customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.text.json" } }, // UTType values
                        { DevicePlatform.Android, new[] { "text/json" } }, // MIME type
                        { DevicePlatform.WinUI, new[] { ".txt", ".bp", ".json" } }, // file extension
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { "txt", "bp", "json" } }, // UTType values
                    });
		PickOptions options = new() { PickerTitle = "Blueprint", FileTypes = customFileType, };

        Task.Run(async () =>
        {
            myFileResult = await FilePicker.PickAsync(options);
        }).Wait();

        if (myFileResult != null)
        {
			string pathOnly = myFileResult.FullPath[..(myFileResult.FullPath.Length - myFileResult.FileName.Length)];
			using (StreamReader sr = File.OpenText(myFileResult.FullPath))
				{
					var s = (string)"";
					while ((s = sr.ReadLine()) != null)
					{
						Console.WriteLine(s);
						fileData += "\r"+s;
					}
				}
			//var bpObject = new BlueprintObject(fileData);
            return ((string)fileData, (string)myFileResult.FullPath);
        }
        return ("none","none");
	}
    
}

