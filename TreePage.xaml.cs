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

	public void OnDropBpOption(object sender, DropEventArgs e)
	{
		Console.WriteLine(sender.ToString());
		Console.WriteLine(e.ToString());

		// Perform logic to take action based on retrieved value.
	}

	/*public void MenuBarFileOpen(object sender, System.EventArgs e)
	//async void MenuTesting(object sender, System.EventArgs e)
	{
		//OpenFile_Bp(sender, e);
		var (RawFileText, fileFullPath) = BpFile.Open(sender, e);
		editor.Text = RawFileText;
		bpFilePath.Text = fileFullPath;
		bpObject = new BlueprintObject(RawFileText);
		if (bpObject is WrathBlueprintTree.BlueprintObject){
			bpAvailable = true;
		}
		//await DisplayAlert("Alert", "You have been alerted", "OK");
	}

	void OnEditorTextChanged(object sender, TextChangedEventArgs e)
	{
		string oldText = e.OldTextValue;
		string newText = e.NewTextValue;
		string currentText = editor.Text;
		Console.WriteLine(currentText.Length);
	}
	
	public void RunBpDotTrace(object sender, TextChangedEventArgs e)
	{
		string text = ((Entry)sender).Text;
		//Console.WriteLine(text);
		//Console.WriteLine("bpObject = "+bpObject);
		if (bpAvailable){
			Console.WriteLine(text);
		}
		//ToDo - converts bpData dot syntax result to text/string and dumps it into a multi-line text box (editor?) 
	}
    */
}

