using System;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class TowerManager : MonoBehaviour
{

  [System.Serializable]
  public class TowerListDict : SerializableDictionaryBase<string, Tower> { }

  [SerializeField]
  public static TowerListDict towerList;

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
    towerList = new TowerListDict();
    FillTowerListDict();
  }

  public static void LoadTowerManager()
  {
    Debug.Log("TowerManager Loaded.");
  }

  public static Tower GetTowerInfo(TowerList towerName)
  {
    return towerList[towerName.ToString()];
  }

  void FillTowerListDict()
  {
    foreach (string towerName in Enum.GetNames(typeof(TowerList)))
    {
      var scriptObj = Resources.Load<Tower>("ScriptableObjects/" + towerName);
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
