using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map1 : MonoBehaviour
{

  [SerializeField]
  Transform[] mapNodes;

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

  void Start()
  {
    //  Enables mouse cursor.
    Cursor.visible = true;
    //  Starts the map runtime.
    StartCoroutine(SpawnManager.Instance.SpawnRuntime(enemyWaves, mapNodes, startupDelay, waveDelay, spawnDelay));
    StartCoroutine(CheckForDefeatContidions());
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (GameManager.Instance.isGamePaused == true)
      {
        GameManager.Instance.Resume();
      }
      else
      {
        GameManager.Instance.Pause();
      }
    }
  }

  public Transform[] GetMapNodes()
  {
    return mapNodes;
  }

}