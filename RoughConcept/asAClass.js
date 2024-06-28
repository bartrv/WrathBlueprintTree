//var bpData = {};
function goParseJson(sourceTextElement){
	return new jbpObject(sourceTextElement);
}

class jbpObject {
	#rawLength;
	#charIndex;
	#nestDepth;
	#treeBranch;
	#isQuote;
	#entryType;
	//#currentString;
	constructor(sourceTextElement){
		this.rawText = sourceTextElement.value;
		this.bpData = {};
		this.#rawLength = this.rawText.length;
		this.#charIndex = 0;
		this.#nestDepth = [];  // ["{", "{", "[", "{"]
		this.#treeBranch = []; //["brancha","branchb",2,"branchc"]
		this.#isQuote = 0;
		this.#entryType = "Key";
		//this.#currentString = "";
		this.mainLoop();
	}
	
	#buildString(){
		let currentString = "";
		while (this.#isQuote && this.#charIndex < this.#rawLength) {
			this.#charIndex++;
			if (this.rawText[this.#charIndex] != "\""){
				currentString += this.rawText[this.#charIndex];
			} else {
				this.#isQuote = !this.#isQuote;
				//console.log([currentString, this.#charIndex]);
				if (this.rawText[this.#charIndex+1] == ":"){
					this.#entryType = "Key";
				} else {
					this.#entryType = "Value";
				}
				this.#charIndex++;
				return currentString; //[this.#currentString, this.#charIndex]
			}
		}
	}
	
	#buildNumber(){
		let numBuildCursor = this.#charIndex;
		let currentNumber = this.rawText[numBuildCursor];
		let endOfNumber = 0;
		while (endOfNumber == 0 && numBuildCursor < this.#rawLength) {
			numBuildCursor++;
			//if (this.rawText[this.#charIndex] =="," || this.rawText[this.#charIndex] == "\n"){
			if (isNaN(this.rawText[numBuildCursor]) && this.rawText[numBuildCursor] != "." || this.rawText[numBuildCursor] === "," || this.rawText[numBuildCursor] === "\n"){
				numBuildCursor++;
				this.#charIndex = numBuildCursor;
				return currentNumber;
			}
			currentNumber += this.rawText[numBuildCursor];
		}
	}
	
	#setBPValue(valueToInsert, currentBranchStruct){
		//ToDo : modify this module to set non-text BP values for null, true, false
		//let branchString = currentBranchStruct.slice(0,currentBranchStruct.length).join(".");
		let branchString = "";
		for (const twigItem of currentBranchStruct){
				branchString += (isNaN(twigItem)) ? "." + twigItem : "["+twigItem+"]";
			}
		//branchString = branchString.slice(1,);
		let mValueFuncText = "";
		//let mValueFuncText = "bpData."+branchString+" = \""+currentString.string+"\";";
		if (isNaN(valueToInsert) || valueToInsert === ""){
			mValueFuncText = "bpData"+branchString+" = \""+valueToInsert+"\";"; //ToDo: variations for t/f/null
		} else {
			mValueFuncText = "bpData"+branchString+" = "+valueToInsert+";";
		}
		let mValueFunc = new Function("bpData", mValueFuncText);
		mValueFunc(this.bpData);
		return branchString;
	}
	
	#initBPKey(keyBranchToInit){
		//let branchString = keyBranchToInit.join(".");
		let branchString = "";
		for (const twigItem of keyBranchToInit){
				branchString += (isNaN(twigItem)) ? "." + twigItem : "["+twigItem+"]";
			}
		branchString = branchString.slice(1,);
		let mValueFuncText = "";
		let initCursor = this.#charIndex;
		while (initCursor < this.rawText.length-1) {
			initCursor++;
			if (this.rawText[initCursor] == "{") { 
				mValueFuncText = "bpData."+branchString+" = {};"; 
				break;
			} else if (this.rawText[initCursor] == "[") {
				mValueFuncText = "bpData."+branchString+" = [];"; 
				break;
			} else if (this.rawText[initCursor] == "\"" || this.rawText[initCursor] == "," 
						|| (!isNaN(this.rawText[initCursor]) && this.rawText[initCursor]!==" " && this.rawText[initCursor]!="\n")) {
				mValueFuncText = "bpData."+branchString+" = \"\";"; 
				break;
			}
		}
		//mValueFuncText = "bpData."+branchString+" = \"\";";

		let mValueFunc = new Function("bpData", mValueFuncText);
		mValueFunc(this.bpData);
		return branchString;
	}
	
	#seedBPArray(seedType){
		seedType = (seedType == "{") ? "{}" : "[]";
		let branchString = "";
		let seedFuncString = "";
		let twigCounter = 0;
		let twigItem = "";
		for (twigCounter; twigCounter < this.#treeBranch.length-1; twigCounter++){
			twigItem = this.#treeBranch[twigCounter];
			branchString += (isNaN(twigItem)) ? "." + twigItem : "["+twigItem+"]";
		}
			branchString = branchString.slice(1,);
		if (this.#nestDepth[this.#nestDepth.length-1] == "{"){
			//branchString = this.#treeBranch.slice(0,this.#treeBranch.length).join(".");
			twigItem = this.#treeBranch[twigCounter+1];
			branchString += (isNaN(twigItem)) ? "." + twigItem : "["+twigItem+"]"
			seedFuncString = "if (bpData."+branchString+" == \"\"){ bpData."+branchString+" = "+seedType+";}";
		} else {
			//branchString = this.#treeBranch.slice(0,this.#treeBranch.length-1).join(".");
			seedFuncString = "bpData."+branchString+".push("+seedType+");";
		}
		let mValueFunc = new Function("bpData", seedFuncString);
		mValueFunc(this.bpData);
		return branchString;
	}
	
	mainLoop(){
		for (this.#charIndex; this.#charIndex < this.#rawLength-1; this.#charIndex++){
			//this.#charIndex++;
			let thisChar = this.rawText[this.#charIndex];
			if (thisChar == "\"") {this.#isQuote = !this.#isQuote;} //toggle in/out of quotes
			if (this.#isQuote == 1){
				let currentString = "";
				currentString = this.#buildString();
				if (this.#entryType == "Value") {
					if (this.#nestDepth[this.#nestDepth.length-1] == "{"){
						console.log(this.#treeBranch.join(".") +": "+currentString);
						this.#setBPValue(currentString, this.#treeBranch);
					} else {
						console.log(this.#treeBranch.join(".") +"Append-> "+currentString);
					}
				} else if (this.#entryType == "Key") {
					if (this.#nestDepth.length == 1){
						this.#treeBranch[0] = currentString;
						//this.bpData[currentString] = ""; //generate bpData key at bottom level
					} else if (this.#treeBranch.length >= this.#nestDepth.length-1){
						this.#treeBranch = this.#treeBranch.slice(0, this.#nestDepth.length-1);
						this.#treeBranch.push(currentString);
						//this.#initBPKey(this.#treeBranch);
					}
					this.#initBPKey(this.#treeBranch);
					console.log("Branch: "+this.#treeBranch.join("."));
					this.#entryType = "Value";
				}
			} else if (thisChar !=" " && thisChar != "\n" && thisChar != ","){
				switch (thisChar) {
					case "{": 
						if (!isNaN(this.#nestDepth[this.#nestDepth.length-1]) && this.#entryType == "Value"){
							if (this.#nestDepth[this.#nestDepth.length-1]>=0 
								&& this.#treeBranch[this.#treeBranch.length-1] === this.#nestDepth[this.#nestDepth.length-1]){
								this.#nestDepth[this.#nestDepth.length-1]++;
								this.#treeBranch[this.#treeBranch.length-1]++;
							} else if (this.#nestDepth[this.#nestDepth.length-1] == -1){
								this.#nestDepth[this.#nestDepth.length-1]++;
								this.#treeBranch.push(this.#nestDepth[this.#nestDepth.length-1]);
							}								
							this.#seedBPArray("{");
						}
						this.#nestDepth.push("{");
						this.#entryType = "Key"
						break;
					case "}": 
						this.#nestDepth.pop();
						this.#entryType = (this.#nestDepth[this.#nestDepth.length-1] == "{") ? "Key" : "Value";
						this.#treeBranch.pop();
						break;
					case "[": 
						this.#nestDepth.push(-1);
						
						this.#entryType = "Value"
						break;
					case "]": 
						this.#nestDepth.pop();
						this.#treeBranch.pop();
						this.#entryType = (this.#nestDepth[this.#nestDepth.length-1] == "{") ? "Key" : "Value";
						break;
				}
				if (this.#entryType == "Value"){
					let currentString = "";
					if (thisChar == "n" && this.rawText[this.#charIndex+1] =="u"){
						currentString = "null";
						this.#setBPValue(currentString, this.#treeBranch);
						console.log("Alt Value: null");
					} else if (thisChar == "f" && this.rawText[this.#charIndex+1] =="a"){
						currentString = "false";
						this.#setBPValue(currentString, this.#treeBranch);
						console.log("Alt Value: false");
					} else if (thisChar == "t" && this.rawText[this.#charIndex+1] =="r"){
						currentString = "true";
						this.#setBPValue(currentString, this.#treeBranch);
						console.log("Alt Value: true");
					} else if (!isNaN(thisChar) || !isNaN(thisChar + this.rawText[this.#charIndex+1])){
						currentString = this.#buildNumber();
						if (currentString.indexOf(".") == -1){
							currentString = parseInt(currentString);
						} else {
							let floatLen = currentString.length - currentString.indexOf(".")-1;
							currentString = parseFloat(currentString).toFixed(floatLen);
						}
						console.log("Num Value:" + currentString);
						this.#setBPValue(currentString, this.#treeBranch);
					} else if (thisChar =="{"){
						
						if (!isNaN(this.#nestDepth[this.#nestDepth.length-2])) { //if there is a number, it is a [] index
							this.#nestDepth[this.#nestDepth.length-2]++;
							this.#treeBranch[this.#nestDepth.length-2] = this.#nestDepth[this.#nestDepth.length-2];
							console.log(this.#treeBranch.join(".")+"push: [{}"+1+this.#nestDepth[this.#nestDepth.length-2]+"]");
						} else {
							console.log(this.#treeBranch.join(".")+"Assign: {}");
							//this.#seedBPArray("{");
						}
					} else if (thisChar === "}" || thisChar === "]"){
						this.#treeBranch = this.#treeBranch.slice(0,this.#nestDepth.length);
						
					} else if (thisChar =="["){
							
						if (!isNaN(this.#nestDepth[this.#nestDepth.length-2])) { //if there is a number, it is a [] index
							this.#nestDepth[this.#nestDepth.length-2]++;
							this.#treeBranch[this.#nestDepth.length-2] = this.#nestDepth[this.#nestDepth.length-2];
							console.log(this.#treeBranch.join(".")+"push: [[]"+1+this.#nestDepth[this.#nestDepth.length-2]+"]");
						} else {
							//this.#nestDepth[this.#nestDepth.length-1]++;
							//this.#treeBranch[this.#nestDepth.length-1] = this.#nestDepth[this.#nestDepth.length-1];
							console.log(this.#treeBranch.join(".")+"Assign: []");
							//this.#seedBPArray("[");
						}
					}
				}				
			} 
		}
	}
}