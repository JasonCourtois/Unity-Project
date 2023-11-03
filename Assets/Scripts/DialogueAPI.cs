using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class DialogueAPI : MonoBehaviour
{
    [SerializeField] TextAsset dialogueText;
    [SerializeField] bool forceAllDialogue;
    [SerializeField] int currentBatch;
    [SerializeField] int timesRead;
    [SerializeField] float scrollSpeed;
    [SerializeField] bool playerInRange;

    float baseSpeed;

    // Variables for animation :D
    int currentFrame;
    Texture2D[] animationFrames;

    PlayerAttributes player;

    bool nextLine;

    RawImage characterPortrait;

    RawImage textBox;
    TextMeshProUGUI textInBox;
    RectTransform textPosition;

    RawImage nameBox;
    TextMeshProUGUI nameText;

    RawImage CG;

    Animator fadeAnim;

    List<string> getBatches;
    List<string[]> dialougeBatches;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttributes>();
        baseSpeed = 0.035f;
        scrollSpeed = 1f;
        timesRead = 0;
        nextLine = false;
        dialougeBatches = new List<string[]>();

        characterPortrait = GameObject.FindGameObjectWithTag("CharacterPortrait").GetComponent<RawImage>();

        textPosition = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<RectTransform>();
        textBox = GameObject.FindGameObjectWithTag("DialogueBox").GetComponent<RawImage>();
        nameBox = GameObject.FindGameObjectWithTag("NameBox").GetComponent<RawImage>();
        nameText = GameObject.FindGameObjectWithTag("NameText").GetComponent<TextMeshProUGUI>();
        textInBox = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TextMeshProUGUI>();
        fadeAnim = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        CG = GameObject.FindGameObjectWithTag("CG").GetComponent<RawImage>();

        getBatches = dialogueText.text.Split("\n*--*\n").ToList();
        foreach (string batch in getBatches)
        {
            dialougeBatches.Add(batch.Split('\n'));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.HasTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.HasTag("Player") && !collision.isTrigger)
        {
            playerInRange = false;
        }
    }

    public void PlayDialogue()
    {
        StartCoroutine(PlayDialogue(forceAllDialogue, false));
    }

    public void PlayDialogueWithFadeOut()
    {
        StartCoroutine(PlayDialogue(forceAllDialogue, true));
    }

    IEnumerator PlayDialogue(bool isForced, bool useFade)
    {
        bool firstLine = true;
        if (player != null && isForced)
        {
            player.ChangeState(PlayerState.forcedReading);
        }
        else if (player != null && !isForced)
        {
            player.ChangeState(PlayerState.unforcedReading);
        }
        int linesRead = 0;
        foreach (string lineInBatch in dialougeBatches[currentBatch])
        {
            linesRead++;

            if (ParseText(lineInBatch))
            {
                continue;
            }

            textInBox.text = lineInBatch;

            textInBox.maxVisibleCharacters = 0;

            if (firstLine)
            {
                yield return null;
                OpenDialogue();
                firstLine = false;
            }

            yield return ScrollDialogue();

            textInBox.maxVisibleCharacters = textInBox.GetTextInfo(textInBox.text).characterCount;

            yield return WaitForInput();

            if (!playerInRange)
            {
                break;
            }
        }

        if (useFade)
        {
            fadeAnim.SetTrigger("Start");
            yield return new WaitForSeconds(1f);
            fadeAnim.ResetTrigger("Start");
            CloseDialogue();
            fadeAnim.SetTrigger("End");
            yield return new WaitForSeconds(1f);
            fadeAnim.ResetTrigger("End");
        }
        else
        {
            CloseDialogue();
        }

        if (player != null)
        {
            player.ChangeState(PlayerState.idle);
        }
        if (linesRead == dialougeBatches[currentBatch].Length)
        {
            timesRead++;
        }
    }

    private void OpenDialogue()
    {
        textBox.enabled = true;
        textInBox.enabled = true;
    }

    private void CloseDialogue()
    {
        CancelInvoke("IncreaseVisable");
        CancelInvoke("ScrollAnimation");
        UnloadImage();
        UnloadName();
        UnloadCG();
        textBox.enabled = false;
        textInBox.enabled = false;
    }
   
    private void LoadImage(string filepath)
    {
        textPosition.sizeDelta = new Vector2(100, 50);
        characterPortrait.texture = Resources.Load<Texture>(filepath);
        characterPortrait.enabled = true;
    }
    private void LoadImage(Texture2D texture)
    {
        textPosition.sizeDelta = new Vector2(100, 50);
        characterPortrait.texture = texture;
        characterPortrait.enabled = true;
    }

    private void LoadCG(string filePath)
    {
        CG.texture = Resources.Load<Texture>(filePath);
        CG.enabled = true;
    }

    private void UnloadCG()
    {
        CG.enabled = false;
    }

    private void LoadName(string name)
    {
        nameText.text = name;
        nameBox.enabled = true;
        nameText.enabled = true;
    }

    private void UnloadImage()
    {
        characterPortrait.enabled = false;
        textPosition.sizeDelta = new Vector2(200, 50);
    }

    private void UnloadName()
    {
        nameBox.enabled = false;
        nameText.enabled = false;
    }

    public bool DialogueOver()
    {
        return timesRead > 0;
    }

    public void SetBatch(int batch)
    {
        currentBatch = batch;
    }

    public void IncrementBatch()
    {
        currentBatch++;
    }

    private bool ParseText(string line)
    {
        if (line.StartsWith("//"))
        {
            return true;
        }
        else if (line.StartsWith("[NAME]"))
        {
            string name = line.Substring(7);
            if (name == "NONE")
            {
                UnloadName();
                return true;
            }
            LoadName(name);
            return true;
        }
        else if (line.StartsWith("[IMG]"))
        {
            string filepath = line.Substring(6);
            CancelInvoke("ScrollAnimation");
            if (filepath == "NONE")
            {
                UnloadImage();
                return true;
            }
            LoadImage(filepath);
            return true;
        }
        else if (line.StartsWith("[ANIM]"))
        {
            line = line.Substring(7);
            CancelInvoke("ScrollAnimation");
            string[] pathAndDelay = line.Split(':');
            animationFrames = Resources.LoadAll<Texture2D>(pathAndDelay[0]);
            float delay = float.Parse(pathAndDelay[1]);
            currentFrame = 0;
            InvokeRepeating("ScrollAnimation", 0, delay);
            return true;
        }
        else if (line.StartsWith("[CG]")) 
        {
            string filepath = line.Substring(5);
            LoadCG(filepath);
            return true;
        }
        else if (line.StartsWith("[SPEED]"))
        {
            scrollSpeed = float.Parse(line.Substring(7));
            return true;
        }
        return false;
    }

    IEnumerator WaitForInput()
    {
        while (!nextLine && playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nextLine = true;
            }
            yield return null;
        }
        nextLine = false;
    }

    void IncreaseVisable()
    {
        textInBox.maxVisibleCharacters++;
    }

    IEnumerator ScrollDialogue()
    {
        float finalSpeed = baseSpeed / scrollSpeed;
        InvokeRepeating("IncreaseVisable", 0, finalSpeed);
        while (!nextLine && playerInRange && textInBox.maxVisibleCharacters < textInBox.GetTextInfo(textInBox.text).characterCount)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nextLine = true;
            }
            yield return null;
        }
        CancelInvoke("IncreaseVisable");
        nextLine = false;
        yield return null;
    }

    void ScrollAnimation()
    {
        if (currentFrame < animationFrames.Count())
        {
            LoadImage(animationFrames[currentFrame++]);
        }
        else
        {
            CancelInvoke("ScrollAnimation");
        }
    }
}