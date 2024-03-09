using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalState : MonoBehaviour
{
    public int animalNum;

    private bool isSpawn = false;

    public bool IsSpawn
    {
        get { return isSpawn; }
        set { isSpawn = value; }
    }

    private int level = 1;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private int price;

    public int Price
    {
        get { return price; }
        set { price = value; }
    }

    private int maxHp;

    public int MaxHp
    {
        get { return maxHp; }
    }

    private int earlyMaxHp;

    public int EarlyMaxHp
    {
        set { earlyMaxHp = value; }
    }

    private int phase;

    public int Phase
    {
        get { return phase; }
        set { phase = value; }
    }

    private int intimacy;

    public int Intimacy
    {
        get { return intimacy; }
        set { intimacy = value; }
    }

    private static int earlyMaxIntimacy = 10;
    private int maxIntimacy;

    public int MaxIntimacy
    {
        get { return maxIntimacy; }
    }

    public static int maxLevel = 11;


    private int basicPlusIntimacy = 1;

    delegate void LevelUpSet();
    LevelUpSet levelUpSet;

    private void Awake()
    {
        isSpawn = true;

        levelUpSet += IntimacyZeroSet;
        levelUpSet += MaxIntimacySet;
        levelUpSet += MaxHpSet;

        MaxHpSet();
        MaxIntimacySet();
    }


    public void IntimacyPlus()
    {
        if (isSpawn == true)
        {
            int plusIntimacy;

            if (EventManager.Instance.IsHeartEvent == true)
            {
                plusIntimacy = basicPlusIntimacy * 3;
                intimacy += plusIntimacy;
            }
            else
            {
                plusIntimacy = basicPlusIntimacy;
                intimacy += plusIntimacy;
            }

            MissionManager.Instance.MissionValueUp("Give intimacy", plusIntimacy);

            if (intimacy >= maxIntimacy && level != maxLevel)
            {
                LevelUp();
            }
        }

        UIManager.Instance.UpdateFollowIntimacy();
    }
    void LevelUp()
    {
        if (isSpawn == true)
        {
            level += 1;
            levelUpSet();
        }

        UIManager.Instance.UpdateFollowLevelText();
        StartCoroutine(ParticleManager.Instance.LevelUpEffect(transform.position, 0.3f));
        //SoundManager.Instance.PlaySFXSound("AnimalLevelUpSound");
    }

    void IntimacyZeroSet()
    {
        if (isSpawn == true)
        {
            if (level <= maxLevel)
            {
                intimacy -= maxIntimacy;
            }
        }
    }

    void MaxIntimacySet()
    {
        if (isSpawn == true)
        {
            maxIntimacy = earlyMaxIntimacy * level;
        }
    }

    void MaxHpSet()
    {
        if (isSpawn == true)
        {
            maxHp = (earlyMaxHp) + ((level - 1) * 10);
        }
    }
}
