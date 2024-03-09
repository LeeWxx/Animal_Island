using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct GoldBox 
{
    private int phase;

    public int Phase
    {
        get { return phase; }

        set { phase = value; }
    }

    public string name;

    public Sprite goldBoxImage;
    public Vector3 inSlotPos;
    public Vector3 inSlotSacle;
    private int getGoldMaxValue;
    private int getGoldMinValue;

    public AnimationClip coinAnimation;
    public AnimationClip barAnimation;
    public AnimationClip gemAnimation;

    public int GetGoldMaxValue
    {
        get { return getGoldMaxValue; }
        set { getGoldMaxValue = value; }
    }
    public int GetGoldMinValue
    {
        get { return getGoldMinValue; }
        set { getGoldMinValue = value; }
    }
}
