using System;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class TowerManager : MonoBehaviour
{

  [SerializeField]
  public Dictionary<string, Tower> towerList;

  List<GameObject> _deployedTowers;

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
    _deployedTowers = new List<GameObject>();
    Debug.Log("TowerManager Awaken.");
    towerList = new Dictionary<string, Tower>();
    FillTowerListDict();
  }

  public void LoadTowerManager()
  {
  }

  public Tower GetTowerInfoByName(Tower tower)
  {
    return towerList[tower.name];
  }

  public Tower GetTowerInfo(Towers towerName)
  {
    return towerList[towerName.ToString()];
  }

  void FillTowerListDict()
  {
    foreach (string towerName in Enum.GetNames(typeof(Towers)))
    {
      var scriptObj = Resources.Load<Tower>("ScriptableObjects/Towers/" + towerName);
      towerList.Add(towerName, scriptObj);
    }
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
    _deployedTowers.Add(tower);
  }

  public void UnregisterTower(GameObject tower)
  {
    var towerScript = tower.GetComponentInChildren<TowerController>();
    Debug.Log(towerScript);
    OnEnemyDeath -= new OnEnemyDeathEventHandler(towerScript.EnemyDied);
    _deployedTowers.Remove(tower);
  }
}
