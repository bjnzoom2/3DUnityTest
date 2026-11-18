using UnityEngine;

interface IInteractable
{
    public bool Toggle
    {
        get;
    }

    public string InteractText { get; set; }

    public virtual void InteractAction(GameObject _interacter)
    {
        Debug.Log("Interacted");
    }
}
