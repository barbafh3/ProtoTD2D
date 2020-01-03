using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileController : AProjectile
{

  float _timer = 5f;

  [SerializeField]
  float areaOfEffect = 0f;

  [SerializeField]
  [Range(0f, 2f)]
  float radius = 0f;

  Vector3 _targetPosition;

  Rigidbody2D _projectileBody = null;

  List<GameObject> _blastTargets = null;

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
    baseDamage = projectileInfo.baseDamage;
    travelSpeed = projectileInfo.travelSpeed;
    rotateSpeed = projectileInfo.rotateSpeed;
    angle = projectileInfo.angle;
    particle = projectileInfo.particle;
  }

  void FindBlastTargets()
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaOfEffect);
    foreach (Collider2D col in colliders)
    {
      if (col != null && col.gameObject.tag == "Enemy")
      {
        var enemyController = col.gameObject.GetComponentInChildren<EnemyController>();
        if (enemyController.currentHealth > 0)
        {
          _blastTargets.Add(col.gameObject);
        }
      }
    }
  }

  void RegisterTargetListeners()
  {
    foreach (GameObject obj in _blastTargets)
    {
      if (obj != null && obj.tag == "Enemy")
      {
        OnHit += new OnHitEventHandler(obj.GetComponent<EnemyController>().TakeDamage);
      }
    }
  }

  void ClearListeners()
  {
    foreach (GameObject obj in _blastTargets)
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
    OnHit(baseDamage);
    ClearListeners();
    //  Instantiates impact particles.
    Instantiate(particle, transform.position, Quaternion.identity);
    //  Destroys self.
    Destroy(gameObject);
  }

  void MoveProjectile()
  {
    float xDistance, yDistance;

    xDistance = target.transform.position.x - transform.position.x;
    yDistance = target.transform.position.y - transform.position.y;

    float projectileAngle;

    projectileAngle = Mathf.Atan((yDistance + _timer) / xDistance);

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
    _targetPosition = target.transform.localPosition;
    _blastTargets = new List<GameObject>();
    _projectileBody = GetComponent<Rigidbody2D>();
    MoveProjectile();
  }

  void FixedUpdate()
  {
    var distance = Vector2.Distance(transform.localPosition, _targetPosition);
    if (distance < radius)
    {
      OnTargetReached();
    }
  }

  // void OnDrawGizmos()
  // {
  //   Gizmos.DrawSphere(targetPosition, radius);
  // }

}