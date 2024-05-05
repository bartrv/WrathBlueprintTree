using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Specialized;
namespace WrathBlueprintTree;

public partial class TreeBuilder
{
	IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
	//AbsoluteLayout targetParent;
    public static bool GenerateTreeFromFileData(Dictionary<string,dynamic> xferObject, AbsoluteLayout? targetParent = null)
    {
		Frame newGenPanel;
		int panelHeight;
		int panelWidth;
        Point dropPosition = new() { X = 5, Y = 5 };
		if (targetParent != null)
		{
			//targetParent = WrathBlueprintTree.TreePage.TreeDropContainer;
			foreach (var (branchName, branchStructure) in xferObject)
			{
				//(newGenPanel, panelHeight, panelWidth) = TreePage.GenerateNewBpTemplatePanelFromListModel(PanelBpUniqueName);
				(newGenPanel, panelHeight, panelWidth) = GenerateNewBpTemplatePanelFromListModel(branchName);

				AbsoluteLayout.SetLayoutBounds(newGenPanel, new Rect(dropPosition.X,dropPosition.Y,panelWidth+16,panelHeight)); //Assign relative location and size data to panel object
				targetParent.Add(newGenPanel); // Assign panel to Frame
				dropPosition.X = dropPosition.X+panelWidth+16+50;
			}
			return true;
		}
        return false;
    }

    public Frame GenerateFrameFromData(string bpModelKey, List<dynamic> PanelData)
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
	
	public static (Frame, int, int) GenerateNewBpTemplatePanelFromListModel(string PanelBpUniqueName)
	{
		IDataTransfer XferObject = DependencyService.Get<IDataTransfer>();
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
		Frame bpFrameVisualMain = new Frame{BackgroundColor = Color.FromRgba("#EEEEEEFF"), //Visual Frame container
										Margin = new Thickness(0,2),
										Padding = new Thickness(0,0),
										CornerRadius = 3
										};
	
		VerticalStackLayout bpFrameVisualMainVertLayout = [];
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
		//rowDef.Height = 12;
		
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
			List<dynamic> AnotherCopyIdx = new() {"0"};
			//List<dynamic> AnotherCopyIdxB;
			string EntryItemKey;
			if (XferObject.IsFile==true)
			{
				//var AnotherCopyIdxb = EntryListEnum.Value as List<dynamic>;
				AnotherCopyIdx = EntryListEnum.Value as List<dynamic>;
				//string Test0 = AnotherCopyIdxB[0];
				//string Test1 = AnotherCopyIdxB[1];
				EntryItemKey = EntryListEnum.Key as string;
			} else {
				AnotherCopyIdx = EntryListEnum.Value as List<dynamic>;
				EntryItemKey = EntryListEnum.Key as string;
			}
			
			//Console.WriteLine("AnotherCopyIdx[0]:"+AnotherCopyIdx[0]);
			Console.WriteLine("EntryItemKey:"+EntryItemKey);
			Console.WriteLine("AnotherCopyIdx:"+AnotherCopyIdx);
			BoxView bpNode = new BoxView{};
			var TempVMLabelForLoop = new Label{
											Text = EntryItemKey,
											//Text = AnotherCopyIdx.Last(),
											FontSize = 10, 
											Margin =  new Thickness(0,0), 
											HorizontalTextAlignment = TextAlignment.Start,
											VerticalTextAlignment = TextAlignment.Start
											};
			
			Grid.SetRow(TempVMLabelForLoop,PanelLayoutGridRow);
			Grid.SetColumn(TempVMLabelForLoop,0);
			panelLayoutGrid.Children.Add(TempVMLabelForLoop);

			if (XferObject.IsFile != true){
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

				AbsoluteLayout.SetLayoutBounds(bpNode, new Rect(panelWidth-18,panelHeight-17,16,16));
				 if (AnotherCopyIdx[2] == "node"){bpFrameContainerAbsLayout.Add(bpNode);}
			} else {
				//DoDo Add Node identifier to tree data for ingested files
			}
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

}