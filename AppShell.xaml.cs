namespace WrathBlueprintTree;

public partial class AppShell : Shell
{
	//BlueprintModels bpModels;
	public AppShell()
	{
		InitializeComponent();
		BlueprintModels.GenerateBpDictionary(); //Build blueprint model data
	}
}
