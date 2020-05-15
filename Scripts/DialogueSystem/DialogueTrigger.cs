using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/********************
 * DIALOGUE TRIGGER *
 ********************
 * A Dialogue Trigger can be anything, an NPC, a signpost, or even an in game event.
 * 
 * A Dialogue Trigger is responsible to activate based on some action in the game e.g. talking to an NPC
 * To handle talking to an NPC, we first attach this script to an NPC along with a dialogue file we write (e.g. .txt)
 */

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset TextFileAsset; // your imported text file for your NPC
    public bool TriggerWithButton;
    public GameObject optionalButtonIndicator;
    public Vector3 optionalIndicatorOffset = new Vector3 (0,0,0);
    private Queue<string> dialogue = new Queue<string>(); // stores the dialogue (Great Performance!)
    private float waitTime = 4f; // lag time for advancing dialogue so you can actually read it
    private float nextTime = 0f; // used with waitTime to create a timer system
    private bool dialogueTiggered;
    private GameObject indicator;
    public bool autoAdvance;
    private bool waiting;

    // public bool useCollision; // unused for now

    private void Start() 
    {
        if (optionalButtonIndicator != null)
        {
            indicator =  GameObject.Instantiate(optionalButtonIndicator);
            indicator.transform.parent = transform;
            indicator.transform.localPosition = optionalIndicatorOffset;
            indicator.SetActive(false);
        }
    }

    private void Update() 
    {
        if (autoAdvance && dialogueTiggered && Time.timeSinceLevelLoad > nextTime)
        {
            AdvanceDialogue();
        }
    }
    /* Called when you want to start dialogue */
    public void TriggerDialogue()
    {
        ReadTextFile(); // loads in the text file
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue); // Accesses Dialogue Manager and Starts Dialogue
    }

    void HandleLine(string line)
    {
         if (line.StartsWith("[")) // e.g [NAME=Michael] Hello, my name is Michael
        {
            int end = line.IndexOf(']');
            string special = (end >= 0) ? line.Substring(0, end + 1) : " "; // special = [NAME=Michael]
            string curr = (end >= 0) ? line.Substring(end + 1) : " "; // curr = Hello, ...
            dialogue.Enqueue(special); // adds to the dialogue to be printed
            HandleLine(curr);
        }
        else
        {
            if (!(string.IsNullOrEmpty(line)))
            {
                dialogue.Enqueue(line); // adds to the dialogue to be printed
            }
        }
    }
    /* loads in your text file */
    private void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(System.Environment.NewLine.ToCharArray()); // Split dialogue lines by newline

        foreach (string line in lines) // for every line of dialogue
        {
            if (!string.IsNullOrEmpty(line) )// ignore empty lines of dialogue
            {
               HandleLine(line);
               // Debug.Log("HERE");
            }
        }
        dialogue.Enqueue("EndQueue");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!TriggerWithButton)
            {
                TriggerDialogue();
            }
            // Debug.Log("Collision");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Debug.Log(other.name);
        if (other.gameObject.tag == "Player" && (Input.GetButton("Jump") || autoAdvance)&& nextTime < Time.timeSinceLevelLoad )
        {
            AdvanceDialogue();
        }
        else if (other.gameObject.tag == "Player")
        {
            if (!dialogueTiggered)
            {
                // Debug.Log("Press Space");
                if (indicator != null && indicator.activeSelf == false)
                {
                    indicator.SetActive(true);
                }
            }
        }
    }

    public void AdvanceDialogue()
    {
        // Debug.Log("advance");
        // waiting = false;
        if (!dialogueTiggered)
        {
            TriggerDialogue();
            dialogueTiggered = true;
            if (indicator != null && indicator.activeSelf == true)
            {
                indicator.SetActive(false);
            }
            nextTime = Time.timeSinceLevelLoad + waitTime;
        }
        else 
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;
            var dm = FindObjectOfType<DialogueManager>();
            dm.AdvanceDialogue();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !autoAdvance)
        {
            EndDialogue();
            if (indicator != null && indicator.activeSelf == true)
            {
                indicator.SetActive(false);
            }
        }
    }

    void EndDialogue()
    {
        FindObjectOfType<DialogueManager>().EndDialogue();
        dialogueTiggered = false;
    }
}
