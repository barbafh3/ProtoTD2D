//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoints {
  public string waypoint;
  public Transform[] target;
}

public class MapsController : MonoBehaviour
{

  // [SerializeField]
  // Transform[] routeA;

  // [SerializeField]
  // Transform[] routeB;

  public Waypoints[] waypoitns;

  [SerializeField]
  List<Wave> enemyWaves;

  [SerializeField]
  float startupDelay;

  [SerializeField]
  float spawnDelay;

  [SerializeField]
  float waveDelay;

  IEnumerator CheckForDefeatContidions()
  {
    //  If there are no remaining waves,
    //  load next map.
    if (SpawnManager.Instance.remainingWaves == 0)
    {
      SceneLoader.LoadScene(GameScenes.GameOver);
    }
    yield return new WaitForSeconds(1);
  }

  void HideWaypoints(Transform[] list)
  {
    for(int i = 0; i < waypoitns.Length; i++) {
      for(int j = 0; j < waypoitns[i].target.Length; j++) {
        waypoitns[i].target[j].GetComponent<SpriteRenderer>().enabled = false;
      }
    }
    // foreach (Transform waypoint in list)
    // {
    //   waypoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    // }
  }


  void Start()
  {
    //  Enables mouse cursor.
    Cursor.visible = true;
    //  Starts the map runtime.
    TowerManager.LoadTowerManager();
    // HideWaypoints(routeA);
    // HideWaypoints(routeB);
    StartCoroutine(SpawnManager.Instance.SpawnRuntime(enemyWaves, waypoitns, startupDelay, waveDelay, spawnDelay));
    StartCoroutine(CheckForDefeatContidions());
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (GameManager.Instance.isGamePaused == true)
      {
        UIManager.Instance.Resume();
      }
      else
      {
        UIManager.Instance.Pause();
      }
    }
  }

  // public Transform[] GetMapNodes()
  // {
  //   return routeA;
  // }

}