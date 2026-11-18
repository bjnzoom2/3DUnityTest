using System;
using System.Collections;
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
    private RectTransform dialogueRect;
    [SerializeField] private Image dialogueImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool displayingDialogue = false;

    private Vector2 targetDialoguePos = new();
    private Vector2 baseDialoguePos = new();
    private Vector2 hiddenDialoguePos = new();
    private Vector2 dialogueVelo = new();

    private void OnEnable()
    {
        DialogueInteract.DisplayDialogueString += DisplayDialogue;
    }

    private void OnDisable()
    {
        DialogueInteract.DisplayDialogueString -= DisplayDialogue;
    }

    private void Awake()
    {
        dialogueRect = dialoguePanel.GetComponent<RectTransform>();

        baseDialoguePos = dialogueRect.anchoredPosition;
        hiddenDialoguePos = new(0.0f, -1080.0f);
        targetDialoguePos = hiddenDialoguePos;
        dialogueRect.anchoredPosition = hiddenDialoguePos;
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
        interactText.text = actions.interactText;

        dialogueRect.anchoredPosition = Vector2.SmoothDamp(dialogueRect.anchoredPosition, targetDialoguePos, ref dialogueVelo, 0.2f);
    }

    private void DisplayDialogue(string _text, Sprite _image, bool _continuous, bool _endedDialogue)
    {
        dialogueText.maxVisibleCharacters = 0;

        if (!_continuous)
        {
            if (!displayingDialogue)
            {
                dialogueText.text = _text;
                dialogueImage.sprite = (_image != null) ? _image : null;
                StartCoroutine(TextIncrement(_text));

                targetDialoguePos = baseDialoguePos;
            } else
            {
                targetDialoguePos = hiddenDialoguePos;
            }
            
            displayingDialogue = !displayingDialogue;
        } else
        {
            if (!_endedDialogue)
            {
                dialogueText.text = _text;
                StartCoroutine(TextIncrement(_text));
                dialogueImage.sprite = (_image != null) ? _image : null;

                targetDialoguePos = baseDialoguePos;
            } else
            {
                if (displayingDialogue)
                {
                    dialogueText.text = _text;
                    dialogueImage.sprite = (_image != null) ? _image : null;
                    StartCoroutine(TextIncrement(_text));

                    targetDialoguePos = baseDialoguePos;
                } else
                {
                    targetDialoguePos = hiddenDialoguePos;
                }

                displayingDialogue = !displayingDialogue;
            }
        }
    }

    private IEnumerator TextIncrement(string _text)
    {
        yield return new WaitForSeconds(0.2f);

        while (dialogueText.maxVisibleCharacters < _text.Length)
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.03f);
        }
    }
}
