using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.IO;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace WrathBlueprintTree;

public class BlueprintModels{
    // This Model is !!!READ ONLY!!!
    // Once initialized, this should be only for *building the templates* used for bluwprint visualization/construction
    // NOT for saving any data.
    // The BlueprintObject Class should be re-instantiated as a new object for the user's working blueprint link map, 
    // so there will probably be 2 instances, one that war read in for referance / original and not manipulated
    // the other either unique, or a copy of the original.
    // The logical structure should match the ingested template structure so referances build from this model match relative to insertion points
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
                    {"Categories",new List<string>{"Targeting", "Area", "Search"}},
                    {"buttonName","Ability Targets Around"},
                    {"$type","757439ef8cc900741bd9b57bf26eb500, AbilityTargetsAround"},
                    {"m_Flags",0},
                    {"name","$TBD$GuidDashed"},
                    {"PrototypeLink",new Dictionary<string,string> {{"guid",""},{"name",""}}},
                    {"m_Overrides", new List<string> {"*"}},
                    {"m_Radius", new Dictionary<string,float>{{"m_Value", 0}}},
                    {"m_TargetType",new List<string> {"Enemy","Self","Party","Friendly"}},  //???
                    {"m_IncludeDead", new List<bool>{true,false}},
                    {"m_Condition", new Dictionary<string, dynamic> {
                        {"Operation", new List<string>{"And","Or"}},
                        {"Conditions", new List<string>{"*"}}
                    }},
                    {"m_SpreadSpeed",new Dictionary<string,float> {{"m_Value",0}}}
        });

        _modelDictionary.Add("AbilityEffectRunAction", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Trigger", "Action", "Effect"}},
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
                    {"Categories",new List<string>{"Trigger", "Action", "Context"}},
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
                    {"Categories",new List<string>{"Trigger", "Action", "Conditional"}},
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
                    {"Categories",new List<string>{"Bool", "Context", "Conditional", "Fact"}},
                    {"buttonName","Context Condition Has Fact"},
                    {"$type","9706de75454abeb48bd4cfa7f526a1c2, ContextConditionHasFact"},
                    {"name","$TBD$GuidDashed"},
                    {"Not",new List<bool>{true,false}},
                    {"m_Fact","!bp_"}                    
                });
        _modelDictionary.Add("HasFact", new Dictionary<string,dynamic> {
                    {"Categories",new List<string>{"Bool", "Conditional", "Fact"}},
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

    public static bool checkBpModel(string key, out object value){
            return _modelDictionary.TryGetValue(key, out value);

    }


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