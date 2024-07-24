using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Animations;
using System.Runtime.CompilerServices;


namespace WrathBlueprintTree;

public partial class Links
{
	//IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
   
    public static (Microsoft.Maui.Controls.Shapes.Path, Guid) InitNew() //overload for creating a new line without referances - Mainly for testing purposes
    {
        return InitNew(null,null);
    }

    public static (Microsoft.Maui.Controls.Shapes.Path, Guid) InitNew(object? sender, DragStartingEventArgs? e = null)
    {
        IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();

        //AbsoluteLayout.add -> Maui.Controls.Shapes.Path
        //Maui.Controls.Shapes.Path.Data = PathGeometry
        //PathGeometry.Figures.Add -> Maui.Controls.Shapes.PathFigure <PathFigureCollection>
        //Maui.Controls.Shapes.PathFigure.StartPoint = Point(x,y)
        //Maui.Controls.Shapes.PathFigure.Segments.Add -> Maui.Controls.Shapes.BezierSegment <PathSegmentCollection>
        //Maui.Controls.Shapes.BezierSegment.Point(n) -> .Point1 = new Point(double, double)
        // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/shapes/geometries?view=net-maui-8.0#create-a-beziersegment

		PathGeometry pathGeometry = new(); //seed a geometry object to define the path shape 
        XferObject.NewLinkPathFigure = new(); //seed a Figure to hold the paths Microsoft.Maui.Controls.Shapes.PathFigure
		XferObject.NewLinkPath = new(); // Microsoft.Maui.Controls.Shapes.Path
		XferObject.NewLinkBezierSegment = new(); // seed a segment as part of the path Microsoft.Maui.Controls.Shapes.BezierSegment
		pathGeometry.Figures.Add(XferObject.NewLinkPathFigure);

		// Add the PathGeometry to a Path element and define style
		XferObject.NewLinkPath.Data = pathGeometry;
		XferObject.NewLinkPath.Stroke = Colors.Black;
		XferObject.NewLinkPath.StrokeThickness = 2;

		//AbsoluteLayout.SetLayoutBounds(path, new Rect(0,0,200,200));
		AbsoluteLayout.SetLayoutFlags(XferObject.NewLinkPath, AbsoluteLayoutFlags.None);

		//Calling use Case --> TreeDropContainer.Children.Add(XferObject.NewLinkPath); 
		Guid NewLinkPathId = XferObject.NewLinkPath.Id;
		string NewLinkPathIdAsString = NewLinkPathId.ToString();
        return (XferObject.NewLinkPath, NewLinkPathId);
    }
    
    public static void DefineNewPathParameters(string newPathId, double pOrigin,double p0X, double p0Y, double p1X, double p1Y, double p2X, double p2Y, double p3X, double p3Y)
    {
		// Define a BezierSegment control points, origin, and build state
        IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
		
        XferObject.NewLinkLineParameters = [pOrigin, p0X, p0Y, p1X, p1Y, p2X, p2Y, p3X, p3Y];

		XferObject.NewLinkPathFigure.StartPoint = new Point(XferObject.NewLinkLineParameters[1],XferObject.NewLinkLineParameters[2]); //StartPoint is a separate declaration
		XferObject.NewLinkPathFigure.Segments.Add(XferObject.NewLinkBezierSegment);
		XferObject.NewLinkBezierSegment.Point1 = new Point(XferObject.NewLinkLineParameters[3],XferObject.NewLinkLineParameters[4]);
		XferObject.NewLinkBezierSegment.Point2 = new Point(XferObject.NewLinkLineParameters[5],XferObject.NewLinkLineParameters[6]);
		XferObject.NewLinkBezierSegment.Point3 = new Point(XferObject.NewLinkLineParameters[7],XferObject.NewLinkLineParameters[8]);
    }

    public static void EmitNewPathToLinkList(string newPathId)
    {
        IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
		//Init WireTest Bezier Curve definition in XferObject.FullBpTree.BpTreeVM["Wires"] dictionary
		XferObject.FullBpTree.BpTreeVM["Wires"].Add(newPathId, new Dictionary<string,dynamic>{
			{"Guid", newPathId},
			{"ParentPanelId", ""},
			{"ParentPanelNodeId", ""},
			{"AnchorPanelId", ""},
			{"NodeList", new Dictionary<string,List<double>>{
				{"Point0", [0,0]},
				{"Point1", [0,0]},
				{"Point2", [0,0]},
				{"Point3", [0,0]},
			}},
            {"ObjectRef", XferObject.NewLinkPath}
		});

		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["ParentPanelId"] = "None";
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["ParentPanelNodeId"] = "None";
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["AnchorPanelId"] = "None";
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point0"][0] = XferObject.NewLinkLineParameters[1];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point0"][1] = XferObject.NewLinkLineParameters[2];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point1"][0] = XferObject.NewLinkLineParameters[3];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point1"][1] = XferObject.NewLinkLineParameters[4];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point2"][0] = XferObject.NewLinkLineParameters[5];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point2"][1] = XferObject.NewLinkLineParameters[6];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point3"][0] = XferObject.NewLinkLineParameters[7];
		XferObject.FullBpTree.BpTreeVM["Wires"][newPathId]["NodeList"]["Point3"][1] = XferObject.NewLinkLineParameters[8];

    }

    public static void initTestBezierCurveStart(object? sender, DragStartingEventArgs? e)
    {
        //Point? relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent);
        Console.WriteLine("Sanity: initBCurve"+sender+", "+e.Data.Properties["Position"]);
    }


}