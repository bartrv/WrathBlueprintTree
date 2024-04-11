using Microsoft.Extensions.Logging;
using System.IO;

namespace WrathBlueprintTree;


public partial class BpFile{

    private readonly static FilePickerFileType customFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.text.json" } }, // UTType values
                        { DevicePlatform.Android, new[] { "text/json" } }, // MIME type
                        { DevicePlatform.WinUI, new[] { ".txt", ".bp", ".json" } }, // file extension
                        { DevicePlatform.Tizen, new[] { "*/*" } },
                        { DevicePlatform.macOS, new[] { "txt", "bp", "json" } }, // UTType values
                    });

    

    //public static async Task<FileResult> PickAndShow(PickOptions options)
    public static async Task<List<string>> OpenBpFile()
    {
        string fileData = "";
        PickOptions options = new() { PickerTitle = "Please select a blueprint file", FileTypes = customFileType, };
        try
        {
            //this hangs here, once the file picker is used to select a file
            var result = await FilePicker.PickAsync(options);
            
            if (result != null)
            {
                if (result.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith(".bp", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {

                    using (StreamReader sr = File.OpenText(result.FullPath))
                    {
                        var s = (string)"";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                            fileData += s;
                        }
                    }
                }
            
            var thisFullPath = (result.FullPath != null) ? result.FullPath : "Error: null path";
            return (List<string>)[fileData,result.FullPath];
            }
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
            Console.WriteLine(ex);
            return (List<string>)["None","File Picker Exited, User canceled"];
        }

        return (List<string>)["None","Error on file picker"];
    }
    
}

