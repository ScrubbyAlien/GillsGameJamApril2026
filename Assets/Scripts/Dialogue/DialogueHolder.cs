using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField]
    private Sensor playerSensor;
    [SerializeField]
    private Dialogue dialogueAsset;
    [SerializeField]
    private Canvas[] dialogueBoxes;
    [SerializeField]
    private Canvas interactionPopup;
    [SerializeField]
    private bool oneTimeDialogue;
    [SerializeField]
    private bool lockPlayer;
    private PlayerController playerController;

    private bool oneTimeDialogueFinished => oneTimeDialogue && dialogueFinished;

    private int sentenceIndex;

    [SerializeField]
    private UnityEvent OnOneTimeDialogueFinished;

    public enum ActivationMode
    {
        Automatic,
        Manual
    }

    [SerializeField]
    private ActivationMode activationMode;

    private bool dialogueOpen;
    private bool dialogueFinished;

    public void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.Interact += PlayerInteracted;
        playerSensor.OnStartSense += PlayerEnter;
        playerSensor.OnStopSense += PlayerExit;
        CloseDialogue();
        interactionPopup.gameObject.SetActive(false);
    }

    private void PlayerInteracted()
    {
        if (playerSensor.sensing && !dialogueOpen)
        {
            OpenDialogue();
        }
        else if (dialogueOpen)
        {
            ProgressDialogue();
        }
    }

    private void PlayerEnter(Collider2D _)
    {
        switch (activationMode)
        {
            case ActivationMode.Automatic:
                OpenDialogue();
                break;
            case ActivationMode.Manual:
                if (!dialogueOpen && !oneTimeDialogueFinished) interactionPopup.gameObject.SetActive(true);
                break;
        }
    }

    private void PlayerExit(Collider2D _)
    {
        if (sentenceIndex == dialogueAsset.sentences.Length - 1 && oneTimeDialogue && !dialogueFinished)
        {
            dialogueFinished = true;
            OnOneTimeDialogueFinished?.Invoke();
        }
        interactionPopup.gameObject.SetActive(false);
        CloseDialogue();
    }

    private void OpenDialogue()
    {
        if (dialogueFinished && !oneTimeDialogue)
        {
            dialogueFinished = false;
            sentenceIndex = 0;
        }
        else if (oneTimeDialogueFinished) return;
        interactionPopup.gameObject.SetActive(false);
        dialogueOpen = true;

        if (lockPlayer)
        {
            playerController.locked = true;
        }

        UpdateDialogue();
    }

    private void CloseDialogue()
    {
        if (lockPlayer)
        {
            playerController.locked = false;
        }
        dialogueOpen = false;
        HideAllBoxes();
    }

    private void ProgressDialogue()
    {
        sentenceIndex++;
        if (dialogueAsset.sentences.Length == sentenceIndex)
        {
            dialogueFinished = true;
            CloseDialogue();
            if (oneTimeDialogue)
            {
                OnOneTimeDialogueFinished?.Invoke();
            }
            if (playerSensor.sensing && activationMode == ActivationMode.Manual && !oneTimeDialogue)
            {
                interactionPopup.gameObject.SetActive(true);
            }
            return;
        }
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        HideAllBoxes();
        if (sentenceIndex >= dialogueAsset.sentences.Length) return;
        if (!dialogueOpen) return;

        Canvas dialogBox = dialogueBoxes[dialogueAsset.sentences[sentenceIndex].characterIndex];
        string sentence = dialogueAsset.sentences[sentenceIndex].words;

        TMP_Text textField = dialogBox.GetComponentInChildren<TMP_Text>();
        dialogBox.gameObject.SetActive(true);
        textField.text = sentence;
    }

    private void HideAllBoxes()
    {
        foreach (Canvas dialogueBox in dialogueBoxes)
        {
            dialogueBox.gameObject.SetActive(false);
        }
    }
}