using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
  Vector2 _localScale;
  EnemyController _parentMonster;

  void Start()
  {
    _localScale = transform.localScale;
    _parentMonster = GetComponentInParent<EnemyController>();
  }

  void Update()
  {
    _localScale.x = Mathf.InverseLerp(0f, _parentMonster.GetMaxHealth(), _parentMonster.currentHealth);
    transform.localScale = _localScale;
  }
}
