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
        private bool _useEncryption;

        private GameData _gameData;
        private List<IDataPersistence> _dataPersistenceObjectList;

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

        public bool HasGameData()
        {
            return _gameData != null;
        }

        public void NewGame()
        {
            _gameData = new GameData();
        }

        public void LoadGame()
        {
            _gameData = FileDataHandler.Load(GameConstants.DATA_FILE_NAME, _useEncryption);

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
                Debug.Log("No data was made. A NewGame must be started before data can be saved.");

                return;
            }

            foreach (IDataPersistence dataPersistenceObject in _dataPersistenceObjectList)
            {
                dataPersistenceObject.SaveData(_gameData);
            }

            FileDataHandler.Save(_gameData, GameConstants.DATA_FILE_NAME, _useEncryption);
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
