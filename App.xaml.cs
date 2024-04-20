using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

//using AuthenticationServices;
//using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
namespace WrathBlueprintTree;

public partial class App : Application
{
	public App()
	//public App(MainPage page)
	{
		InitializeComponent();
		DependencyService.Register<IDataTransfer, DataTransfer>();

		MainPage = new AppShell();
		//MainPage = page;
	}
}
