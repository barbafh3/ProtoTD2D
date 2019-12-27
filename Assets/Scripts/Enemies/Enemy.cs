using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
  public float maxHealth;
  public int returnedCurrency;
  public float moveSpeed;
}
