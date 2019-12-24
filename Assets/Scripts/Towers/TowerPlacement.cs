using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour, IPointerClickHandler
{

  [SerializeField]
  List<GameObject> towerList;

  Canvas canvas;

  GameObject tower;

  bool isSlotAvalable = true;

  // void OnMouseDown()
  public void OnPointerClick(PointerEventData pointer)
  {
    if (isSlotAvalable)
    {
      if (GameManager.Instance.currentPlayerCurrency >= 75)
      {
        GameManager.Instance.SpendCurrency(75);
        tower = Instantiate(towerList[0], transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = 5;
        isSlotAvalable = false;
      }
      else
      {
        Debug.Log("Not enough gold.");
      }
    }
    else
    {
      canvas.enabled = true;
    }
  }

  public void SellTower()
  {
    canvas.enabled = false;
    Destroy(tower);
    isSlotAvalable = true;
  }

  void Start()
  {
    canvas = gameObject.GetComponentInChildren<Canvas>();
    canvas.enabled = false;
  }
}
