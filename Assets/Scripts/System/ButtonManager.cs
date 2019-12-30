using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

  public void GoToMap1()
  {
    GameManager.Instance.Resume();
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void GoToMainMenu()
  {
    GameManager.Instance.Resume();
    SceneLoader.LoadScene(GameScenes.MainMenu);
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void ResumeGame()
  {
    GameManager.Instance.Resume();
  }
}
