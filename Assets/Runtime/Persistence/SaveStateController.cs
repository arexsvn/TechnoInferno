using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using signals;

public class SaveStateController
{
    public Signal<SaveState> saveGameSelected;
    public Signal closeButtonClicked;
    private SaveState _currentSave;
    private GameConfig _gameConfig;
    private string _gameConfigFileName = "config.txt";
    private string _saveSuffix = "_save.txt";
    private string _saveFolderPath = "saveGames/";
    private string _sceneThumbnailPath = "images/sceneThumbnails/";
    private SaveGameSelectionView _view;
    private static string SAVE_GAME_SELECTOR_PREFAB = "UI/SaveGameSelection";
    readonly LocaleManager _localeManager;
    readonly ISerializationOption _serializationOption;

    public SaveStateController(LocaleManager localeManager, ISerializationOption serializationOption)
    {
        _localeManager = localeManager;
        _serializationOption = serializationOption;

        init();
    }

    public void Show(bool show = true)
    {
        _view.show(show);

        if (show)
        {
            displaySaveGames();
        }
    }

    public void Save()
    {
        _currentSave.LastSaveTime = TimeUtils.currentTime;

        string saveFileName = Path.Combine(_saveFolderPath, _currentSave.Id + _saveSuffix);
        string currentSavePath = Path.Combine(Application.persistentDataPath, saveFileName);
        string jsonString = _serializationOption.Serialize(_currentSave);
        using (StreamWriter streamWriter = File.CreateText(currentSavePath))
        {
            streamWriter.Write(jsonString);
        }
    }

    public void CreateNewSave()
    {
        resetCurrentSave();
        _currentSave.Id = GetTotalSaves().ToString();
        _gameConfig.CurrentSaveId = _currentSave.Id;
        _gameConfig.LastUpdateTime = TimeUtils.currentTime;
        saveConfig();
    }

    public void SaveScene(string sceneId)
    {
        _currentSave.SceneId = sceneId;
        _currentSave.ThumbnailPath = Path.Combine(_sceneThumbnailPath, sceneId);
        Save();
    }

    public void SaveOutcome(string outcome)
    {
        if (!HasOutcome(outcome))
        {
            _currentSave.Outcomes.Add(outcome);
            Save();
        }
    }

    public bool LoadCurrentSave()
    {
        string configPath = Path.Combine(Application.persistentDataPath, _gameConfigFileName);
        bool newGame = true;

        if (File.Exists(configPath))
        {
            using (StreamReader streamReader = File.OpenText(configPath))
            {
                string jsonString = streamReader.ReadToEnd();
                _gameConfig = _serializationOption.Deserialize<GameConfig>(jsonString);
            }
            Load(_gameConfig.CurrentSaveId);
            newGame = !isSaveGameValid(_currentSave);
        }

        if (newGame)
        {
            createNewGameConfig();
        }
        
        return newGame;
    }

    public int GetTotalSaves()
    {
        string saveDirectoryPath = Path.Combine(Application.persistentDataPath, _saveFolderPath);

        if (string.IsNullOrEmpty(saveDirectoryPath)) { return 0; }

        string[] filePaths = Directory.GetFiles(saveDirectoryPath);

        return filePaths.Length;
    }

    public SaveState Load(string id)
    {
        string saveFileName = Path.Combine(_saveFolderPath, id + _saveSuffix);
        string currentSavePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (!File.Exists(currentSavePath))
        {
            return null;
        }

        using (StreamReader streamReader = File.OpenText(currentSavePath))
        {
            string jsonString = streamReader.ReadToEnd();
            _currentSave = validateAndFixSave(_serializationOption.Deserialize<SaveState>(jsonString));
            return _currentSave;
        }
    }

    public void Delete(string id)
    {
        if (_currentSave.Id == id)
        {
            resetCurrentSave();
        }

        string saveFileName = Path.Combine(_saveFolderPath, id + _saveSuffix);
        string savePathToDelete = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(savePathToDelete))
        {
            File.Delete(savePathToDelete);
            displaySaveGames();
        }
    }

    public bool HasOutcome(string outcome)
    {
        return _currentSave.Outcomes.Contains(outcome);
    }

    public bool CheckStat(string statName, float requirement)
    {
        return GetStat(statName) > requirement;
    }

    public float GetStat(string statName)
    {
        float statValue = 0f;

        if (_currentSave.BaseStats.ContainsKey(statName))
        {
            statValue = _currentSave.BaseStats[statName];

            if (_currentSave.UpgradedStats.ContainsKey(statName))
            {
                statValue += _currentSave.UpgradedStats[statName];
            }
        }

        return statValue;
    }

    private void createNewGameConfig()
    {
        _gameConfig = new GameConfig();
    }

    private bool isSaveGameValid(SaveState saveGame)
    {
        return saveGame != null && !string.IsNullOrEmpty(saveGame.SceneId);
    }

    private void saveConfig()
    {
        string configPath = Path.Combine(Application.persistentDataPath, _gameConfigFileName);
        string jsonString = _serializationOption.Serialize(_gameConfig);
        using (StreamWriter streamWriter = File.CreateText(configPath))
        {
            streamWriter.Write(jsonString);
        }
    }

    private void init()
    {
        _currentSave = new SaveState();
        _currentSave.Outcomes = new List<string>();
        _currentSave.BaseStats = new Dictionary<string, float>();
        _currentSave.UpgradedStats = new Dictionary<string, float>();
        _currentSave.Inventory = new Inventory();

        saveGameSelected = new Signal<SaveState>();
        closeButtonClicked = new Signal();

        GameObject prefab = (GameObject)UnityEngine.Object.Instantiate(Resources.Load(SAVE_GAME_SELECTOR_PREFAB));
        _view = prefab.GetComponent<SaveGameSelectionView>();
        _view.show(false, 0);
        _view.closeButton.onClick.AddListener(closeButtonClicked.Dispatch);

        string saveDirectoryPath = Path.Combine(Application.persistentDataPath, _saveFolderPath);
        if (!Directory.Exists(saveDirectoryPath))
        {
            Directory.CreateDirectory(saveDirectoryPath);
        }
    }

    private void displaySaveGames()
    {
        _view.clearSaveGames();

        string saveDirectoryPath = Path.Combine(Application.persistentDataPath, _saveFolderPath);
        string[] filePaths = Directory.GetFiles(saveDirectoryPath);

        foreach (string filePath in filePaths)
        {
            using (StreamReader streamReader = File.OpenText(filePath))
            {
                string jsonString = streamReader.ReadToEnd();
                SaveState saveGame = _serializationOption.Deserialize<SaveState>(jsonString);
                SaveGameEntryView entry = _view.addSaveGame(saveGame);
                entry.sceneLabel.text = _localeManager.lookup(saveGame.SceneId + "_scene_label");
                entry.saveTime.text = saveGame.LastSaveTime.ToString();
                entry.imageContainerView.loadImageResource(saveGame.ThumbnailPath);
                entry.mainButton.onClick.AddListener(() => handleSaveGameSelected(saveGame));

                if (saveGame.Id == _currentSave.Id)
                {
                    entry.deleteButton.gameObject.SetActive(false);
                    entry.saveTime.text += " (Current)";
                }
                else
                {
                    entry.deleteButton.onClick.AddListener(() => Delete(saveGame.Id));
                }
            }
        }
    }

    private void handleSaveGameSelected(SaveState saveGame)
    {
        _currentSave = saveGame;
        _gameConfig.CurrentSaveId = _currentSave.Id;
        saveConfig();
        saveGameSelected.Dispatch(saveGame);
    }

    private void resetCurrentSave()
    {
        _currentSave.Outcomes.Clear();
        _currentSave.BaseStats.Clear();
        _currentSave.UpgradedStats.Clear();
        _currentSave.Inventory?.items.Clear();
        _currentSave.SceneId = null;
        _currentSave.ThumbnailPath = null;
        _currentSave.LastSaveTime = 0;
        _currentSave.Id = null;
    }

    private SaveState validateAndFixSave(SaveState saveState)
    {
        if (saveState == null) 
        {
            saveState = new SaveState();
        }

        if (saveState.Outcomes == null)
        {
            saveState.Outcomes = new List<string>();
        }

        if (saveState.BaseStats == null)
        {
            saveState.BaseStats = new Dictionary<string, float>();
        }

        if (saveState.UpgradedStats == null)
        {
            saveState.UpgradedStats = new Dictionary<string, float>();
        }

        if (saveState.Inventory == null) 
        { 
            saveState.Inventory = new Inventory();
        }

        return saveState;
    }

    public SaveState CurrentSave
    {
        get
        {
            return _currentSave;
        }
    }
}

[Serializable]
public class SaveState
{
    public string Id;
    public double LastSaveTime;
    public string SceneId;
    public string ThumbnailPath;
    public float MusicVolume;
    public float SfxVolume;
    public List<string> Outcomes;
    public Dictionary<string, float> BaseStats;
    public Dictionary<string, float> UpgradedStats;
    public Inventory Inventory;
}

[Serializable]
public class GameConfig
{
    public double LastUpdateTime;
    public string CurrentSaveId;
}
