using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

  List<GameObject> deployedTowers;

  public delegate void OnEnemyDeathEventHandler(GameObject obj);
  public event OnEnemyDeathEventHandler OnEnemyDeath;

  private static TowerManager instance;

  public static TowerManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<TowerManager>();
        if (instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(TowerManager).Name;
          instance = obj.AddComponent<TowerManager>();
        }
      }
      return instance;
    }
  }

  void OnDisable()
  {
    instance = null;
  }

  void Awake()
  {
    if (instance != this && instance != null)
    {
      Destroy(gameObject);
    }
    instance = this;
    deployedTowers = new List<GameObject>();
  }

  public void EnemyDied(GameObject enemy, int? value)
  {
    if (enemy != null)
    {
      if (OnEnemyDeath != null)
      {
        OnEnemyDeath(enemy);
      }
    }
  }

  public void RegisterTower(GameObject tower)
  {
    var towerScript = tower.GetComponentInChildren<TowerController>();
    OnEnemyDeath += new OnEnemyDeathEventHandler(towerScript.EnemyDied);
    deployedTowers.Add(tower);
  }

  public void UnregisterTower(GameObject tower)
  {
    Debug.Log(tower);
    var towerScript = tower.GetComponentInChildren<TowerController>();
    Debug.Log(towerScript);
    OnEnemyDeath -= new OnEnemyDeathEventHandler(towerScript.EnemyDied);
    deployedTowers.Remove(tower);
  }
}
