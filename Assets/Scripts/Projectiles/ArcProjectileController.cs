using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileController : AProjectile
{

  protected float _timer = 5f;

  [SerializeField]
  protected float areaOfEffect = 0f;

  [SerializeField]
  [Range(0f, 2f)]
  protected float radius = 0f;

  protected Vector3 _targetPosition;

  protected Rigidbody2D _projectileBody = null;

  protected List<GameObject> _blastTargets = null;

  protected float _duration = 0f;
  protected float _effectValue = 0f;

  public delegate void OnHitEventHandler(Effects? effect, EffectParams effectParams);
  public event OnHitEventHandler OnHit;

  // Method called on target`s death
  protected void OnTargetDeath(GameObject obj, int? value)
  {
    if (gameObject != null)
    {
      _enemyController.OnDeath -= OnTargetDeath;
      Destroy(gameObject);
    }
  }

  protected void LoadProjectileInfo()
  {
    baseDamage = projectileInfo.baseDamage;
    travelSpeed = projectileInfo.travelSpeed;
    rotateSpeed = projectileInfo.rotateSpeed;
    angle = projectileInfo.angle;
    particle = projectileInfo.particle;
    _duration = projectileInfo.effectDuration;
    _effectValue = projectileInfo.effectValue;
  }

  protected void FindBlastTargets()
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

  protected void RegisterTargetListeners()
  {
    foreach (GameObject obj in _blastTargets)
    {
      if (obj != null && obj.tag == "Enemy")
      {
        var objController = obj.GetComponent<EnemyController>();
        OnHit += new OnHitEventHandler(objController.TakeHit);
        objController.OnDeath += new EnemyController.OnDeathEventHandler(TargetMissing);

      }
    }
  }

  protected void ClearListeners()
  {
    foreach (GameObject obj in _blastTargets)
    {
      if (obj != null && obj.tag == "Enemy")
      {
        var objController = obj.GetComponent<EnemyController>();
        OnHit -= new OnHitEventHandler(objController.TakeHit);
        objController.OnDeath -= new EnemyController.OnDeathEventHandler(TargetMissing);
      }
    }
  }

  void TargetMissing(GameObject obj, int? value)
  {
    OnHit -= new OnHitEventHandler(obj.GetComponent<EnemyController>().TakeHit);
  }

  protected void OnTargetReached(Effects effect, EffectParams effectParams)
  {
    FindBlastTargets();
    RegisterTargetListeners();
    OnHit(effect, effectParams);
    ClearListeners();
    Instantiate(particle, transform.position, Quaternion.identity);
    Destroy(gameObject);
  }

  protected void MoveProjectile()
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

  // void FixedUpdate()
  // {
  //   var distance = Vector2.Distance(transform.localPosition, _targetPosition);
  //   if (distance < radius)
  //   {
  //     OnTargetReached();
  //   }
  // }

  void OnDrawGizmos()
  {
    Gizmos.DrawSphere(_targetPosition, radius);
  }

}