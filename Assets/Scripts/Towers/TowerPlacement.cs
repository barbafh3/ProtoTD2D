using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{

  [SerializeField]
  List<GameObject> towerList;

  Canvas sellCanvas;
  Canvas optionsCanvas;

  GameObject tower;

  bool isSlotAvalable = true;

  public void PlaceTower()
  {
    if (GameManager.Instance.currentPlayerCurrency >= 75)
    {
      GameManager.Instance.SpendCurrency(75);
      tower = Instantiate(towerList[0], transform.position, Quaternion.identity);
      tower.GetComponent<SpriteRenderer>().sortingOrder = 5;
      isSlotAvalable = false;
      optionsCanvas.enabled = false;
    }
    else
    {
      Debug.Log("Not enough gold.");
    }
  }

  public void SellTower()
  {
    sellCanvas.enabled = false;
    isSlotAvalable = true;
    Destroy(tower);
    MouseManager.Instance.selectedObject = null;
  }

  void SetCanvas()
  {
    Canvas[] canvasList = gameObject.GetComponentsInChildren<Canvas>();
    foreach (Canvas canvas in canvasList)
    {
      Debug.Log(canvas.name);
      switch (canvas.name)
      {
        case "SellUI":
          sellCanvas = canvas;
          sellCanvas.enabled = false;
          break;
        case "OptionsUI":
          optionsCanvas = canvas;
          optionsCanvas.enabled = false;
          break;
      }
    }
  }

  void Start()
  {
    SetCanvas();
  }

  void Update()
  {
    // Checks if this object is the current selected object.
    // If true, checks if the current slot is available
    // for construction.
    if (MouseManager.Instance.selectedObject == gameObject)
    {
      // If current slot is available, opens store
      // and removes current object from selection.
      if (isSlotAvalable)
      {
        optionsCanvas.enabled = true;
        MouseManager.Instance.selectedObject = null;
      }
      // If false, shows sell button instead.
      else
      {
        sellCanvas.enabled = true;
      }
    }
    // If false, disables sellCanvas.
    else
    {
      sellCanvas.enabled = false;
    }
  }
}
