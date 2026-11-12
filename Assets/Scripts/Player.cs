using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [NonSerialized] public GameInput inputs;

    private void Awake()
    {
        inputs = new();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}
