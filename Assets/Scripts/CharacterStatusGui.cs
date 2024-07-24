using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusGui : MonoBehaviour
{
    float baseWidth = 854f;
    float baseHeight = 480f;

    // 스테이터스.
    CharacterStatus playerStatus;
    Vector2 playerStatusOffset = new Vector2(8f, 60f);

    // 이름.
    Rect nameRect = new Rect(0f, 0f, 120f, 24f);
    public GUIStyle playerNameLabelStyle;
    public GUIStyle enemyNameLabelStyle;

    // 라이프바.
    public Texture backLifeBarTexture;
    public Texture frontLifeBarTexture;
    float frontLifeBarOffsetX = 2f;
    float lifeBarTextureWidth = 128f;
    Rect playerLifeBarRect = new Rect(0f, 0f, 128f, 16f);
    Color playerFrontLifeBarColor = Color.green;
    Rect enemyLifeBarRect = new Rect(0f, 0f, 128f, 24f);
    Color enemyFrontLifeBarColor = Color.red;

    // 아이템창
    public Texture quickSlot;
    public Texture powerUpItem;
    public Texture healItem;

    public Texture itemCoolingTime;

    // 플레이어 스테이터스 표시.
    void DrawPlayerStatus()
    {
        float x = playerStatusOffset.x;
        float y = playerStatusOffset.y;
        DrawCharacterStatus(
            x, y,
            playerStatus,
            playerLifeBarRect,
            playerFrontLifeBarColor,
            playerNameLabelStyle);
    }

    // 적 스테이터스 표시.
    void DrawEnemyStatus()
    {
        if (playerStatus.lastAttackTarget != null)
        {
            CharacterStatus target_status = playerStatus.lastAttackTarget.GetComponent<CharacterStatus>();
            DrawCharacterStatus(
                (baseWidth - enemyLifeBarRect.width) / 2.0f, 0f,
                target_status,
                enemyLifeBarRect,
                enemyFrontLifeBarColor,
                enemyNameLabelStyle);
        }
    }

    // 캐릭터 스테이터스 표시.
    void DrawCharacterStatus(float x, float y, CharacterStatus status, Rect bar_rect, Color front_color, GUIStyle style)
    {
        // 이름.
        GUI.Label(
            new Rect(x, y, nameRect.width, nameRect.height),
            status.characterName,
            style);

        float life_value = (float)status.HP / status.MaxHP;
        if (backLifeBarTexture != null)
        {
            // 후면 라이프바.
            y += nameRect.height;
            GUI.DrawTexture(new Rect(x, y, bar_rect.width, bar_rect.height), backLifeBarTexture);
        }

        // 전면 라이프바.
        if (frontLifeBarTexture != null)
        {
            float resize_front_bar_offset_x = frontLifeBarOffsetX * bar_rect.width / lifeBarTextureWidth;
            float front_bar_width = bar_rect.width - resize_front_bar_offset_x * 2;
            var gui_color = GUI.color;
            GUI.color = front_color;
            GUI.DrawTexture(new Rect(x + resize_front_bar_offset_x, y, front_bar_width * life_value, bar_rect.height), frontLifeBarTexture);
            GUI.color = gui_color;
        }
    }

    // 아이템 퀵슬롯 표시
    void ItemQuickSlot()
    {
        GUI.DrawTexture(
            new Rect(10.0f, baseHeight - 60.0f, 50.0f, 50.0f),
            quickSlot);
        GUI.DrawTexture(
            new Rect(60.0f, baseHeight - 60.0f, 50.0f, 50.0f),
            quickSlot);

        var gui_color = GUI.color;

        GUI.color = Color.gray;
        GUI.Label(new Rect(15.0f, baseHeight - 30.0f, 20.0f, 20.0f),
            "1");
        GUI.Label(new Rect(65.0f, baseHeight - 30.0f, 20.0f, 20.0f),
            "2");
        GUI.color = gui_color;

        if(playerStatus.powerUpItem > 0)
        {
            GUI.DrawTexture(
                new Rect(15.0f, baseHeight - 55.0f, 40.0f, 40.0f),
                powerUpItem);
        }
        else
        {
            GUI.color = new Color(0.3f, 0.3f, 0.3f);
            GUI.DrawTexture(
                new Rect(15.0f, baseHeight - 55.0f, 40.0f, 40.0f),
                powerUpItem);
            GUI.color = gui_color;
        }

        if (playerStatus.healItem > 0)
        {
            GUI.DrawTexture(
                new Rect(65.0f, baseHeight - 55.0f, 40.0f, 40.0f),
                healItem);
        }
        else
        {
            GUI.color = new Color(0.3f, 0.3f, 0.3f);
            GUI.DrawTexture(
                new Rect(65.0f, baseHeight - 55.0f, 40.0f, 40.0f),
                healItem);
            GUI.color = gui_color;
        }

        GUI.color = Color.black;
        GUI.Label(new Rect(48.0f, baseHeight - 30.0f, 20.0f, 20.0f),
            playerStatus.powerUpItem.ToString());
        GUI.Label(new Rect(98.0f, baseHeight - 30.0f, 20.0f, 20.0f),
            playerStatus.healItem.ToString());
        GUI.color = gui_color;

        GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.4f);
        if (playerStatus.powerUpItemCoolingTime > 0.0f)
        {
            float height = playerStatus.powerUpItemCoolingTime / playerStatus.itemCooingTime;

            GUI.DrawTexture(
                new Rect(15.0f, baseHeight - 15.0f - (40.0f * height), 40.0f, 40.0f * height),
                itemCoolingTime);

        }

        if (playerStatus.healItemCoolingTime > 0.0f)
        {
            float height = playerStatus.healItemCoolingTime / playerStatus.itemCooingTime;

            GUI.DrawTexture(
                new Rect(65.0f, baseHeight - 15.0f - (40.0f * height), 40.0f, 40.0f * height),
                itemCoolingTime);

        }

        GUI.color = gui_color;
    }

    void Awake()
    {
        PlayerCtrl player_ctrl = GameObject.FindObjectOfType(typeof(PlayerCtrl)) as PlayerCtrl;
        playerStatus = player_ctrl.GetComponent<CharacterStatus>();
    }

    void OnGUI()
    {
        // 해상도 대응.
        GUI.matrix = Matrix4x4.TRS(
            Vector3.zero,
            Quaternion.identity,
            new Vector3(Screen.width / baseWidth, Screen.height / baseHeight, 1f));

        // 스테이터스.
        DrawPlayerStatus();
        DrawEnemyStatus();

        if(playerStatus.gameObject.GetComponent<PlayerCtrl>().state != PlayerCtrl.State.Talking)
            ItemQuickSlot();
    }
}