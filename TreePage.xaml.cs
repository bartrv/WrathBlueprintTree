using Microsoft.Extensions.Options;
//using Microsoft.UI.Windowing;
//using Microsoft.UI.Xaml;
namespace WrathBlueprintTree;

public partial class TreePage : ContentPage
{
	
	public TreePage()
	{
		InitializeComponent();
        //Button buttonAA = new Button {Text = "Button text"};
        int btnI = 0;
        int ButtonCount = 12;
        for (; btnI < ButtonCount; btnI++){
			Frame newFrame = new Frame{BackgroundColor = Color.FromRgba("#AA9999FF"),
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3,
										WidthRequest= 200,
										HeightRequest = 24,
										VerticalOptions = LayoutOptions.Center,
										HorizontalOptions = LayoutOptions.Center
									};
			DragGestureRecognizer DragFrame = new DragGestureRecognizer{};
			DragFrame.CanDrag = true;
			newFrame.GestureRecognizers.Add(DragFrame);
			Label frameLabel = new Label{Text = $"Button text: {btnI}", 
										FontSize = 12, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Center,
										VerticalTextAlignment = TextAlignment.Center
									};
			newFrame.Content = frameLabel;
            treeDragListA.Add(newFrame);
        }   
	}

	Point? relativeToContainerPosition;
	public void OnPointerMovedInTreeView(object sender, PointerEventArgs e)
	{
		relativeToContainerPosition = e.GetPosition((View)sender);
		Console.WriteLine(relativeToContainerPosition);
	}

	public void OnDragHandler(object sender, DragEventArgs e)
	{
		relativeToContainerPosition = e.GetPosition((View)sender);
		Console.WriteLine(relativeToContainerPosition);

	}
	public void OnDropIntoTreeLayout(object sender, DropEventArgs e)
	{
		Console.WriteLine(sender.ToString());
		Console.WriteLine(e.ToString());
		Console.WriteLine((sender as Element)?.Parent as AbsoluteLayout);
		AbsoluteLayout senderParent = (sender as Element)?.Parent as AbsoluteLayout;
		//Console.WriteLine((sender);
		//relativeToContainerPosition = e.GetPosition(treeScrollView);
		relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent);
		//if (relativeToContainerPosition!=null){
			Console.WriteLine($"X = {relativeToContainerPosition.Value.X}");
			Console.WriteLine($"Y = {relativeToContainerPosition.Value.Y}");
		//}
		placeNewBpTemplateFrame(senderParent, relativeToContainerPosition, "TestData Keyed");
		// Perform logic to take action based on retrieved value.
	}

	public void placeNewBpTemplateFrame(AbsoluteLayout targetParent, Point? dropPosition, string dropData)
	{
		Frame newBpFrame = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"),
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3,
										//WidthRequest= 200,
										//HeightRequest = 400,
										//VerticalOptions = LayoutOptions.Top,
										//HorizontalOptions = LayoutOptions.Left
									};
		//newBpFrame.AbsoluteLayout.LayoutBoundsProperty=(5,5,50,50);

		Label newBpframeLabel = new Label{Text = $"Data text: {dropData}", 
										FontSize = 12, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Center,
										VerticalTextAlignment = TextAlignment.Center
									};
		newBpFrame.Content = newBpframeLabel;
		AbsoluteLayout.SetLayoutBounds(newBpFrame, new Rect(dropPosition.Value.X,dropPosition.Value.Y,200,400));
        //AbsoluteLayout.SetLayoutFlags(newBpFrame, AbsoluteLayoutFlags.PositionProportional);
		//targetParent.Add(newBpFrame, new Rect(dropPosition.Value.X,dropPosition.Value.Y,200,400));
		targetParent.Add(newBpFrame);
	}
}

