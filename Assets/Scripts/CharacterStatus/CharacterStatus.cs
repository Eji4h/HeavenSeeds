using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class CharacterStatus : MonoBehaviour
{
    protected enum GrowRateType
    {
        Main,
        Secondary,
        Third,
        Bad
    }

    struct TierDetail
    {
        public readonly int initNextLvExp,
            increaseNextLvExp,
            maxLv;

        public readonly int growRateMain,
            growRateSecondary,
            growRateThird,
            growRateBad,
            growRateHp;

        public TierDetail(int initNextLvExp, int increaseNextLvExp, int maxLv,
            int growRateMain, int growRateSecondary, int growRateThird,
            int growRateBad, int growRateHp)
        {
            this.initNextLvExp = initNextLvExp;
            this.increaseNextLvExp = increaseNextLvExp;
            this.maxLv = maxLv;

            this.growRateMain = growRateMain;
            this.growRateSecondary = growRateSecondary;
            this.growRateThird = growRateThird;
            this.growRateBad = growRateBad;
            this.growRateHp = growRateHp;
        }
    }

    static TierDetail tierLv1,
        tierLv2;

    public static void SetInitTier()
    {
        tierLv1 = new TierDetail(200, 100, 10, 6, 5, 4, 3, 50);
        tierLv2 = new TierDetail(300, 150, 15, 7, 6, 5, 4, 55);
    }

    TierDetail tierDetail;

    int initValueSword,
        initValueBow,
        initValueWand,
        initValueShield,
        initValueScroll,
        initHp;

    int increaseValuePerLvSword,
        increaseValuePerLvBow,
        increaseValuePerLvWand,
        increaseValuePerLvShield,
        increaseValuePerLvScroll,
        increaseHpPerLv;

    GrowRateType swordGrowRate,
        bowGrowRate,
        wandGrowRate,
        shieldGrowRate,
        scrollGrowRate;

    int nextLvExp;

    int lv,
        tierLv,
        currentExp,
        swordValue,
        bowValue,
        wandValue,
        shiedlValue,
        scrollValue,
        hp;

    string characterName;

    public int Lv
    {
        get { return lv; }
        set 
        {
            lv = value;
            PlayerPrefs.SetInt(characterName + "Lv", lv);
        }
    }

    public int TierLv
    {
        get { return tierLv; }
    }

    public int CurrentExp
    {
        get { return currentExp; }
        set 
        {
            currentExp = value; 
            while (nextLvExp > 0 && 
                currentExp >= nextLvExp)
            {
                currentExp -= nextLvExp;
                Lv++;
            }
            PlayerPrefs.SetInt(characterName + "CurrentExp", currentExp);
        }
    }

    public int SwordValue
    {
        get { return swordValue; }
    }

    public int BowValue
    {
        get { return bowValue; }
    }

    public int WandValue
    {
        get { return wandValue; }
    }

    public int ShiedlValue
    {
        get { return shiedlValue; }
    }

    public int ScrollValue
    {
        get { return scrollValue; }
    }

    public int Hp
    {
        get { return hp; }
    }

    protected abstract void SetInitValue();

    protected void SetInitValue(string characterName,
        int initValueSword, int initValueBow, int initValueWand,
        int initValueShield, int initValueScroll, int initHp)
    {
        this.characterName = characterName;
        this.initValueSword = initValueSword;
        this.initValueBow = initValueBow;
        this.initValueWand = initValueWand;
        this.initValueShield = initValueShield;
        this.initValueScroll = initValueScroll;
        this.initHp = initHp;
    }

    protected abstract void SetGrowRate();

    protected void SetGrowRate(GrowRateType swordGrowRate,
        GrowRateType bowGrowRate,
        GrowRateType wandGrowRate,
        GrowRateType shieldGrowRate,
        GrowRateType scrollGrowRate)
    {
        this.swordGrowRate = swordGrowRate;
        this.bowGrowRate = bowGrowRate;
        this.wandGrowRate = wandGrowRate;
        this.shieldGrowRate = shieldGrowRate;
        this.scrollGrowRate = scrollGrowRate;
    }

    void SetInitAndIncreasePerLvValueInTierLv()
    {
        if (TierLv > 1)
        {
            int lvFromLowerTier;

            //SetIncreasePerLvTier2
            SetIncreaseValuePerLv(out increaseValuePerLvSword, swordGrowRate, tierLv1);
            SetIncreaseValuePerLv(out increaseValuePerLvBow, bowGrowRate, tierLv1);
            SetIncreaseValuePerLv(out increaseValuePerLvWand, wandGrowRate, tierLv1);
            SetIncreaseValuePerLv(out increaseValuePerLvShield, shieldGrowRate, tierLv1);
            SetIncreaseValuePerLv(out increaseValuePerLvScroll, scrollGrowRate, tierLv1);
            increaseHpPerLv = tierLv1.growRateHp;

            //SetInitValueTier2
            lvFromLowerTier = 5;
            initValueSword += increaseValuePerLvSword * lvFromLowerTier;
            initValueBow += increaseValuePerLvBow * lvFromLowerTier;
            initValueWand += increaseValuePerLvWand * lvFromLowerTier;
            initValueShield += increaseValuePerLvShield * lvFromLowerTier;
            initValueScroll += increaseValuePerLvScroll * lvFromLowerTier;
            initHp += increaseHpPerLv * lvFromLowerTier;
        }
    }

    void SetIncreaseValuePerLv(out int itemIncreaseValuePerLv, GrowRateType growRate, TierDetail tierLvDetail)
    {
        switch(growRate)
        {
            case GrowRateType.Main:
                itemIncreaseValuePerLv = tierLvDetail.growRateMain;
                break;
            case GrowRateType.Secondary:
                itemIncreaseValuePerLv = tierLvDetail.growRateSecondary;
                break;
            case GrowRateType.Third:
                itemIncreaseValuePerLv = tierLvDetail.growRateThird;
                break;
            case GrowRateType.Bad:
                itemIncreaseValuePerLv = tierLvDetail.growRateBad;
                break;
            default:
                itemIncreaseValuePerLv = tierLvDetail.growRateBad;
                break;
        }
    }

    protected virtual void Awake()
    {
        SetInitValue();
        SetGrowRate();
        tierLv = PlayerPrefs.GetInt(characterName + "TierLvSelected", 1);

        switch (TierLv)
        {
            case 1:
                tierDetail = tierLv1;
                break;
            case 2:
                tierDetail = tierLv2;
                break;
            default:
                tierDetail = tierLv1;
                break;
        }

        SetInitAndIncreasePerLvValueInTierLv();

        int lvDecreaseOne = lv - 1;
        nextLvExp = (lv == tierDetail.maxLv) ? 0 : tierDetail.initNextLvExp + lvDecreaseOne * tierDetail.increaseNextLvExp;

        Lv = PlayerPrefs.GetInt(characterName + "Lv", 1);
        CurrentExp = PlayerPrefs.GetInt(characterName + "CurrentExp", 0);
        swordValue = initValueSword + lvDecreaseOne * increaseValuePerLvSword;
        bowValue = initValueBow + lvDecreaseOne * increaseValuePerLvBow;
        wandValue = initValueWand + lvDecreaseOne * increaseValuePerLvWand;
        shiedlValue = initValueShield + lvDecreaseOne * increaseValuePerLvShield;
        scrollValue = initValueScroll + lvDecreaseOne * increaseValuePerLvScroll;
        hp = initHp + lvDecreaseOne * increaseHpPerLv;
    }
}
