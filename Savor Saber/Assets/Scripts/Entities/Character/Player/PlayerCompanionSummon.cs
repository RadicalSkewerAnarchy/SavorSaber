using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompanionSummon : MonoBehaviour
{

    //references to other relevant gameobjects
    private GameObject player;
    private PlayerData somaData;
    public GameObject menuCanvas;
    private readonly string menuOpenFlag = "CompanionMenuOpen";
    private Commander partyCommander; // each scene's party commander will find Soma and assign itself to this field via AssignCommander();
    public IngredientData overchargeIngredient;

    //cooldown fields
    public float cooldownTime = 30; //time until the player can summon the fruitant again
    private WaitForSeconds CooldownTimer;
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

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject;
        somaData = player.GetComponent<PlayerData>();
        CooldownTimer = new WaitForSeconds(cooldownTime);
        baseFixedTimeScale = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        ToggleUI();
    }

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
    }

    public void SummonCompanion(string fruitantName)
    {
        if (isOnCooldown)
        {
            Debug.Log("Cannot summon companion while on cooldown");
            CloseUI();
            return;
        }
        Debug.Log("Attempting to summon fruitant: " + fruitantName);
        GameObject companion = Instantiate(unlockedFruitants[fruitantName].monster);
        companion.transform.position = player.transform.position + new Vector3(-1.25f, 0, 0);

        StopAllCoroutines();
        StartCoroutine(Cooldown());
        StartCoroutine(DelayedAI(companion));
        //todo: add spawned companion to party


        CloseUI();
    }

    public void ToggleUI()
    {
        //toggle the menu
        if (InputManager.GetButtonDown(control, axis))
        {
            if (MenuOpen) CloseUI();
            else OpenUI();
        }
    }

    public void CloseUI()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = baseFixedTimeScale;
        menuCanvas.SetActive(false);
        MenuOpen = false;
        FlagManager.SetFlag(menuOpenFlag, "False");
    }

    public void OpenUI()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        menuCanvas.SetActive(true);
        MenuOpen = true;
        FlagManager.SetFlag(menuOpenFlag, "True");
    }

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

    private IEnumerator Cooldown()
    {
        Debug.Log("Companion summon on cooldown");
        isOnCooldown = true;
        yield return CooldownTimer;
        isOnCooldown = false;
        Debug.Log("Companion summon off cooldown");
    }

}
