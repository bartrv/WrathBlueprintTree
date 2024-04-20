using System.Windows.Input;
using Microsoft.Extensions.Options;
//using Microsoft.UI.Xaml.Controls.Primitives;
//using Microsoft.UI.Windowing;
//using Microsoft.UI.Xaml;
namespace WrathBlueprintTree;

public partial class TreePage : ContentPage
{
	IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
	Dictionary<string,string> ButtonList = [];
	public TreePage()
	{
		InitializeComponent();
        //Button buttonAA = new Button {Text = "Button text"};
        int btnI = 0;
        int ButtonCount = BlueprintModels.ModelDictionary.Count;
		
		foreach (KeyValuePair<string, dynamic> entry in BlueprintModels.ModelDictionary) //Build the button list - Currently everything in the model(W.i.P), will need sub categories eventually
		{
			var newFrame = new Frame{BackgroundColor = Color.FromRgba("#AA9999FF"),
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3,
										WidthRequest= 200,
										HeightRequest = 24,
										VerticalOptions = LayoutOptions.Center,
										HorizontalOptions = LayoutOptions.Center
									};
            
			Label frameLabel = new Label{Text = entry.Value["buttonName"], 
										FontSize = 12, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Center,
										VerticalTextAlignment = TextAlignment.Center
									};
			
			newFrame.Content = frameLabel;

			DragGestureRecognizer DragFrame = new()
            {
                CanDrag = true
            };
			//DragFrame.DragStarting += OnDragStarting; - with a little explanation of the DragStarting notation from ChatGPT 
			DragFrame.DragStarting += (sender, e) => OnDragStarting(sender, e, entry.Key); // final suggestion from Chat
			newFrame.GestureRecognizers.Add(DragFrame);
			
            treeDragListA.Add(newFrame);
        }   
	}

	Point? relativeToContainerPosition;
	public void OnPointerMovedInTreeView(object sender, PointerEventArgs e)
	{
		relativeToContainerPosition = e.GetPosition((View)sender);
		Console.WriteLine(relativeToContainerPosition);
	}

	public void OnDragStarting(object? sender, DragStartingEventArgs e, string bpModelKey)
        {
            Console.WriteLine($"Frame dragged! : {bpModelKey}");
			e.Data.Text = bpModelKey;
        }

	public async void OnDropIntoTreeLayout(object sender, DropEventArgs e)
	{
		string bpModelKey = await e.Data.GetTextAsync();
		Console.WriteLine(sender.ToString());
		Console.WriteLine(e.ToString());
		Console.WriteLine($"bpModelKey: {bpModelKey}");
		object bpModelReferanceObject;
		if (BlueprintModels.checkBpModel(bpModelKey, out bpModelReferanceObject)){
			Dictionary<string,dynamic> bpModelDictionary = (Dictionary<string,dynamic>)bpModelReferanceObject;

			Console.WriteLine($"bpModel $type: {bpModelDictionary["$type"]}");
			Console.WriteLine((sender as Element)?.Parent as AbsoluteLayout);
			AbsoluteLayout? senderParent = (sender as Element)?.Parent as AbsoluteLayout;
			//Console.WriteLine((sender);
			//relativeToContainerPosition = e.GetPosition(treeScrollView);
			relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent);
			//if (relativeToContainerPosition!=null){
				Console.WriteLine($"X = {relativeToContainerPosition.Value.X}");
				Console.WriteLine($"Y = {relativeToContainerPosition.Value.Y}");
			//}
			placeNewBpTemplateFrame(senderParent, relativeToContainerPosition, bpModelKey);
		} else {
			Console.WriteLine("Exception!! BlueprintModels Dictionaty key not found, this should be immpossible. TreePage.OnDropIntoTreeLayout");
		}
		// Perform logic to take action based on retrieved value.
	}

	public void placeNewBpTemplateFrame(AbsoluteLayout targetParent, Point? dropPosition, string bpModelKey)
	{

		Frame newGenFrame = GenerateNewBpTemplateFrame(bpModelKey);
		AbsoluteLayout.SetLayoutBounds(newGenFrame, new Rect(dropPosition.Value.X,dropPosition.Value.Y,200,400));
		//Todo Generate new class to hold data object
		targetParent.Add(newGenFrame);
	}

	public Frame GenerateNewBpTemplateFrame(string bpModelKey)
	{
		//var dropData = "testData";
		int testWidth = 200;
		int testHeight = 400;
		Frame bpFrameContainer = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Overall Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 0
										};
		AbsoluteLayout bpFrameContainerAbsLayout = [];
		
		BoxView bpNode = new BoxView{BackgroundColor = Color.FromRgba("#BBBBBBFF"),
						Margin = new Thickness (0,0),
						//WidthRequest = 16,
						//HeightRequest = 16,
						CornerRadius = 8
						};
		AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(testWidth-20,28,16,16));

		Frame bpFrameVisualMain = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3
										};
		AbsoluteLayout.SetLayoutBounds(bpFrameVisualMain, new Rect(10,0,testWidth-20,testHeight-4));

		VerticalStackLayout bpFrameVisualMainVertLayout = [];

		
		Label bpframeLabel = new Label{Text = $": {BlueprintModels.ModelDictionary[bpModelKey]["buttonName"]} :", 
										FontSize = 12, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Center,
										VerticalTextAlignment = TextAlignment.Start
									};
		Label bpframeLineItem = new Label{Text = $"$type: {BlueprintModels.ModelDictionary[bpModelKey]["$type"]}", 
										FontSize = 10, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Start,
										VerticalTextAlignment = TextAlignment.Start
									};
		bpFrameVisualMainVertLayout.Add(bpframeLabel);		
		bpFrameVisualMainVertLayout.Add(bpframeLineItem);
		bpFrameVisualMain.Content = bpFrameVisualMainVertLayout;

		bpFrameContainerAbsLayout.Add(bpFrameVisualMain);
		bpFrameContainerAbsLayout.Add(bpNode);
		bpFrameContainer.Content = bpFrameContainerAbsLayout;
		return bpFrameContainer;
		
	}
}

