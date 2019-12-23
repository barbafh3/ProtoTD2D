using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Tower")]
public class Tower : ScriptableObject
{

  public float fireRate;

  public Tower[] upgradeList;

  public GameObject projectileSprite;

}
