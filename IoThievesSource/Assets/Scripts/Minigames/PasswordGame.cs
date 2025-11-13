using TMPro;
using UnityEngine;

public class PasswordGame : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI displayText;
    private int maxPasswordLength = 7; 

    private void Awake()
    {
        displayText.text = "";
    }
    
    public void OnDigitClicked(string digit)
    {
        if (displayText.text.Length < maxPasswordLength)
        {
            displayText.text += digit;
        }
    }
    
    public void ClearText()
    {
        displayText.text = "";
    }

    public void Confirm()
    {
        Debug.Log(displayText.text);
    }
    
}
