using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

  [SerializeField]
  Tower towerInfo;

  float _range;

  float _baseDamage;

  float _fireRate;

  Tower[] _upgradeList;

  GameObject _projectileSprite;

  private GameObject _currentTarget = null;

  void FindAndUpdateTarget()
  {
    //  Retrieves a list of all objects with tag 'Enemy'.
    GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    //  Sets initial shortest distance to infinity.
    float shortestDistance = Mathf.Infinity;
    GameObject nearestEnemy = null;

    foreach (GameObject enemy in enemyList)
    {
      float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
      if (distanceToEnemy < shortestDistance)
      {
        shortestDistance = distanceToEnemy;
        nearestEnemy = enemy;
      }
    }

    if (nearestEnemy != null && shortestDistance <= _range)
    {
      _currentTarget = nearestEnemy;
    }
    else
    {
      _currentTarget = null;
    }
  }

  void SpawnProjectile()
  {
    var projectileObject = Instantiate<GameObject>(_projectileSprite, transform.position, Quaternion.identity);
    projectileObject.GetComponent<AProjectile>().target = _currentTarget;
  }

  void DoDamage()
  {
    if (_currentTarget)
    {
      var enemyController = _currentTarget.GetComponent<EnemyController>();
      SpawnProjectile();
      if (enemyController.currentHealth <= 0)
      {
        _currentTarget = null;
      }
    }
  }

  void LoadTowerInfo()
  {
    _fireRate = towerInfo.fireRate;
    _range = towerInfo.range;
    _upgradeList = towerInfo.upgradeList;
    _projectileSprite = towerInfo.projectileSprite;
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
