using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Transform pivotPoint;

    private bool isOpened = false;

    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;
    private float rotateVelo = 0.0f;

    public bool Toggle
    {
        get; private set;
    }

    public string InteractText { get; set; }
    private string closedText = "E to Open";
    private string openedText = "E to Close";

    private void Awake()
    {
        pivotPoint = transform.Find("Pivot");
        Toggle = false;
        InteractText = closedText;
    }

    private void Start()
    {
        currentAngle = pivotPoint.transform.eulerAngles.y;
    }

    private void Update()
    {
        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref rotateVelo, 0.2f);
        pivotPoint.transform.eulerAngles = new(0.0f, currentAngle, 0.0f);
    }

    public void InteractAction(GameObject _interacter)
    {
        if (!isOpened)
        {
            targetAngle = -90.0f;
            InteractText = openedText;
        } else
        {
            targetAngle = 0.0f;
            InteractText = closedText;
        }

        isOpened = !isOpened;
    }
}
