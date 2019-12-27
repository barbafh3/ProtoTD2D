using UnityEngine;

public class EnemyController : MonoBehaviour
{

  GameObject _mapController;

  float _maxHealth;

  int _returnedCurrency = 25;

  Animator _animator;

  float _moveSpeed;

  int _waypointIndex = 0;

  bool _targetReached = false;

  Transform[] waypoints;

  public float currentHealth;

  [SerializeField]
  Enemy enemyInfo;

  public delegate void OnDeathEventHandler(GameObject obj, int? value);
  public event OnDeathEventHandler OnDeath;

  public delegate void OnTargetReachedEventHandler();

  public event OnTargetReachedEventHandler OnTargetReached;

  public float GetMaxHealth()
  {
    return this._maxHealth;
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

  void RegisterEventListeners()
  {
    OnDeath += new OnDeathEventHandler(KillSelf);
    OnDeath += new OnDeathEventHandler(GameManager.Instance.ReceiveCurrency);
    OnTargetReached += new OnTargetReachedEventHandler(GameManager.Instance.PlayerTakeDamage);
  }

  void LoadEnemyInfo()
  {
    _maxHealth = enemyInfo.maxHealth;
    _returnedCurrency = enemyInfo.returnedCurrency;
    _moveSpeed = enemyInfo.moveSpeed;
    currentHealth = _maxHealth;
    _mapController = GameObject.Find("MapController");
    waypoints = _mapController.GetComponent<Map1>().GetMapNodes();
  }

  // void Awake()
  // {
  //   _animator = GetComponent<Animator>();
  // }

  // Start is called before the first frame update
  void Start()
  {
    LoadEnemyInfo();
    RegisterEventListeners();
  }

  // Update is called once per frame
  void Update()
  {
    if (_targetReached == false)
    {
      MoveToWaypoints();
    }
    if (this.currentHealth <= 0)
    {
      OnDeath(gameObject, _returnedCurrency);
    }
  }
}
