using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ParkourNews.Scripts
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        private string _filePath;
        private bool _newGame = true;
        private int _stars;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            //Debug.Log("Starting DataManager");
            // default values
            gameData.musicEnabled = true;
            gameData.sfxEnabled = true;
            gameData.musicVolume = 0.05f;
            gameData.sfxVolume = 0.06f;
            
            _filePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            if (File.Exists(_filePath))
            {
                Load();
                //Debug.Log("Printing volume music" + gameData.musicVolume + " " + gameData.musicEnabled);
                _newGame = false;
            }
            else
            {
                FileStream oFileStream = null;
                oFileStream = new FileStream(_filePath, FileMode.Create);
                oFileStream.Close();

                gameData.lastLevelUnlocked = 1;
                gameData.playerResults = new List<Vector2>();
                gameData.playerResults.Add(new Vector2(1,0));
                Save();
            }

            EventManager.StartListening("Save", Save);
            EventManager.StartListening("Load", Load);
            
        }

        public bool IsNewGame()
        {
            bool b = _newGame;
            _newGame = false;
            return b;
        }

        private void Save()
        {
            File.WriteAllText(_filePath, JsonUtility.ToJson(gameData));
        }

        private void Load()
        {
            gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(_filePath));
        }

        public int getStars()
        {
            return _stars;
        }
        public GameData GetData()
        {
            return gameData;
        }

        public void SetData(int cLevel, float plPoints)
        {
            Debug.Log("clevel= " + cLevel + "points= " + plPoints);
            gameData.lastLevelUnlocked = Math.Max(cLevel + 1, gameData.lastLevelUnlocked);

            

            switch (plPoints)
            {
                //to assign stars

                case >= 1:
                    _stars = 3;
                    break;
                case >= 0.66f:
                    _stars = 2;
                    break;
                case >= 0.33f:
                    _stars = 1;
                    break;
                default:
                    _stars = 0;
                    break;
            }

            Debug.Log("stars= " + _stars);

            if (gameData.playerResults.Count < cLevel)
                gameData.playerResults.Add(new Vector2(cLevel, _stars));
            else if (_stars > gameData.playerResults[cLevel - 1].y)
                gameData.playerResults[cLevel - 1] = new Vector2(cLevel, _stars);
        }

        public double getLastUnlockedLevel()
        {
            Load();
            return gameData.lastLevelUnlocked;
        }

        public List<Vector2> getResults()
        {
            return gameData.playerResults;
        }

        public float GetMusicVolume()
        {
            return gameData.musicVolume;
        }

        public void SetMusicVolume(float volume)
        {
            gameData.musicVolume = volume;
            Save();
        }

        public float GetSfxVolume()
        {
            
            return gameData.sfxVolume;
            
        }

        public void SetSfxVolume(float volume)
        {
            gameData.sfxVolume = volume;
            Save();
        }

        public bool GetMusicEnabled()
        {
            return gameData.musicEnabled;
        }

        public void SetMusicEnabled(bool musicEnabled)
        {
            gameData.musicEnabled = musicEnabled;
            Save();
        }

        public bool GetSfxEnabled()
        {
            return gameData.sfxEnabled;
        }

        public void SetSfxEnabled(bool sfxEnabled)
        {
            gameData.sfxEnabled = sfxEnabled;
            Save();
        }
    }
}