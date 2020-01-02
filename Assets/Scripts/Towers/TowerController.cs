using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

  public Tower towerInfo;

  Animator animator;

  float range;

  float fireRate;

  public int refundValue { get; private set; }

  Tower[] upgradeList;

  GameObject projectileSprite;

  private GameObject currentTarget = null;

  void FindAndUpdateTarget()
  {
    //  Retrieves a list of all objects with tag 'Enemy'.
    GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    //  Sets initial shortest distance to infinity.
    float shortestDistance = Mathf.Infinity;
    GameObject nearestEnemy = null;

    foreach (GameObject enemy in enemyList)
    {
      var enemyController = enemy.GetComponent<EnemyController>();
      float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
      if (distanceToEnemy < shortestDistance && enemyController.currentHealth > 0)
      {
        shortestDistance = distanceToEnemy;
        nearestEnemy = enemy;
      }
    }

    if (nearestEnemy != null && shortestDistance <= range)
    {
      currentTarget = nearestEnemy;
    }
    else
    {
      currentTarget = null;
    }
  }

  void CheckTowerType()
  {
    if (towerInfo.name == TowerList.CannonTower.ToString())
    {
      animator.SetBool("isCannonTower", true);
      animator.SetBool("isArrowTower", false);
    }
    if (towerInfo.name == TowerList.ArrowTower.ToString())
    {
      animator.SetBool("isCannonTower", false);
      animator.SetBool("isArrowTower", true);
    }
  }

  void Start()
  {
    LoadTowerInfo();
    CheckTowerType();
    InvokeRepeating("FindAndUpdateTarget", 0.5f, 0.1f);
    InvokeRepeating("DoDamage", 0f, fireRate);
  }

  public void EnemyDied(GameObject enemy)
  {
    currentTarget = null;
  }

  void SpawnProjectile()
  {
    var projectileObject = Instantiate<GameObject>(projectileSprite, transform.position, Quaternion.identity);
    projectileObject.GetComponent<AProjectile>().target = currentTarget;
  }

  void DoDamage()
  {
    if (currentTarget)
    {
      var enemyController = currentTarget.GetComponent<EnemyController>();
      SpawnProjectile();
      if (towerInfo.name == TowerList.CannonTower.ToString())
      {
        animator.Play("CannonFire");
      }
      if (enemyController.currentHealth <= 0)
      {
        currentTarget = null;
      }
    }
  }

  void LoadTowerInfo()
  {
    fireRate = towerInfo.fireRate;
    range = towerInfo.range;
    upgradeList = towerInfo.upgradeList;
    projectileSprite = towerInfo.projectileSprite;
    refundValue = towerInfo.refundValue;
    transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = towerInfo.towerSprite;
    animator = transform.parent.GetComponentInChildren<Animator>();
  }

}
