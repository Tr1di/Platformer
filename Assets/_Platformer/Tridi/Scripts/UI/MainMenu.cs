using Tridi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Object scene;

    public void StartNewGame()
    {
        SaveManager.NewGame();
        SaveGameAndLoadScene();
    }

    public void LoadGame()
    { 
        SaveGameAndLoadScene();
    }
    
    private void SaveGameAndLoadScene() 
    {
        // save the game anytime before loading a new scene
        SaveManager.SaveGame();
        // load the scene
        SceneManager.LoadSceneAsync(scene.name);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
