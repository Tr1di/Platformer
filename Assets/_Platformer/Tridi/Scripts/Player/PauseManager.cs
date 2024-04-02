using System;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;

    private bool _isPaused;
    
    public static event Action onPaused;
    public static event Action onUnPaused;

    public static bool IsPaused => _instance._isPaused;
    
    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    public static void Pause()
    {
        _instance.Pause(true);
    }

    public static void UnPause()
    {
        _instance.Pause(false);
    }

    public static void TogglePause()
    {
        _instance.Pause(!IsPaused);
    }
    
    private void Pause(bool isPaused)
    {
        if (isPaused == _isPaused) return;

        _isPaused = isPaused;

        if (_isPaused)
        {
            Time.timeScale = 0f;
            onPaused?.Invoke();
        }
        else
        {
            Time.timeScale = 1f;
            onUnPaused?.Invoke();
        }
    }

    #region Editor
#if UNITY_EDITOR
    
    [MenuItem("Platformer/Add PauseManager")]
    public static void AddPauseManager()
    {
        var go = new GameObject("Pause Manager");
        go.AddComponent<PauseManager>();
    }

    [MenuItem("Platformer/Add PauseManager", true)]
    public static bool AddPauseManager_Validate()
    {
        return !FindObjectOfType<PauseManager>();
    }
    
#endif
    #endregion
}
