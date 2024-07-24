//using Microsoft.Extensions.Logging;
//using Microsoft.VisualBasic;
//using System.Net.Cache;
//using System.Runtime.CompilerServices;
//using System.Collections.Generic;
//using System.ComponentModel;
using Microsoft.Maui.Controls.Shapes;

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
    FullBpTreeCollection? FullBpTree { get; set; } //
    List<double> NewLinkLineParameters { get; set; } //Parameters of a currently generated link line - to be transferred to linkList: state(0/none,1/link,2/anchor),p1(x,y),p2,p3,p4
    Microsoft.Maui.Controls.Shapes.Path NewLinkPath { get; set; } // any Bezier curve segment requires a Figure, Segment, and Path
    PathFigure NewLinkPathFigure { get; set; }  // any Bezier curve segment requires a Figure, Segment, and Path
    BezierSegment NewLinkBezierSegment { get; set; } // any Bezier curve segment requires a Figure, Segment, and Path
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
    public Dictionary<string,dynamic> IngestedBpObjectFlat { get; set; } = [];
    public FullBpTreeCollection? FullBpTree { get; set; } // .FullBpTreeVM(Tree-View Model), .tree(Data model) | .FullBpTreeVm["Wires"] | .FullBpTreeVm["Panels"] | .tree["Guid as string"]
    public List<double> NewLinkLineParameters {get; set;} = new();
    public Microsoft.Maui.Controls.Shapes.Path NewLinkPath { get; set; } = new();
    public PathFigure NewLinkPathFigure { get; set; } = new();
    public BezierSegment NewLinkBezierSegment { get; set; } = new();
}
