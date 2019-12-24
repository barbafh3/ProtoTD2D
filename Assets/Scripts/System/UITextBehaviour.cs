using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextBehaviour : MonoBehaviour
{

  string healthValue;
  TextMeshProUGUI healthTextObj = null;
  string currencyValue;
  TextMeshProUGUI currencyTextObj = null;

  void SetInfoTexts()
  {
    // if (healthTextObj == null || currencyTextObj == null)
    // {
    //   LoadTextObjects();
    // }
    // else
    // {
    healthTextObj.text = GameManager.Instance.currentPlayerHealth.ToString();
    currencyTextObj.text = GameManager.Instance.currentPlayerCurrency.ToString();
    // }
  }

  void LoadTextObjects()
  {
    healthTextObj = gameObject.transform.Find("HealthValue").GetComponent<TextMeshProUGUI>();
    currencyTextObj = gameObject.transform.Find("CurrencyValue").GetComponent<TextMeshProUGUI>();
  }

  // Start is called before the first frame update
  void Start()
  {
    LoadTextObjects();
    SetInfoTexts();
  }

  // Update is called once per frame
  void Update()
  {
    SetInfoTexts();
  }
}
