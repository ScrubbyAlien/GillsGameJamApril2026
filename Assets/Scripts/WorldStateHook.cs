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
        SwitchMusic();
    }

    public void SwitchMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(recordScratch);
        audioSource.clip = angry;
        audioSource.PlayDelayed(1f);
    }
}