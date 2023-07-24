using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Combo")] 
public class Combo : ScriptableObject
{
    public string _name;
    public List<ComboEvents> _inputs;

    public ComboEvents GetActionByIndex(int index) 
    {
        if (index >= 0 && index < _inputs.Count) 
        { 
            return _inputs[index];
        }

        return ComboEvents.None;
    } 
}
