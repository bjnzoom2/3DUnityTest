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

public class Pickup : MonoBehaviour, IInteractable
{
    private Vector3 targetPosition = new();
    private Rigidbody rigidbody;
    private GameObject interacter;
    private bool pickedUp = false;
    private float targetDistance = 0.0f;

    public bool Toggle
    {
        get; private set;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Toggle = true;
    }

    private void FixedUpdate()
    {
        if (pickedUp && interacter != null)
        {
            targetPosition = Camera.main.transform.position + Camera.main.transform.forward * targetDistance;
            Vector3 displacement = targetPosition - rigidbody.position;
            rigidbody.linearVelocity = displacement * 10.0f;
        }
    }

    public void InteractAction(GameObject _interacter)
    {
        if (!pickedUp)
        {
            interacter = _interacter;
            targetDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
            rigidbody.useGravity = false;
        }
        else
        {
            interacter = null;
            targetDistance = 0.0f;
            rigidbody.useGravity = true;
        }

        pickedUp = !pickedUp;
    }
}
