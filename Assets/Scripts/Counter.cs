using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private TMP_Text counterText;
    [SerializeField]
    private Image icon;

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
        StartCoroutine(FadeIn());
    }

    private void Reset()
    {
        worldState.Reset -= Reset;
        worldState.OnTomatoKilled -= UpdateCounter;
        UpdateCounter();
    }

    private float currentAlpha;
    private IEnumerator FadeIn()
    {
        currentAlpha = 0;
        while (currentAlpha < 1)
        {
            SetAlphas(currentAlpha);
            currentAlpha += Time.deltaTime / 4f;
            yield return null;
        }
    }

    private void SetAlphas(float alpha)
    {
        Color textColor = counterText.color;
        textColor.a = alpha;
        counterText.color = textColor;

        Color iconColor = icon.color;
        iconColor.a = alpha;
        icon.color = iconColor;
    }
}