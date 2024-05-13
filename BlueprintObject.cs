//using CoreImage;

using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Specialized;
//using AuthenticationServices;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Layouts;
using System.ComponentModel;
//using UIKit;

namespace WrathBlueprintTree;
public class BlueprintObject
{
	string rawText;
	int rawLength;
	int charIndex;
	List<string> nestDepth;
	List<string> treeBranch;
	int isQuote;
	string entryType;
	Dictionary<string, dynamic> bpData;

	public BlueprintObject(string sourceTextElement)
	{
		this.rawText = sourceTextElement;
		this.bpData = new Dictionary<string, dynamic>();
		this.rawLength = this.rawText.Length;
		this.charIndex = -1;
		this.nestDepth = [];  // ["{", "{", "[", "{"]
		this.treeBranch = []; //["brancha","branchb",2,"branchc"]
		this.isQuote = 0;
		this.entryType = "Key";

		this.MainLoop();

	}
	
	private string BuildString()
	{
		string currentString = "";
		while ((isQuote != 0) && this.charIndex < rawLength)
		{
			this.charIndex++;
			if (this.rawText[this.charIndex] != '"')
			{
				currentString += this.rawText[this.charIndex];
			}
			else
			{
				isQuote = Math.Abs(isQuote - 1);
				//console.log([currentString, this.#charIndex]);
				if (rawText[this.charIndex + 1] == ':')
				{
					entryType = "Key";
				}
				else
				{
					entryType = "Value";
				}
				charIndex++;
				return currentString; //[this.#currentString, this.#charIndex]
			}
		}
		return currentString;
	}

	private string BuildNumber()
	{
		int numBuildCursor = this.charIndex;
		string currentNumber = this.rawText[numBuildCursor].ToString();
		int endOfNumber = 0;
		while (endOfNumber == 0 && numBuildCursor < this.rawLength)
		{
			numBuildCursor++;
			//if (this.rawText[this.#charIndex] =="," || this.rawText[this.#charIndex] == "\n"){

			if (!int.TryParse(this.rawText[numBuildCursor].ToString(), out _) && this.rawText[numBuildCursor] != '.' || this.rawText[numBuildCursor] == ',' || this.rawText[numBuildCursor] == '\n')
			{
				numBuildCursor++;
				this.charIndex = numBuildCursor;
				return currentNumber;
			}
			currentNumber += this.rawText[numBuildCursor];
		}
		return currentNumber;
	}

	private string SetBPValue(object valueToInsert, List<string> currentBranchStruct)
	{
		//ToDo : modify this module to set non-text BP values for null, true, false, missing append strings to List<T>
		string branchString = "";
		foreach (string twigItem in currentBranchStruct)
		{
			branchString += (!int.TryParse(twigItem.ToString(), out _)) ? "." + twigItem : $"[{twigItem}]";
		}

		string mValueFuncText = "";

		if (!int.TryParse(valueToInsert.ToString(), out _) || valueToInsert == null || valueToInsert.ToString() == "")
		{
			mValueFuncText = "bpData" + branchString + " = \"" + valueToInsert + "\";"; //ToDo: variations for t/f/null
		}
		else
		{
			mValueFuncText = "bpData" + branchString + " = " + valueToInsert + ";";
		}
		Console.WriteLine(mValueFuncText);
		if (currentBranchStruct.Count == 1)
		{
			this.bpData[currentBranchStruct[0]] = valueToInsert ?? "";
		}
		else
		{
			//Dictionary<string,dynamic> tempBpDict = this.bpData;
			dynamic tempBpDict = this.bpData;
			int k = 0;
			int tempListIndex = -1;
			for (; k < currentBranchStruct.Count - 1; k++)
			{
				//tempBpDict = (Dictionary<string,dynamic>)tempBpDict[currentBranchStruct[k]];

				if (int.TryParse(currentBranchStruct[k], out _))
				{ //if false, sets target variable to 0, regardless
					tempListIndex = int.Parse(currentBranchStruct[k]);
					tempBpDict = tempBpDict[tempListIndex];
				}
				else
				{
					tempBpDict = tempBpDict[currentBranchStruct[k]];
					tempListIndex = -1;
				}

				//tempBpDict = tempBpDict[currentBranchStruct[k]];
			}

			switch (valueToInsert)
			{
				case "True":
					valueToInsert = (bool)true;
					break;
				case "False":
					valueToInsert = (bool)false;
					break;
			}
			if (!int.TryParse(currentBranchStruct[k], out _))
			{
				tempBpDict[currentBranchStruct[k]] = valueToInsert ?? "";
			}
			else if (int.TryParse(currentBranchStruct.Last(), out _))
			{
				tempBpDict.Add(valueToInsert);
			}
			else
			{
				tempBpDict[tempListIndex] = valueToInsert ?? "";
			}

		}
		return branchString;
	}

	private string InitBPKey(List<string> currentBranchStruct)
	{
		string branchString = "";
		foreach (string twigItem in currentBranchStruct)
		{
			branchString += (!int.TryParse(twigItem.ToString(), out _)) ? $"." + twigItem : $"[{twigItem}]";
		}
		branchString = branchString[1..];
		//string mValueFuncText = "";
		int initCursor = this.charIndex;

		dynamic tempBpDict = this.bpData;
		int k = 0;
		string twig = currentBranchStruct[0];
		//Type NestType = tempBpDict[currentBranchStruct[0]].GetType();
		foreach (string twigItem in currentBranchStruct)
		{
			k++;
			//if (twigItem != currentBranchStruct.Last()){  //if keys of same name as last item, could skip a loop
			if (k < currentBranchStruct.Count())
			{
				int tempInt = 0;
				tempBpDict = (int.TryParse(twigItem, out tempInt)) ? tempBpDict[tempInt] : tempBpDict[twigItem]; //if a number is returned, its a List - use [int.number]
				//twig = twigItem;
			}
			twig = twigItem;
		}
		while (initCursor < this.rawText.Length - 1)
		{
			initCursor++;
			if (this.rawText[initCursor] == '{')
			{
				branchString = "bpData." + branchString + " = {};";
				tempBpDict.Add($"{twig}", new Dictionary<string, dynamic> { });
				break;
			}
			else if (this.rawText[initCursor] == '[')
			{
				branchString = "bpData." + branchString + " = [];";
				tempBpDict.Add($"{twig}", new List<dynamic> { });
				break;
			}
			else if (this.rawText[initCursor] == '"' || this.rawText[initCursor] == ','
						|| (int.TryParse(this.rawText[initCursor].ToString(), out _)) && this.rawText[initCursor] != ' ' && this.rawText[initCursor] != '\n' && this.rawText[initCursor] != '\r')
			{
				branchString = "bpData." + branchString + " = \"\";";
				tempBpDict.Add($"{twig}", "");
				break;
			}
		}
		//Console.WriteLine(branchString);
		this.entryType = "Value";
		return branchString;
	}

	private string SeedBPArray(string seedType, List<string> currentBranchStruct)
	{ // ToDo ALL instantiation methods are different needs a complete rewrite, Can I even build a string as a lambda?? - recursion loop?
	  //string branchString = "";
		string seedFuncString = "bpData";
		dynamic tempBpDict = this.bpData;
		string twig = currentBranchStruct[0];
		//Type NestType = tempBpDict[currentBranchStruct[0]].GetType();
		int branchIndexCount = 0;
		foreach (string twigItem in currentBranchStruct)
		{
			branchIndexCount++;
			//if (twigItem != currentBranchStruct.Last()){
			if (branchIndexCount < currentBranchStruct.Count())
			{
				int tempInt = 0;
				seedFuncString += (int.TryParse(twigItem, out tempInt)) ? $"[{tempInt}]" : $".{twigItem}"; //build representitive parse string for debugging purposes
				tempBpDict = (int.TryParse(twigItem, out tempInt)) ? tempBpDict[tempInt] : tempBpDict[twigItem]; //if a number is returned, its a List - use [int.number]
			}
			twig = twigItem;
		}
		switch (seedType)
		{
			case "{":
				tempBpDict.Add(new Dictionary<string, dynamic> { });
				break;
			case "[]":
				tempBpDict.Add(new List<dynamic> { });
				break;
			case "$":
				tempBpDict.Add("");
				break;
		}
		return seedFuncString;
	}

	private void MainLoop()
	{
		while (this.charIndex < this.rawLength - 1)
		{
			this.charIndex++;
			if (this.charIndex == this.rawLength - 3)
			{
				/*Pause here to check final bpData object*/
			}
			char thisChar = this.rawText[this.charIndex];
			if (thisChar == '"') { this.isQuote = Math.Abs(this.isQuote - 1); } //toggle in/out of quotes
			if (this.isQuote == 1)
			{
				string currentString = "";
				currentString = this.BuildString();
				if (this.entryType == "Value")
				{
					if (this.nestDepth.Last() == "{")
					{
						//Console.WriteLine(String.Join(".",this.treeBranch) + ": " + currentString);
						this.SetBPValue(currentString, this.treeBranch);
					}
					else if (this.nestDepth.Last() == "-1")
					{
						this.nestDepth[this.nestDepth.Count() - 1] = "0";
						if (this.treeBranch.Count() < this.nestDepth.Count())
						{
							this.treeBranch.Add(this.nestDepth.Last());
						}
						else if (this.treeBranch.Count() == this.nestDepth.Count())
						{
							this.treeBranch[this.treeBranch.Count() - 1] = (this.nestDepth.Last());
						}
						//Console.WriteLine(String.Join(".",this.treeBranch) + "Append-> " + currentString); //This may be an unnecessaty check
						this.SetBPValue(currentString, this.treeBranch);
					}
					else if (int.TryParse(this.nestDepth.Last(), out _))
					{
						this.nestDepth[this.nestDepth.Count() - 1] = $"{int.Parse(this.nestDepth.Last()) + 1}";
						if (this.treeBranch.Count() == this.nestDepth.Count())
						{
							this.treeBranch[this.treeBranch.Count() - 1] = (this.nestDepth.Last());
						}
						//Console.WriteLine(String.Join(".",this.treeBranch) + "Append-> " + currentString); //This may be an unnecessaty check
						this.SetBPValue(currentString, this.treeBranch);
					}
				}
				else if (this.entryType == "Key")
				{
					if (this.nestDepth.Count == 1)
					{
						this.treeBranch.Clear();
						this.treeBranch.Add(currentString);

					}
					else if (this.treeBranch.Count >= this.nestDepth.Count - 1)
					{
						this.treeBranch = this.treeBranch.GetRange(0, this.nestDepth.Count - 1);
						this.treeBranch.Add(currentString);
					}
					this.InitBPKey(this.treeBranch);
					//Console.WriteLine("Branch: "+String.Join(".",this.treeBranch));
					this.entryType = "Value";
				}
			}
			else if (thisChar != ' ' && thisChar != '\n' && thisChar != ',' && thisChar != '\r' && thisChar != '\t')
			{
				switch (thisChar)
				{
					case '{':
						if (this.nestDepth.Count > 1)
						{
							if (int.TryParse(this.nestDepth.Last(), out _) && this.entryType == "Value")
							{
								if (int.Parse(this.nestDepth.Last()) >= 0
									&& this.treeBranch.Last() == this.nestDepth.Last())
								{
									this.nestDepth[this.nestDepth.Count - 1] = (int.Parse(this.nestDepth.Last()) + 1).ToString();
									this.treeBranch[this.treeBranch.Count - 1] = (int.Parse(this.treeBranch.Last()) + 1).ToString();
								}
								else if (int.Parse(this.nestDepth.Last()) == -1)
								{
									this.nestDepth[this.nestDepth.Count - 1] = (int.Parse(this.nestDepth.Last()) + 1).ToString();
									this.treeBranch.Add(this.nestDepth.Last());
								}
								this.SeedBPArray("{", this.treeBranch);
							}
						}
						this.nestDepth.Add("{");
						this.entryType = "Key";
						break;
					case '}':
						if (this.nestDepth.Count > 0)
						{
							this.nestDepth.RemoveAt(this.nestDepth.Count - 1);
							//this.treeBranch.RemoveAt(this.treeBranch.Count - 1);
							if (this.treeBranch.Count>0){
								if (!int.TryParse(this.treeBranch.Last(), out _))
								{
									this.treeBranch.RemoveAt(this.treeBranch.Count - 1);
								} 
								else if (int.Parse(this.treeBranch.Last()) == -1)
								{
									this.treeBranch.RemoveAt(this.treeBranch.Count - 1);
								}
							} 
						}
						if (this.nestDepth.Count > 0)
						{
							this.entryType = (this.nestDepth.Last() == "{") ? "Key" : "Value";
						}
						break;
					case '[':
						this.nestDepth.Add("-1");
						this.entryType = "Value";
						break;
					case ']':
						this.nestDepth.RemoveAt(this.nestDepth.Count - 1);
						this.treeBranch.RemoveAt(this.treeBranch.Count - 1);
						if (this.nestDepth.Count > 0)
						{
							this.entryType = (this.nestDepth.Last() == "{") ? "Key" : "Value";
						}
						break;
				}
				if (this.entryType == "Value")
				{
					string currentString = "";
					if (thisChar == 'n' && this.rawText[this.charIndex + 1] == 'u')
					{
						currentString = "null";
						this.SetBPValue(currentString, this.treeBranch);
						Console.WriteLine("Alt Value: null");
					}
					else if (thisChar == 'f' && this.rawText[this.charIndex + 1] == 'a')
					{
						currentString = "false";
						this.SetBPValue(currentString, this.treeBranch);
						Console.WriteLine("Alt Value: false");
					}
					else if (thisChar == 't' && this.rawText[this.charIndex + 1] == 'r')
					{
						currentString = "true";
						this.SetBPValue(currentString, this.treeBranch);
						Console.WriteLine("Alt Value: true");
					}
					else if (int.TryParse(thisChar.ToString(), out _) || int.TryParse(thisChar.ToString() + this.rawText[this.charIndex + 1], out _))
					{
						currentString = this.BuildNumber();
						if (!currentString.Contains("."))
						{
							//int CurrentStringAsInt = int.Parse(currentString);
							long CurrentStringAsInt = long.Parse(currentString);
							Console.WriteLine("Num Value:" + CurrentStringAsInt.ToString());
							this.SetBPValue(CurrentStringAsInt, this.treeBranch);
						}
						else
						{
							float currentStringAsFloat = float.Parse(currentString);
							Console.WriteLine("Num Value:" + currentStringAsFloat.ToString());
							this.SetBPValue(currentStringAsFloat, this.treeBranch);
						}

					}
					else if (thisChar == '{')
					{
						int tempTryParse = 0;
						if (int.TryParse(this.nestDepth[this.nestDepth.Count - 2].ToString(), out tempTryParse))
						{ //if there is a number, it is a [] index
							this.nestDepth[this.nestDepth.Count - 2] = $"{tempTryParse++}";
							this.treeBranch[this.nestDepth.Count - 2] = this.nestDepth[this.nestDepth.Count - 2];
							Console.WriteLine(String.Join(".", this.treeBranch) + "Add: [{}" + this.nestDepth[this.nestDepth.Count - 2] + "]");
						}
						else
						{
							Console.WriteLine(String.Join(".", this.treeBranch) + "Assign: {}");

						}
					}
					else if (thisChar == '}' || thisChar == ']')
					{
						if (this.nestDepth.Count <= this.treeBranch.Count){this.treeBranch = this.treeBranch.GetRange(0, this.nestDepth.Count);}

					}
					else if (thisChar == '[')
					{
						int tempTryParse = 0;
						if (int.TryParse(this.nestDepth[this.nestDepth.Count - 2].ToString(), out tempTryParse))
						{ //if there is a number, it is a [] index
							this.nestDepth[this.nestDepth.Count - 2] = $"{tempTryParse++}";
							this.treeBranch[this.nestDepth.Count - 2] = this.nestDepth[this.nestDepth.Count - 2];
							Console.WriteLine(String.Join(".", this.treeBranch) + "Add: [[]" + this.nestDepth[this.nestDepth.Count - 2] + "]");
						}
						else
						{
							//*This section moved to Switch Case "{"
							//
							//tempTryParse = int.Parse(this.nestDepth[this.nestDepth.Count-1]);
							//tempTryParse++;
							//this.nestDepth[this.nestDepth.Count-1] = tempTryParse.ToString();
							//if (this.treeBranch.Count >= this.nestDepth.Count){
							//	this.treeBranch[this.nestDepth.Count-1] = this.nestDepth[this.nestDepth.Count-1];
							//} else {
							//	this.treeBranch.Add(this.nestDepth[this.nestDepth.Count-1]);
							//}
							//Console.WriteLine(String.Join(".",this.treeBranch)+"Assign: []");
							//this.SeedBPArray("[", this.treeBranch);
						}
					}
				}
			}
		}
	}


	public static dynamic? Fetch(string dotTarget){
		dynamic? bpFetchResult = null;


		return bpFetchResult;
	}

	public Dictionary<string, dynamic> GenerateDataTables()
    {
        Dictionary<string, dynamic> bpFlatArrayBuildDict = new();
		//string bpFlatArrayBuildDict = "Test";
		//Dictionary<string, dynamic> recursiveBpData = this.bpData as Dictionary<string, dynamic>;

        CollapseBpObject(this.bpData as Dictionary<string, dynamic>);
		void CollapseBpObject(Dictionary<string, dynamic> dict, string path = "dbObj")
		{
			foreach (var kvp in dict)
			{
				string currentPath = $"{path}.{kvp.Key}";

				if (kvp.Value is Dictionary<string, dynamic>)
				{
					CollapseBpObject(kvp.Value, currentPath);
				}
				else if (kvp.Value is List<dynamic>)
				{
					for (int i = 0; i < kvp.Value.Count; i++)
					{
						PrintListValue(kvp.Value[i], $"{currentPath}.{i}");
					}
				}
				else
				{
					Console.WriteLine($"{currentPath} = {GetValueAsString(kvp.Value)}");
					bpFlatArrayBuildDict.Add(currentPath,kvp.Value);
				}
			}
		}

		void PrintListValue(dynamic value, string path)
		{
			if (value is Dictionary<string, dynamic> dict)
			{
				CollapseBpObject(dict, path);
			}
			else
			{
				Console.WriteLine($"{path} = {GetValueAsString(value)}");
			}
		}

		static string GetValueAsString(dynamic value)
		{
			if (value == null)
			{
				return "null";
			}
			else if (value is string)
			{
				return $"\"{value}\"";
			}
			else
			{
				return value.ToString();
			}
		}
        return bpFlatArrayBuildDict;
    }
}
	//For managing data, I have 2 broad thoughts:
    //First is access the bData object directly with get/set or Assign/Fetch methods.
	//This requires a loop to traverse every level of the object every time something is updates and on every value that will need to be generated.
	//since the objects can get fairly deep, this seems like it will slow down. 

	//the other is to mimic a nSQL data structure, using the desired key traversal as a single key string so db.data.override[1]=x becomes db.Fetch("data.override.1")
	//since the objects themselves are tiny, this seems the better approach.  
	// *Under that assumption, remaining notes will follow the paradigm of a Flat nSQL Dictionary where the Value fields are Object references to the actual bpData Object

	//For splitting the data out to individual forms and re-conecting everything:
	//		1. EVERY "$type": referance tree becomes its own form 
	//			a. as a result, it also becomes its own OBJECT
	//		2. "$type" Objects are to be directly referenced in the data structure as an Object so the memory referencing stays intact
	//		3. The UI form should be built from the _modelDictionary found in BlueprintModels
	//		4. The fields in the UI form should contain OPTIONS as defined in BlueprintModels, where appropriate
	//		5. UI field selections or input are to be remitted to the Flat file
	//		6. Question: in its current form the bpData object Dictionary seems unnecessary when paired withe the flattened data frame, 
	//				once the tree/frame is generated should BlueprintObjects.bpData be left as a historical referance to the imported object?
public class FullBpTreeCollection
{
//tree:
//	{id, Dict} id = AssetId, guid, or name(unique) - depends on what the bp object provides, root bp = AssetId, everything else should have a name, guid as a fallback
//	Dict:
//		{data, Dict} flat bp referance table/dictionary; values = object referance to BlueprintObject.bpData
//		{panel, Dict} {id=id,rect=rect(x,y,Xw,Yh),links=Dict{nodeid=[targetId,linkname/id]}}
//		{form, Dict} key = xaml form item x:Name, Value = List<?> {UI Designation, object referance, IList if applicable, null-object Initial Value}
//					UI Designation<string>: when applicable use MAUI interface name/method (Picker, Check, etc.), else use "LinkConnect"
//	{AllLinks,Dict<string,List<string>>}  This serves as a rollover referance and a reposition/drag referance {<linkidstring>, [node0,nodeLinked,Point0,PointLinked,QSplineObj?]}
//					eg. {[0]"13f7ab9d...",
//							{"data",{"Blueprint.bpData.Data.Components[0]",<object>"a1234b..."}},
//							{"form",{"name", {"Entry", "13f7ab9d.name",<object>Blueprint.bpData.Data.Components[0].name,"","Enter Unique Name Here"}}, }	}
//			
	
	
	
	public Dictionary<string,dynamic> tree;
	public Dictionary<string,dynamic> BpTreeVM;
	//public Dictionary<string,dynamic>? wireList;
	public FullBpTreeCollection(Dictionary<string,dynamic>? bpFlatData ){
		
		this.tree = [];
		//this.wireList = [];
		this.BpTreeVM = [];
		this.BpTreeVM.Add("Panels",new Dictionary<string,dynamic>{});
		this.BpTreeVM.Add("Wires",new Dictionary<string,dynamic>{});

		if (bpFlatData is not null){
			BranchTree(bpFlatData);
		} else {
			SeedBpTree();
		}
	}

	private void SeedBpTree()
	{
		string AssetId = $"{Guid.NewGuid()}";
		this.tree.Add(AssetId, new List<dynamic>{"dbObj.Data"});
		
	}
	private void BranchTree(Dictionary<string,dynamic> bpFlatData) // only called when parsing a file
	{
		if (bpFlatData is not null){
			string rootPath;
			string AssetId = bpFlatData["dbObj.AssetId"];
			int bpFlatDataKeyLength = 0;
			string currentBpKey;
			string bpLineKeyCheck;
			this.tree.Add(AssetId, new OrderedDictionary{});
			this.tree[AssetId].Add("rootPath", new List<string>{"dbObj.Data"});
			//tree[AssetId].Add(["rootPath", "dbObj.Data"]);
			//loop 1, Discover references and
			foreach (var (bpLineKey, bpLineValue) in bpFlatData)
			{
				//Console.WriteLine(bpLineKey+" : "+bpLineValue);
				if (bpLineKey[^4..] == "name" && bpLineKey[^9..^5] != "Data")
				{
					rootPath = bpLineKey[..bpLineKey.LastIndexOf('.')];

					this.tree.Add(bpLineValue, new OrderedDictionary{});
					this.tree[bpLineValue].Add("rootPath", new List<dynamic>{rootPath});
					//this.tree[bpLineValue].Add(rootPath);

					Console.WriteLine($"Tree Item Added: {bpLineValue}");
				}
			}

			foreach (var (typeName,_) in this.tree)
			{
				//rootPath = this.tree[typeName][0];
				rootPath = this.tree[typeName]["rootPath"][0];
				//this.tree[typeName].Add("Categories", new List<string>{"FromFile"});
				//this.tree[typeName].Add("buttonName", new List<string>{"FromFile"});
				//this.tree[typeName].Add("title", new List<string>{"Label", "auto", "visible", "string", "ToDo:NeedToExtract"});
				this.tree[typeName].Add("Categories", new List<dynamic>{"FromFile"});
				this.tree[typeName].Add("buttonName", new List<dynamic>{"FromFile"});
				this.tree[typeName].Add("title", new List<dynamic>{"Label", "auto", "visible", "string", "ToDo:NeedToExtract"});
				//this.tree[typeName].Add("$AssetId", new List<string>{typeName});
				foreach (var (bpLineKey, bpLineValue) in bpFlatData)
				{
					bpFlatDataKeyLength = bpLineKey.LastIndexOf('.');
					bpLineKeyCheck = bpLineKey[..bpFlatDataKeyLength];
					if (bpLineKeyCheck == rootPath)
					{
						//if (bpFlatDataKeyLength == rootPath.Length)
						//{
							currentBpKey = bpLineKey[(bpFlatDataKeyLength+1)..];
							this.tree[typeName].Add(currentBpKey,new List<dynamic>{"FromFile",bpLineValue});
							
						//}
					} else if (bpFlatDataKeyLength > rootPath.Length)
					{
						if (bpLineKey[^4..] == "name")
						{
							//this.tree[typeName].Add(new List<dynamic>{bpLineKeyCheck,bpLineValue});
							this.tree[typeName].Add(bpLineKeyCheck,new List<dynamic>{"Link","FromFile",bpLineKeyCheck,bpLineValue});
						}
					}
				}
			}
		}
	}
}
