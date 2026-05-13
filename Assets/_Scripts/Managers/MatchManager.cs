using DG.Tweening;
  using UnityEngine;
  using UnityEngine.SceneManagement;

  public class MatchManager : MonoBehaviour
  {
      public void EndSession()
      {
          DOTween.KillAll();

          if (SceneController.Instance == null)
          {
              Debug.LogWarning("MatchManager.EndSession: SceneController not found. Falling back to direct scene load.");
              SceneManager.LoadScene(SceneDatabase.Scenes.MainMenu.ToString());
              return;
          }

          SceneController.Instance
              .NewTransition()
              .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu, setActive: true)
              .Unload(SceneDatabase.Slots.Session)
              .Unload(SceneDatabase.Slots.SessionContent)
              .WithClearUnusedAssets()
              .WithOverlay()
              .Perform();
      }

      public void SwitchToDogSelector()
      {

      }
  }