using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
//using ABI.System.Collections.Generic;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
//using Microsoft.UI.Xaml.Controls;
//using Windows.Devices.Input;
using Microsoft.Maui.Animations;
//using Windows.UI.Notifications;
namespace WrathBlueprintTree;

public partial class TreePage : ContentPage
{
	IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
	//Dictionary<string,string> ButtonList = [];
	Point? relativeToContainerPosition;
    //Dictionary<string,dynamic> TreeViewMVVM = [];
	public TreePage()
	{
		InitializeComponent();
        //Button buttonAA = new Button {Text = "Button text"};
        //int btnI = 0;
        int ButtonCount = BlueprintModels.ModelDictionary.Count;
		generateTreePageButtonList(); //Build the button list - Currently everything in the model(W.i.P), will need sub categories eventually
		XferObject.SidePanelContainer = FindByName("sbEditPanelVStack") as VerticalStackLayout;
		//VerticalStackLayout? SideBarPanelVertStack = FindByName("sbEditPanelVStack") as VerticalStackLayout;
		if (XferObject.IsFile == true) 
		{
			//if (!TreeBuilder.GenerateTreeFromFileData(XferObject.IngestedBpObjectFlat)) 
			//if (!TreeBuilder.GenerateTreeFromFileData(XferObject.FullBpTree.tree, this.TreeDropContainer, SideBarPanelVertStack)) 
			if (!TreeBuilder.GenerateTreeFromFileData(XferObject.FullBpTree, this.TreeDropContainer)) 
			{
				ThrowAlert();
			} else {

			}

		} else {
			XferObject.FullBpTree = new FullBpTreeCollection(null);
			XferObject.IsNew = true;
			XferObject.IsSaved = false;
		}
	}

	async public static void ThrowAlert()
	{
			bool x =  await Application.Current.MainPage.DisplayAlert("Title","Hello","OK","NotOK");
	}
	private void generateTreePageButtonList()
	{
		//Build the button list - Currently everything in the model(W.i.P), will need sub categories eventually
		foreach (KeyValuePair<string, dynamic> entry in BlueprintModels.ModelDictionary) 
		{
			//var newFrame = new Frame{BackgroundColor = Color.FromRgba("#AA9999FF"),
			var newFrame = new Border{BackgroundColor = Color.FromRgba("#AA9999FF"),
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										//CornerRadius = 3,  //Frame Control
										WidthRequest= 200,
										HeightRequest = 24,
										VerticalOptions = LayoutOptions.Center,
										HorizontalOptions = LayoutOptions.Center,
									};

            Label frameLabel;
			Type testingType = entry.Value["buttonName"].GetType();
			if (testingType == typeof(List<string>)){
				frameLabel = new Label{Text = entry.Value["buttonName"][0], 
											FontSize = 12, 
											Margin =  new Thickness(0,0), 
											HorizontalTextAlignment = TextAlignment.Center,
											VerticalTextAlignment = TextAlignment.Center
										};
			} else {
				frameLabel = new Label{Text = entry.Value["buttonName"], 
											FontSize = 12, 
											Margin =  new Thickness(0,0), 
											HorizontalTextAlignment = TextAlignment.Center,
											VerticalTextAlignment = TextAlignment.Center
										};
			}
			newFrame.Content = frameLabel;

			//Drag Gesture definition
			DragGestureRecognizer DragFrame = new()
            {
                CanDrag = true
				//AllowedOperations = DataPackageOperation.Copy -> suggested fromChat - seems to not exist
            };
			
			//DragFrame.DragStarting += OnDragStarting; - with a little explanation of the DragStarting notation from ChatGPT 
			DragFrame.DragStarting += (sender, e) => OnDragStarting(sender, e, entry.Key, "MainMenuListItem"); // final suggestion from Chat
			//Attach (Add) Drag Gesture definition and actions to newFrame
			newFrame.GestureRecognizers.Add(DragFrame);
			
            treeDragListA.Add(newFrame); //Append the just built button to the main option menu VerticalLayout object
        } 
	}

	public void OnPointerMovedInTreeView(object sender, PointerEventArgs e)
	{
		relativeToContainerPosition = e.GetPosition((View)sender);
		Console.WriteLine(relativeToContainerPosition);
	}
	//public static void OnDragStarting(object? sender, DragStartingEventArgs e, string bpModelKey)
	public static void OnDragStarting(object? sender, DragStartingEventArgs e, string bpModelKey, string dragItemContext)
	{
		//Border? senderParent = (GestureRecognizer)sender.Parent;
		Console.WriteLine($"Frame dragged! : {bpModelKey}");
		//Point? mousePanelOffset = new();

		Point? mousePanelOffset = e.GetPosition((sender as Element)?.Parent);

		//e.Data.Text = bpModelKey;
		e.Data.Text = bpModelKey+","+dragItemContext+","+mousePanelOffset.Value.X+","+mousePanelOffset.Value.Y;
	}

	public static void OnDragOver(object? sender, DragEventArgs e)
	{
		//e.AcceptedOperation = DataPackageOperation.None; 
		#if WINDOWS
			var dragUI = e.PlatformArgs.DragEventArgs.DragUIOverride;
			dragUI.IsCaptionVisible = false;
			dragUI.IsGlyphVisible = false;
		#endif
	}

	public static void OnMouseClick_TreeView(object sender, TappedEventArgs e)
	{
		IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
		//if (sender is ScrollView)
		if (sender is AbsoluteLayout)
		{
			ClearPanelSelections(XferObject, sender, "AbsoluteLayout");
		} 
		else if (sender is Border)
		{
			ClearPanelSelections(XferObject, sender, "Border");
			SetPanelSelections(XferObject, sender);
		}
		Console.WriteLine(sender.ToString());
		//(sender as Border).Stroke = Color.FromRgba("#990000FF");
		//(sender as Border).StrokeThickness = 3;
	}
	public static void ClearPanelSelections(IDataTransfer XferObject, object sender, string senderType)
	{
		//if (sender is AbsoluteLayout parentLayout && parentLayout.AutomationId == "TreeDropContainer")
		//{
		//if (senderType == "AbsoluteLayout")
		//{
			//TreePage treePageInstance = (sender as VisualElement)?.Parent.Parent.Parent as TreePage;
			//if (treePageInstance != null)
			//{
				//var panels =  XferObject.FullBpTree.BpTreeVM["Panels"] as Dictionary<string,dynamic>;
				foreach (KeyValuePair<string,dynamic> panelEntry in XferObject.FullBpTree.BpTreeVM["Panels"])
				//foreach (Dictionary<string,dynamic> panelEntry in panels)
				{
					Dictionary<string,dynamic> panelDict = panelEntry.Value as Dictionary<string,dynamic>;
					//Border panelID = treePageInstance.FindByName<Border>((string)uiPanel.Key);
					
					//Border panelObjectContainer = panelDict["GeneratedPanelObject"];
					//Border panelObjectVisual = panelObjectContainer.FindByName<Border>(panelDict["StyleId"]);
					Border panelObjectVisual = panelDict["GeneratedViewableObject"];
					//panelObjectContainer.StrokeThickness = 1;
					//panelObjectContainer.Stroke = Color.FromRgba("#3333FFFF");
					panelObjectVisual.StrokeThickness = 1;
					panelObjectVisual.Stroke = Color.FromRgba("#3333FFFF");

					panelDict["selectState"] = -1;
					panelDict["strokeThickness"] = 1;
					panelDict["stroke"] = Color.FromRgba("#3333FFFF");
				}
				XferObject.SidePanelContainer.Clear();
			//}
		//}
	}
	public static void SetPanelSelections(IDataTransfer XferObject, object sender)
	{
		var obj = sender as Border;
		string PanelBpUniqueName = obj.StyleId;
		
		XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["selectState"] = 0;
		//XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["borderColor"] = Color.FromRgba("#990000FF");
		//XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["borderWidth"] = (double)3;
		Console.WriteLine(PanelBpUniqueName.ToString());
		obj.Stroke = Color.FromRgba("#990000FF");
		obj.StrokeThickness = 1;

		// This is for Referance : TreeBuilder.cs -> GenerateTreeFromFileData () // XferObjectFBPTree.BpTreeVM["Panels"][PanelBpUniqueName].Add("SideBarObject",sideBarPanel); //trying assigning the Side Bar <Border> Object to it's associated "Panels" Entry
		XferObject.SidePanelContainer.Add(XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["SidePanelObject"]); // Append the generated side panel int he interface VM to the SidePanelContainer
	}
	public async void OnDropIntoTreeLayout(object sender, DropEventArgs e)
	{
		//string bpModelKey = await e.Data.GetTextAsync();
		string dropDataString = await e.Data.GetTextAsync();
		List<string> dragData = dropDataString.Split(',').ToList();
		string bpModelKeyOrUniqueName = dragData[0];
		string dragItemContext = dragData[1];

		Console.WriteLine(sender.ToString());
		Console.WriteLine(e.ToString());
		Console.WriteLine($"bpModelKey: {bpModelKeyOrUniqueName}");
		object bpModelReferanceObject;
		if (dragItemContext == "MainMenuListItem")
		{
			string bpModelKey = bpModelKeyOrUniqueName;
			if (BlueprintModels.GetBpModel(bpModelKey, out bpModelReferanceObject)) //verify model data exists in model library and extract lib referance before generating panel
			{ 
				AbsoluteLayout? senderParent = (sender as Element)?.Parent as AbsoluteLayout; // grab Frame layout object that received drop and re-assert the coordinate system
				Console.WriteLine(senderParent.ToString());
				//Console.WriteLine((sender);
				relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent); // grab the drop location from drop event args
				//if (relativeToContainerPosition!=null){
					//Console.WriteLine($"X = {relativeToContainerPosition.Value.X}");
					//Console.WriteLine($"Y = {relativeToContainerPosition.Value.Y}");
				//}
				PlaceNewBpTemplatePanel(senderParent, relativeToContainerPosition, bpModelKey); //call new panel generation method and attach it to the parent Frame at the Drop location
			} else {
				Console.WriteLine("Exception!! BlueprintModels Dictionaty key not found, this should be immpossible. TreePage.OnDropIntoTreeLayout");
			}
		} 
		else if (dragItemContext == "ExistingPanel")
		{
			string PanelBpUniqueName = bpModelKeyOrUniqueName;
			AbsoluteLayout? senderParent = (sender as Element)?.Parent as AbsoluteLayout; // grab Frame layout object that received drop and re-assert the coordinate system
			relativeToContainerPosition = e.GetPosition((sender as Element)?.Parent); // grab the drop location from drop event args
			RepositionExistingPanel(senderParent, relativeToContainerPosition, PanelBpUniqueName, dragData);
		}
		// Perform logic to take action based on retrieved value.
	}

	public void RepositionExistingPanel(AbsoluteLayout? senderParent, Point? relativeToContainerPosition, string PanelBpUniqueName, List<string> dragData)
	{
		SizeF? relativeOffsetVector = new SizeF(float.Parse(dragData[2]),float.Parse(dragData[3])); //using the relative cursor position from the moved sender to the mouse create an offset vector
		relativeToContainerPosition -= relativeOffsetVector; //subtract the offset vector from the drop location so the new panel location is observable as well as predictable.

		Console.WriteLine(PanelBpUniqueName+": Old Point =>"+XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["rect"].Location);
		XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["rect"].Location = relativeToContainerPosition ?? new Point(0,0);
		Console.WriteLine(PanelBpUniqueName+": New Point =>"+XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["rect"].Location);
		AbsoluteLayout.SetLayoutBounds((Border)XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["GeneratedPanelObject"],(Rect)XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName]["rect"]);
	}

	public void PlaceNewBpTemplatePanel(AbsoluteLayout targetParent, Point? dropPosition, string bpModelKey)
	{
		//Frame newGenPanel;
		string PanelBpUniqueName;
		int panelHeight;
		int panelWidth;
		Border newGenPanel;
		Border visualGenPanel;
		List<dynamic> sideBarItems;
		FullBpTreeCollection XferObjectFBPTree = XferObject.FullBpTree;

		if (bpModelKey == "BlueprintProgression") //Currently the only Blueprint in the ListModel that has all of the appropriate data fields to generate the panels
		{
			PanelBpUniqueName = BpTreeFlatFileAddTo_PreVM(bpModelKey);
			BpTreeVmAddTo(PanelBpUniqueName, dropPosition);
			//(PanelBpName, newGenPanel) = GenerateNewBpTemplatePanelFromListModel(bpModelKey); //Generate Panel Object, this is backwards... Should Add/populate FlatFileData 1st, then generate VM, THEN VMMV
			//(newGenPanel, panelHeight, panelWidth) = GenerateNewBpTemplatePanelFromListModel(PanelBpUniqueName);
			TreeBuilder.SeedTreePanelDictionary(XferObjectFBPTree, PanelBpUniqueName, "Seed");
			(newGenPanel, visualGenPanel, panelHeight, panelWidth, sideBarItems) = TreeBuilder.GenerateNewBpTemplatePanelFromListModel(PanelBpUniqueName, XferObjectFBPTree);
			
			XferObjectFBPTree.BpTreeVM["Panels"][PanelBpUniqueName].Add("rect", new Rect(dropPosition.Value.X,dropPosition.Value.Y,panelWidth+16,panelHeight));

			AbsoluteLayout.SetLayoutBounds(newGenPanel, new Rect(dropPosition.Value.X,dropPosition.Value.Y,panelWidth+16,panelHeight)); //Assign relative location and size data to panel object
			TreeBuilder.SeedTreePanelDictionary(XferObjectFBPTree, PanelBpUniqueName, "Panel", newGenPanel);
			TreeBuilder.SeedTreePanelDictionary(XferObjectFBPTree, PanelBpUniqueName, "Viewable", visualGenPanel);
			//targetParent.Add(newGenPanel); // Assign panel to ScrollFrame (Absolute Layout)
			targetParent.Add(XferObjectFBPTree.BpTreeVM["Panels"][PanelBpUniqueName]["GeneratedPanelObject"]);
			TreeBuilder.SeedTreePanelDictionary(XferObjectFBPTree, PanelBpUniqueName, "SideItems", sideBarItems);
			Border sideBarPanel = TreeBuilder.GenerateSideBarPanel(PanelBpUniqueName, XferObjectFBPTree.BpTreeVM["Panels"][PanelBpUniqueName]["SideBarItems"]);
			TreeBuilder.SeedTreePanelDictionary(XferObjectFBPTree, PanelBpUniqueName, "SidePanel", sideBarPanel);

		} else{
			(PanelBpUniqueName, newGenPanel) = GenerateNewBpTemplatePanel(bpModelKey); //Generate Panel Object
			AbsoluteLayout.SetLayoutBounds(newGenPanel, new Rect(dropPosition.Value.X,dropPosition.Value.Y,200,400)); //Assign relative location and size data to panel object
			targetParent.Add(newGenPanel); // Assign panel to Frame
			BpTreeVmAddTo(PanelBpUniqueName, dropPosition);
			BpTreeFlatFileAddTo(PanelBpUniqueName, bpModelKey);
		}

	}

	private void BpTreeVmAddTo(string PanelBpUniqueName, Point? dropPosition)
	{
		XferObject.FullBpTree.BpTreeVM["Panels"].Add(PanelBpUniqueName, new Dictionary<string,dynamic>{});
		XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName].Add("Location",dropPosition);
		XferObject.FullBpTree.BpTreeVM["Panels"][PanelBpUniqueName].Add("Wires",new Dictionary<string,string>{});
	}

	private string BpTreeFlatFileAddTo_PreVM(string bpModelKey)
	{
		object bpModelReferanceObject;
		string PanelBpNameBase = BlueprintModels.ModelDictionary[bpModelKey]["$type"][4].Split(',')[1];
		string PanelBpUniqueName = $"${PanelBpNameBase}${Guid.NewGuid()}";

		if (BlueprintModels.GetBpModel(bpModelKey, out bpModelReferanceObject)) //verify model data exists in model library and extract lib referance before generating panel
		{ 
			Type testingType = bpModelReferanceObject.GetType();
			if (testingType == typeof(Dictionary<string,dynamic>)){
				Dictionary<string,dynamic> bpModelDictionary = (Dictionary<string,dynamic>)bpModelReferanceObject;
				
				XferObject.FullBpTree.tree.Add(PanelBpUniqueName, new List<dynamic>{});
				foreach (string modelItemKey in bpModelDictionary.Keys)
				{
					Console.WriteLine(modelItemKey);
					Console.WriteLine(bpModelDictionary[modelItemKey]);
					XferObject.FullBpTree.tree[PanelBpUniqueName].Add(new List<dynamic>{modelItemKey,""});
				}
			} else {
				OrderedDictionary bpModelDictionary = (OrderedDictionary)bpModelReferanceObject;
				
				//XferObject.FullBpTree.tree.Add(PanelBpUniqueName, new List<string>{});
				XferObject.FullBpTree.tree.Add(PanelBpUniqueName, new OrderedDictionary{});
				List<string> tempList;
				foreach (string modelItemKey in bpModelDictionary.Keys)
				{
					Console.WriteLine(modelItemKey);
					Console.WriteLine(bpModelDictionary[modelItemKey]);
					//tempList = new List<string>{modelItemKey};
					//tempList.AddRange((List<string>)bpModelDictionary[modelItemKey]);
					//tempList = [modelItemKey, .. (List<string>)bpModelDictionary[modelItemKey]];
					XferObject.FullBpTree.tree[PanelBpUniqueName].Add(modelItemKey,bpModelDictionary[modelItemKey]);
				}
			}

		}
		return PanelBpUniqueName;
	}

	private void BpTreeFlatFileAddTo(string PanelBpName, string bpModelKey)
	{
		object bpModelReferanceObject;
		
		if (BlueprintModels.GetBpModel(bpModelKey, out bpModelReferanceObject)) //verify model data exists in model library and extract lib referance before generating panel
		{ 
			Type testingType = bpModelReferanceObject.GetType();
			if (testingType == typeof(Dictionary<string,dynamic>)){
				Dictionary<string,dynamic> bpModelDictionary = (Dictionary<string,dynamic>)bpModelReferanceObject;
				
				XferObject.FullBpTree.tree.Add(PanelBpName, new List<dynamic>{});
				foreach (string modelItemKey in bpModelDictionary.Keys)
				{
					Console.WriteLine(modelItemKey);
					Console.WriteLine(bpModelDictionary[modelItemKey]);
					XferObject.FullBpTree.tree[PanelBpName].Add(new List<dynamic>{modelItemKey,""});
				}
			} else {
				Dictionary<string,List<string>> bpModelDictionary = (Dictionary<string,List<string>>)bpModelReferanceObject;
				
				XferObject.FullBpTree.tree.Add(PanelBpName, new List<string>{});
				List<string> tempList;
				foreach (string modelItemKey in bpModelDictionary.Keys)
				{
					Console.WriteLine(modelItemKey);
					Console.WriteLine(bpModelDictionary[modelItemKey]);
					//tempList = new List<string>{modelItemKey};
					//tempList.AddRange(bpModelDictionary[modelItemKey]);
					tempList = [modelItemKey, .. bpModelDictionary[modelItemKey]];

					XferObject.FullBpTree.tree[PanelBpName].Add(tempList);
				}
			}

		}
	}

	public (Frame, int, int) GenerateNewBpTemplatePanelFromListModel(string PanelBpUniqueName)
	{
		int panelWidth = 300;
		int panelHeight = 36;
		//Entry TempVMEntryForLoop;

		Frame bpFrameContainer = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Overall Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 0
										};
		AbsoluteLayout bpFrameContainerAbsLayout = [];
		
		
		/*BoxView bpNode = new BoxView{BackgroundColor = Color.FromRgba("#BBBBBBFF"),
						Margin = new Thickness (0,0),
						CornerRadius = 8,
						};
		AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(panelWidth-20,28,16,16));
		*/
		//Frame bpFrameVisualMain = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
		Border bpFrameVisualMain = new Border{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										//CornerRadius = 3
										Stroke = Color.FromRgba("#333333FF"),
										StrokeThickness = 2,
										StrokeShape = new RoundRectangle
											{
												CornerRadius = new CornerRadius(3, 3, 3, 3)
											}
										};
	
		VerticalStackLayout bpFrameVisualMainVertLayout = [];

		TapGestureRecognizer TapClickEvent = new TapGestureRecognizer();
		TapClickEvent.Tapped += (s,e) => {TreePage.OnMouseClick_TreeView(s, e);};
		bpFrameVisualMain.GestureRecognizers.Add(TapClickEvent);

		// XferObject.FullBpTree.tree[PanelBpUniqueName].Add(tempList);
		//[0] = ["Categories", ..], [1] = ["buttonName", .. ], [2] = ["title", .. ]
		Console.WriteLine("buttonName"+XferObject.FullBpTree.tree[PanelBpUniqueName]["buttonName"][0]); //Static issues, works when parent method is not static

		/*
		Label bpframeBtnName = new Label{Text = $": {XferObject.FullBpTree.tree[PanelBpUniqueName]["buttonName"][0]} :", 
										FontSize = 12, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Center,
										VerticalTextAlignment = TextAlignment.Start
									};
		Label bpframeTitle = new Label{Text = $"{XferObject.FullBpTree.tree[PanelBpUniqueName]["title"][4]}", 
										FontSize = 10, 
										Margin =  new Thickness(0,0), 
										HorizontalTextAlignment = TextAlignment.Start,
										VerticalTextAlignment = TextAlignment.Start
									};
		
		bpFrameVisualMainVertLayout.Add(bpframeBtnName);
		bpFrameVisualMainVertLayout.Add(bpframeTitle);	
		*/
		int ItemCount = XferObject.FullBpTree.tree[PanelBpUniqueName].Count;
		Grid panelLayoutGrid = new Grid{
			 RowDefinitions = new RowDefinitionCollection()
		};
		RowDefinition rowDef = new RowDefinition{ Height = new GridLength(18, GridUnitType.Absolute) }; //18 = Node rect height + 2

		
		ColumnDefinition ColOne = new ColumnDefinition();
		ColumnDefinition ColTwo = new ColumnDefinition();
		ColumnDefinition ColThree = new ColumnDefinition();
		ColOne.Width = 180;
		ColTwo.Width = 100;
		ColThree.Width = 20;
		
		for (int i = 2; i < ItemCount; i++)
		{
			panelLayoutGrid.RowDefinitions.Add(rowDef);
		}
		panelLayoutGrid.ColumnDefinitions.Add(ColOne);
		panelLayoutGrid.ColumnDefinitions.Add(ColTwo);
		panelLayoutGrid.ColumnDefinitions.Add(ColThree);

		int PanelLayoutGridRow = 0;
		OrderedDictionary EntryListItem = XferObject.FullBpTree.tree[PanelBpUniqueName];
		IDictionaryEnumerator EntryListEnum = EntryListItem.GetEnumerator();
		EntryListEnum.MoveNext();
		EntryListEnum.MoveNext();
		EntryListEnum.MoveNext();
		//for (int i = 3; i < ItemCount; i++)
		while (EntryListEnum.MoveNext())
		{
			//List<string> AnotherCopyIdx = EntryListItem[i] as List<string>;
			//string EntryItemKey = EntryListItem[i].Last();
			List<string> AnotherCopyIdx = EntryListEnum.Value as List<string>;
			string EntryItemKey = EntryListEnum.Key as string;
			//Console.WriteLine("AnotherCopyIdx[0]:"+AnotherCopyIdx[0]);
			Console.WriteLine("AnotherCopyIdx[0]:"+EntryItemKey);
			BoxView bpNode = new BoxView{};
			var TempVMLabelForLoop = new Label{
											Text = EntryItemKey,
											//Text = AnotherCopyIdx.Last(),
											FontSize = 10, 
											Margin =  new Thickness(0,0), 
											HorizontalTextAlignment = TextAlignment.Start,
											VerticalTextAlignment = TextAlignment.Start
											};
			if (AnotherCopyIdx[2] == "node"){
				//BoxView bpNode = new BoxView{
						bpNode.BackgroundColor = Color.FromRgba("#CCBBBBFF");
						bpNode.Margin = new Thickness (0,0);
						bpNode.WidthRequest = 16;
						bpNode.HeightRequest = 16;
						bpNode.CornerRadius = 8;
						bpNode.ZIndex = 100;
						//bpNode.VerticalOptions = Rect;
						//};
				//AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(panelWidth-20,panelHeight-17,16,16));
				//Grid.SetRow(bpNode,PanelLayoutGridRow);
				//Grid.SetColumn(bpNode,2);
				//panelLayoutGrid.Children.Add(bpNode);
			}

			Grid.SetRow(TempVMLabelForLoop,PanelLayoutGridRow);
			Grid.SetColumn(TempVMLabelForLoop,0);
			panelLayoutGrid.Children.Add(TempVMLabelForLoop);
			AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(panelWidth-18,panelHeight-17,16,16));
			if (AnotherCopyIdx[2] == "node"){bpFrameContainerAbsLayout.Add(bpNode);}
			PanelLayoutGridRow++;
			panelHeight = panelHeight+18;
			
		}
		AbsoluteLayout.SetLayoutBounds(bpFrameVisualMain, new Rect(10,0,panelWidth-20, panelHeight-4)); //Set width, height of Visual Container based on Generated Content

		bpFrameVisualMainVertLayout.Add(panelLayoutGrid);
		bpFrameVisualMain.Content = bpFrameVisualMainVertLayout;

		bpFrameContainerAbsLayout.Add(bpFrameVisualMain);
		//bpFrameContainerAbsLayout.Add(bpNode);
		bpFrameContainer.Content = bpFrameContainerAbsLayout;

		return (bpFrameContainer, panelHeight, panelWidth);
	}
	public (string, Border) GenerateNewBpTemplatePanel(string bpModelKey) //New empty panel generator - pulls data fields from BlueprintModels 
	{
		//var dropData = "testData";
		int testWidth = 200;
		int testHeight = 400;
		//string PanelBpName = $"${BlueprintModels.ModelDictionary[bpModelKey]["$type"]}${Guid.NewGuid()}";
		string PanelBpNameBase = BlueprintModels.ModelDictionary[bpModelKey]["$type"];
		string PanelBpName = $"${PanelBpNameBase}${Guid.NewGuid()}";


		Border bpFrameContainer = new Border{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Overall Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										//CornerRadius = 0
										};
		AbsoluteLayout bpFrameContainerAbsLayout = [];
		
		//Node Box/circle/Disc
		BoxView bpNode = new BoxView{BackgroundColor = Color.FromRgba("#BBBBBBFF"),
						Margin = new Thickness (0,0),
						//WidthRequest = 16,
						//HeightRequest = 16,
						CornerRadius = 8
						};
		AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(testWidth-20,28,16,16));
		/*
		Frame bpFrameVisualMain = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3
										};*/
		Border bpFrameVisualMain = new Border{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										//CornerRadius = 3
										Stroke = Color.FromRgba("#333333FF"),
										StrokeThickness = 2,
										StrokeShape = new RoundRectangle
											{
												CornerRadius = new CornerRadius(3, 3, 3, 3)
											}
										};
		AbsoluteLayout.SetLayoutBounds(bpFrameVisualMain, new Rect(10,0,testWidth-20,testHeight-4));

		//Add Tap Gesture Recognizer (mouse Click) to each panel for panel selection code
		TapGestureRecognizer TapClickEvent = new TapGestureRecognizer();
		TapClickEvent.Tapped += (s,e) => {TreePage.OnMouseClick_TreeView(s, e);};
		bpFrameVisualMain.GestureRecognizers.Add(TapClickEvent);

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
		return (PanelBpName, bpFrameContainer);
		
	}
}

