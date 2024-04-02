using Tridi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Object mainMenu;
    
    public void Continue()
    {
        PauseManager.UnPause();
    }

    public void SaveGame()
    {
        PauseManager.UnPause();
        SaveManager.SaveGame();
    }
    
    public void LoadGame()
    {
        PauseManager.UnPause();
        SaveManager.LoadGame();
    }
    
    public void ReturnToMainMenu()
    {
        SaveManager.SaveGame();
        SceneManager.LoadSceneAsync(mainMenu.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
