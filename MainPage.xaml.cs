namespace WrathBlueprintTree;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);

		var bpObject = new BlueprintObject(editor.Text);
	}

	private void OpenFile_Bp(object sender, EventArgs e)
	{
		//var bpObject = new BlueprintObject(editor.Text);
		FileResult myFileResult = null;
		string fileData = "";
        Task.Run(async () =>
        {
            myFileResult = await FilePicker.PickAsync();
        }).Wait();

        if (myFileResult != null)
        {
            //DisplayAlert("File Picker Result", myFileResult.FullPath, "OK");
			CounterBtn.Text =  myFileResult.FullPath;
			using (StreamReader sr = File.OpenText(myFileResult.FullPath))
				{
					var s = (string)"";
					while ((s = sr.ReadLine()) != null)
					{
						Console.WriteLine(s);
						fileData += "\r"+s;
					}
				}
			editor.Text = fileData;
			var bpObject = new BlueprintObject(fileData);
        }
		//var bpFileData = BpFile.OpenBpFile();
		
		//CounterBtn.Text = bpFileData.Result[1];
		//var bpObject = new BlueprintObject(bpFileData.Result[0]);
	}
	void OnEditorTextChanged(object sender, TextChangedEventArgs e)
	{
		string oldText = e.OldTextValue;
		string newText = e.NewTextValue;
		string currentText = editor.Text;
		Console.WriteLine(currentText.Length);
	}
	
}

