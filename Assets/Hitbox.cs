using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public void TakeDamage(int damage) => GetComponentInParent<Character>().TakeDamage(damage);
}
