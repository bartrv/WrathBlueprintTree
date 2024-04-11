//using CoreImage;

using Microsoft.Maui.Controls.Platform.Compatibility;

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
		public Dictionary<string, dynamic> bpData;

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
			while ((isQuote != 0) && this.charIndex < rawLength) {
				this.charIndex++;
				if (this.rawText[this.charIndex] != '"'){
					currentString += this.rawText[this.charIndex];
				} else {
					isQuote = Math.Abs(isQuote-1);
					//console.log([currentString, this.#charIndex]);
					if (rawText[this.charIndex+1] == ':'){
						entryType = "Key";
					} else {
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
			int  endOfNumber = 0;
			while (endOfNumber == 0 && numBuildCursor < this.rawLength) {
				numBuildCursor++;
				//if (this.rawText[this.#charIndex] =="," || this.rawText[this.#charIndex] == "\n"){

				if (!int.TryParse(this.rawText[numBuildCursor].ToString(), out _) && this.rawText[numBuildCursor] != '.' || this.rawText[numBuildCursor] == ',' || this.rawText[numBuildCursor] == '\n'){
					numBuildCursor++;
					this.charIndex = numBuildCursor;
					return currentNumber;
				}
				currentNumber += this.rawText[numBuildCursor];
			}
			return currentNumber;
		}
	
		private string SetBPValue(object valueToInsert, List<string> currentBranchStruct){
			//ToDo : modify this module to set non-text BP values for null, true, false, missing append strings to List<T>
			string branchString = "";
			foreach (string twigItem in currentBranchStruct){
				branchString += (!int.TryParse(twigItem.ToString(), out _)) ? "." + twigItem : $"[{twigItem}]";
			}
			
			string mValueFuncText = "";

			if (!int.TryParse(valueToInsert.ToString(), out _) || valueToInsert == null || valueToInsert.ToString() == ""){
				mValueFuncText = "bpData"+branchString+" = \""+valueToInsert+"\";"; //ToDo: variations for t/f/null
			} else {
				mValueFuncText = "bpData"+branchString+" = "+valueToInsert+";";
			}
			Console.WriteLine(mValueFuncText);
            if (currentBranchStruct.Count == 1){
                this.bpData[currentBranchStruct[0]] = valueToInsert ?? "";
            } else {
                //Dictionary<string,dynamic> tempBpDict = this.bpData;
				dynamic tempBpDict = this.bpData;
                int k = 0;
				int tempListIndex = -1;
                for (; k < currentBranchStruct.Count-1; k++){
                    //tempBpDict = (Dictionary<string,dynamic>)tempBpDict[currentBranchStruct[k]];
					
					if (int.TryParse(currentBranchStruct[k], out _)){ //if false, sets target variable to 0, regardless
						tempListIndex = int.Parse(currentBranchStruct[k]);
						tempBpDict = tempBpDict[tempListIndex];
					} else {
						tempBpDict = tempBpDict[currentBranchStruct[k]];
						tempListIndex = -1;
					}
					
					//tempBpDict = tempBpDict[currentBranchStruct[k]];
                }

				switch (valueToInsert){
					case "true":
					case "True":
						valueToInsert = (bool)true;
						break;
					case "false":
					case "False":
						valueToInsert = (bool)false;
						break;
				}
				if (!int.TryParse(currentBranchStruct[k], out _)){
                	tempBpDict[currentBranchStruct[k]] = valueToInsert ?? "";
				} else if (int.TryParse(currentBranchStruct.Last(), out _)) {
					tempBpDict.Add(valueToInsert);
				} else {
					tempBpDict[tempListIndex] = valueToInsert ?? "";
				}
                
            }
			return branchString;
		}
		
		private string InitBPKey(List<string> currentBranchStruct){
			string branchString = "";
			foreach (string twigItem in currentBranchStruct){
					branchString += (!int.TryParse(twigItem.ToString(), out _)) ? $"." + twigItem : $"[{twigItem}]";
			}
			branchString = branchString[1..];
			//string mValueFuncText = "";
			int initCursor = this.charIndex;

				dynamic tempBpDict = this.bpData;
                int k = 0;
				string twig = currentBranchStruct[0];
				//Type NestType = tempBpDict[currentBranchStruct[0]].GetType();
				foreach (string twigItem in currentBranchStruct){
					k++;
					//if (twigItem != currentBranchStruct.Last()){  //if keys of same name as last item, could skip a loop
					if (k < currentBranchStruct.Count()){ 
						int tempInt = 0;
						tempBpDict = (int.TryParse(twigItem, out tempInt)) ? tempBpDict[tempInt] : tempBpDict[twigItem]; //if a number is returned, its a List - use [int.number]
						//twig = twigItem;
					}
					twig = twigItem;
				}
				while (initCursor < this.rawText.Length-1) {
					initCursor++;
					if (this.rawText[initCursor] == '{') { 
						branchString = "bpData."+branchString+" = {};"; 
						tempBpDict.Add($"{twig}", new Dictionary<string,dynamic>{});
						break;
					} else if (this.rawText[initCursor] == '[') {
						branchString = "bpData."+branchString+" = [];"; 
						tempBpDict.Add($"{twig}", new List<dynamic>{});
						break;
					} else if (this.rawText[initCursor] == '"' || this.rawText[initCursor] == ','
								|| (int.TryParse(this.rawText[initCursor].ToString(), out _)) && this.rawText[initCursor] != ' ' && this.rawText[initCursor] != '\n' && this.rawText[initCursor] != '\r' ) {
						branchString = "bpData."+branchString+" = \"\";"; 
						tempBpDict.Add($"{twig}","");
						break;
					}
				}
			//Console.WriteLine(branchString);
			this.entryType = "Value";
			return branchString;
		}

		private string SeedBPArray(string seedType, List<string> currentBranchStruct){ // ToDo ALL instantiation methods are different needs a complete rewrite, Can I even build a string as a lambda?? - recursion loop?
			//string branchString = "";
			string seedFuncString = "bpData";
			dynamic tempBpDict = this.bpData;
			string twig = currentBranchStruct[0];
			//Type NestType = tempBpDict[currentBranchStruct[0]].GetType();
			int branchIndexCount = 0;
			foreach (string twigItem in currentBranchStruct){
				branchIndexCount++;
				//if (twigItem != currentBranchStruct.Last()){
				if (branchIndexCount < currentBranchStruct.Count()){
					int tempInt = 0;
					seedFuncString += (int.TryParse(twigItem, out tempInt)) ? $"[{tempInt}]" : $".{twigItem}"; //build representitive parse string for debugging purposes
					tempBpDict = (int.TryParse(twigItem, out tempInt)) ? tempBpDict[tempInt] : tempBpDict[twigItem]; //if a number is returned, its a List - use [int.number]
				}
				twig = twigItem;
			}
			switch (seedType){
				case "{":
					tempBpDict.Add(new Dictionary<string,dynamic>{});
					break;
				case "[]":
					tempBpDict.Add(new List<dynamic>{});
					break;
				case "$":
					tempBpDict.Add("");
					break;

			}
			return seedFuncString;
			
		}
		
		private void MainLoop(){
			while (this.charIndex < this.rawLength-1){
				this.charIndex++;
				if (this.charIndex == this.rawLength-3) {
					/*Pause here to check final bpData object*/
					}
				char thisChar = this.rawText[this.charIndex];
				if (thisChar == '"') {this.isQuote = Math.Abs(this.isQuote-1);} //toggle in/out of quotes
				if (this.isQuote == 1){
					string currentString = "";
					currentString = this.BuildString();
					if (this.entryType == "Value") {
						if (this.nestDepth.Last() == "{"){
							//Console.WriteLine(String.Join(".",this.treeBranch) + ": " + currentString);
							this.SetBPValue(currentString, this.treeBranch);
						} else if (this.nestDepth.Last() == "-1"){
							this.nestDepth[this.nestDepth.Count()-1] = "0";
							if (this.treeBranch.Count() < this.nestDepth.Count()){
								this.treeBranch.Add(this.nestDepth.Last());
							} else if (this.treeBranch.Count() == this.nestDepth.Count()){
								this.treeBranch[this.treeBranch.Count()-1]=(this.nestDepth.Last());
							}
							//Console.WriteLine(String.Join(".",this.treeBranch) + "Append-> " + currentString); //This may be an unnecessaty check
							this.SetBPValue(currentString, this.treeBranch);
						} else if (int.TryParse(this.nestDepth.Last(), out _)){
							this.nestDepth[this.nestDepth.Count()-1] = $"{int.Parse(this.nestDepth.Last())+1}";
							if (this.treeBranch.Count() == this.nestDepth.Count()){
								this.treeBranch[this.treeBranch.Count()-1]=(this.nestDepth.Last());
							}
							//Console.WriteLine(String.Join(".",this.treeBranch) + "Append-> " + currentString); //This may be an unnecessaty check
							this.SetBPValue(currentString, this.treeBranch);
						}
					} else if (this.entryType == "Key") {
						if (this.nestDepth.Count == 1){
                            this.treeBranch.Clear();
							this.treeBranch.Add(currentString);

						} else if (this.treeBranch.Count >= this.nestDepth.Count-1){
							this.treeBranch = this.treeBranch.GetRange(0, this.nestDepth.Count-1);
							this.treeBranch.Add(currentString);
						}
						this.InitBPKey(this.treeBranch);
						//Console.WriteLine("Branch: "+String.Join(".",this.treeBranch));
						this.entryType = "Value";
					}
				} else if (thisChar != ' ' && thisChar != '\n' && thisChar != ',' && thisChar != '\r'){
					switch (thisChar) {
						case '{': 
							if (this.nestDepth.Count >1){
                                if (int.TryParse(this.nestDepth.Last(), out _) && this.entryType == "Value"){
                                    if (int.Parse(this.nestDepth.Last())>=0 
                                        && this.treeBranch.Last() == this.nestDepth.Last()){
                                        this.nestDepth[this.nestDepth.Count-1] = (int.Parse(this.nestDepth.Last()) + 1).ToString();
                                        this.treeBranch[this.treeBranch.Count-1] = (int.Parse(this.treeBranch.Last()) + 1).ToString();
                                    } else if (int.Parse(this.nestDepth.Last()) == -1){
                                        this.nestDepth[this.nestDepth.Count-1] = (int.Parse(this.nestDepth.Last()) + 1).ToString();
                                        this.treeBranch.Add(this.nestDepth.Last());
                                    }								
                                    this.SeedBPArray("{", this.treeBranch);
                                }
                            }
							this.nestDepth.Add("{");
							this.entryType = "Key";
							break;
						case '}': 
							if (this.nestDepth.Count>0) {
								this.nestDepth.RemoveAt(this.nestDepth.Count-1);
								this.treeBranch.RemoveAt(this.treeBranch.Count-1);
							}
                            if (this.nestDepth.Count>0) { 
                                this.entryType = (this.nestDepth.Last() == "{") ? "Key" : "Value";
                            }
							break;
						case '[': 
							this.nestDepth.Add("-1");
							this.entryType = "Value";
							break;
						case ']': 
							this.nestDepth.RemoveAt(this.nestDepth.Count-1);
							this.treeBranch.RemoveAt(this.treeBranch.Count-1);
							if (this.nestDepth.Count>0) { 
                                this.entryType = (this.nestDepth.Last() == "{") ? "Key" : "Value";
                            }
							break;
					}
					if (this.entryType == "Value"){
						string currentString = "";
						if (thisChar == 'n' && this.rawText[this.charIndex+1] =='u'){
							currentString = "null";
							this.SetBPValue(currentString, this.treeBranch);
							Console.WriteLine("Alt Value: null");
						} else if (thisChar == 'f' && this.rawText[this.charIndex+1] =='a'){
							currentString = "false";
							this.SetBPValue(currentString, this.treeBranch);
							Console.WriteLine("Alt Value: false");
						} else if (thisChar == 't' && this.rawText[this.charIndex+1] =='r'){
							currentString = "true";
							this.SetBPValue(currentString, this.treeBranch);
							Console.WriteLine("Alt Value: true");
						} else if (int.TryParse(thisChar.ToString(), out _) || int.TryParse(thisChar.ToString()+ this.rawText[this.charIndex+1], out _)){
							currentString = this.BuildNumber();
							if (!currentString.Contains(".")){
								//int CurrentStringAsInt = int.Parse(currentString);
								long CurrentStringAsInt = long.Parse(currentString);
								Console.WriteLine("Num Value:" + CurrentStringAsInt.ToString());
								this.SetBPValue(CurrentStringAsInt, this.treeBranch);
							} else {
								float currentStringAsFloat = float.Parse(currentString);
								Console.WriteLine("Num Value:" + currentStringAsFloat.ToString());
								this.SetBPValue(currentStringAsFloat, this.treeBranch);
							}
							
						} else if (thisChar == '{'){
							int tempTryParse = 0;
							if (int.TryParse(this.nestDepth[this.nestDepth.Count-2].ToString(), out tempTryParse)) { //if there is a number, it is a [] index
								this.nestDepth[this.nestDepth.Count-2] = $"{tempTryParse++}";
								this.treeBranch[this.nestDepth.Count-2] = this.nestDepth[this.nestDepth.Count-2];
								Console.WriteLine(String.Join(".",this.treeBranch)+"Add: [{}"+this.nestDepth[this.nestDepth.Count-2]+"]");
							} else {
								Console.WriteLine(String.Join(".",this.treeBranch)+"Assign: {}");
								
							}
						} else if (thisChar == '}' || thisChar == ']'){
							this.treeBranch = this.treeBranch.GetRange(0,this.nestDepth.Count);
							
						} else if (thisChar == '['){
								int tempTryParse = 0;
							if (int.TryParse(this.nestDepth[this.nestDepth.Count-2].ToString(), out tempTryParse)) { //if there is a number, it is a [] index
								this.nestDepth[this.nestDepth.Count-2] = $"{tempTryParse++}";
								this.treeBranch[this.nestDepth.Count-2] = this.nestDepth[this.nestDepth.Count-2];
								Console.WriteLine(String.Join(".",this.treeBranch)+"Add: [[]"+this.nestDepth[this.nestDepth.Count-2]+"]");
							} else {
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
	}