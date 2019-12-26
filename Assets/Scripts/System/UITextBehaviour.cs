using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextBehaviour : MonoBehaviour
{

  string _healthValue;
  TextMeshProUGUI _healthTextObj = null;
  string _currencyValue;
  TextMeshProUGUI _currencyTextObj = null;

  void SetInfoTexts()
  {
    // if (healthTextObj == null || currencyTextObj == null)
    // {
    //   LoadTextObjects();
    // }
    // else
    // {
    _healthTextObj.text = GameManager.Instance.currentPlayerHealth.ToString();
    _currencyTextObj.text = GameManager.Instance.currentPlayerCurrency.ToString();
    // }
  }

  void LoadTextObjects()
  {
    _healthTextObj = gameObject.transform.Find("HealthValue").GetComponent<TextMeshProUGUI>();
    _currencyTextObj = gameObject.transform.Find("CurrencyValue").GetComponent<TextMeshProUGUI>();
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
