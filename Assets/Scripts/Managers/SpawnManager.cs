using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{

  List<GameObject> _spawnedEnemies = null;

  [SerializeField]
  public int? remainingWaves = null;

  public Transform[] mapNodes = null;

  float _waveDelay = 0f;

  float _spawnDelay = 0f;

  bool _waveEnded = false;

  List<Wave> _enemyWaves = null;

  Transform[] _routeA = null;
  Transform[] _routeB = null;

  [SerializeField]
  float victoryTimer = 5f;

  private static SpawnManager instance;

  public static SpawnManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = FindObjectOfType<SpawnManager>();
        if (instance == null)
        {
          GameObject obj = new GameObject();
          obj.name = typeof(SpawnManager).Name;
          instance = obj.AddComponent<SpawnManager>();
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
    _spawnedEnemies = new List<GameObject>();
  }

  void Update()
  {
    if (UIManager.Instance.timer <= 0)
    {
      _waveEnded = false;
    }
  }

  public void ResetVariables()
  {
    _spawnedEnemies = null;
    remainingWaves = null;
    mapNodes = null;
  }

  void LoadMap1()
  {
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  void LoadMap2()
  {
    SceneLoader.LoadScene(GameScenes.Map2);
  }

  void ShowVictory()
  {
    UIManager.Instance.ToggleVictoryPanel(true);
  }

  void HideVictory()
  {
    UIManager.Instance.ToggleVictoryPanel(false);
  }

  void LoadGameOver()
  {
    SceneLoader.LoadScene(GameScenes.GameOver);
  }

  void WaveEnded()
  {
    _waveEnded = true;
  }

  void OnMonsterDeath(GameObject obj, int? value)
  {
    if (_spawnedEnemies.Count >= 1)
    {
      _spawnedEnemies.RemoveAt(_spawnedEnemies.Count - 1);
      if (_spawnedEnemies.Count <= 0)
      {
        remainingWaves--;
        if (_enemyWaves.Count > 0)
        {
          UIManager.Instance.ToggleTimer(true, _waveDelay, _enemyWaves[0]);
        }
        else
        {
          EndRound();
        }
      }
    }
    if (remainingWaves <= 0)
    {
      EndRound();
    }
  }

  private void EndRound()
  {
    ShowVictory();
    GameManager.Instance.gameEnded = true;
    if (SceneManager.GetActiveScene().name == "Map1")
    {
      Invoke("LoadMap2", victoryTimer);
    }
    else
    {
      Invoke("LoadGameOver", victoryTimer);
    }
  }

  Transform[] PickRoute(Transform[] a, Transform[] b)
  {
    if (b.Length != 0)
    {
      int random = Random.Range(0, 2);
      switch (random)
      {
        case 0:
          return a;
        case 1:
          return b;
        default:
          return a;
      }
    }
    else
    {
      return a;
    }
  }

  public IEnumerator SpawnRuntime(List<Wave> enemyWaves,
                                  Transform[] route1,
                                  Transform[] route2,
                                  float startupDelay,
                                  float waveDelay,
                                  float spawnDelay)
  {
    remainingWaves = enemyWaves.Count;
    yield return new WaitForSeconds(startupDelay);
    _enemyWaves = enemyWaves;
    _waveDelay = waveDelay;
    _spawnDelay = spawnDelay;
    _routeA = route1;
    _routeB = route2;
    StartCoroutine(SpawnWave(_enemyWaves[0]));
    yield return null;
  }

  public IEnumerator SpawnWave(Wave wave)
  {
    _spawnedEnemies.AddRange(wave.monsterPrefabList);
    foreach (GameObject monster in wave.monsterPrefabList)
    {
      mapNodes = PickRoute(_routeA, _routeB);
      var startPosition = mapNodes[0];
      var monsterInstance = Instantiate(monster,
                                        new Vector2(startPosition.position.x, startPosition.position.y),
                                        Quaternion.identity);
      var enemyController = monsterInstance.GetComponent<EnemyController>();
      enemyController.OnDeath += new EnemyController.OnDeathEventHandler(OnMonsterDeath);
      enemyController.waypoints = mapNodes;
      yield return new WaitForSeconds(_spawnDelay);
    };
    _enemyWaves.Remove(wave);
  }

  // public IEnumerator SpawnRuntime(List<Wave> enemyWaves,
  //                                 Transform[] route1,
  //                                 Transform[] route2,
  //                                 float startupDelay,
  //                                 float waveDelay,
  //                                 float spawnDelay)
  // {
  //   if (enemyWaves.Count == 0)
  //   {
  //     Debug.Log("No waves in wave list");
  //   }
  //   else
  //   {
  //     _waveDelay = waveDelay;
  //     remainingWaves = enemyWaves.Count;
  //     yield return new WaitForSeconds(startupDelay);
  //     foreach (Wave wave in enemyWaves)
  //     {
  //       _waveEnded = false;
  //       _spawnedEnemies.AddRange(wave.monsterPrefabList);
  //       foreach (GameObject monster in wave.monsterPrefabList)
  //       {
  //         mapNodes = PickRoute(route1, route2);
  //         var startPosition = mapNodes[0];
  //         var monsterInstance = Instantiate(monster,
  //                                           new Vector2(startPosition.position.x, startPosition.position.y),
  //                                           Quaternion.identity);
  //         var enemyController = monsterInstance.GetComponent<EnemyController>();
  //         enemyController.OnDeath += new EnemyController.OnDeathEventHandler(OnMonsterDeath);
  //         enemyController.waypoints = mapNodes;
  //         yield return new WaitForSeconds(spawnDelay);
  //       };
  //       yield return new WaitUntil(() => _waveEnded);
  //       // while (!_waveEnded)
  //       // { }
  //       // _waveEnded = false;
  //     }
  //   }
  // }

}
