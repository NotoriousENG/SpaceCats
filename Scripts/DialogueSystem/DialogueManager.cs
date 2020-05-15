using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/********************
 * DIALOGUE MANAGER *
 ********************
 * This Dialogue Manager is what links your dialogue which is sent by the Dialogue Trigger to Unity
 *
 * The Dialogue Manager navigates the sent text and prints it to text objects in the canvas and will toggle
 * the Dialogue Box when appropriate
 */

public class DialogueManager : MonoBehaviour
{
    public GameObject CanvasBox; // your fancy canvas box that holds your text objects
    public Text TextBox; // the text body
    public Text NameText; // the text body of the name you want to display
    public Image image;
    public bool freezePlayerOnDialogue = true;
    public string curr = "";
    public string next = "";
    public string portraitPath;

    // private bool isOpen; // represents if the dialogue box is open or closed

    private Queue<string> inputStream = new Queue<string>(); // stores dialogue
    private M_PlayerController pController;

    private void Start()
    {
        CanvasBox.SetActive(false); // close the dialogue box on play
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<M_PlayerController>(out pController);
    }

    private void DisablePlayerController()
    {
        pController.zeroMovement = true;
    }

    private void EnablePlayerController()
    {
        pController.zeroMovement = false;
    }

    public void StartDialogue(Queue<string> dialogue)
    {
        if (freezePlayerOnDialogue)
        {
            DisablePlayerController();
        }

        CanvasBox.SetActive(true); // open the dialogue box
        // isOpen = true;
        inputStream = dialogue; // store the dialogue from dialogue trigger
        PrintDialogue(); // Prints out the first line of dialogue
    }

    public void AdvanceDialogue() // call when a player presses a button in Dialogue Trigger
    {
        if (inputStream.Count > 0)
        {
            PrintDialogue();
        }
    }

    private void PrintDialogue()
    {
        if (inputStream.Peek().Contains("EndQueue")) // special phrase to stop dialogue
        {
            inputStream.Dequeue(); // Clear Queue
            EndDialogue();
        }
        else if (inputStream.Peek().Contains("[NAME="))
        {
            string name = inputStream.Peek();
            name = inputStream.Dequeue().Substring(name.IndexOf('=') + 1, name.IndexOf(']') - (name.IndexOf('=') + 1));
            NameText.text = name;
            StoreDialogue();
            PrintDialogue(); // print the rest of this line
        }
        else if (inputStream.Peek().Contains("[IMAGE="))
        {
            string imageString = inputStream.Peek();
            imageString = inputStream.Dequeue().Substring(imageString.IndexOf('=') + 1, imageString.IndexOf(']') - (imageString.IndexOf('=') + 1));
            image.sprite = Resources.Load<Sprite>(portraitPath + imageString) as Sprite;
            // Debug.Log("Image: " + imageString);
            StoreDialogue();
            PrintDialogue(); // print the rest of this line
        }
        else
        {
            TextBox.text = inputStream.Dequeue();
            StoreDialogue();
        }
    }

    public void EndDialogue()
    {
        TextBox.text = "";
        NameText.text = "";
        inputStream.Clear();
        CanvasBox.SetActive(false);
        // isOpen = false;
        if (freezePlayerOnDialogue)
        {
            EnablePlayerController();
        }
    }

    void StoreDialogue()
    {
        if (inputStream.Count > 0)
            {
                curr = next;
                if (inputStream.Count > 1)
                {
                    next = inputStream.Peek();
                }
            }
    }

}