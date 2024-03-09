using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPanel : MonoBehaviour
{
    MissionManager missionManager;
    public Text resetTimerText;

    private void OnEnable()
    {
        missionManager = MissionManager.Instance;
        missionManager.MissionSlotSet();
        missionManager.MissionSlotRange();
        missionManager.ResetTimeCheck();
    }

    private void Update()
    {
        resetTimerText.text = missionManager.GetResetTime();
    }
}
