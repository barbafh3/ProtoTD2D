using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

  GameObject _buildingSlot = null;

  void Awake()
  {
    _buildingSlot = transform.parent.parent.parent.parent.gameObject;
  }

  void OnMouseEnter()
  {
    UIManager.Instance.ShowInfoPanel();
  }

  void OnMouseExit()
  {
    UIManager.Instance.HideInfoPanel();
  }

}
