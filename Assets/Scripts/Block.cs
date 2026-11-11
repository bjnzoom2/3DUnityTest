using UnityEngine;

interface IInteractable
{
    public void InteractAction(GameObject interacter);
}

public class Block : MonoBehaviour, IInteractable
{
    private Vector3 targetPosition = new();
    private Rigidbody rigidbody;
    private GameObject interacter;
    private bool pickedUp = false;
    private float targetDistance = 0.0f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
        } else
        {
            interacter = null;
            targetDistance = 0.0f;
            rigidbody.useGravity = true;
        }

        pickedUp = !pickedUp;
    }
}
