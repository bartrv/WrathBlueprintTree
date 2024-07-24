using Microsoft.Extensions.Options;
using Microsoft.Maui.Controls;
using System;
#if WINDOWS
//using MsUiXaml = Microsoft.UI.Xaml;
//using MsUiXamlInput = Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml;
using Windows.System;
using Microsoft.UI.Xaml.Input;
#endif


namespace WrathBlueprintTree;

public partial class MainPage : ContentPage
{
	LastFilePath LastBPFilePath = new LastFilePath();
	//BlueprintObject bpObject;
	IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();

	object bpDataAsTable = new Dictionary<string,dynamic>{};
	bool bpAvailable = false;
	public MainPage()
	{
		InitializeComponent();
		this.BindingContext = this; // set the bindingContext to the page itself
		LastBPFilePath.SetPath("C:\\Users"); //File Picker has Documents folder hard coded in the Maui Library.... WTF
		//Title = Set in AppShell.xaml
		//Page title (in menu bar) ALSO set in AppShell.xaml
	}

	public async void MenuBarFileOpen(object sender, System.EventArgs e)
	//async void MenuTesting(object sender, System.EventArgs e)
	{
		//OpenFile_Bp(sender, e);
		var (RawFileText, fileFullPath) = BpFile.Open(sender, e);
		XferObject.OpenedFile = RawFileText;
		//editor.Text = RawFileText;
		editor.Text = XferObject.OpenedFile;
		//bpFilePath.Text = fileFullPath;
		XferObject.OpenedFileFullPath = fileFullPath;
		//bpObject = new BlueprintObject(RawFileText);
		XferObject.IngestedBpObject = new BlueprintObject(RawFileText);
		//if (bpObject is WrathBlueprintTree.BlueprintObject){
		if (XferObject.IngestedBpObject is WrathBlueprintTree.BlueprintObject){
			bpAvailable = true;
			//bpDataAsTable = bpObject.GenerateDataTables();
			XferObject.IngestedBpObjectFlat = XferObject.IngestedBpObject.GenerateDataTables();
			XferObject.FullBpTree = new FullBpTreeCollection(XferObject.IngestedBpObjectFlat);
			XferObject.IsFile = true;
			await Shell.Current.GoToAsync("//TreePage");
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

