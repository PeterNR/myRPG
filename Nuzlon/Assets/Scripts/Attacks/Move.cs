using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public BaseMove Base { get; set; }
    public int PP { get; set; }
    public bool IsRanged { get; set; }

    public Move(BaseMove cBase)
    {
        Base = cBase;
        PP = cBase.PP;
        IsRanged = cBase.IsRanged;
    }
}
