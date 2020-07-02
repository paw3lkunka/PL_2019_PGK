﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviour : IMonoBehaviour
{
    void UpdateTarget(Vector3? target);
}
