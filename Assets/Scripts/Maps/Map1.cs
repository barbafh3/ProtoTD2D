using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1 : MonoBehaviour
{

  [SerializeField]
  Transform[] mapNodes;

  [SerializeField]
  List<Wave> monsterWaves;

  [SerializeField]
  float startupTime;

  [SerializeField]
  float spawnDelay;

  [SerializeField]
  float waveDelay;

  Transform startPosition;

  List<GameObject> spawnedMonsters;

  int remainingWaves;

  void OpenMenuUI()
  {

  }

  void OnMonsterDeath(GameObject obj, int? value)
  {
    // print(spawnedMonsters.Contains(obj));
    spawnedMonsters.RemoveAt(spawnedMonsters.Count - 1);
    if (spawnedMonsters.Count <= 0)
    {
      remainingWaves--;
    }
  }

  private IEnumerator MapRuntime()
  {
    WaitForSeconds waitStart = new WaitForSeconds(startupTime);
    yield return waitStart;
    WaitForSeconds waitWave = new WaitForSeconds(waveDelay);
    foreach (Wave wave in monsterWaves)
    {
      WaitForSeconds waitSpawn = new WaitForSeconds(spawnDelay);
      spawnedMonsters.AddRange(wave.monsterPrefabList);
      foreach (GameObject monster in wave.monsterPrefabList)
      {
        var monsterInstance = Instantiate(monster, new Vector2(startPosition.position.x, startPosition.position.y), Quaternion.identity);
        monsterInstance.GetComponent<MonsterBehaviour>().OnDeath += new MonsterBehaviour.OnDeathEventHandler(OnMonsterDeath);
        yield return waitSpawn;
      };
      // monsterWaves.Remove(wave);
      yield return waitWave;
    }
  }

  void Awake()
  {
    startPosition = mapNodes[0];
    spawnedMonsters = new List<GameObject>();
  }

  void Start()
  {
    Cursor.visible = true;
    remainingWaves = monsterWaves.Count;
    StartCoroutine(MapRuntime());
  }

  void Update()
  {
    Debug.Log("Waves remaining: " + remainingWaves);
    if (remainingWaves <= 0)
    {
      GameManager.Instance.LoadNextMap("gameOver");
    }
  }

  public Transform[] GetMapNodes()
  {
    return mapNodes;
  }

}