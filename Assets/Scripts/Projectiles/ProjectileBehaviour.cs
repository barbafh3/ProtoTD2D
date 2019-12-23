using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{

  [SerializeField]
  Projectile projectileInfo;

  public GameObject target;

  MonsterBehaviour monsterBehaviour;

  float baseDamage;
  float travelSpeed;
  float rotateSpeed;
  ParticleSystem particle;

  public delegate void OnHitEventHandler(float damage);
  public event OnHitEventHandler OnHit;

  // Method called on target`s death
  void OnTargetDeath(GameObject obj, int? value)
  {
    if (gameObject != null)
    {
      monsterBehaviour.OnDeath -= OnTargetDeath;
      Destroy(gameObject);
    }
  }

  void LoadProjectileInfo()
  {
    baseDamage = projectileInfo.baseDamage;
    travelSpeed = projectileInfo.travelSpeed;
    rotateSpeed = projectileInfo.rotateSpeed;
    particle = projectileInfo.particle;
  }

  void Awake()
  {
    var projectileBody = GetComponent<Rigidbody2D>();
    projectileBody.velocity = new Vector2(travelSpeed, travelSpeed);
  }

  // Start is called before the first frame update
  void Start()
  {
    LoadProjectileInfo();
    // Run OnTargetDeath() when OnDeath event triggers
    monsterBehaviour = target.GetComponent<MonsterBehaviour>();
    monsterBehaviour.OnDeath += new MonsterBehaviour.OnDeathEventHandler(OnTargetDeath);
    OnHit += new OnHitEventHandler(monsterBehaviour.TakeDamage);
  }

  // Update is called once per frame
  void Update()
  {
    if (gameObject != null && target != null)
    {
      transform.position = Vector2.MoveTowards(transform.position, target.transform.position, travelSpeed * Time.deltaTime);

      Vector3 vectorToTarget = target.transform.position - transform.position;
      float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
      Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
      transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);

      if (transform.position == target.transform.position)
      {
        // Removes itself from OnDeath event handler
        monsterBehaviour.OnDeath -= OnTargetDeath;
        // Calls OnHit event
        OnHit(baseDamage);
        // Instantiates impact particles
        Instantiate(particle, target.transform.position, Quaternion.identity);
        Destroy(gameObject);
      }

    }
  }
}
