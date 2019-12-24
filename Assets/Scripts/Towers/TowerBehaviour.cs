using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{

  [SerializeField]
  Tower tower;

  float range;

  float baseDamage;

  float fireRate;

  Tower[] upgradeList;

  GameObject projectileSprite;

  private GameObject currentTarget = null;

  void FindAndUpdateTarget()
  {
    Debug.Log("Find Start");
    GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    float shortestDistance = Mathf.Infinity;
    GameObject nearestEnemy = null;

    foreach (GameObject enemy in enemyList)
    {
      float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
      Debug.Log("Distance: " + distanceToEnemy);
      Debug.Log("Shortest: " + shortestDistance);
      if (distanceToEnemy < shortestDistance)
      {
        Debug.Log("New Shortest");
        shortestDistance = distanceToEnemy;
        nearestEnemy = enemy;
      }
    }

    Debug.Log("Shortest: " + shortestDistance + " - Range: " + range);
    if (nearestEnemy != null && shortestDistance <= range)
    {
      Debug.Log("Target Found");
      currentTarget = nearestEnemy;
    }
    else
    {
      currentTarget = null;
    }
  }

  void SpawnProjectile()
  {
    var projectileObject = Instantiate<GameObject>(projectileSprite, transform.position, Quaternion.identity);
    projectileObject.GetComponent<ProjectileBehaviour>().target = currentTarget;
  }

  void DoDamage()
  {
    if (currentTarget)
    {
      Debug.Log("Shot");
      var monsterScript = currentTarget.GetComponent<MonsterBehaviour>();
      SpawnProjectile();
      if (monsterScript.currentHealth <= 0)
      {
        currentTarget = null;
      }
    }
  }

  void LoadTowerInfo()
  {
    fireRate = tower.fireRate;
    range = tower.range;
    upgradeList = tower.upgradeList;
    projectileSprite = tower.projectileSprite;
  }

  void ShowTowerUI()
  {
    var canvas = GetComponent<Canvas>();
    canvas.enabled = !canvas.enabled;
  }

  void Start()
  {
    LoadTowerInfo();
    InvokeRepeating("FindAndUpdateTarget", 0f, 0.5f);
    InvokeRepeating("DoDamage", 0f, 1f);
  }

}
