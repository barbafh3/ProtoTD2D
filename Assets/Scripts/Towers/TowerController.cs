using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

  [SerializeField]
  Transform rangeSprite = null;

  public Tower towerInfo = null;

  Animator _animator = null;

  float _range = 0f;

  float _fireRate = 0f;

  public int refundValue { get; private set; }

  Tower[] _upgradeList = null;

  GameObject _projectileSprite = null;

  GameObject _currentTarget = null;

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

    if (nearestEnemy != null && shortestDistance <= _range)
    {
      _currentTarget = nearestEnemy;
    }
    else
    {
      _currentTarget = null;
    }
  }

  public void TowerSelected()
  {

  }

  void Start()
  {
    LoadTowerInfo();
    SetRangeSprite();
    InvokeRepeating("FindAndUpdateTarget", 0.5f, 0.1f);
    InvokeRepeating("DoDamage", 0f, _fireRate);
  }

  void SetRangeSprite()
  {
    var scale = rangeSprite.localScale;
    scale.x = _range * 4f;
    scale.y = _range * 4f;
    rangeSprite.localScale = scale;
    rangeSprite.GetComponent<SpriteRenderer>().enabled = false;
  }

  public void SetRangeVisibility(bool state)
  {
    rangeSprite.GetComponent<SpriteRenderer>().enabled = state;
  }

  public void EnemyDied(GameObject enemy)
  {
    _currentTarget = null;
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
      // if (towerInfo.name == TowerList.CannonTower.ToString())
      // {
      _animator.Play("Fire");
      // }
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
    refundValue = towerInfo.refundValue;
    transform.parent.GetComponentInChildren<SpriteRenderer>().sprite = towerInfo.towerSprite;
    _animator = transform.parent.GetComponentInChildren<Animator>();
    _animator.runtimeAnimatorController = towerInfo.animator;
  }

  void OnDrawGizmos()
  {
    Gizmos.DrawWireSphere(transform.position, _range);
  }

}
