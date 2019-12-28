using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileController : AProjectile
{

  [SerializeField]
  float _timer;

  [SerializeField]
  float areaOfEffect;

  Rigidbody2D _projectileBody;

  List<GameObject> blastTargets;

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

  void FindBlastTargets()
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaOfEffect);
    foreach (Collider2D col in colliders)
    {
      if (col != null)
      {
        blastTargets.Add(col.gameObject);
      }
    }
  }

  void RegisterTargetListeners()
  {
    foreach (GameObject obj in blastTargets)
    {
      if (obj != null && obj.tag == "Enemy")
      {
        OnHit += new OnHitEventHandler(obj.GetComponent<EnemyController>().TakeDamage);
      }
    }
  }

  void ClearListeners()
  {
    foreach (GameObject obj in blastTargets)
    {
      if (obj != null && obj.tag == "Enemy")
      {
        OnHit -= new OnHitEventHandler(obj.GetComponent<EnemyController>().TakeDamage);
      }
    }
  }

  void OnTargetReached()
  {
    FindBlastTargets();
    RegisterTargetListeners();
    //  Calls OnHit event with the damage value to be taken.
    OnHit(_baseDamage);
    ClearListeners();
    //  Instantiates impact particles.
    Instantiate(_particle, transform.position, Quaternion.identity);
    //  Destroys self.
    Destroy(gameObject);
  }

  void MoveProjectile()
  {
    float xDistance, yDistance;

    xDistance = target.transform.position.x - transform.position.x;
    yDistance = target.transform.position.y - transform.position.y;

    float projectileAngle;

    projectileAngle = Mathf.Atan((yDistance + 4f) / xDistance);

    float totalVelocity = xDistance / Mathf.Cos(projectileAngle);

    float xVel, yVel;

    xVel = totalVelocity * Mathf.Cos(projectileAngle);
    yVel = totalVelocity * Mathf.Sin(projectileAngle);

    _projectileBody.velocity = new Vector2(xVel, yVel);

  }

  // Start is called before the first frame update
  void Start()
  {
    LoadProjectileInfo();
    blastTargets = new List<GameObject>();
    _projectileBody = GetComponent<Rigidbody2D>();
    MoveProjectile();
    // Set projectile RigidBody2D velocity to travelSpeed.
    Invoke("OnTargetReached", _timer);
  }

  void Update()
  {
    if (transform.position == target.transform.position)
    {
      OnTargetReached();
    }
  }

}