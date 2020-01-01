using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;


public class BuildingController : MonoBehaviour
{

  [System.Serializable]
  public class TowerListDict : SerializableDictionaryBase<string, Tower> { }

  [SerializeField]
  public TowerListDict towerList;

  [SerializeField]
  GameObject towerContent;

  [SerializeField]
  GameObject upgradeContent;

  [SerializeField]
  GameObject prefab;

  GameObject currentTower;

  public bool isAvailable = true;


  // void Update()
  // {
  //   // Checks if this object is the current selected object.
  //   // If true, checks if the current slot is available
  //   // for construction.
  //   if (UIManager.Instance.selectedObject == gameObject)
  //   {
  //     // If current slot is available, opens store
  //     // and removes current object from selection.
  //     if (isSlotAvalable)
  //     {
  //       upgradeController.SetActive(true);
  //     }
  //     // If false, shows sell button instead.
  //     else
  //     {
  //       towerContent.SetActive(true);
  //     }
  //   }
  //   // If false, disables sellCanvas.
  //   else
  //   {
  //     upgradeController.SetActive(false);
  //     towerContent.SetActive(false);
  //   }
  // }

  private void OnMouseDown()
  {
    if (isAvailable)
      towerContent.SetActive(!towerContent.activeSelf);
    else
      upgradeContent.SetActive(!upgradeContent.activeSelf);
  }

  public void BuyTower(Tower tower)
  {
    currentTower = Instantiate(prefab, transform.position, Quaternion.identity);
    currentTower.transform.parent = transform;
    TowerManager.Instance.RegisterTower(currentTower);
    currentTower.GetComponentInChildren<TowerController>().towerInfo = towerList[tower.name];
    GameManager.Instance.SpendCurrency(towerList[tower.name].price);
    towerContent.SetActive(false);
    isAvailable = false;
  }

  public void SellTower()
  {
    upgradeContent.SetActive(false);
    GameManager.Instance.ReceiveCurrency(null, currentTower.GetComponentInChildren<TowerController>().refundValue);
    TowerManager.Instance.UnregisterTower(currentTower);
    Destroy(currentTower);
    isAvailable = true;
  }

}