using System;

namespace WrathBlueprintTree;

public class Queue : TreePage
{
    Dictionary<string, List<dynamic>> Items;

    public Queue()
    {
        Items = Reset();
    }

    private static Dictionary<string, List<dynamic>> Reset() //internal method to force the initiation  of all queqes
    {
        Dictionary<string, List<dynamic>> resetItems = new()
        {
            { "Wires", [] },
            { "Panels", [] },
            { "Generic", [] }
        };
        return resetItems;
    }

    public bool Reset(string subQueue) //Accessable Overload method clear/re-init each sub-queue as needed
    {
        if (subQueue == "Wires"){
            Items["Wires"] = [];
            return true;
        } else if (subQueue == "Panels"){
            Items["Panels"] = [];
            return true;
        } else if (subQueue == "Generic"){
            Items["Generic"] = [];
            return true;
        } else if (subQueue == "Kill"){
            Items = Reset();
            return true;
        } 
        return false;
    }

    public bool AddItem(object ItemToAdd)
    {
        if (ItemToAdd is Border){
            if (!CheckItem(Items["Panels"]))
            {
                Items["Panels]"].Add(ItemToAdd);
                return true;
            }
        } else if (ItemToAdd is Microsoft.Maui.Controls.Shapes.Path)
        {
           if (!CheckItem(Items["Wires"]))
            {
                Items["Wires"].Add(ItemToAdd);
                return true;
            }
        } else if (ItemToAdd is string)
        {
           if (!CheckItem(Items["Generic"]))
            {
                Items["Generic"].Add(ItemToAdd);
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(object ItemToRemove)
    {
         if (ItemToRemove is Border){
            if (!CheckItem(Items["Panels"]))
            {
                Items["Panels]"].Remove(ItemToRemove);
                return true;
            }
        } else if (ItemToRemove is Microsoft.Maui.Controls.Shapes.Path)
        {
           if (!CheckItem(Items["Wires"]))
            {
                Items["Wires"].Remove(ItemToRemove);
                return true;
            }
        } else if (ItemToRemove is string)
        {
           if (!CheckItem(Items["Generic"]))
            {
                Items["Generic"].Remove(ItemToRemove);
                return true;
            }
        }
        return false;
    }
    
    public bool CheckItem(object ItemToFind)
    {
       if (ItemToFind is Border){
            return Items["Panels"].Contains(ItemToFind); 
        } else if (ItemToFind is Microsoft.Maui.Controls.Shapes.Path)
        {
           return Items["Wires"].Contains(ItemToFind); 
        } else if (ItemToFind is string)
        {
           return Items["Generic"].Contains(ItemToFind);  
        }
        return false;
    }

}