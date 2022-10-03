using System.Collections.Generic;
using UnityEngine;

public class GUIMessageHelper : MonoBehaviour
{
    public Queue<string> messages;

    private GUIContent content;
    private GUIStyle style = new GUIStyle();


    public static GUIMessageHelper Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        style.fontSize = 24;
    }

    public static void PrintConsole(string message)
    {
        if (Instance.messages == null)
            Instance.messages = new Queue<string>();


        Instance.messages.Enqueue(message);

        Instance.PrepareMessages();
    }

    private void DeleteMessage()
    {

    }

    private void PrepareMessages()
    {
        content = new GUIContent();

        while(messages.Count > 5)
        {
            messages.Dequeue();
        }

        string[] _arr = messages.ToArray();

        for (int i = 0; i < messages.Count; i++)
        {
            content.text += _arr[i];
            if (i != messages.Count - 1)
                content.text += "\n";
        }
    }

    private void OnGUI()
    {
        if(content != null && content.text != string.Empty)
        {
            GUI.Label(new Rect(32, 32, 250, 24 * messages.Count), content, style);
        }
    }
}
