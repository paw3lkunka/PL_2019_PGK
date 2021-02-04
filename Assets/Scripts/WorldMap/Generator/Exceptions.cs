using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LocationValidationException : System.Exception
{
    public LocationValidationException(GameObject obj, int number)
        : base(number == 0 ? obj + " is not a location" : obj + " has multiple Location scripts (" + number + ")")
    {

    }
}
