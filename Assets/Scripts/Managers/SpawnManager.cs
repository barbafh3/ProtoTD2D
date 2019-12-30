using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

  private List<GameObject> spawnedEnemies;

  [SerializeField]
  public int? remainingWaves;

  public Transform[] mapNodes;

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
    // DontDestroyOnLoad(gameObject);
    spawnedEnemies = new List<GameObject>();
  }

  public void ResetVariables()
  {
    spawnedEnemies = null;
    remainingWaves = null;
    mapNodes = null;
  }

  void OnMonsterDeath(GameObject obj, int? value)
  {
    //  Checks if there are any enemies on spawned list.
    //  If true, removes it from the list.
    if (spawnedEnemies.Count >= 1)
    {
      spawnedEnemies.RemoveAt(spawnedEnemies.Count - 1);
      if (spawnedEnemies.Count <= 0)
      {
        remainingWaves--;
      }
    }
    if (remainingWaves <= 0)
    {
      SceneLoader.LoadScene(GameScenes.GameOver);
    }
  }

  public IEnumerator SpawnRuntime(List<Wave> enemyWaves,
                                  Transform[] newMapNodes,
                                  float startupDelay,
                                  float waveDelay,
                                  float spawnDelay)
  {
    if (enemyWaves.Count == 0)
    {
      Debug.Log("No waves in wave list");
    }
    else
    {
      remainingWaves = enemyWaves.Count;
      mapNodes = newMapNodes;
      var startPosition = mapNodes[0];
      WaitForSeconds waitStart = new WaitForSeconds(startupDelay);
      yield return waitStart;
      WaitForSeconds waitWave = new WaitForSeconds(waveDelay);
      foreach (Wave wave in enemyWaves)
      {
        WaitForSeconds waitSpawn = new WaitForSeconds(spawnDelay);
        //  Adds the wave to a control list used to
        //  check if the wave is fully killed.
        spawnedEnemies.AddRange(wave.monsterPrefabList);
        foreach (GameObject monster in wave.monsterPrefabList)
        {
          //  Spawns monsters from the wave and sets
          //  the local method OnMonsterDeath as listener
          //  to the monster OnDeath event handler.
          var monsterInstance = Instantiate(monster,
                                            new Vector2(startPosition.position.x, startPosition.position.y),
                                            Quaternion.identity);
          var enemyController = monsterInstance.GetComponent<EnemyController>();
          enemyController.OnDeath += new EnemyController.OnDeathEventHandler(OnMonsterDeath);
          yield return waitSpawn;
        };
        yield return waitWave;

      }
    }
  }
}
