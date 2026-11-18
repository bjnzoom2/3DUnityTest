using UnityEngine;
using System.Collections.Generic;

public class TobyTheFishTalk : DialogueInteract
{
    private void Start()
    {
        Continuous = false;

        dialogueLines = new List<string>
        {
            "Hello I am test guy",
            "Check out the toby the fish tiktok",
            "Type shi"
        };
    }
}