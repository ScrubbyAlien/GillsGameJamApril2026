using UnityEngine;
using UnityEngine.Events;

public class WorldStateHook : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private UnityEvent OnTurnAggressive;

    private void Start()
    {
        worldState.OnTomatoKilled += InvokeOnTurnAggressive;
    }

    private void OnDestroy()
    {
        worldState.OnTomatoKilled -= InvokeOnTurnAggressive;
    }

    private void InvokeOnTurnAggressive()
    {
        OnTurnAggressive?.Invoke();
    }
}