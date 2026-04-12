using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GetItemScreen : MonoBehaviour
{
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private float lingerTime;

    [SerializeField]
    private Image[] images;
    [SerializeField]
    private TMP_Text[] texts;

    [SerializeField]
    private UnityEvent AfterFadeIn;

    private float currentAlpha;
    public bool fading { get; private set; }

    private PlayerController player;

    public void Appear()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.locked = true;
        gameObject.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        fading = true;
        while (currentAlpha < 1)
        {
            SetAlphas(currentAlpha);
            currentAlpha += Time.deltaTime / fadeTime;
            yield return null;
        }
        currentAlpha = 1;
        SetAlphas(currentAlpha);

        AfterFadeIn?.Invoke();

        yield return new WaitForSeconds(lingerTime);

        while (currentAlpha > 0)
        {
            SetAlphas(currentAlpha);
            currentAlpha -= Time.deltaTime / fadeTime;
            yield return null;
        }
        currentAlpha = 0;
        SetAlphas(currentAlpha);
        fading = false;
        gameObject.SetActive(false);
        player.locked = false;
    }

    private void SetAlphas(float alpha)
    {
        foreach (TMP_Text text in texts)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}