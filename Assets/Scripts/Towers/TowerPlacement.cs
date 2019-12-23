using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{

  [SerializeField]
  List<GameObject> towerList;

  bool isSlotAvalable = true;

  void OnMouseUp()
  {
    if (isSlotAvalable)
    {
      Debug.Log(GameManager.Instance.currentPlayerCurrency);
      if (GameManager.Instance.currentPlayerCurrency >= 75)
      {
        GameManager.Instance.SpendCurrency(75);
        var tower = Instantiate(towerList[0], transform.position, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = 5;
        isSlotAvalable = false;
      }
      else
      {
        Debug.Log("Not enough gold.");
      }
    }
  }
}
