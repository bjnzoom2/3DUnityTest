using UnityEngine;

interface IInteractable
{
    public bool Toggle
    {
        get;
    }

    public virtual void InteractAction(GameObject _interacter)
    {
        Debug.Log("Interacted");
    }
}
