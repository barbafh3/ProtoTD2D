using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsController : MonoBehaviour
{

  [SerializeField]
  Transform[] routeA = new Transform[0];


  [SerializeField]
  Transform[] routeB = new Transform[0];

  [SerializeField]
  List<Wave> enemyWaves = null;

  [SerializeField]
  float startupDelay = 0f;

  [SerializeField]
  float spawnDelay = 0f;

  [SerializeField]
  float waveDelay = 0f;

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
    foreach (Transform waypoint in list)
    {
      waypoint.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
  }

  void Start()
  {
    Cursor.visible = true;
    TowerManager.LoadTowerManager();
    HideWaypoints(routeA);
    HideWaypoints(routeB);
    StartCoroutine(SpawnManager.Instance.SpawnRuntime(enemyWaves, routeA, routeB, startupDelay, waveDelay, spawnDelay));
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

  public Transform[] GetMapNodes()
  {
    return routeA;
  }

}