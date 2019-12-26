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

  Transform _startPosition;

  List<GameObject> _spawnedMonsters;

  int _remainingWaves;

  //  When monster OnDeath event is called,
  //  removes one monster from the controle list.
  //  If there are no remaining monsters alive,
  //  remove one wave from the wave counter.
  void OnMonsterDeath(GameObject obj, int? value)
  {
    _spawnedMonsters.RemoveAt(_spawnedMonsters.Count - 1);
    if (_spawnedMonsters.Count <= 0)
    {
      _remainingWaves--;
    }
  }

  //  MapRuntime uses 3 timers, startup,
  //  delay between waves and delay between
  //  enemy spawns.
  private IEnumerator MapRuntime()
  {
    WaitForSeconds waitStart = new WaitForSeconds(startupTime);
    yield return waitStart;
    WaitForSeconds waitWave = new WaitForSeconds(waveDelay);
    foreach (Wave wave in monsterWaves)
    {
      WaitForSeconds waitSpawn = new WaitForSeconds(spawnDelay);
      //  Adds the wave to a control list used to
      //  check if the wave is fully killed.
      _spawnedMonsters.AddRange(wave.monsterPrefabList);
      foreach (GameObject monster in wave.monsterPrefabList)
      {
        //  Spawns monsters from the wave and sets
        //  the local method OnMonsterDeath as listener
        //  to the monster OnDeath event handler.
        var monsterInstance = Instantiate(monster, new Vector2(_startPosition.position.x, _startPosition.position.y), Quaternion.identity);
        monsterInstance.GetComponent<EnemyController>().OnDeath += new EnemyController.OnDeathEventHandler(OnMonsterDeath);
        yield return waitSpawn;
      };
      yield return waitWave;
    }
  }

  void Awake()
  {
    _startPosition = mapNodes[0];
    _spawnedMonsters = new List<GameObject>();
  }

  void Start()
  {
    //  Enables mouse cursor.
    Cursor.visible = true;
    //  Sets remaning waves as the number of waves
    //  on the waves list.
    _remainingWaves = monsterWaves.Count;
    //  Starts the map runtime.
    StartCoroutine(MapRuntime());
  }

  void Update()
  {
    //  If there are no remaining waves,
    //  load next map.
    if (_remainingWaves <= 0)
    {
      // GameManager.Instance.LoadNextMap("gameOver");
      SceneLoader.LoadScene(GameScenes.GameOver);
    }
  }

  public Transform[] GetMapNodes()
  {
    return mapNodes;
  }

}