using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{

  public float baseDamage;
  public float travelSpeed;
  public float rotateSpeed;
  public float angle;
  public float damageOverTime;

  public ParticleSystem particle;

}
