using UnityEngine;

public class CoreManager : MonoBehaviour
{
    void Start()
    {
        // Core Setup for the game
        // Load everything like AudioManagers, Save System, ...
        SceneController.Instance
            .NewTransition()
            .Load(SceneDatabase.Slots.Menu, SceneDatabase.Scenes.MainMenu)
            .Perform();
    }
}
