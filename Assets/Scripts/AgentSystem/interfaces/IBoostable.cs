using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoostableState
{
    normal,
    boosted,
    decresed,
}

public interface IBoostable
{
    BoostableState BState { get; set; }
    bool CanBeBoosted { get; set; }
    bool CanBeDecresed { get; set; }

    bool IsBoosted { get; }
    bool IsDecresed { get; }
}
