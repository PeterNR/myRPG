using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName = "Nuzlon/Create new move")]
public class BaseMove : ScriptableObject
{
    [SerializeField]
    private string name;

    [TextArea]
    [SerializeField]
    private string description;

    [SerializeField]
    private Type type;
    [SerializeField]
    private int power, accuracy, pp;

    public string Name
    {
        get { return name; }
    }
    public string Descrption
    {
        get { return description; }
    }
    public Type Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
    public int Accuracy
    {
        get { return accuracy; }
    }
    public int PP
    {
        get { return pp; }
    }
}
