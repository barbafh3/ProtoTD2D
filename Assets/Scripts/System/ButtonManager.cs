using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

  public void GoToMap1()
  {
    if (GameManager.Instance.isGamePaused)
    {
      UIManager.Instance.Resume();
    }
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void ReplayMap1()
  {
    SceneLoader.LoadScene(GameScenes.Map1);
  }

  public void GoToMainMenu()
  {
    if (GameManager.Instance.isGamePaused)
    {
      UIManager.Instance.Resume();
    }
    SceneLoader.LoadScene(GameScenes.MainMenu);
  }

  public void QuitGame()
  {
    Application.Quit();
  }

  public void ResumeGame()
  {
    if (GameManager.Instance.isGamePaused)
    {
      UIManager.Instance.Resume();
    }
  }
}
