using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

  GameObject mapController = null;

  float _maxHealth = 0;

  int _returnedCurrency = 25;

  float _moveSpeed = 0f;

  int _waypointIndex = 0;

  bool _targetReached = false;

  SpriteRenderer _bodyRenderer = null;

  SpriteRenderer _healthBarRenderer = null;

  Vector2 _direction;

  [SerializeField]
  float fadeOutTime = 0f;

  public Transform[] waypoints = null;

  public float currentHealth = 0;

  [SerializeField]
  Enemy enemyInfo = null;

  public bool isDead = false;

  public delegate void OnDeathEventHandler(GameObject obj, int? value);
  public event OnDeathEventHandler OnDeath;

  public delegate void OnTargetReachedEventHandler();

  public event OnTargetReachedEventHandler OnTargetReached;

  void Awake()
  {
    OnDeath += new OnDeathEventHandler(TowerManager.Instance.EnemyDied);
    _bodyRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
    _healthBarRenderer = transform.Find("HealthBG").GetComponent<SpriteRenderer>();
  }

  // Start is called before the first frame update
  void Start()
  {
    LoadEnemyInfo();
    RegisterEventListeners();
  }

  // Update is called once per frame
  void Update()
  {
    if (_targetReached == false && currentHealth > 0)
    {
      MoveToWaypoints();
    }
    if (this.currentHealth <= 0)
    {
      if (isDead == false)
      {
        isDead = true;
        OnDeath(gameObject, _returnedCurrency);
        StartCoroutine(FadeOutAndDie());
      }
    }
  }

  public float GetMaxHealth()
  {
    return this._maxHealth;
  }
  public void TakeDamage(float damage)
  {
    this.currentHealth -= damage;
  }

  Vector2 GetDirectionValue(Transform target, Transform location)
  {
    var heading = target.position - location.position;
    var distance = heading.magnitude;
    var direction = heading / distance;
    return direction;
  }

  void MoveToWaypoints()
  {
    transform.position = Vector2.MoveTowards(transform.position,
                                             waypoints[_waypointIndex].transform.position,
                                             _moveSpeed * Time.deltaTime);
    _direction = GetDirectionValue(waypoints[_waypointIndex].transform, transform);
    if (_direction.x > 0)
    {
      GetComponent<Animator>().Play("MoveRight");
    }
    if (_direction.x <= 0)
    {
      GetComponent<Animator>().Play("MoveLeft");
    }
    if (transform.position == waypoints[_waypointIndex].transform.position)
    {
      _waypointIndex += 1;
    }
    if (_waypointIndex == waypoints.Length)
    {
      _targetReached = true;
      OnTargetReached();
      OnDeath(gameObject, null);
      StartCoroutine(FadeOutAndDie());
    }
  }

  void RegisterEventListeners()
  {
    OnDeath += new OnDeathEventHandler(GameManager.Instance.ReceiveCurrency);
    OnTargetReached += new OnTargetReachedEventHandler(GameManager.Instance.PlayerTakeDamage);
  }

  void LoadEnemyInfo()
  {
    _maxHealth = enemyInfo.maxHealth;
    _returnedCurrency = enemyInfo.returnedCurrency;
    _moveSpeed = enemyInfo.moveSpeed;
    currentHealth = _maxHealth;
    mapController = GameObject.Find("MapController");
    // waypoints = mapController.GetComponent<MapsController>().GetMapNodes();
  }

  IEnumerator PlayAnimation()
  {
    GetComponent<Animator>().Play("Death");
    yield return null;
  }
  IEnumerator FadeOutAndDie()
  {
    StartCoroutine(FadeOut(_healthBarRenderer));
    yield return StartCoroutine(PlayAnimation());
    yield return new WaitForSeconds(0.5f);
    yield return StartCoroutine(FadeOut(_bodyRenderer));
    Destroy(gameObject);
  }

  IEnumerator FadeOut(SpriteRenderer targetRenderer)
  {
    Color color = targetRenderer.material.color;
    float startOpacity = color.a;

    // Track how many seconds we've been fading.
    float t = 0;

    while (t < fadeOutTime)
    {
      // Step the fade forward one frame.
      t += Time.deltaTime;
      // Turn the time into an interpolation factor between 0 and 1.
      float blend = Mathf.Clamp01(t / fadeOutTime);

      // Blend to the corresponding opacity between start & target.
      color.a = Mathf.Lerp(startOpacity, 0.0f, blend);

      // Apply the resulting color to the material.
      targetRenderer.material.color = color;

      // Wait one frame, and repeat.
      yield return null;
    }

  }
}
