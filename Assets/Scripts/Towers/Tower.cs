using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class Tower : ScriptableObject
{

  public Sprite towerSprite;

  public float fireRate;

  public float range;

  public Tower[] upgradeList;

  public int refundValue;

  public GameObject projectileSprite;

}
