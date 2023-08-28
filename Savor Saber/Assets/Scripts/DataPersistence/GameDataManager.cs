using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager gameDataInstance;
    public bool isNewGame = true; 

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (gameDataInstance == null)
            gameDataInstance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNewGame(bool isNew)
    {
        isNewGame = isNew;
    }
}
