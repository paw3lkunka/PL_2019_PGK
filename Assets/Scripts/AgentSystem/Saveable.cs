using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour
{
    static int newID = 1;
    int id = 0;
    private void OnValidate()
    {
        if(id == 0)
        {
            id = newID++;
        }
    }
}
