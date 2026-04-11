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
        deadTomatoes += 1;
        OnTomatoKilled?.Invoke();
    }

    private void OnEnable()
    {
        totalTomatoes = 0;
        deadTomatoes = 0;
    }
}
