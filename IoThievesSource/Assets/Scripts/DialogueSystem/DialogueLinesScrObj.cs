using UnityEngine;

namespace DialogueSystem
{
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
        [TextArea(3, 5)]
        public string text;
    }
}
