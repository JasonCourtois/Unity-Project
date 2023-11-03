using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueList : ScriptableObject
{
    public List<string> defaultDialogue;

    public List<string> preAcceptQuest;
    public List<string> postAcceptQuest;
    public List<string> finishedQuest;
}
