using UnityEngine.SceneManagement;

public class Tutorial : Gamemode
{
    public const string DISPLAY_NAME = "튜토리얼";

    private readonly TutorialModeScript script;

    public Tutorial(TutorialModeScript script)
    {
        numberOfPlayers = 1;
        numberOfSpectators = 0;
        turn = 0;
        this.script = script;
    }

    public override void End()
    {
        //complete tutorial
        SceneManager.LoadScene(Constants.SCENE_MAIN_MENU);
    }

    public override void Load()
    {
        //script.SetDialogTitle(title);
        //script.SetDialogText(text);
    }

    public override void Start()
    {
        script.SetDialogEnabled(true);
    }

    public override void Turn()
    {
        switch (turn)
        {
            case 0:
                //fill text for defense tutorial
                //hide control UI & show dialog
                turn++;
                break;
            case 1:
                //fill text for item tutorial
                //hide control UI & show dialog
                turn++;
                break;
            default:
                End();
                break;
        }
    }

    public TutorialType Phase
    {
        get
        {
            return turn switch
            {
                0 => TutorialType.ATTACK,
                1 => TutorialType.DEFENSE,
                2 => TutorialType.ITEM,
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}

public enum TutorialType { ATTACK = 1, DEFENSE, ITEM }
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

    private Queue<string> sentences;

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
        SkipAnimation();
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
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
