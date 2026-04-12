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
    [SerializeField]
    private Image background;

    private float textA;
    private float iconA;
    private float backgroundA;

    [SerializeField]
    private float fadeTime = 10f;

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
        textA = counterText.color.a;
        iconA = icon.color.a;
        backgroundA = background.color.a;

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
            currentAlpha += Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    private void SetAlphas(float alpha)
    {
        Color textColor = counterText.color;
        textColor.a = Mathf.Lerp(0, textA, alpha);
        counterText.color = textColor;

        Color iconColor = icon.color;
        iconColor.a = Mathf.Lerp(0, iconA, alpha);
        icon.color = iconColor;

        Color bgColor = background.color;
        bgColor.a = Mathf.Lerp(0, backgroundA, alpha);
        background.color = bgColor;
    }
}