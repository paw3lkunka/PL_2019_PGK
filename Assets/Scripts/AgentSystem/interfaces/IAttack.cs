using UnityEngine;

public interface IAttack : IMonoBehaviour
{
    float DamageBaseMultiplier { get; set; }
    void Attack(Vector3 target);
    void HoldFire();
}
