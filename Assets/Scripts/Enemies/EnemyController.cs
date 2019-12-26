using UnityEngine;

public class EnemyController : MonoBehaviour
{

  public int id;

  GameObject _customTypesObject;

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

  Animator _animator;

  float _moveSpeed;

  int _waypointIndex = 0;
  bool _targetReached = false;

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
                                             waypoints[_waypointIndex].transform.position,
                                             _moveSpeed * Time.deltaTime);
    if (transform.position == waypoints[_waypointIndex].transform.position)
    {
      _waypointIndex += 1;
    }
    if (_waypointIndex == waypoints.Length)
    {
      _targetReached = true;
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
    _customTypesObject = GameObject.Find("MapController");
    waypoints = _customTypesObject.GetComponent<Map1>().GetMapNodes();
    _moveSpeed = 1f;
    currentHealth = maxHealth;
    _animator = GetComponent<Animator>();
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
    if (!_targetReached)
    {
      MoveToWaypoints();
    }
    if (this.currentHealth <= 0)
    {
      OnDeath(gameObject, returnedCurrency);
    }
  }
}
