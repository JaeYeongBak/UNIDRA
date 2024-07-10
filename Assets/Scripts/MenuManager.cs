using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    InputManager inputManager;
    public AudioClip clickSEClip;
    AudioSource clickSESource;
    bool activateMenu;

    void ActiveMenu()
    {
        if (activateMenu && inputManager.MenuKeyDown())
        {
            activateMenu = false;
            Time.timeScale = 1.0f;

            return;
        }
    }

    void DisabledMenu()
    {
        if (!activateMenu && inputManager.MenuKeyDown())
        {
            activateMenu = true;
            Time.timeScale = 0.0f;

            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 오디오 초기화
        clickSESource = gameObject.AddComponent<AudioSource>();
        clickSESource.loop = false;
        clickSESource.clip = clickSEClip;

        inputManager = FindObjectOfType<InputManager>();
        activateMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(activateMenu)
            ActiveMenu();
        else
            DisabledMenu();
    }

    void OnGUI()
    {
        if(activateMenu)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

            GUI.matrix = Matrix4x4.TRS(
                Vector3.zero,
                Quaternion.identity,
                new Vector3(Screen.width / 854.0f, Screen.height / 480.0f, 1.0f));

            if (GUI.Button(new Rect(327, 290, 200, 54), "Title", buttonStyle))
            {
                Application.LoadLevel("TitleScene");
            }

            if (GUI.Button(new Rect(327, 220, 200, 54), "Save", buttonStyle))
            {
                clickSESource.Play();

            }

            if (GUI.Button(new Rect(327, 150, 200, 54), "Load", buttonStyle))
            {
                clickSESource.Play();

            }
        }
    }
}
