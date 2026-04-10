using TMPro;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField]
    private Sensor playerSensor;
    [SerializeField]
    private Dialogue dialogueAsset;
    [SerializeField]
    private Canvas dialogueBox;
    [SerializeField]
    private TMP_Text textField;
    [SerializeField]
    private Canvas interactionPopup;
    [SerializeField]
    private bool oneTimeDialogue;

    private int sentenceIndex;

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
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.Interact += PlayerInteracted;
        playerSensor.OnStartSense += PlayerEnter;
        playerSensor.OnStopSense += PlayerExit;
        dialogueBox.gameObject.SetActive(false);
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
                if (!dialogueOpen) interactionPopup.gameObject.SetActive(true);
                break;
        }
    }

    private void PlayerExit(Collider2D _)
    {
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
        else if (dialogueFinished && oneTimeDialogue) return;
        interactionPopup.gameObject.SetActive(false);
        dialogueOpen = true;
        dialogueBox.gameObject.SetActive(true);
        textField.text = dialogueAsset.sentences[sentenceIndex];
    }

    private void CloseDialogue()
    {
        dialogueOpen = false;
        dialogueBox.gameObject.SetActive(false);
    }

    private void ProgressDialogue()
    {
        sentenceIndex++;
        if (dialogueAsset.sentences.Length == sentenceIndex)
        {
            dialogueFinished = true;
            CloseDialogue();
            if (playerSensor.sensing && activationMode == ActivationMode.Manual && !oneTimeDialogue)
            {
                interactionPopup.gameObject.SetActive(true);
            }
            return;
        }
        textField.text = dialogueAsset.sentences[sentenceIndex];
    }
}