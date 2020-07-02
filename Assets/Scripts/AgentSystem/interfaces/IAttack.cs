using UnityEngine;

public interface IAttack : IMonoBehaviour
{

    void Attack(Vector3 target);
    void HoldFire();
}
