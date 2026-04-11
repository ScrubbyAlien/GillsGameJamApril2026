using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldState", menuName = "Scriptable Objects/WorldState")]
public class WorldState : ScriptableObject
{
    public event Action OnTomatoKilled;

    public int totalTomatoes { get; private set; }
    public int deadTomatoes { get; private set; }

    public void RegisterTomato(Tomato tomato)
    {
        totalTomatoes += 1;
    }

    public void KillTomato(Tomato tomato)
    {
        if (tomato.disliked) return;
        deadTomatoes += 1;
        OnTomatoKilled?.Invoke();
    }

    public void OnEnable()
    {
        totalTomatoes = 0;
        deadTomatoes = 0;
    }

    public void KillZone(Collider2D col)
    {
        if (col.TryGetComponent<IKillable>(out IKillable killable))
        {
            killable.Die();
        }
    }
}