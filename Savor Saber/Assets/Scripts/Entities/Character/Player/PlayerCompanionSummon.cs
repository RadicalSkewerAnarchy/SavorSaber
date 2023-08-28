using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SerializableCollections;

public class PlayerCompanionSummon : MonoBehaviour, IDataPersistence
{

    public static PlayerCompanionSummon instance;

    //references to other relevant gameobjects
    private GameObject player;
    private PlayerData somaData;
    private AudioSource sfx;
    public PlayerController somaController;

    public PlayerCompanionUIButton[] buttons;
    private GameObject companion;
    private Slider companionTimerSlider;
    public Slider companionCooldownSlider;
    public Text companionCooldownText;

    private Commander partyCommander; // each scene's party commander will find Soma and assign itself to this field via AssignCommander();
    public GameObject menuCanvas;
    private readonly string menuOpenFlag = "CompanionMenuOpen";

    public IngredientData overchargeIngredient;

    //cooldown fields
    private float ticLength = 0.25f;
    public float cooldownTime = 30; //time until the player can summon the fruitant again
    public float maxActiveTime = 15; //how long the fruitant will remain active
    private WaitForSeconds activeTimerTic; //in quarter seconds
    private WaitForSeconds cooldownTimerTic; //in quarter seconds
    private bool isOnCooldown = false;

    //Fields for keeping track of which fruitants the player can or has summoned
    private Dictionary<string, IngredientData> unlockedFruitants = new Dictionary<string, IngredientData>();

    //controls
    public Control control;
    public InputAxis axis;

    //flags
    private bool MenuOpen { get; set; } = false;
    private bool companionSummoned = false;

    //AI Director fields
    private Commander.Criteria ObjectCriteria = Commander.Criteria.None;
    private GameObject Object;
    private AIData.Protocols Verb;
    private Vector2 Location = Vector2.zero;

    //time scale fields
    private float baseFixedTimeScale;
    public float timeDilation = 0.1f;

    //sfx
    public AudioClip notReadyAudio;
    public AudioClip readyAudio;
    public AudioSource statusAudioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        sfx = GetComponent<AudioSource>();
        somaData = player.GetComponent<PlayerData>();
        cooldownTimerTic = new WaitForSeconds(ticLength);
        activeTimerTic = new WaitForSeconds(ticLength);
        baseFixedTimeScale = Time.fixedDeltaTime;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    public void LoadData(GameData data)
    {

        foreach(IngredientData fruitantData in data.unlockedFruitants)
        {
            UnlockFruitant(fruitantData);
        }
    }


    public void SaveData(ref GameData data)
    {
        
        for(int i = 0; i < data.unlockedFruitants.Length; i++)
        {
            data.unlockedFruitants[i] = null;
        }
        int j = 0; 
        foreach(var kvp in unlockedFruitants)
        {
            data.unlockedFruitants[j] = kvp.Value;
            j++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        ToggleUI();
    }
    #region Core Functionality
    public void AssignCommander(Commander c)
    {
        partyCommander = c;
    }

    //add a fruitant's ingredientdata to the list of summon-able fruitants
    public void UnlockFruitant(IngredientData data)
    {
        if (data == null) Debug.LogError("PlayerCompanionSummon Error: IngredientData is null");
        if (data.displayName == null) Debug.LogError("PlayerCompanionSummon Error: IngredientData display name is null");
        Debug.Log("Unlocking fruitant: " + data.displayName);
        unlockedFruitants.Add(data.displayName, data);

        //turn on button
        PlayerCompanionUIButton[] buttonArray = GetComponentsInChildren<PlayerCompanionUIButton>(true);
        foreach(PlayerCompanionUIButton button in buttonArray)
        {
            if(button.fruitantData.displayName == data.displayName)
            {
                //Debug.Log("Found button matching unlocked fruitant");
                GameObject buttonObj = button.gameObject;
                buttonObj.GetComponent<Button>().interactable = true;
                button.silhouette.color = Color.white;
            }
        }
    }

    public void SummonCompanion(string fruitantName)
    {
        if (isOnCooldown)
        {
            Debug.Log("Cannot summon companion while on cooldown");
            statusAudioPlayer.clip = notReadyAudio;
            statusAudioPlayer.Play();
            CloseUI();
            return;
        }

        //Debug.Log("Attempting to summon fruitant: " + fruitantName);
        companion = Instantiate(unlockedFruitants[fruitantName].monster);
        companion.transform.position = player.transform.position + new Vector3(-1.25f, 0, 0);
        companionTimerSlider = companion.GetComponent<CompanionTimerSlider>().slider;
        companionTimerSlider.gameObject.SetActive(true);
        isOnCooldown = true;
        companionCooldownText.text = "Please wait";

        StartCoroutine(Cooldown(0));
        StartCoroutine(DelayedAI(companion));
        StartCoroutine(ActiveTimer(0));

        CloseUI();
    }
    #endregion

    #region User Interface
    public void ToggleUI()
    {
        //toggle the menu
        if (InputManager.GetButtonDown(control, axis) && FlagManager.GetFlag("CanSummonCompanion") == "True")
        {
            if (MenuOpen) CloseUI();
            else OpenUI();
        }
        else if(InputManager.GetButtonDown(Control.Cancel) && MenuOpen)
        {
            CloseUI();
        }
    }

    public void CloseUI()
    {
        sfx.Play();
        foreach (PlayerCompanionUIButton button in buttons)
        {
            button.ShrinkButton();
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = baseFixedTimeScale;
        menuCanvas.SetActive(false);
        MenuOpen = false;
        FlagManager.SetFlag(menuOpenFlag, "False");
        
    }

    public void OpenUI()
    {
        Time.timeScale = timeDilation;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        menuCanvas.SetActive(true);
        MenuOpen = true;
        FlagManager.SetFlag(menuOpenFlag, "True");
        sfx.Play();
    }

    #endregion

    #region timers
    private IEnumerator DelayedAI(GameObject companion)
    {
        yield return new WaitForSeconds(0.1f);

        somaData.JoinTeam(companion, 1, true);
        FlavorInputManager newFIM = companion.GetComponent<FlavorInputManager>();
        newFIM.isCompanion = true;
        newFIM.PlaySpawnParticles();
        //newFIM.Feed(overchargeIngredient, true, somaData);
        companionSummoned = true;

        Verb = AIData.Protocols.Chase;
        ObjectCriteria = Commander.Criteria.None;
        Object = player;
        partyCommander.GroupCommand(somaData.party, Verb, ObjectCriteria, Object, Location);

        yield return null;
    }

    private IEnumerator Cooldown(float timeActive)
    {
        //Debug.Log(timeActive);
        companionCooldownSlider.value = (float)(timeActive / cooldownTime);
        if (timeActive >= cooldownTime)
        {
            isOnCooldown = false;
            //Debug.Log("Cooldown elapsed, can now summon again");
            companionCooldownText.text = "Ready to Go!";
            statusAudioPlayer.clip = readyAudio;
            statusAudioPlayer.Play();
            yield return null;
        }
        else
        {
           //Debug.Log("cooldown not elapsed, resuming loop");
            yield return cooldownTimerTic;
            yield return Cooldown(timeActive + ticLength);
        }
    }

    private IEnumerator ActiveTimer(float timeActive)
    {
        companionTimerSlider.value = (float)(1f - (timeActive / maxActiveTime));
        if(timeActive >= maxActiveTime)
        {
            //Debug.Log("Timer elapsed, companion retreating");
            if (somaController.riding)
            {
                Debug.Log("Soma is riding, do not despawn");
                somaController.despawnOnDismount = true;
                yield return null;
            }
            else
            {
                Destroy(companion);
                yield return null;
            }
        }
        else
        {
            yield return activeTimerTic;
            yield return ActiveTimer(timeActive + ticLength);
        }
    }
    #endregion
}
