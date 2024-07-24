using System;
using System.Collections;
using UnityEngine;

public class NPCCtrl : MonoBehaviour
{
    [Serializable]
    public struct ArrayString
    {
        public string[] String;
    }    

    enum TALKNUM
    {
        NORMAL,
        QUESTPLAYING,
        QUESTCLEAR,
        CLEAR,
    }

    float baseWidth = 854f;
    float baseHeight = 480f;

    InputManager inputManager;
    NPCAnimator animator;
    QuestManager questManager;
    PlayerCtrl playerCtrl;
    GameRuleCtrl gameRuleCtrl;

    public Quest[] quest;
    int questCount = 0;

    public Camera mainCam;

    public string chaName;

    public ArrayString[] talks;
    public GUIStyle nameLabelStyle;
    public GUIStyle talkLabelStyle;

    TALKNUM talkNum = TALKNUM.NORMAL;
    int talkCount = 0;

    public bool isTalk = false;

    public Texture talkTexture;

    public AudioClip clickSeClip;
    AudioSource clickSeAudio;

    public void ClickNPC(PlayerCtrl playerCtrl)
    {
        if (questManager.quest.tNum == questManager.quest.cNum &&
            talkNum == TALKNUM.QUESTPLAYING)
        {
            if(questManager.quest.target == "Dragon")
            {
                talkNum = TALKNUM.CLEAR;
            }
            else
            {
                talkNum = TALKNUM.QUESTCLEAR;
            }
        }

        clickSeAudio.Play();

        animator.setIsTalk();
        isTalk = true;
        this.playerCtrl = playerCtrl;
        mainCam.cullingMask = mainCam.cullingMask & ~(1 << 12) & ~(1 << 14);
    }

    public void EndTalk()
    {
        animator.setIsTalk();
        isTalk = false;
        talkCount = 0;

        mainCam.cullingMask = mainCam.cullingMask | 1 << 12 | 1 << 14;

        if (talkNum == TALKNUM.NORMAL || talkNum == TALKNUM.QUESTCLEAR)
        {
            if(inputManager.KeyDown() != KeyCode.Escape)
                talkNum = TALKNUM.QUESTPLAYING;
        }
    }

    public void DrowTalk()
    {
        if (isTalk)
        {
            Color gui_color = GUI.color;

            GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.3f);
            GUI.DrawTexture(new Rect(0.0f, baseHeight - 170.0f, baseWidth, 170.0f), talkTexture);
            GUI.color = gui_color;

            Rect nameRect = new Rect(20.0f, baseHeight - 150.0f, 300.0f, 30.0f);
            Rect talkRect = new Rect(20.0f, baseHeight - 110.0f, 300.0f, 180.0f);

            GUI.Label(nameRect, chaName, nameLabelStyle);
            GUI.Label(talkRect, talks[(int)talkNum].String[talkCount], talkLabelStyle);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<NPCAnimator>();
        inputManager = FindObjectOfType<InputManager>();
        questManager = FindObjectOfType<QuestManager>();
        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();

        // 오디오 초기화
        clickSeAudio = gameObject.AddComponent<AudioSource>();
        clickSeAudio.loop = false;
        clickSeAudio.clip = clickSeClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTalk)
        {
            if (inputManager.KeyDown() == KeyCode.Escape)
            {
                EndTalk();
            }
            else if (inputManager.KeyDown() == KeyCode.Space ||
                inputManager.KeyDown() == KeyCode.Return)
            {
                if (talks[(int)talkNum].String.Length <= ++talkCount)
                {
                    if(talkNum == TALKNUM.NORMAL || talkNum == TALKNUM.QUESTCLEAR)
                    {
                        questManager.quest = quest[questCount++];
                    }
                    else if(talkNum == TALKNUM.CLEAR)
                    {
                        gameRuleCtrl.GameClear();
                    }

                    EndTalk();
                }
            }
        }
    }

    //private void OnGUI()
    //{
    //    // 해상도 대응.
    //    GUI.matrix = Matrix4x4.TRS(
    //        Vector3.zero,
    //        Quaternion.identity,
    //        new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));

    //    DrowTalk();
    //}
}
