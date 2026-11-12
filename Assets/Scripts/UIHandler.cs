using System;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    static public UIHandler instance;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject player;
    private PlayerAction actions;

    private void Start()
    {
        actions = player.GetComponent<PlayerAction>();
    }

    private void Update()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }

        text.alpha = (actions.hitInteractable) ? 1.0f : 0.0f;
    }
}
