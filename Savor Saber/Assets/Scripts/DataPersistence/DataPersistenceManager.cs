using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    public static DataPersistenceManager instance { get; private set; }
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private bool newGame;

    private void Awake()
    {
        newGame = FindObjectOfType<GameDataManager>().isNewGame;
        if (instance != null)
        {
            Debug.LogError("Error: Found more than one DataPersistenceManager in the scene");
        }
            instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //TODO: Load any saved data from a file using the data handler.
        this.gameData = dataHandler.Load();
        //If no data can be found, initialize to a new game.
        if(this.gameData == null || newGame)
        {
            if (newGame) Debug.Log("New game starting...");
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
            return;
        }
        // TODO: Push the loaded data to all other scripts that need it.'
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("Loaded test value: " + gameData.testField);
    }

    public void SaveGame()
    {
        // TODO: Pass all the data to other scripts so they can update it.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log("Saved test value: " + gameData.testField);
        // TODO: Save that data to a file using the data handler
        dataHandler.Save(gameData);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    /*
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    */
    
    
}
