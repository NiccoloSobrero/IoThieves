using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class PasswordDecryptorManager : MonoBehaviour
{
    [SerializeField] private TerminalLine word1;
    [SerializeField] private TerminalLine word2;
    [SerializeField] private TerminalLine word3;

    [SerializeField] private string solution;
    
    private List<char> specialChars = new List<char>(){'&', '!', '?', '$', '%', '=', '[', ']', '\"', '*', '{', '}', 
        ':', ';', '-', '+', '@', '#', '(', ')', '\\', '^', '|', '\'', '~', '<', '>', ','};

    private List<char> addressChars = new() {'C', 'F', 'B', 'A', 'D', 'E', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    
    [SerializeField] private Transform parent;
    [SerializeField] private int line_len;
    [SerializeField] private int addr_len;
    

    private void OnEnable()
    {
        EventsManager.OnClickDecryptPassword.AddListener(CheckSolution);
    }

    private void OnDisable()
    {
        EventsManager.OnClickDecryptPassword.RemoveListener(CheckSolution);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Customize();
        }
    }

    private void CheckSolution(string solution)
    {
        // Print on screen the result -> pass or fail (How many correct letters)
        
    }

    private void Customize()
    {
        foreach (Transform child in parent)
        {
            TextMeshProUGUI address = child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            TextMeshProUGUI line = address.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

            string newLine = "";
            for (int i = 0; i < line_len; i++)
            {
                newLine += specialChars[Random.Range(0, specialChars.Count)];
            }
            line.text = newLine;

            string newAddr = "0x";
            for (int i = 0; i < addr_len; i++)
            {
                newAddr += addressChars[Random.Range(0, addressChars.Count)];
            }
            address.text = newAddr;
        }
    }
    
    

    
}
