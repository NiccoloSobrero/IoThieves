using TMPro;
using UnityEngine;

public class InputFieldManager : MonoBehaviour
{
    public TMP_InputField inputField;
    
    [SerializeField] private TextMeshProUGUI displayedText;
    [SerializeField] private string target;
    
    [SerializeField] private int maxCharacters;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(CheckTarget);
        
        displayedText.text = target;
    }

    private void CheckTarget(string value)
    {
        if (inputField.text.Length > maxCharacters)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            
            inputField.caretPosition = inputField.text.Length;
        }
        
        if (target == "")
        {
            displayedText.text = inputField.text;
            
            return;
        }

        if (inputField.text.Length > target.Length)
        {
            inputField.text = inputField.text.Remove(inputField.text.Length - 1);
            
            inputField.caretPosition = inputField.text.Length;
        }

        string result = "";

        for (int i = 0; i < target.Length; i++)
        {
            if (i < inputField.text.Length)
            {
                char currTextChar = inputField.text[i];
                char currTargetChar = target[i];
            
                if (currTextChar == currTargetChar)
                {
                    result += "<color=green>" + currTargetChar + "</color>";
                }
                else
                {
                    result += "<color=red>" + currTargetChar + "</color>";
                }
            }
            else
            {
                result += target[i];
            }
        }
        
        displayedText.text = result;
    }
}
