using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    private Vector3 targetPosition = new();
    private Rigidbody rigidbody;
    private GameObject interacter;
    private bool pickedUp = false;
    private float targetDistance = 0.0f;
    private float baseAngularDamping;
    private float grabbedAngularDamping = 0.8f;

    public bool Toggle
    {
        get; private set;
    }

    public string InteractText
    {
        get; set;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        baseAngularDamping = rigidbody.angularDamping;
        Toggle = true;
        InteractText = "E to Grab";
    }

    private void FixedUpdate()
    {
        if (pickedUp && interacter != null)
        {
            targetPosition = Camera.main.transform.position + Camera.main.transform.forward * targetDistance;
            Vector3 displacement = targetPosition - rigidbody.position;
            rigidbody.linearVelocity = displacement * 12.0f;
        }
    }

    public void InteractAction(GameObject _interacter)
    {
        if (!pickedUp)
        {
            interacter = _interacter;
            targetDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
            rigidbody.useGravity = false;
            rigidbody.angularDamping = grabbedAngularDamping;
        }
        else
        {
            interacter = null;
            targetDistance = 0.0f;
            rigidbody.useGravity = true;
            rigidbody.angularDamping = baseAngularDamping;
        }

        pickedUp = !pickedUp;
    }
}
