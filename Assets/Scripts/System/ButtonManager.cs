using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

  public void GoToMap1()
  {
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void GoToMainMenu()
  {
    SceneLoader.LoadScene(GameScenes.MainMenu);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
