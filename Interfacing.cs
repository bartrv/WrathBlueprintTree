using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;

namespace WrathBlueprintTree;

public interface IDataTransfer{
    string OpenedFile { get; set; }
    string OpenedFileFullPath { get; set; }
    bool IsFile { get; set; }  //is the current data originally loaded from a file? 
    bool IsChanged { get; set; }  //has current data been changed since loaded
    bool IsNew { get; set; }  //is this data a new tree, not loaded from a file
    bool IsSaved { get; set; } //has the tree been saved/exported since the mose receint change
    VerticalStackLayout? SidePanelContainer { get; set; }
    BlueprintObject? IngestedBpObject { get; set; }
    Dictionary<string,dynamic> IngestedBpObjectFlat { get; set; }
    FullBpTreeCollection? FullBpTree { get; set; } 
}

public class DataTransfer : IDataTransfer
//public class DataTransfer : IDataTransfer, INotifyPropertyChanged
{
    public bool IsFile { get; set; } = false; //is the current data originally loaded from a file? 
    public bool IsChanged { get; set; } = false;  //has current data been changed since loaded
    public bool IsNew { get; set; } = false;  //is this data a new tree, not loaded from a file
    public bool IsSaved { get; set; } = false; //has the tree been saved/exported since the mose receint change
    public string OpenedFile { get; set; } = "";
    public string OpenedFileFullPath { get; set; } = "";
    public VerticalStackLayout? SidePanelContainer { get; set; }
    public BlueprintObject? IngestedBpObject { get; set; }
    public Dictionary<string,dynamic> IngestedBpObjectFlat { get; set; } = new();
    private FullBpTreeCollection? _fullBpTree; 
    public FullBpTreeCollection? FullBpTree { get; set; }
   /*
    public FullBpTreeCollection? FullBpTree 
    { 
        get => _fullBpTree; 
        set
        {
            if (_fullBpTree != value)
            {
                _fullBpTree = value;
                OnPropertyChanged(nameof(FullBpTree));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    */
}
