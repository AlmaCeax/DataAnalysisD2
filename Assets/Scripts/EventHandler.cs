using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using static Gamekit3D.Damageable;


public class EventHandler : MonoBehaviour
{
    long MaxValue = 9223372036854775807;

    public EventList<KillEvent> killEvents;
    public EventList<DeathEvent> deathEvents;
    public EventList<PositionEvent> positionEvents;
    public EventList<LifeLostEvent> lifeLostEvents;
    public EventList<BoxDestroyedEvent> boxDestroyedEvents;
    public EventList<JumpEvent> jumpEvents;

    public List<string> sessions;

    uint evendIdCount = 0;
    public GameObject player;
    PlayerController playerController;
    PlayerData playerData;

    public static EventHandler instance = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        Damageable.myOnDamageReceived += FillLifeLostEventData;
        Damageable.myOnDeath += FillKillEventData;
        Damageable.myOnDeath += FillDeathEventData;
        Damageable.myOnDeath += FillBoxDestroyedEventData;
        PlayerController.myOnJump += FillJumpEventData;
        StartCoroutine("FillPlayerPosition");
    }

    private void OnDisable()
    {
        Damageable.myOnDamageReceived -= FillLifeLostEventData;
        Damageable.myOnDeath -= FillKillEventData;
        Damageable.myOnDeath -= FillDeathEventData;
        Damageable.myOnDeath -= FillBoxDestroyedEventData;
        PlayerController.myOnJump -= FillJumpEventData;
        StopCoroutine("FillPlayerPosition");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        Initialize();
    }


    void Initialize()
    {
        killEvents = new EventList<KillEvent>();
        deathEvents = new EventList<DeathEvent>();
        positionEvents = new EventList<PositionEvent>();
        lifeLostEvents = new EventList<LifeLostEvent>();
        boxDestroyedEvents = new EventList<BoxDestroyedEvent>();
        jumpEvents = new EventList<JumpEvent>();

        Writer.instance.ReadEventFile(ref killEvents, "KillEvents.json");
        Writer.instance.ReadEventFile(ref deathEvents, "DeathEvents.json");
        Writer.instance.ReadEventFile(ref positionEvents, "PositionEvents.json");
        Writer.instance.ReadEventFile(ref lifeLostEvents, "LifeLostEvents.json");
        Writer.instance.ReadEventFile(ref boxDestroyedEvents, "BoxDestroyedEvents.json");
        Writer.instance.ReadEventFile(ref jumpEvents, "JumpEvents.json");

        //Generate Session ID
        playerData.sessionId = Random.Range(0, MaxValue).GetHashCode().ToString();
        PlayerPrefs.SetString("sessionID", playerData.sessionId);
        playerData.playerID = PlayerPrefs.GetString("playerId").GetHashCode() & 0xfffffff;
        playerData.playerName = PlayerPrefs.GetString("playerId");
        playerData.playerSex = PlayerPrefs.GetString("playerSex");
        playerData.playerCountry = PlayerPrefs.GetString("playerCountry");

        CountSessions();
    }

    void CountSessions()
    {
        sessions.Add(playerData.sessionId);

        foreach (var item in positionEvents.events)
        {
            if(!sessions.Contains(item.pdata.sessionId))
            {
                sessions.Add(item.pdata.sessionId);
            }
        }
    }

    // Update is called once per frame
    void OnApplicationQuit()
    {
        Writer.instance.WriteEventFile(killEvents, "KillEvents.json");
        Writer.instance.WriteEventFile(deathEvents, "DeathEvents.json");
        Writer.instance.WriteEventFile(positionEvents, "PositionEvents.json");
        Writer.instance.WriteEventFile(lifeLostEvents, "LifeLostEvents.json");
        Writer.instance.WriteEventFile(boxDestroyedEvents, "BoxDestroyedEvents.json");
        Writer.instance.WriteEventFile(jumpEvents, "JumpEvents.json");
    }

    public void FillKillEventData(Damageable damageable, DamageMessage enemyData) {
        LayerMask enemyMask = LayerMask.NameToLayer("Enemy");
        if (damageable.gameObject.layer == enemyMask)
        {
            KillEvent newKillEvent = new KillEvent();
            newKillEvent.pdata = playerData;
            newKillEvent.eventID = ++evendIdCount;
            newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            newKillEvent.rotation = damageable.gameObject.GetComponent<Transform>().rotation;
            if (damageable.gameObject.name == "Spitter") newKillEvent.enemyType = "Spitter";
            else if (damageable.gameObject.name == "Chomper") newKillEvent.enemyType = "Chomper";
            newKillEvent.timeStamp = Time.time;
            killEvents.events.Add(newKillEvent);
        }
    }

    public void FillDeathEventData(Damageable damageable, DamageMessage enemyData)
    {
        LayerMask enemyMask = LayerMask.NameToLayer("Enemy");
        LayerMask environmentMask = LayerMask.NameToLayer("Environment");
        if (enemyData.damager.gameObject.layer == enemyMask || enemyData.damager.gameObject.layer == environmentMask || enemyData.damager.GetComponent<GrenadierGrenade>() || enemyData.damager.transform.parent.gameObject.layer == enemyMask)
        {
            DeathEvent newDeathEvent = new DeathEvent();
            newDeathEvent.pdata = playerData;
            newDeathEvent.eventID = ++evendIdCount;
            newDeathEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            newDeathEvent.rotation = damageable.gameObject.GetComponent<Transform>().rotation;
            if (enemyData.damager.gameObject.name == "Spitter") newDeathEvent.enemyType = "Spitter";
            else if (enemyData.damager.gameObject.name == "Acid") newDeathEvent.enemyType = "Acid";
            else if (enemyData.damager.GetComponent<GrenadierGrenade>()) newDeathEvent.enemyType = "Spitter";
            else
            {
                newDeathEvent.enemyType = "Chomper";
            }
            newDeathEvent.timeStamp = Time.time;
            deathEvents.events.Add(newDeathEvent);
        }
    }

    public void FillLifeLostEventData(Damageable damageable, DamageMessage enemyData)
    {
        LayerMask playerMask = LayerMask.NameToLayer("Player");
        if (damageable.gameObject.layer == playerMask)
        {
            LifeLostEvent newLifeLostEvent = new LifeLostEvent();
            newLifeLostEvent.pdata = playerData;
            newLifeLostEvent.eventID = ++evendIdCount;
            newLifeLostEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            if (enemyData.damager.gameObject.name == "Spitter") newLifeLostEvent.enemyType = "Spitter";
            else if (enemyData.damager.GetComponent<GrenadierGrenade>()) newLifeLostEvent.enemyType = "Spitter";
            else if (enemyData.damager.gameObject.transform.parent.gameObject.name == "Chomper") newLifeLostEvent.enemyType = "Chomper";
            newLifeLostEvent.timeStamp = Time.time;
            lifeLostEvents.events.Add(newLifeLostEvent);
        }
    }

    public void FillBoxDestroyedEventData(Damageable damageable, DamageMessage data)
    {
        if (damageable.gameObject.name == "Cube")
        {
            BoxDestroyedEvent newBoxDestroyedEvent = new BoxDestroyedEvent();
            newBoxDestroyedEvent.pdata = playerData;
            newBoxDestroyedEvent.eventID = ++evendIdCount;
            newBoxDestroyedEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            newBoxDestroyedEvent.timeStamp = Time.time;
            boxDestroyedEvents.events.Add(newBoxDestroyedEvent);
        }
    }    
    
    public void FillJumpEventData(PlayerController playerController)
    {
        JumpEvent newJumpEvent = new JumpEvent();
        newJumpEvent.pdata = playerData;
        newJumpEvent.eventID = ++evendIdCount;
        newJumpEvent.position = playerController.gameObject.GetComponent<Transform>().position;
        newJumpEvent.timeStamp = Time.time;
        jumpEvents.events.Add(newJumpEvent);
    }

    public void FillPositionEvent()
    {
        PositionEvent newPositionEvent = new PositionEvent();
        newPositionEvent.pdata = playerData;
        newPositionEvent.eventID = ++evendIdCount;
        newPositionEvent.position = playerController.gameObject.transform.position;
        newPositionEvent.rotation = playerController.gameObject.transform.rotation;
        newPositionEvent.timeStamp = Time.time;
        positionEvents.events.Add(newPositionEvent);
    }

    IEnumerator FillPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            FillPositionEvent();
        }
    }
}
