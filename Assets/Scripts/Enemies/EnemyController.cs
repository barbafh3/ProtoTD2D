﻿using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

  GameObject mapController;

  float maxHealth;

  int returnedCurrency = 25;

  Animator animator;

  float moveSpeed;

  int waypointIndex = 0;

  bool targetReached = false;

  SpriteRenderer bodyRenderer;
  SpriteRenderer healthBarRenderer;

  [SerializeField]
  float fadeOutTime;

  Transform[] waypoints;

  public float currentHealth;

  [SerializeField]
  Enemy enemyInfo;

  public bool isDead = false;

  Vector2 direction;

  public delegate void OnDeathEventHandler(GameObject obj, int? value);
  public event OnDeathEventHandler OnDeath;

  public delegate void OnTargetReachedEventHandler();

  public event OnTargetReachedEventHandler OnTargetReached;

  void Awake()
  {
    OnDeath += new OnDeathEventHandler(TowerManager.Instance.EnemyDied);
    bodyRenderer = transform.Find("Body").GetComponent<SpriteRenderer>();
    healthBarRenderer = transform.Find("HealthBG").GetComponent<SpriteRenderer>();
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
    if (targetReached == false && currentHealth > 0)
    {
      MoveToWaypoints();
    }
    if (this.currentHealth <= 0)
    {
      if (isDead == false)
      {
        isDead = true;
        OnDeath(gameObject, returnedCurrency);
        StartCoroutine(FadeOutAndDie());
      }
    }
  }

  public float GetMaxHealth()
  {
    return this.maxHealth;
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
                                             waypoints[waypointIndex].transform.position,
                                             moveSpeed * Time.deltaTime);
    direction = GetDirectionValue(waypoints[waypointIndex].transform, transform);
    if (direction.x > 0)
    {
      GetComponent<Animator>().Play("MoveRight");
    }
    if (direction.x <= 0)
    {
      GetComponent<Animator>().Play("MoveLeft");
    }
    if (transform.position == waypoints[waypointIndex].transform.position)
    {
      waypointIndex += 1;
    }
    if (waypointIndex == waypoints.Length)
    {
      targetReached = true;
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
    maxHealth = enemyInfo.maxHealth;
    returnedCurrency = enemyInfo.returnedCurrency;
    moveSpeed = enemyInfo.moveSpeed;
    currentHealth = maxHealth;
    mapController = GameObject.Find("MapController");
    waypoints = mapController.GetComponent<Map1>().GetMapNodes();
  }

  IEnumerator PlayAnimation()
  {
    GetComponent<Animator>().Play("Death");
    yield return null;
  }
  IEnumerator FadeOutAndDie()
  {
    StartCoroutine(FadeOut(healthBarRenderer));
    yield return StartCoroutine(PlayAnimation());
    yield return new WaitForSeconds(0.5f);
    yield return StartCoroutine(FadeOut(bodyRenderer));
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
