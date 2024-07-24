using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

//Aliased using references
using MsUiXaml = Microsoft.UI.Xaml;

namespace WrathBlueprintTree;

public partial class App : Application
{
	private GlobalKeyboardListener? _keyboardListener; //Chat Kbd Low Level hook
	public App()
	//public App(MainPage page)
	{
		InitializeComponent();

		// Register dependencies
		DependencyService.Register<IDataTransfer, DataTransfer>();

		MainPage = new AppShell();
		//MainPage = page; // Legacy original code for reference only
		InitKeyboardListener(); // Initialize after MainPage
	}
	private void InitKeyboardListener()
	{
		 if (_keyboardListener == null)
            {
                _keyboardListener = new GlobalKeyboardListener();
                _keyboardListener.KeyboardKeyPressed += OnKeyboardKeyPressed;
                _keyboardListener.KeyboardKeyReleased += OnKeyboardKeyReleased;
            }
	}
	// Chat Kbd Low Level Hook start
	private void OnKeyboardKeyPressed(object? sender, KeyEventArgs e)
	{
		// Handle Control key pressed globally
		Console.WriteLine("{0} key pressed", e.KeyCode);
		TreePage.OnKeyboardKeyPressed(sender, e); 
	}

	private void OnKeyboardKeyReleased(object? sender, KeyEventArgs e)
	{
		// Handle Control key released globally
		Console.WriteLine("{0} key released", e.KeyCode);
		TreePage.OnKeyboardKeyReleased(sender, e);
	}

	protected override void OnStart()
	{
		base.OnStart();
		InitKeyboardListener();
	}

	protected override void OnSleep()
	{
		base.OnSleep();
		// Dispose the keyboard listener when the app sleeps
		_keyboardListener?.Dispose();
		_keyboardListener = null;
	}

	protected override void OnResume()
	{
		base.OnResume();
		InitKeyboardListener();
	}
	//Chat Kbd Low Level Hook end

	//Chat Windows Specific Focus issue workaround
	// Windows-specific implementation
    public void OnWindowActivated(object sender, MsUiXaml.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == MsUiXaml.WindowActivationState.CodeActivated ||
                e.WindowActivationState == MsUiXaml.WindowActivationState.PointerActivated)
            {
                InitKeyboardListener();
            }
            else if (e.WindowActivationState == MsUiXaml.WindowActivationState.Deactivated)
            {
                DisposeKeyboardListener();
            }
        }

        public void OnWindowLostFocus(object sender, MsUiXaml.RoutedEventArgs e)
        {
            DisposeKeyboardListener();
        }

        public void OnWindowGotFocus(object sender, MsUiXaml.RoutedEventArgs e)
        {
            InitKeyboardListener();
        }

	private void DisposeKeyboardListener()
    {
        if (_keyboardListener != null)
        {
            _keyboardListener.Dispose();
            _keyboardListener = null;
        }
    }
}