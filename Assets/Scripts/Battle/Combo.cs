using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Combo")] 
public class Combo : ScriptableObject
{
    public string _name;
    public List<ComboEvents> _inputs;

    public ComboEvents GetActionInIndex(int index) 
    {
        if (index > -1 && index < _inputs.Count) 
        { 
            return _inputs[index];
        }

        return ComboEvents.None;
    } 
}
