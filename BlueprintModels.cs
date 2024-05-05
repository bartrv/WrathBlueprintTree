using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.IO;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WrathBlueprintTree;

public class BlueprintModels{
    // This Model is !!!READ ONLY!!!
    // Once initialized, this should be only for *building the templates* used for bluwprint visualization/construction
    // NOT for saving any data.
    // The BlueprintObject Class should be re-instantiated as a new object for the user's working blueprint link map, 
    // so there will probably be 2 instances, one that was read in for referance / original and not manipulated
    // the other either unique, or a copy of the original.
    // The logical structure should match the ingested template structure so referances build from this model match relative to insertion points
    // Since there is the "Object" ingested template and the "Flat" version, Duplicate-ish code blocks are testing which version is more friendly to the BPModel build/use.
    //       (Leaning toward Flat)
    //eg. {Data.Components[0].type}
    private static Dictionary<string,dynamic> _modelDictionary = [];
    //Dictionary<string, dynamic> bpDict = [];

    public static Dictionary<string,dynamic> ModelDictionary
    {
        get { return _modelDictionary; }
    }

    //Categories: Targeting, Area, Search, Trigger, Action, Effect
    public static void GenerateBpDictionary()
    {
        _modelDictionary.Add("AbilityTargetsAround", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function","Targeting", "Area", "Search"}},
                    {"buttonName","Ability Targets Around"},
                    {"$type","757439ef8cc900741bd9b57bf26eb500, AbilityTargetsAround"},
                    {"m_Flags",0},
                    {"name","$TBD$GuidDashed"},
                    {"PrototypeLink",new Dictionary<string,string> {{"guid",""},{"name",""}}},
                    {"m_Overrides", new List<string> {"*"}},
                    {"m_Radius", new Dictionary<string,float>{{"m_Value", 0}}},
                    {"m_TargetType",new List<string> {"Enemy","Self","Party","Friendly"}},
                    {"m_IncludeDead", new List<bool>{true,false}},
                    {"m_Condition", new Dictionary<string, dynamic> {
                        {"Operation", new List<string>{"And","Or"}},
                        {"Conditions", new List<string>{"*"}}
                    }},
                    {"m_SpreadSpeed",new Dictionary<string,float> {{"m_Value",0}}}
        });

        _modelDictionary.Add("BlueprintProgression", new OrderedDictionary {
                    {"rootPath",new List<string>{"notSet"}},
                    {"Categories",new List<string>{"Asset","Progression", "CharGen", "Class", "Mythic"}},   //Asset/Function, sorting categories...
                    {"buttonName",new List<string>{"Progression (Generic)"}},                                         //Raw text for drag list object/button
                    {"title",new List<string>{"Label","hardCoded","visible","string","Progression"}},       //Ui element or type, entry source, ui visible, data type, content
                    {"$AssetId",new List<string>{"Value","auto","hidden","NewGuid", ""}},
                    {"$type",new List<string>{"Value","hardCoded","hidden","string","bec71e89a676a99458c9e2d0804f2a0c, BlueprintProgression"}},
                    {"m_Flags",new List<string>{"Value","auto","hidden","int","0"}},
                    {"name",new List<string>{"Value","auto","hidden","string","$TBD$GuidDashed"}},
                    {"PrototypeLink",new List<string>{"DoubleEntryOrNull","string,string","null"}},
                    {"m_Overrides", new List<string> {"List","AutoPopulate","hidden","string","Components"}},
                    {"Components", new List<string>{"LinkList","user","node","string","*"}},
                    {"Comment", new List<string> {"Entry","user","visible","string",""}},
                    {"m_AllowNonContextActions", new List<string> {"Picker","user","visible","bool","true","false"}},
                    {"m_DisplayName.m_Key", new List<string> {"Entry","either","visible","string",""}},
                    {"m_DisplayName.m_OwnerString", new List<string> {"Value","auto","hidden","string","$AssetId"}},
                    {"m_DisplayName.m_OwnerPropertyPath", new List<string> {"Value","hardCoded","hidden","string","m_DisplayName"}},
                    {"m_DisplayName.m_JsonPath", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DisplayName.Shared", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_Description.m_Key", new List<string> {"Entry","either","visible","string",""}},
                    {"m_Description.m_OwnerString", new List<string> {"Value","auto","hidden","string","$AssetId"}},
                    {"m_Description.m_OwnerPropertyPath", new List<string> {"Value","hardCoded","hidden","string","m_Description"}},
                    {"m_Description.m_JsonPath", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_Description.Shared", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DescriptionShort.m_Key", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DescriptionShort.m_OwnerString", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DescriptionShort.m_OwnerPropertyPath", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DescriptionShort.m_JsonPath", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_DescriptionShort.Shared", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"m_Icon", new List<string> {"Value","hardCoded","hidden","string",""}},
                    {"HideInUI", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"HideInCharacterSheetAndLevelUp", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"HideNotAvailibleInUI", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"Groups", new List<string> {"EditorList","user","visible","string",""}},
                    {"Ranks", new List<string> {"Picker","user","visible","int","1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20"}},
                    {"ReapplyOnLevelUp", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"IsClassFeature", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"IsPrerequisiteFor", new List<string> {"ListNull","user","visible","string","null"}},
                    {"m_Classes", new List<string> {"DoubleEmpty","user","visible","pointer,int","m_Class","*","AdditionalLevel","0"}},
                    {"m_Archetypes", new List<string> {"DoubleEmpty","user","visible","string,int","m_Class","*","AdditionalLevel","0"}},
                    {"ForAllOtherClasses", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"m_AlternateProgressionClasses", new List<string> {"DoubleEmpty","user","visible","string,int","m_Class","*","AdditionalLevel","0"}},
                    {"AlternateProgressionType", new List<string> {"Picker","user","visible","string","Div2","OnePlusDiv2"}},
                    {"LevelEntries.n.Level", new List<string> {"CheckBoxVoid","user","visible","int","1"}},
                    {"LevelEntries.n.m_Features", new List<string> {"PointerListVoid","user","node","string","*"}},
                    {"UIGroups", new List<string> {"ListEmpty","user","visible","string",""}},
                    {"m_UIDeterminatorsGroup", new List<string> {"ListEmpty","user","visible","string",""}},
                    {"m_ExclusiveProgression", new List<string> {"PointerListNull","user","node","string","null"}},
                    {"GiveFeaturesForPreviousLevels", new List<string> {"CheckBox","user","visible","bool","true","false"}},
                    {"m_FeaturesRankIncrease", new List<string> {"PointerListNull","user","node","string","null"}},                    
              });

        _modelDictionary.Add("AbilityEffectRunAction", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function","Trigger", "Action", "Effect"}},
                    {"buttonName","Ability Effect Run Action"},
                    {"$type","66e032e5cf38801428940a1a0d14b946, AbilityEffectRunAction"},
                    {"m_Flags",0},
                    {"name","$TBD$GuidDashed"},
                    {"PrototypeLink",new Dictionary<string,string> {{"guid",""},{"name",""}}},
                    {"m_Overrides", new List<string> {"*"}},
                    {"SavingThrowType",new List<string> {"Fortitude","Willpower","Reflex"}},
                    {"Actions", new Dictionary<string,List<string>> {{"Actions",["*"]}}}
                });
        _modelDictionary.Add("AddFactContextActions", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function","Trigger", "Action", "Context"}},
                    {"buttonName","Ability Fact Context Actions"},
                    {"$type","25d172d2be8f52f468b2050d14d59806, AddFactContextActions"},
                    {"m_Flags",0},
                    {"name","$TBD$GuidDashed"},
                    {"PrototypeLink",new Dictionary<string,string> {{"guid",""},{"name",""}}},
                    {"m_Overrides", new List<string> {"*"}},
                    {"Activated",new Dictionary<string,dynamic> {
                            {"Actions", new List<Dictionary<string,dynamic>> {}
                            }
                        }
                    }
                });
        _modelDictionary.Add("Conditional", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function", "Conditional","Trigger", "Action"}},
                    {"buttonName","Conditional Action Trigger"},
                    {"$type","52d8973f2e470e14c97b74209680491a, Conditional"},
                    {"name","$TBD$GuidDashed"},
                    {"Comment",""},
                    {"ContitionsChecker",new Dictionary<string,dynamic>{
                            {"Operation",new List<string> {"And","Or"}},
                            {"Contitions",new List<Dictionary<string,dynamic>>{}}
                    }},
                    {"IfTrue",new List<Dictionary<string,dynamic>>{}},
                    {"IfFalse",new List<Dictionary<string,dynamic>>{}}                    
                });
        _modelDictionary.Add("ContextConditionHasFact", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function", "Bool", "Context", "Conditional", "Fact"}},
                    {"buttonName","Context Condition Has Fact"},
                    {"$type","9706de75454abeb48bd4cfa7f526a1c2, ContextConditionHasFact"},
                    {"name","$TBD$GuidDashed"},
                    {"Not",new List<bool>{true,false}},
                    {"m_Fact","!bp_"}                    
                });
        _modelDictionary.Add("HasFact", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Function", "Bool", "Conditional", "Fact"}},
                    {"buttonName","Has Fact (Non-Contextual)"},
                    {"$type","f310985bf2724df4a97b165f74b806e8, HasFact"},
                    {"name","$TBD$GuidDashed"},
                    {"Not",new List<bool>{true,false}},
                    {"Unit",new Dictionary<string,dynamic>{
                        {"$type",new Dictionary<string,string>{
                            {"Party Unit","2b9ad38748400fb4a9db077957c3a839"},
                            {"Player Character","25c132cb07bfaef4683b062a74f6e012"}
                        }},
                        {"name","$TBD$GuidDashed"}
                    }},
                    {"m_Fact","!bp_"}                    
                });
    }

    public static bool GetBpModel(string key, out object value){
            return _modelDictionary.TryGetValue(key, out value);
    }
    //BlueprintModels.ModelDictionary.Add("TestEntry", new List<List<string>>{});  //perhapse for adding additional entries from ingested blueprints
    // adding at runtime is useless if there are no save/update/loadModel methods

    //* Blueprint templates *//
    //BlueprintCharacterClass
    //BlueprintArchetype
    //Race
    //AddFacts
    //Fact
    //BlueprintFeature
    //BlueprintFeatureSelection
    //BlueprintProgression
    //BlueprintAbility
    //BlueprintSpellList --> WizardSpellList: ba0401fdeb4062f40a7aa95b6f07fe89
    //BlueprintProjectile
    //BluprintProjectileTrajectory
    //AbilityEffectRunAction
    //AbilityDeliverProjectile
    //AbilityTargetsAround

    //ManeuverBonus
    //ManeuverDefenseBonus
    //PrerequisiteFeature
    //PrerequisiteNoFeature
    //PrerequisiteNoArchetype
    //PrerequisiteCondition
    //PrerequisiteStatValue
    //PrerequisiteFeatureFromList
        //* Condition / Context *//
        //OrAndLogic typeId: 1d392c8d9feed78408fdcb18f9468fb9
        //ContextRankConfig
        //ContextConditionHasFact
        //ContextConditionCharacterClass
        //ContextActionCombatManeuver
        //ContextActionDealDamage
        //ContextActionPush
        //ContextActionSavingThrow
        //ContextActionConditionalSaved
        //ContextActionKnockdownTarget
        //ContextActionDisableBonusForDamage
        //ContextSetAbilityParams

        //*Configure*//
        //AbilityRankConfig

    
    //BlueprintParametrizedFeature
    
    //LearnSpellParametrized
    
    //AddKnownSpell
    //spell-> Spells are 'Abilities', see BlueprintAbility
    //SpellDescriptorComponent
    //item
    //BlueprintItem Weapon
    //decision
    //Encyclopedia Page

    //* Components *//
    //AddStatBonus typeId: a2844c135c0324e439072bd3cc2f9260
    //AddStatBonusIfHasFact typeId: affe7e2f9f61c4044b1c5102a4dfc5e0
    //m_DisplayName
    //m_Description
    //SpellComponent
    //SpellListComponent
    //CraftInfoComponent

}