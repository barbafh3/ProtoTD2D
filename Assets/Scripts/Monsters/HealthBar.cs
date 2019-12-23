using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
  Vector2 localScale;
  MonsterBehaviour parentMonster;

  void Start()
  {
    localScale = transform.localScale;
    parentMonster = GetComponentInParent<MonsterBehaviour>();
  }

  void Update()
  {
    localScale.x = Mathf.InverseLerp(0f, parentMonster.GetMaxHealth(), parentMonster.currentHealth);
    transform.localScale = localScale;
  }
}
