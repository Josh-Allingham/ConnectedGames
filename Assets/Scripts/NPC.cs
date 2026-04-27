using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public string highlightText = "[F]";
    public bool isEvil = false;

    public string[] dialogueStrings = new string[] {"Hey, you there!", //0
                                                    "Another band of drifters, daring to test their luck. How many more will come, I wonder?", //1
                                                    "I will no longer allow the unworthy to pass. If you value your lives, turn back now… return to whatever place you still call home.", //2
                                                    "…Or prove yourselves worthy.", //3
                                                    "Show me that you possess the strength… the skill… the will to survive.", //4
                                                    "Do you see those windmills in the distance?", //5 TRIGGER
                                                    "Bring them to life. Set them turning with your own power… or accept your weakness and walk away.", //6
                                                    "" //7 NULL
                                                     };
    public int dialogueIndex = 0;
    [SerializeField] private Transform newSpawnPos;
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
        if (!isEvil)
        {
            if (dialogueIndex == 5)
            {
                CameraManager.main.ActivateCamera("AllWindmills");
            }
            else if (dialogueIndex == 6)
            {
                StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("AllWindmills", 0));
                CameraManager.main.ActivateCamera("OldMan");
            }
        }
        dialogueIndex++;

        if (dialogueIndex >= dialogueStrings.Length)
        {
            //Dialogue Finished
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("OldMan", 0));
            dialogueIndex = -1;
        }
        return dialogue;
    }

    public void PrepareForBattle()
    {
        transform.parent.transform.position = newSpawnPos.position;
        dialogueStrings = new string[] {"You are quite persistent travelers. I didn’t expect anyone else capable of driving away those storm clouds.",
                                        "But that won’t help you much…",
                                        "You don’t seriously think I’ll let you pass, do you?",
                                        "" //NULL
                                        };
        isEvil = true;
        dialogueIndex = 0;
    }
    public bool HasFinishedDialogue()
    {
        return dialogueIndex == -1;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Player player) && !HasFinishedDialogue())
        {
            Debug.Log("FOUND IT");
            player.SetInteractee(this);
            string camString = isEvil ? "OldManBridge" : "OldMan";
            CameraManager.main.ActivateCamera(camString);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Left");
            player.SetInteractee(null);
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("OldMan", 0));
        }
    }
}
