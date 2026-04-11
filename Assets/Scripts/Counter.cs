using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private TMP_Text counterText;

    private void Start()
    {
        worldState.OnTomatoKilled += UpdateCounter;
        worldState.Reset += Reset;
        gameObject.SetActive(false);
    }

    private void UpdateCounter()
    {
        counterText.text = $"{worldState.deadTomatoes:0} / {worldState.totalTomatoes:0}";
    }

    public void Display()
    {
        gameObject.SetActive(true);
        UpdateCounter();
    }

    private void Reset()
    {
        worldState.Reset -= Reset;
        worldState.OnTomatoKilled -= UpdateCounter;
        UpdateCounter();
    }
}