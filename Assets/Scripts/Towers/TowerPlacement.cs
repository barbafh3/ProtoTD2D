using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{

  [SerializeField]
  List<GameObject> towerList;

  GameObject sellCanvas;
  GameObject optionsCanvas;

  public GameObject tower { get; set; }

  public bool isSlotAvalable = true;

  void SetCanvas()
  {
    optionsCanvas = transform.Find("OptionsUI").gameObject;
    optionsCanvas.SetActive(false);
    sellCanvas = transform.Find("SellUI").gameObject;
    sellCanvas.SetActive(false);
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
    if (UIManager.Instance.selectedObject == gameObject)
    {
      // If current slot is available, opens store
      // and removes current object from selection.
      if (isSlotAvalable)
      {
        optionsCanvas.SetActive(true);
      }
      // If false, shows sell button instead.
      else
      {
        sellCanvas.SetActive(true);
      }
    }
    // If false, disables sellCanvas.
    else
    {
      optionsCanvas.SetActive(false);
      sellCanvas.SetActive(false);
    }
  }
}
