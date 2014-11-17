using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public abstract class CharacterStatus : MonoBehaviour
{
    #region EnumType
    protected enum GrowRateType
    {
        Main,
        Secondary,
        Third,
        Bad
    }
    #endregion

    #region Struct
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
    #endregion

    #region Static Variable
    static TierDetail tierLv1,
        tierLv2,
        tierLv3;
    #endregion

    #region Static Method
    public static void SetInitTier()
    {
        tierLv1 = new TierDetail(200, 100, 10, 6, 5, 4, 3, 50);
        tierLv2 = new TierDetail(300, 150, 15, 7, 6, 5, 4, 55);
        tierLv3 = new TierDetail(400, 200, 20, 8, 7, 6, 5, 60);
    }
    #endregion

    #region Variable
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
    #endregion

    #region Properties
    public int Lv
    {
        get { return lv; }
        set { lv = value; }
    }

    public int TierLv
    {
        get { return tierLv; }
        set { tierLv = value; }
    }

    public int CurrentExp
    {
        get { return currentExp; }
        set { currentExp = value; }
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
    #endregion

    #region Method
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

    protected void SetInitAndIncreasePerLvValueInTierLv()
    {
        if (TierLv > 1)
        {
            int increaseValuePerLvSword = 0,
                increaseValuePerLvBow = 0,
                increaseValuePerLvWand = 0,
                increaseValuePerLvShield = 0,
                increaseValuePerLvScroll = 0,
                increaseHpPerLv = 0;

            //IncreaseValuePerLvSword
            switch(swordGrowRate)
            {
                case GrowRateType.Main:
                    increaseValuePerLvSword = tierLv1.growRateMain;
                    break;
                case GrowRateType.Secondary:
                    increaseValuePerLvSword = tierLv1.growRateSecondary;
                    break;
                case GrowRateType.Third:
                    increaseValuePerLvSword = tierLv1.growRateThird;
                    break;
                case GrowRateType.Bad:
                    increaseValuePerLvSword = tierLv1.growRateBad;
                    break;
            }

            //IncreaseValuePerLvBow
            switch (bowGrowRate)
            {
                case GrowRateType.Main:
                    increaseValuePerLvBow = tierLv1.growRateMain;
                    break;
                case GrowRateType.Secondary:
                    increaseValuePerLvBow = tierLv1.growRateSecondary;
                    break;
                case GrowRateType.Third:
                    increaseValuePerLvBow = tierLv1.growRateThird;
                    break;
                case GrowRateType.Bad:
                    increaseValuePerLvBow = tierLv1.growRateBad;
                    break;
            }

            //IncreaseValuePerLvWand
            switch (wandGrowRate)
            {
                case GrowRateType.Main:
                    increaseValuePerLvWand = tierLv1.growRateMain;
                    break;
                case GrowRateType.Secondary:
                    increaseValuePerLvWand = tierLv1.growRateSecondary;
                    break;
                case GrowRateType.Third:
                    increaseValuePerLvWand = tierLv1.growRateThird;
                    break;
                case GrowRateType.Bad:
                    increaseValuePerLvWand = tierLv1.growRateBad;
                    break;
            }

            //IncreaseValuePerLvShield
            switch (shieldGrowRate)
            {
                case GrowRateType.Main:
                    increaseValuePerLvShield = tierLv1.growRateMain;
                    break;
                case GrowRateType.Secondary:
                    increaseValuePerLvShield = tierLv1.growRateSecondary;
                    break;
                case GrowRateType.Third:
                    increaseValuePerLvShield = tierLv1.growRateThird;
                    break;
                case GrowRateType.Bad:
                    increaseValuePerLvShield = tierLv1.growRateBad;
                    break;
            }

            //IncreaseValuePerLvScroll
            switch (scrollGrowRate)
            {
                case GrowRateType.Main:
                    increaseValuePerLvScroll = tierLv1.growRateMain;
                    break;
                case GrowRateType.Secondary:
                    increaseValuePerLvScroll = tierLv1.growRateSecondary;
                    break;
                case GrowRateType.Third:
                    increaseValuePerLvScroll = tierLv1.growRateThird;
                    break;
                case GrowRateType.Bad:
                    increaseValuePerLvScroll = tierLv1.growRateBad;
                    break;
            }

            //IncraseHpPerLv
            increaseHpPerLv = tierLv1.growRateHp;

            //SetInitValueTierLv2
            initValueSword += increaseValuePerLvSword * 5;
            initValueBow += increaseValuePerLvBow * 5;
            initValueWand += increaseValuePerLvWand * 5;
            initValueShield += increaseValuePerLvShield * 5;
            initValueScroll += increaseValuePerLvScroll * 5;
            initHp += increaseHpPerLv * 5;

            if (TierLv > 2)
            {
                //IncreaseValuePerLvSword
                switch (swordGrowRate)
                {
                    case GrowRateType.Main:
                        increaseValuePerLvSword = tierLv2.growRateMain;
                        break;
                    case GrowRateType.Secondary:
                        increaseValuePerLvSword = tierLv2.growRateSecondary;
                        break;
                    case GrowRateType.Third:
                        increaseValuePerLvSword = tierLv2.growRateThird;
                        break;
                    case GrowRateType.Bad:
                        increaseValuePerLvSword = tierLv2.growRateBad;
                        break;
                }

                //IncreaseValuePerLvBow
                switch (bowGrowRate)
                {
                    case GrowRateType.Main:
                        increaseValuePerLvBow = tierLv2.growRateMain;
                        break;
                    case GrowRateType.Secondary:
                        increaseValuePerLvBow = tierLv2.growRateSecondary;
                        break;
                    case GrowRateType.Third:
                        increaseValuePerLvBow = tierLv2.growRateThird;
                        break;
                    case GrowRateType.Bad:
                        increaseValuePerLvBow = tierLv2.growRateBad;
                        break;
                }

                //IncreaseValuePerLvWand
                switch (wandGrowRate)
                {
                    case GrowRateType.Main:
                        increaseValuePerLvWand = tierLv2.growRateMain;
                        break;
                    case GrowRateType.Secondary:
                        increaseValuePerLvWand = tierLv2.growRateSecondary;
                        break;
                    case GrowRateType.Third:
                        increaseValuePerLvWand = tierLv2.growRateThird;
                        break;
                    case GrowRateType.Bad:
                        increaseValuePerLvWand = tierLv2.growRateBad;
                        break;
                }

                //IncreaseValuePerLvShield
                switch (shieldGrowRate)
                {
                    case GrowRateType.Main:
                        increaseValuePerLvShield = tierLv2.growRateMain;
                        break;
                    case GrowRateType.Secondary:
                        increaseValuePerLvShield = tierLv2.growRateSecondary;
                        break;
                    case GrowRateType.Third:
                        increaseValuePerLvShield = tierLv2.growRateThird;
                        break;
                    case GrowRateType.Bad:
                        increaseValuePerLvShield = tierLv2.growRateBad;
                        break;
                }

                //IncreaseValuePerLvScroll
                switch (scrollGrowRate)
                {
                    case GrowRateType.Main:
                        increaseValuePerLvScroll = tierLv2.growRateMain;
                        break;
                    case GrowRateType.Secondary:
                        increaseValuePerLvScroll = tierLv2.growRateSecondary;
                        break;
                    case GrowRateType.Third:
                        increaseValuePerLvScroll = tierLv2.growRateThird;
                        break;
                    case GrowRateType.Bad:
                        increaseValuePerLvScroll = tierLv2.growRateBad;
                        break;
                }

                //IncraseHpPerLv
                increaseHpPerLv = tierLv2.growRateHp;

                //SetInitValueTierLv2
                initValueSword += increaseValuePerLvSword * 10;
                initValueBow += increaseValuePerLvBow * 10;
                initValueWand += increaseValuePerLvWand * 10;
                initValueShield += increaseValuePerLvShield * 10;
                initValueScroll += increaseValuePerLvScroll * 10;
                initHp += increaseHpPerLv * 10;
            }
        }
    }

    protected virtual void Awake()
    {
        SetInitValue();
        SetGrowRate();
        TierLv = PlayerPrefs.GetInt(characterName + "TierLv", 1);

        switch (TierLv)
        {
            case 1:
                tierDetail = tierLv1;
                break;
            case 2:
                tierDetail = tierLv2;
                break;
            case 3:
                tierDetail = tierLv3;
                break;
            default:
                tierDetail = tierLv1;
                break;
        }

        SetInitAndIncreasePerLvValueInTierLv();

        nextLvExp = (lv == tierDetail.maxLv) ? 0 : tierDetail.initNextLvExp + (lv - 1) * tierDetail.increaseNextLvExp;

        Lv = PlayerPrefs.GetInt(characterName + "Lv", 1);
        CurrentExp = PlayerPrefs.GetInt(characterName + "CurrentExp", 0);
        swordValue = initValueSword + (lv - 1) * increaseValuePerLvSword;
        bowValue = initValueBow + (lv - 1) * increaseValuePerLvBow;
        wandValue = initValueWand + (lv - 1) * increaseValuePerLvWand;
        shiedlValue = initValueShield + (lv - 1) * increaseValuePerLvShield;
        scrollValue = initValueScroll + (lv - 1) * increaseValuePerLvScroll;
        hp = initHp + (lv - 1) * increaseHpPerLv;
    }

    // Use this for initialization
    protected virtual void Start()
    {
    }
    #endregion
}
