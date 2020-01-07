using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class EnemyController : MonoBehaviour
{

  [System.Serializable]
  public class EffectDict : SerializableDictionaryBase<string, Func<IEnumerator, EffectParams>> { }

  EffectDict _effectList = null;

  [SerializeField]
  Rigidbody2D rigidBody = null;

  GameObject mapController = null;

  float _maxHealth = 0;

  int _returnedCurrency = 25;

  float _defaultMoveSpeed = 0f;

  float _currentMoveSpeed = 0f;

  int _waypointIndex = 0;

  bool _targetReached = false;

  [SerializeField]
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

  Transform _currentTarget = null;

  float _radius = 0.1f;

  float _slowTimer = 9f;

  float _slowDuration = 0f;

  public delegate void OnDeathEventHandler(GameObject obj, int? value);
  public event OnDeathEventHandler OnDeath;

  public delegate void OnTargetReachedEventHandler();

  public event OnTargetReachedEventHandler OnTargetReached;

  void Awake()
  {
    OnDeath += new OnDeathEventHandler(TowerManager.Instance.EnemyDied);
    // _bodyRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
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
    _slowTimer += Time.deltaTime;
    if (_slowTimer >= _slowDuration)
    {
      _currentMoveSpeed = _defaultMoveSpeed;
      _slowDuration = 0f;
      _bodyRenderer.color = Color.white;
    }
    MoveToWaypoints();
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

  // public IEnumerator SlowEffect(EffectParams effectParams)
  // {
  //   for (var i = effectParams.duration; i > 0; i++)
  //   {
  //     _currentMoveSpeed = _defaultMoveSpeed - (_defaultMoveSpeed * effectParams.value);
  //     yield return new WaitForSecondsRealtime(1f);
  //   }
  //   _currentMoveSpeed = _defaultMoveSpeed;
  //   _isSlowed = false;
  //   yield return null;
  // }

  public IEnumerator OverTimeEffect(EffectParams effectParams)
  {
    for (var i = effectParams.duration; i > 0; i++)
    {
      currentHealth -= effectParams.damage;
      yield return new WaitForSecondsRealtime(1f);
    }
  }

  public float GetMaxHealth()
  {
    return this._maxHealth;
  }
  public void TakeHit(EffectList? effect, EffectParams effectParams)
  {
    // Action<EffectParams> hit = _effectList[effect.ToString()];
    // hit(effectParams);
    if (effect == EffectList.Slow)
    {
      _bodyRenderer.color = Color.gray;
      _slowDuration = effectParams.duration;
      _slowTimer = 0f;
      currentHealth -= effectParams.damage;
      _currentMoveSpeed = _defaultMoveSpeed - (_defaultMoveSpeed * effectParams.value);
    }
    if (effect == EffectList.Damage)
    {
      currentHealth -= effectParams.damage;
    }
    if (effect == EffectList.Dot)
    {
      StartCoroutine(OverTimeEffect(effectParams));
    }
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
    if (_targetReached == false && currentHealth > 0)
    {
      _currentTarget = waypoints[_waypointIndex].transform;
      var normalized = (_currentTarget.position - transform.position).normalized;
      // Debug.Log(_currentMoveSpeed);
      rigidBody.velocity = normalized * _currentMoveSpeed;
      _direction = GetDirectionValue(_currentTarget, transform);
      if (_direction.x > 0)
      {
        GetComponent<Animator>().Play("MoveRight");
      }
      if (_direction.x <= 0)
      {
        GetComponent<Animator>().Play("MoveLeft");
      }
      var distance = Vector2.Distance(transform.localPosition, _currentTarget.position);
      if (distance <= _radius)
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
    _currentMoveSpeed = enemyInfo.moveSpeed;
    _defaultMoveSpeed = enemyInfo.moveSpeed;
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

  // void OnDrawGizmos()
  // {
  //   Gizmos.DrawSphere(_currentTarget.position, _radius);
  // }
}
