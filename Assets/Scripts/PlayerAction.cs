using UnityEngine;
using UnityEngine.Windows;

public class PlayerAction : MonoBehaviour
{
    private GameInput inputs;
    private float interactionRayDistance = 2.0f;
    public bool hitInteractable = false;
    private IInteractable interactable = null;

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

        if (interactable == null)
        {
            if (hit)
            {
                hitInteractable = hitResults.collider.gameObject.TryGetComponent(out IInteractable _interactable);

                if (inputs.Player.Interact.WasPressedThisFrame())
                {
                    interactable = _interactable;
                    interactable.InteractAction(gameObject);
                }
            } else
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
    }
}
