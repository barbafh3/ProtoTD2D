using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class EnemyController : MonoBehaviour
{

  [System.Serializable]
  public class EffectDict : SerializableDictionaryBase<string, Action<EffectParams>> { }

  EffectDict _effectList = null;

  [SerializeField]
  Rigidbody2D rigidBody;

  GameObject mapController = null;

  float _maxHealth = 0;

  int _returnedCurrency = 25;

  float _defaultMoveSpeed = 0f;

  float _currentMoveSpeed = 0f;

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

  Transform _currentTarget = null;

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
    LoadEffects();
  }

  // Update is called once per frame
  void Update()
  {
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

  void LoadEffects()
  {
    _effectList = new EffectDict();
    _effectList.Add("Slow", (EffectParams effectParams) =>
    {
      currentHealth -= effectParams.damage;
      for (var i = effectParams.duration; i >= 0; i--)
      {
        if (i == effectParams.duration)
        {
          _currentMoveSpeed -= _defaultMoveSpeed * effectParams.value;
          MoveToWaypoints();
        }
        if (i == 0)
        {
          _currentMoveSpeed = _defaultMoveSpeed;
          MoveToWaypoints();
        }
      }
    });
    _effectList.Add("Dot", (EffectParams effectParams) =>
    {
      for (var i = effectParams.duration; i > 0; i--)
      {
        currentHealth -= effectParams.damage;
      }
    });
    _effectList.Add("Damage", (EffectParams effectParams) =>
    {
      currentHealth -= effectParams.damage;
    });
  }

  public float GetMaxHealth()
  {
    return this._maxHealth;
  }
  public void TakeHit(EffectList? effect, EffectParams effectParams)
  {
    Action<EffectParams> hit = _effectList[effect.ToString()];
    hit(effectParams);
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
      // transform.position = Vector2.MoveTowards(transform.position,
      //                                          waypoints[_waypointIndex].transform.position,
      //                                          _currentMoveSpeed * Time.deltaTime);
      var step = normalized * _currentMoveSpeed * Time.deltaTime;
      transform.position += step;
      _direction = GetDirectionValue(_currentTarget, transform);
      if (_direction.x > 0)
      {
        GetComponent<Animator>().Play("MoveRight");
      }
      if (_direction.x <= 0)
      {
        GetComponent<Animator>().Play("MoveLeft");
      }
      if (transform.position == _currentTarget.position)
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
}
