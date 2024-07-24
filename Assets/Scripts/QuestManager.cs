using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Quest
{
    public string title;
    public string story;
    public string target;
    public int cNum;
    public int tNum;
}

public class QuestManager : MonoBehaviour
{
    float baseWidth = 854f;
    float baseHeight = 480f;

    InputManager inputManager;

    public Quest quest;

    public bool openWindow = false;
    public GameObject player;
    public Texture qWindow;
    public GUIStyle titleLabelStyle;
    public GUIStyle storyLabelStyle;
    public GUIStyle numLabelStyle;

    public void startQuest(string title, string story, string target, int num)
    {
        quest.title = title;
        quest.story = story;
        quest.target = target;
        quest.cNum = 0;
        quest.tNum = num;
    }

    public void endQuest()
    {
        quest.title = "";
        quest.story = "";
        quest.target = "";
        quest.cNum = 0;
        quest.tNum = 0;
    }

    public void QuestCountUp(string target)
    {
        if (target == quest.target & quest.cNum != quest.tNum)
        {
            quest.cNum++;
        }
    }

    public void OpenQuestWindow()
    {
        if (openWindow)
        {
            Rect windowRect = new Rect(baseWidth / 2.0f - 150.0f, baseHeight / 2.0f - 150.0f, 300.0f, 300.0f);
            Rect titleRect = new Rect(baseWidth / 2.0f - 150.0f, baseHeight / 2.0f - 130.0f, 300.0f, 50.0f);
            Rect storyRect = new Rect(baseWidth / 2.0f - 150.0f, baseHeight / 2.0f - 80.0f, 300.0f, 180.0f);
            Rect numRect = new Rect(baseWidth / 2.0f - 150.0f, baseHeight / 2.0f + 70.0f, 300.0f, 50.0f);

            GUI.DrawTexture(windowRect, qWindow);
            //GUI.DrawTexture(titleRect, qWindow);
            //GUI.DrawTexture(storyRect, qWindow);
            //GUI.DrawTexture(numRect, qWindow);

            if (quest.title == "")
            {
                GUI.Label(titleRect, "퀘스트가 없습니다", titleLabelStyle);
            }
            else
            {
                GUI.Label(titleRect, quest.title, titleLabelStyle);
                GUI.Label(storyRect, quest.story, storyLabelStyle);
                GUI.Label(numRect, "       " + quest.target + " : " + quest.cNum + " / " + quest.tNum, numLabelStyle);
            }

        }
    }

    private void Update()
    {
        if (inputManager.KeyDown() == KeyCode.Q)
        {
            openWindow = !openWindow;
        }
    }

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();

        endQuest();
    }

    //private void OnGUI()
    //{
    //    // 해상도 대응.
    //    GUI.matrix = Matrix4x4.TRS(
    //        Vector3.zero,
    //        Quaternion.identity,
    //        new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));

    //    OpenQuestWindow();
    //}
}
