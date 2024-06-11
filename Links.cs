using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Animations;


namespace WrathBlueprintTree;

public partial class Links
{
	IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
   
    public static void initTestBezierCurve(object sender, DragStartingEventArgs e)
    {
        //Point? relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent);
        Console.WriteLine("Sanity: initBCurve"+sender+", "+e.Data.Properties["Position"]);
    }


}