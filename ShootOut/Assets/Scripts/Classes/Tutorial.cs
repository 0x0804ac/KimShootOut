using UnityEngine.SceneManagement;

public class Tutorial : Gamemode
{
    public const string DISPLAY_NAME = "튜토리얼";

    public Tutorial()
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
    }

    public override void End()
    {
        //complete tutorial
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public override void Load()
    {
        //load scene
        //show attacker UI
        //show tutorial dialog
    }

    public override void Start()
    {
        //enable attacker UI
    }

    public override void Turn()
    {
        switch (turn)
        {
            case 0:
                //hide attacker UI
                //show defender UI
                turn++;
                break;
            case 1:
                //hide defender UI
                //show item UI
                turn++;
                break;
            default:
                End();
                break;
        }
    }
}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/System")]
public class Dialogue : ScriptableObject
{
    public string speakerName;
    [TextArea(3, 10)] // Gives you a nice large text area in the inspector
    public string[] lines;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Settings")]
    public float typeSpeed = 0.05f;

    private Queue<string> sentences;
    private bool isTyping = false;
    private string currentSentence = "";

    void Awake()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); // Hide panel at start
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.speakerName;

        sentences.Clear();

        foreach (string sentence in dialogue.lines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            // If the user clicks while text is typing, instantly finish the line
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed); // Creates a typewriter effect
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueData;
    private DialogueManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        // Press E to advance or start dialogue when near/interacting
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (manager.dialoguePanel.activeSelf)
            {
                manager.DisplayNextSentence();
            }
            else
            {
                manager.StartDialogue(dialogueData);
            }
        }
    }
}
 */
