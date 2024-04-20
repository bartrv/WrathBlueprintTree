using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace WrathBlueprintTree;

public interface IDataTransfer{
    string OpenedFile { get; set; }
    string OpenedFileFullPath { get; set; }
    BlueprintObject IngestedBpObject { get; set; }
    Dictionary<string,dynamic> IngestedBpObjectFlat { get; set; }
    FullBpTreeCollection FullBpTree { get; set; } 
}

public class DataTransfer : IDataTransfer
{
    public string OpenedFile { get; set; } = "";
    public string OpenedFileFullPath { get; set; } = "";
    public BlueprintObject IngestedBpObject { get; set; }
    public Dictionary<string,dynamic> IngestedBpObjectFlat { get; set; } = new();
    public FullBpTreeCollection FullBpTree { get; set; }
}