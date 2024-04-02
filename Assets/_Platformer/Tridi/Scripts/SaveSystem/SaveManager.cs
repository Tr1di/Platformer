using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tridi
{
    public class SaveManager : MonoBehaviour
    {
        [Header("Debugging")] [SerializeField] private bool disableDataPersistence;
        [SerializeField] private bool initializeDataIfNull;
        [SerializeField] private bool overrideSelectedProfileId;
        [SerializeField] private string testSelectedProfileId = "test";

        [Header("File Storage Config")] [SerializeField]
        private string fileName;

        [SerializeField] private bool useEncryption;

        [Header("Auto Saving Configuration")] [SerializeField]
        private bool useAutoSave;

        [SerializeField] private float autoSaveTimeSeconds = 60f;

        private SaveGame _save;
        private List<ISavable> _dataPersistenceObjects;
        private SaveFileHandler _handler;

        private string _selectedProfileId = "";

        private Coroutine _autoSaveCoroutine;

        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (disableDataPersistence)
            {
                Debug.LogWarning("Data Persistence is currently disabled!");
            }

            _handler = new SaveFileHandler(Application.persistentDataPath, fileName, useEncryption);

            InitializeSelectedProfileId();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();

            // start up the auto saving coroutine
            if (_autoSaveCoroutine != null)
            {
                StopCoroutine(_autoSaveCoroutine);
            }

            if (useAutoSave) _autoSaveCoroutine = StartCoroutine(AutoSave());
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            // update the profile to use for saving and loading
            _selectedProfileId = newProfileId;
            // load the game, which will use that profile, updating our game data accordingly
            LoadGame();
        }

        public void DeleteProfileData(string profileId)
        {
            // delete the data for this profile id
            _handler.Delete(profileId);
            // initialize the selected profile id
            InitializeSelectedProfileId();
            // reload the game so that our data matches the newly selected profile id
            LoadGame();
        }

        private void InitializeSelectedProfileId()
        {
            _selectedProfileId = _handler.GetMostRecentlyUpdatedProfileId();
            if (overrideSelectedProfileId)
            {
                _selectedProfileId = testSelectedProfileId;
                Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
            }
        }

        public static void NewGame()
        {
            Instance.Internal_NewGame();
        }

        private void Internal_NewGame()
        {
            _save = new SaveGame();
        }

        public static void LoadGame()
        {
            Instance.Internal_LoadGame();
        }

        private void Internal_LoadGame()
        {
            // return right away if data persistence is disabled
            if (disableDataPersistence)
            {
                return;
            }

            // load any saved data from a file using the data handler
            _save = _handler.Load(_selectedProfileId);

            // start a new game if the data is null and we're configured to initialize data for debugging purposes
            if (_save == null && initializeDataIfNull)
            {
                NewGame();
            }

            // if no data can be loaded, don't continue
            if (_save == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                return;
            }

            // push the loaded data to all other scripts that need it
            foreach (var dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.LoadGame(_save);
            }
        }

        public static void SaveGame()
        {
            Instance.Internal_SaveGame();
        }

        private void Internal_SaveGame()
        {
            // return right away if data persistence is disabled
            if (disableDataPersistence)
            {
                return;
            }

            // if we don't have any data to save, log a warning here
            if (_save == null)
            {
                Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
                return;
            }

            // pass the data to other scripts so they can update it
            foreach (var dataPersistenceObj in _dataPersistenceObjects)
            {
                dataPersistenceObj.SaveGame(_save);
            }

            // timestamp the data so we know when it was last saved
            _save.lastUpdated = DateTime.Now.ToBinary();

            // save that data to a file using the data handler
            _handler.Save(_save, _selectedProfileId);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISavable> FindAllDataPersistenceObjects()
        {
            // FindObjectsofType takes in an optional boolean to include inactive gameobjects
            var dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<ISavable>();

            return new List<ISavable>(dataPersistenceObjects);
        }

        public bool HasGameData()
        {
            return _save != null;
        }

        public Dictionary<string, SaveGame> GetAllProfilesGameData()
        {
            return _handler.LoadAllProfiles();
        }

        private IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoSaveTimeSeconds);
                SaveGame();
                Debug.Log("Auto Saved Game");
            }
        }
    }
}
