using System;
using Cinemachine;
using Tridi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private Character initialCharacter;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private Character _character;
    private InputManager _inputManager;
    
    private void InitInputManager()
    {
        if (_inputManager) return;
            
        var go = new GameObject("InputManager");
        go.transform.SetParent(gameObject.transform);
        _inputManager = go.AddComponent<InputManager>();
    }

    private Character Character
    {
        get => _character;
        set
        {
            if (_character)
            {
                virtualCamera.Follow = null;
                virtualCamera.LookAt = null;
                _character.InputManager = null;
                _character.Health.onDeath -= OnCharacterDeath;
            }

            _character = value;

            if (_character)
            {
                virtualCamera.Follow = _character.transform;
                virtualCamera.LookAt = _character.transform;
                _character.InputManager = _inputManager;
                _character.Health.onDeath += OnCharacterDeath;
            }
        }
    }
    
    private void Awake()
    {
        InitInputManager();
    }

    private void Start()
    {
        SaveManager.SaveGame();
        Character = initialCharacter;
    }
    
    private void OnEnable()
    {
        Character = _character;

        _inputManager.onPause += PauseGame;

        PauseManager.onPaused += GamePaused;
        PauseManager.onUnPaused += GameUnPaused;
    }

    private void OnDisable()
    {
        Character = null;
        
        _inputManager.onPause -= PauseGame;
        
        PauseManager.onPaused -= GamePaused;
        PauseManager.onUnPaused -= GameUnPaused;
    }
    
    private void PauseGame()
    {
        PauseManager.TogglePause();
    }

    private void GamePaused()
    {
        Character.InputManager = null;
    }

    private void GameUnPaused()
    {
        Character.InputManager = _inputManager;
    }
    
    private void OnCharacterDeath(IHealth comp, DamageInfo info)
    {
        Invoke(nameof(LoadGame), 1f);
    }

    private void LoadGame()
    {
        SaveManager.LoadGame();
    }
    
#region Editor
#if UNITY_EDITOR

    [MenuItem("Platformer/Add Player Controller")]
    public static void AddPlayerController()
    {
        var go = new GameObject("Player Controller");
        var controller = go.AddComponent<PlayerController>();
        Selection.activeGameObject = go;
    }
    
    [MenuItem("Platformer/Add Player Controller", true)]
    public static bool AddPlayerController_Validate()
    {
        return !FindObjectOfType<PlayerController>();
    }
    
#endif
#endregion

}
