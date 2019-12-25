using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{

  public int id;

  GameObject customTypesObject;

  [SerializeField]
  public float currentHealth { get; set; }

  [SerializeField]
  float maxHealth;

  [SerializeField]
  int returnedCurrency = 25;

  [SerializeField]
  Transform[] waypoints;

  [SerializeField]
  ParticleSystem damageParticle;

  Animator animator;

  float moveSpeed;

  int waypointIndex = 0;
  bool targetReached = false;

  public delegate void OnDeathEventHandler(GameObject obj, int? value);
  public event OnDeathEventHandler OnDeath;

  public delegate void OnTargetReachedEventHandler();

  public event OnTargetReachedEventHandler OnTargetReached;

  public float GetMaxHealth()
  {
    return this.maxHealth;
  }
  public void TakeDamage(float damage)
  {
    this.currentHealth -= damage;
  }

  void MoveToWaypoints()
  {
    transform.position = Vector2.MoveTowards(transform.position,
                                             waypoints[waypointIndex].transform.position,
                                             moveSpeed * Time.deltaTime);
    if (transform.position == waypoints[waypointIndex].transform.position)
    {
      waypointIndex += 1;
    }
    if (waypointIndex == waypoints.Length)
    {
      targetReached = true;
      OnTargetReached();
      OnDeath(gameObject, null);
      Destroy(gameObject);
    }
  }

  void KillSelf(GameObject obj, int? value)
  {
    Destroy(gameObject);
  }

  void Awake()
  {
    customTypesObject = GameObject.Find("MapController");
    waypoints = customTypesObject.GetComponent<Map1>().GetMapNodes();
    moveSpeed = 1f;
    currentHealth = maxHealth;
    animator = GetComponent<Animator>();
  }

  // Start is called before the first frame update
  void Start()
  {
    OnDeath += new OnDeathEventHandler(KillSelf);
    OnDeath += new OnDeathEventHandler(GameManager.Instance.ReceiveCurrency);
    OnTargetReached += new OnTargetReachedEventHandler(GameManager.Instance.PlayerTakeDamage);
    // transform.position = waypoints[waypointIndex].transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    // transform.position = Vector2.MoveTowards(transform.position,
    //                                          waypoint.transform.position,
    //                                          moveSpeed * Time.deltaTime);
    if (!targetReached)
    {
      MoveToWaypoints();
    }
    if (this.currentHealth <= 0)
    {
      OnDeath(gameObject, returnedCurrency);
    }
  }
}
