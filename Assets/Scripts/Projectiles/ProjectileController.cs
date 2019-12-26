﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

  [SerializeField]
  Projectile projectileInfo;

  public GameObject target;

  EnemyController _enemyController;

  float _baseDamage;
  float _travelSpeed;
  float _rotateSpeed;
  ParticleSystem _particle;

  public delegate void OnHitEventHandler(float damage);
  public event OnHitEventHandler OnHit;

  // Method called on target`s death
  void OnTargetDeath(GameObject obj, int? value)
  {
    if (gameObject != null)
    {
      _enemyController.OnDeath -= OnTargetDeath;
      Destroy(gameObject);
    }
  }

  void LoadProjectileInfo()
  {
    _baseDamage = projectileInfo.baseDamage;
    _travelSpeed = projectileInfo.travelSpeed;
    _rotateSpeed = projectileInfo.rotateSpeed;
    _particle = projectileInfo.particle;
  }

  void RegisterEventListeners()
  {
    // Set enemy script instance to local variable.
    _enemyController = target.GetComponent<EnemyController>();
    //  Register local method OnTargetDeath to enemy OnDeath event handler.
    _enemyController.OnDeath += new EnemyController.OnDeathEventHandler(OnTargetDeath);
    //  Registers enemy method TakeDamage to local OnHit event handler.
    OnHit += new OnHitEventHandler(_enemyController.TakeDamage);
  }

  void OnTargetReached()
  {
    //  Removes itself from enemy OnDeath event handler.
    _enemyController.OnDeath -= OnTargetDeath;
    //  Calls OnHit event with the damage value to be taken.
    OnHit(_baseDamage);
    //  Instantiates impact particles.
    Instantiate(_particle, target.transform.position, Quaternion.identity);
    //  Destroys self.
    Destroy(gameObject);
  }

  void MoveProjectile()
  {
    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, _travelSpeed * Time.deltaTime);

    Vector3 vectorToTarget = target.transform.position - transform.position;
    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
    transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotateSpeed);

    if (transform.position == target.transform.position)
    {
      OnTargetReached();
    }

  }

  void Awake()
  {
    // Set projectile RigidBody2D velocity to travelSpeed.
    var projectileBody = GetComponent<Rigidbody2D>();
    projectileBody.velocity = new Vector2(_travelSpeed, _travelSpeed);
  }

  // Start is called before the first frame update
  void Start()
  {
    LoadProjectileInfo();
    RegisterEventListeners();
  }

  // Update is called once per frame
  void Update()
  {
    if (gameObject != null && target != null)
    {
      MoveProjectile();
    }
  }
}
