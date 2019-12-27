using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
  Vector2 _localScale;
  EnemyController _parentEnemy;

  void Start()
  {
    _localScale = transform.localScale;
    _parentEnemy = GetComponentInParent<EnemyController>();
  }

  void Update()
  {
    //  Normalizes the health to %, giving min, max and current health to Mathf.InveseLerp
    _localScale.x = Mathf.InverseLerp(0f, _parentEnemy.GetMaxHealth(), _parentEnemy.currentHealth);
    transform.localScale = _localScale;
  }
}
