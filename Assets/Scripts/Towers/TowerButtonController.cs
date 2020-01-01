using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonController : MonoBehaviour
{

  [SerializeField]
  TowerButton buttonInfo;

  [SerializeField]
  GameObject towerPrefab;

  Transform rootObj;
  BuildingController rootScript;
  GameObject optionsCanvas;
  GameObject sellCanvas;

  Button button;

  int towerCost;

  void LoadButtonInfo()
  {
    towerCost = buttonInfo.towerCost;
    optionsCanvas = transform.parent.gameObject;
    sellCanvas = transform.parent.parent.Find("SellUI").gameObject;
    button = GetComponentInChildren<Button>();
    rootObj = transform.parent.parent;
    rootScript = rootObj.GetComponent<BuildingController>();
  }

  public void SellTower()
  {
    // sellCanvas.SetActive(false);
    // rootScript.isAvalable = true;
    // var towerController = rootScript.tower.GetComponentInChildren<TowerController>();
    // GameManager.Instance.ReceiveCurrency(null, towerController.refundValue);
    // TowerManager.Instance.UnregisterTower(rootScript.tower);
    // Destroy(rootScript.tower);
    // UIManager.Instance.selectedObject = null;
  }

  // public void PlaceTower()
  // {
  //   if (rootScript.isAvalable == true)
  //   {
  //     if (GameManager.Instance.currentPlayerCurrency >= towerCost)
  //     {
  //       GameManager.Instance.SpendCurrency(towerCost);
  //       GameObject newTower = Instantiate(towerPrefab, rootObj.position, Quaternion.identity);
  //       TowerManager.Instance.RegisterTower(newTower);
  //       rootScript.tower = newTower;
  //       rootScript.tower.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
  //       rootScript.isAvalable = false;
  //       optionsCanvas.SetActive(false);
  //       UIManager.Instance.selectedObject = null;
  //     }
  //     else
  //     {
  //       Debug.Log("Not enough gold.");
  //     }
  //   }
  //   else
  //   {

  // }
  // }

  void Start()
  {
    // LoadButtonInfo();
  }

  // Update is called once per frame
  void Update()
  {
    // if (GameManager.Instance.currentPlayerCurrency < towerCost)
    // {
    //   button.interactable = false;
    // }
    // else
    // {
    //   button.interactable = true;
    // }

  }
}
