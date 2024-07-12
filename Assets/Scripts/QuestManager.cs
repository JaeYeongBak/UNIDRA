using System.Collections;
using UnityEngine;

struct Quest
{
    public string target;
    public int num;
}

public class QuestManager : MonoBehaviour
{
    float baseWidth = 854f;
    float baseHeight = 480f;

    Quest quest;

    public bool openWindow = false;
    public GameObject player;
    public Texture qWindow;

    public void startQuest(string target, int num)
    {
        quest.target = target;
        quest.num = num;
    }

    public void endQuest()
    {
        quest.target = "";
        quest.num = 0;
    }

    void OpenQuestWindow()
    {
        if (openWindow)
        {
            Rect rect = new Rect((Screen.width / 2.0f) - (qWindow.width / 2.0f),
                (Screen.height / 2.0f) - (qWindow.height / 2.0f),
                Screen.width / 3.0f, Screen.height / 1.5f);
            GUI.DrawTexture(rect, qWindow);
        }
    }

    private void Awake()
    {
        // 해상도 대응.
        GUI.matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));
    }

    private void OnGUI()
    {
        OpenQuestWindow();
    }
}
