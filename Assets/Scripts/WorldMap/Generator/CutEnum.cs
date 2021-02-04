using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Cut
{
    XAxisP = 0b0001,
    XAxisN = 0b0010,
    ZAxisP = 0b0100,
    ZAxisN = 0b1000,
}
