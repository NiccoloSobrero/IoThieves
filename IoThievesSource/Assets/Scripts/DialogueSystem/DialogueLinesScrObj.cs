using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public enum DialogueTag
    {
        None,
        TriggerOnLineEnd,
        TriggerOnLineStart,
        TriggerOnContinueDialogue,
    }
    
    [CreateAssetMenu(menuName = "Dialogue Lines", fileName = "DialogueLines")]
    public class DialogueLinesScrObj : ScriptableObject
    {
        public DialogueLine[] dialogueLines;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public Sprite icon;
        [TextArea(3, 5)] public string text;
        public List<DialogueTag> tags;
    }
}
