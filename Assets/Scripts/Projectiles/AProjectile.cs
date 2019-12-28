using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProjectile : MonoBehaviour
{

  [SerializeField]
  protected Projectile projectileInfo;

  public GameObject target;

  protected EnemyController _enemyController;

  protected float _baseDamage;
  protected float _travelSpeed;
  protected float _rotateSpeed;
  protected float _angle;
  protected ParticleSystem _particle;
}
