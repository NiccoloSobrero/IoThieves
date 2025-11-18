using Events;
using TMPro;
using UnityEngine;

public class TerminalLine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lineText;

    private string _word;
    
    public void Customize(string word, string line)
    {
        _word = word;
        lineText.text = line;
    }

    public void OnClick()
    {
        // Fire event with string solution
        EventsManager.OnClickDecryptPassword.Invoke(_word);
    }
}
