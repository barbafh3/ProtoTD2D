using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{

  [SerializeField]
  protected Projectile projectileInfo;

  public GameObject target;

  protected EnemyController _enemyController;

  protected float baseDamage;
  protected float travelSpeed;
  protected float rotateSpeed;
  protected float angle;
  protected ParticleSystem particle;
}
