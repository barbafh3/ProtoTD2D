using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextBehaviour : MonoBehaviour
{

  string healthValue;
  string currencyValue;

  void SetHealthText()
  {
    GameObject healthTextObj = GameObject.Find("HealthValue");
    healthValue = GameManager.Instance.currentPlayerHealth.ToString();
    healthTextObj.GetComponent<Text>().text = healthValue;
  }

  void SetCurrencyText()
  {
    GameObject currencyTextObj = GameObject.Find("CurrencyValue");
    currencyValue = GameManager.Instance.currentPlayerCurrency.ToString();
    currencyTextObj.GetComponent<Text>().text = currencyValue;
  }

  // Start is called before the first frame update
  void Start()
  {
    SetHealthText();
    SetCurrencyText();
  }

  // Update is called once per frame
  void Update()
  {
    SetHealthText();
    SetCurrencyText();
  }
}
