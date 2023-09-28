using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VinhLB
{
    public class DataPersistenceManager : PersistentMonoSingleton<DataPersistenceManager>
    {
        [SerializeField]
        private string _fileName;
        [SerializeField]
        private bool _useEncryption;

        private FileDataHandler _dataHandler;
        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjectList;

        protected override void Awake()
        {
            base.Awake();

            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SceneManager_OnSceneLoaded;
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        public bool IsGameDataExist()
        {
            return _gameData != null;
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();

            if (_gameData == null)
            {
                Debug.Log("No data was found. A NewGame must be started before data can be loaded.");

                return;
            }

            foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjectList)
            {
                dataPersistenceObject.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            if (_gameData == null)
            {
                Debug.Log("No data was found. A NewGame must be started before data can be loaded.");

                return;
            }

            foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjectList)
            {
                dataPersistenceObject.SaveData(_gameData);
            }

            _dataHandler.Save(_gameData);
        }

        private List<IDataPersistence> FindAllDataPersisteceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects =
                FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        private void SceneManager_OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _dataPersistenceObjectList = FindAllDataPersisteceObjects();

            LoadGame();
        }
    }
}
