using UnityEngine;

interface IInteractable
{
    public void InteractAction();
}

public class Block : MonoBehaviour, IInteractable
{
    public void InteractAction()
    {
        Debug.Log("Interacted");
    }
}
