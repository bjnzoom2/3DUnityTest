using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    static public UIHandler instance;

    [SerializeField] private TextMeshProUGUI interactText;

    [SerializeField] private GameObject player;
    private PlayerAction actions;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool displayingDialogue = false;

    private void OnEnable()
    {
        DialogueInteract.DisplayDialogueString += DisplayDialogue;
    }

    private void OnDisable()
    {
        DialogueInteract.DisplayDialogueString -= DisplayDialogue;
    }

    private void Start()
    {
        actions = player.GetComponent<PlayerAction>();
    }

    private void Update()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        interactText.alpha = (actions.hitInteractable) ? 1.0f : 0.0f;
    }

    private void DisplayDialogue(string _text, Sprite _image, bool _continuous, bool _endedDialogue)
    {
        if (!_continuous)
        {
            dialoguePanel.SetActive(!displayingDialogue);
            if (!displayingDialogue)
            {
                dialogueText.text = _text;
                dialogueImage.sprite = (_image != null) ? _image : null;
            }

            displayingDialogue = !displayingDialogue;
        } else
        {
            if (!_endedDialogue)
            {
                dialoguePanel.SetActive(true);
                dialogueText.text = _text;
                dialogueImage.sprite = (_image != null) ? _image : null;
            } else
            {
                dialoguePanel.SetActive(displayingDialogue);
                if (displayingDialogue)
                {
                    dialogueText.text = _text;
                    dialogueImage.sprite = (_image != null) ? _image : null;
                }

                displayingDialogue = !displayingDialogue;
            }
        }
    }
}
