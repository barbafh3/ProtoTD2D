using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
  Vector2 localScale;
  EnemyController parentEnemy;

  void Start()
  {
    localScale = transform.localScale;
    parentEnemy = GetComponentInParent<EnemyController>();
  }

  void Update()
  {
    //  Normalizes the health to %, giving min, max and current health to Mathf.InveseLerp
    localScale.x = Mathf.InverseLerp(0f, parentEnemy.GetMaxHealth(), parentEnemy.currentHealth);
    transform.localScale = localScale;
  }
}
