using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;
using static Gamekit3D.Damageable;

public class EventHandler : MonoBehaviour
{
    EventList<KillEvent> killEvents;
    EventList<DeathEvent> deathEvents;
    EventList<PositionEvent> positionEvents;
    EventList<LifeLostEvent> lifeLostEvents;
    EventList<BoxDestroyedEvent> boxDestroyedEvents;
    EventList<JumpEvent> jumpEvents;

    uint evendIdCount = 0;

    private void OnEnable()
    {
        Damageable.myOnDamageReceived += FillLifeLostEventData;
        Damageable.myOnDeath += FillKillEventData;
        Damageable.myOnDeath += FillDeathEventData;
        Damageable.myOnDeath += FillBoxDestroyedEventData;
        PlayerController.myOnJump += FillJumpEventData;
    }

    private void OnDisable()
    {
        Damageable.myOnDamageReceived -= FillLifeLostEventData;
        Damageable.myOnDeath -= FillKillEventData;
        Damageable.myOnDeath -= FillDeathEventData;
        Damageable.myOnDeath -= FillBoxDestroyedEventData;
        PlayerController.myOnJump -= FillJumpEventData;
    }

    // Start is called before the first frame update
    void Start()
    {
        killEvents = new EventList<KillEvent>();
        deathEvents = new EventList<DeathEvent>();
        positionEvents = new EventList<PositionEvent>();
        lifeLostEvents = new EventList<LifeLostEvent>();
        boxDestroyedEvents = new EventList<BoxDestroyedEvent>();
        jumpEvents = new EventList<JumpEvent>();
    }

    // Update is called once per frame
    void OnApplicationQuit()
    {
        Writer.instance.GenerateJsonString(killEvents, "KillEvents.txt");
        Writer.instance.GenerateJsonString(deathEvents, "DeathEvents.txt");
        Writer.instance.GenerateJsonString(positionEvents, "PositionEvents.txt");
        Writer.instance.GenerateJsonString(lifeLostEvents, "LifeLostEvents.txt");
        Writer.instance.GenerateJsonString(boxDestroyedEvents, "BoxDestroyedEvents.txt");
        Writer.instance.GenerateJsonString(jumpEvents, "JumpEvents.txt");
    }

    public void FillKillEventData(Damageable damageable, DamageMessage enemyData) {
        LayerMask enemyMask = LayerMask.NameToLayer("Enemy");
        if (damageable.gameObject.layer == enemyMask)
        {
            KillEvent newKillEvent = new KillEvent();
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
        LayerMask playerMask = LayerMask.NameToLayer("Player");
        if (damageable.gameObject.layer == playerMask)
        {
            DeathEvent newDeathEvent = new DeathEvent();
            newDeathEvent.eventID = ++evendIdCount;
            newDeathEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            newDeathEvent.rotation = damageable.gameObject.GetComponent<Transform>().rotation;
            if (enemyData.damager.gameObject.name == "Spitter") newDeathEvent.enemyType = "Spitter";
            else if (enemyData.damager.gameObject.name == "Chomper") newDeathEvent.enemyType = "Chomper";
            else if (enemyData.damager.gameObject.name == "Acid") newDeathEvent.enemyType = "Acid";
            newDeathEvent.timeStamp = Time.time;
            deathEvents.events.Add(newDeathEvent);
        }
    }

    public void FillPositionEventData(Damageable damageable)
    {
        PositionEvent newPositionEvent = new PositionEvent();
        newPositionEvent.eventID = ++evendIdCount;
        newPositionEvent.position = damageable.gameObject.GetComponent<Transform>().position;
        newPositionEvent.rotation = damageable.gameObject.GetComponent<Transform>().rotation;
        newPositionEvent.timeStamp = Time.time;
        positionEvents.events.Add(newPositionEvent);
    }

    public void FillLifeLostEventData(Damageable damageable, DamageMessage enemyData)
    {
        LayerMask playerMask = LayerMask.NameToLayer("Player");
        if (damageable.gameObject.layer == playerMask)
        {
            LifeLostEvent newLifeLostEvent = new LifeLostEvent();
            newLifeLostEvent.eventID = ++evendIdCount;
            newLifeLostEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            if (enemyData.damager.gameObject.name == "Spitter") newLifeLostEvent.enemyType = "Spitter";
            else if (enemyData.damager.gameObject.name == "Chomper") newLifeLostEvent.enemyType = "Chomper";
            newLifeLostEvent.timeStamp = Time.time;
            lifeLostEvents.events.Add(newLifeLostEvent);
        }
    }

    public void FillBoxDestroyedEventData(Damageable damageable, DamageMessage data)
    {
        if (damageable.gameObject.name == "Cube")
        {
            BoxDestroyedEvent newBoxDestroyedEvent = new BoxDestroyedEvent();
            newBoxDestroyedEvent.eventID = ++evendIdCount;
            newBoxDestroyedEvent.position = damageable.gameObject.GetComponent<Transform>().position;
            newBoxDestroyedEvent.timeStamp = Time.time;
            boxDestroyedEvents.events.Add(newBoxDestroyedEvent);
        }
    }    
    
    public void FillJumpEventData(PlayerController playerController)
    {
        JumpEvent newJumpEvent = new JumpEvent();
        newJumpEvent.eventID = ++evendIdCount;
        newJumpEvent.position = playerController.gameObject.GetComponent<Transform>().position;
        newJumpEvent.timeStamp = Time.time;
        jumpEvents.events.Add(newJumpEvent);
    }
}
