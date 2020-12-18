using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit3D;

public class EventHandler : MonoBehaviour
{
    List<KillEvent> killEvents;
    List<DeathEvent> deathEvents;
    List<PositionEvent> positionEvents;
    List<LifeLostEvent> lifeLostEvents;
    List<BoxDestroyedEvent> boxDestroyedEvents;
    List<JumpEvent> jumpEvents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FillKillEventData(Damageable damageable)
    {
        KillEvent newKillEvent = new KillEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
        newKillEvent.rotation = damageable.gameObject.GetComponent<Transform>().rotation;

    }

    public void FillDeathEventData(Damageable damageable)
    {
        DeathEvent newKillEvent = new DeathEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
    }

    public void FillPositionEventData(Damageable damageable)
    {
        KillEvent newKillEvent = new KillEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
    }

    public void FillLifeLostEventData(Damageable damageable)
    {
        KillEvent newKillEvent = new KillEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
    }

    public void FillBoxDestroyedEventData(Damageable damageable)
    {
        KillEvent newKillEvent = new KillEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
    }    
    
    public void FillJumpEventData(Damageable damageable)
    {
        KillEvent newKillEvent = new KillEvent();
        newKillEvent.position = damageable.gameObject.GetComponent<Transform>().position;
    }
}
