using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

  public void GoToMap1()
  {

    GameManager.Instance.LoadNextMap("map1");
  }

  public void GoToMainMenu()
  {

    GameManager.Instance.LoadNextMap("mainMenu");
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
