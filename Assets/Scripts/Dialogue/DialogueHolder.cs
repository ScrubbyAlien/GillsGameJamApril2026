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
    private Canvas interactionPopup;

    public enum ActivationMode
    {
        Automatic,
        Manual
    }

    [SerializeField]
    private ActivationMode activationMode;

    private bool dialogueOpen;

    public void Start()
    {
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.Interact += PlayerInteracted;
        playerSensor.OnStartSense += OpenDialogue;
        playerSensor.OnStopSense += CloseDialogue;
        dialogueBox.gameObject.SetActive(false);
        interactionPopup.gameObject.SetActive(false);
    }

    private void PlayerInteracted()
    {
        if (playerSensor.sensing && activationMode == ActivationMode.Manual && !dialogueOpen)
        {
            OpenDialogue();
        }
    }

    private void OpenDialogue(Collider2D col = null)
    {
        if (activationMode == ActivationMode.Automatic || !col)
        {
            interactionPopup.gameObject.SetActive(false);
            dialogueOpen = true;
            dialogueBox.gameObject.SetActive(true);
        }
        if (activationMode == ActivationMode.Manual && !dialogueOpen)
        {
            interactionPopup.gameObject.SetActive(true);
        }
    }

    private void CloseDialogue(Collider2D _ = null)
    {
        if (dialogueOpen)
        {
            dialogueOpen = false;
            dialogueBox.gameObject.SetActive(false);
        }
    }
}