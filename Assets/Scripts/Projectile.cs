using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    rock,
    arrow,
    fireball
}

public class Projectile : MonoBehaviour
{

    [SerializeField] private int attackStrenght;
    [SerializeField] private ProjectileType projectileType;


    public int AttackStrenght { get => attackStrenght; }
    public ProjectileType ProjectileType { get => projectileType; }

}
