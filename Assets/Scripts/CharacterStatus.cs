using UnityEngine;
using System.Collections;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CharacterStatus : MonoBehaviour {

    //---------- 공격 장에서 사용한다. ----------
    // 체력.
    public int HP = 100;
    public int MaxHP = 100;

    // 공격력.
    public int Power = 10;
    
    // 마지막에 공격한 대상.
    public GameObject lastAttackTarget = null;
    
    //---------- GUI 및 네트워크 장에서 사용한다. ----------
    // 플레이어 이름.
    public string characterName = "Player";
    
    //--------- 애니메이션 장에서 사용한다. -----------
    // 상태.
    public bool attacking = false;
    public bool died = false;

    // 공격력 강화.
    public bool powerBoost = false;
    // 공격력 강화 시간.
    float powerBoostTime = 0.0f;

    // 아이템 갯수
    public int powerUpItem;
    public float powerUpItemCoolingTime = 0.0f;
    public int healItem;
    public float healItemCoolingTime = 0.0f;
    public float itemCooingTime = 5.0f;

    // 아이템 사용 효과음
    public AudioClip itemPowerUpSeClip;
    public AudioClip itemHealSeClip;

    // 공격력 강화 효과.
    ParticleSystem powerUpEffect;

    // 아이템 획득.
    public void GetItem(DropItem.ItemKind itemKind)
    {
        switch (itemKind)
        {
            case DropItem.ItemKind.Attack:
                powerUpItem++;
                break;
            case DropItem.ItemKind.Heal:
                healItem++;
                break;
        }
    }

    // 파워업
    public void PowerUp()
    {
        if(powerUpItem > 0 && powerUpItemCoolingTime <= 0.0f)
        {
            powerBoostTime = 10.0f;
            powerUpEffect.Play();
            // 오디오 재생.
            AudioSource.PlayClipAtPoint(itemPowerUpSeClip, transform.position);
            powerUpItem--;
            powerUpItemCoolingTime = itemCooingTime;
        }
    }

    // 회복
    public void Heal()
    {
        if (healItem > 0 && healItemCoolingTime <= 0.0f)
        {
            // MaxHP의 절반 회복.
            HP = Mathf.Min(HP + MaxHP / 2, MaxHP);
            // 오디오 재생.
            AudioSource.PlayClipAtPoint(itemHealSeClip, transform.position);
            healItem--;
            healItemCoolingTime = itemCooingTime;
        }
    }

    void Start()
    {
        if (gameObject.tag == "Player")
        {
            powerUpEffect = transform.Find("PowerUpEffect").GetComponent<ParticleSystem>();
            powerUpItem = 0;
            healItem = 0;
        }
    }
 
    void Update()
    {
        if (gameObject.tag != "Player")
        {
            return;
        }
        powerBoost = false;
        if (powerBoostTime > 0.0f)
        {
            powerBoost = true;
            powerBoostTime = Mathf.Max(powerBoostTime - Time.deltaTime, 0.0f);
        }
        else
        {
            powerUpEffect.Stop();
        }

        if(powerUpItemCoolingTime > 0.0f)
        {
            powerUpItemCoolingTime -= Time.deltaTime;

            if(powerUpItemCoolingTime < 0.0f)
                powerUpItemCoolingTime = 0.0f;
        }

        if (healItemCoolingTime > 0.0f)
        {
            healItemCoolingTime -= Time.deltaTime;

            if(healItemCoolingTime < 0.0f)
                healItemCoolingTime = 0.0f;
        }
    }

}
