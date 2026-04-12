using UnityEngine;
using UnityEngine.Events;

public class WorldStateHook : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip nice, angry, recordScratch;

    [SerializeField]
    private UnityEvent OnTurnAggressive;

    private bool inAggressive;

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
        worldState.OnTomatoKilled -= InvokeOnTurnAggressive;
        SwitchMusic();
        inAggressive = true;
    }

    public void SwitchMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(recordScratch);
        audioSource.clip = angry;
        audioSource.volume *= 2;
        audioSource.PlayDelayed(1f);
    }
}