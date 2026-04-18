using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public string highlightText = "[R]";

    public string[] dialogueStrings = new string[] {"Hey, you there",
                                                    "Another band of drifters, daring to test their luck. How many more will come, I wonder?",
                                                    "I will no longer allow the unworthy to pass. If you value your lives, turn back now… return to whatever place you still call home.",
                                                    "…Or prove yourselves worthy.",
                                                    "Show me that you possess the strength… the skill… the will to survive.",
                                                    "Do you see those windmills in the distance?",
                                                    "Bring them to life. Set them turning with your own power… or accept your weakness…",
                                                    "…and walk away.",
                                                    ""
                                                     };
    public int dialogueIndex = 0;

    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetDialogue()
    {
        string dialogue = dialogueStrings[dialogueIndex];
        dialogueIndex++;
        if (dialogueIndex >= dialogueStrings.Length)
        {
            dialogueIndex = -1;
        }
        return dialogue;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Detect");
            player.setInteractee(this);
            CameraManager.main.ActivateCamera("OldMan");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Left");
            player.setInteractee(null);
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("OldMan", 1));
        }
    }
}
