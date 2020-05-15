using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnStart : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    private void Start() {
        dialogueTrigger.TriggerDialogue();
        InvokeRepeating("AdvanceDialogue", 0,4);
    }

    public void Advance()
    {
        var dm = FindObjectOfType<DialogueManager>();
        dm.AdvanceDialogue();
        if (dm.next.Contains("EndQueue"))
        {
            CancelInvoke();
        }
    }
}
