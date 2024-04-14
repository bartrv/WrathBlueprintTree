using Microsoft.Extensions.Options;
//using Microsoft.UI.Windowing;
//using Microsoft.UI.Xaml;
namespace WrathBlueprintTree;

public partial class MainPage : ContentPage
{
	int count = 0;
	LastFilePath LastBPFilePath = new LastFilePath();
	object bpObject = new Dictionary<string,dynamic>{};
	bool bpAvailable = false;
	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = this; // set the bindingContext to the page itself
		LastBPFilePath.SetPath("C:\\Users"); //File Picker has Documents folder hard coded in the Maui Library.... WTF
		//Title = Set in AppShell.xaml
		//Page title (in menu bar) ALSO set in AppShell.xaml
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;
		Console.WriteLine();
		//if (count == 1)
		//	CounterBtn.Text = $"Clicked {count} time";
		//else
		//	CounterBtn.Text = $"Clicked {count} times";

		//SemanticScreenReader.Announce(CounterBtn.Text);

		//var bpObject = new BlueprintObject(editor.Text);
	}

	public void MenuBarFileOpen(object sender, System.EventArgs e)
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
}

