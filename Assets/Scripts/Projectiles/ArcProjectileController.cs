using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileController : AProjectile
{

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
    _angle = projectileInfo.angle;
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

  Vector2 MoveProjectile()
  {
    var dir = target.transform.position - transform.position;  // get target direction
    var h = dir.y;  // get height difference
    dir.y = 0;  // retain only the horizontal direction
    var dist = dir.magnitude;  // get horizontal distance
    var a = _angle * Mathf.Deg2Rad;  // convert angle to radians
    dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
    dist += h / Mathf.Tan(a);  // correct for small height differences
                               // calculate the velocity magnitude
    var vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
    return vel * dir.normalized;
  }

  // Start is called before the first frame update
  void Start()
  {
    LoadProjectileInfo();
    RegisterEventListeners();
    // Set projectile RigidBody2D velocity to travelSpeed.
    var projectileBody = GetComponent<Rigidbody2D>();
    projectileBody.velocity = MoveProjectile();
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