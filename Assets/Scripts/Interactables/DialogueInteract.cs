using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class DialogueInteract : MonoBehaviour, IInteractable
{
    public bool Toggle => false;
    protected List<string> dialogueLines = new();
    protected int index = 0;
    [SerializeField] protected Sprite dialogueImage;
    public bool Continuous
    {
        get; protected set;
    }
    protected bool displayingCheck = false;
    protected bool endedDialogueLines = false;

    static public event Action<string, Sprite, bool, bool> DisplayDialogueString;

    public virtual void InteractAction(GameObject _interacter)
    {
        DisplayDialogueString?.Invoke(dialogueLines[index], dialogueImage, Continuous, endedDialogueLines);

        if (Continuous)
        {
            if (index + 1 < dialogueLines.Count)
            {
                index++;
            } else
            {
                endedDialogueLines = true;
            }
        } else
        {
            if (index + 1 < dialogueLines.Count)
            {
                if (!displayingCheck)
                {
                    index++;
                }
            } else
            {
                endedDialogueLines = true;
            }

            displayingCheck = !displayingCheck;
        }
    }
}