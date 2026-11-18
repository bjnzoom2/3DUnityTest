using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.RectTransform;

public class PlayerAction : MonoBehaviour
{
    private GameInput inputs;
    private float interactionRayDistance = 2.0f;
    public bool hitInteractable = false;
    public string interactText = null;
    private IInteractable interactable = null;
    private bool interactToggle = false;

    private void Start()
    {
        inputs = gameObject.GetComponent<Player>().inputs;
    }

    private void Update()
    {
        InteractionRay();
    }

    private void InteractionRay()
    {
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitResults, interactionRayDistance);
        IInteractable _interactable = null;

        if (hit)
        {
            _interactable = hitResults.collider.gameObject.GetComponentInParent<IInteractable>() ?? hitResults.collider.gameObject.GetComponentInChildren<IInteractable>();
            hitInteractable = _interactable != null;
            
            if (hitInteractable)
            {
                interactToggle = _interactable.Toggle;
                interactText = _interactable.InteractText;
            }
        } else
        {
            hitInteractable = false;
            _interactable = null;
        }

        if (interactToggle)
        {
            if (interactable == null)
            {
                if (hit)
                {
                    if (inputs.Player.Interact.WasPressedThisFrame())
                    {
                        interactable = _interactable;
                        interactable.InteractAction(gameObject);
                    }
                }
                else
                {
                    hitInteractable = false;
                }
            }
            else
            {
                if (inputs.Player.Interact.WasPressedThisFrame())
                {
                    interactable.InteractAction(gameObject);
                    interactable = null;
                }
            }
        } else
        {
            if (hit)
            {
                if (inputs.Player.Interact.WasPressedThisFrame())
                {
                    _interactable.InteractAction(gameObject);
                }
            }
        }
    }
}
