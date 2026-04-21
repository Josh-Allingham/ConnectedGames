using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public string highlightText = "[R]";

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

        if (dialogueIndex == 5)
        {
            CameraManager.main.ActivateCamera("AllWindmills");            
        }
        else if (dialogueIndex == 6)
        {
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("AllWindmills", 0, "OldMan"));
        }

        dialogueIndex++;

        if (dialogueIndex >= dialogueStrings.Length)
        {
            //Dialogue Finished
            dialogueIndex = -1;
        }
        return dialogue;
    }

    public bool HasFinishedDialogue()
    {
        return dialogueIndex == -1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.SetInteractee(this);
            CameraManager.main.ActivateCamera("OldMan");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            Debug.Log("Left");
            player.SetInteractee(null);
            StartCoroutine(CameraManager.main.DisableCameraAfterXSeconds("OldMan", 0, "Player"));
        }
    }
}
