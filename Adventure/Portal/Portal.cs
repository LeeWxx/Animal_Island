using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Quaternion thisRotation;
    public static Vector3 thisScale;

    public static PlayerMovement playerMovement;

    bool check;

    public void PortalSet()
    {
        thisRotation = transform.localRotation;
        thisScale = transform.localScale;

        check = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(check == false)
        {
            PlayerMovement.movingStage += 1;
            playerMovement.SpeedSet();
            BridgeLoop.Instance.StageUp();
            SoundManager.Instance.PlayBGMSound();

            if (PlayerMovement.movingStage == 2)
            {
                AdventureParticleManager.Instance.SandEffectPlay();
            }
            else if (PlayerMovement.movingStage == 3)
            {
                AdventureParticleManager.Instance.SandEffectStop();
            }
            else if (PlayerMovement.movingStage == 4)
            {
                AdventureParticleManager.Instance.LavaEffectPlay();
            }
            check = true;
        }
    }
}
