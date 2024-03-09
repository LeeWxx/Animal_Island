using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMoving : MonoBehaviour
{
    public static float centerValue;
    public static float sideMove = 1.2f;
    protected float sideMovingTime = 0.05f;

    public static float speed;

    public enum PosState { Mid, Right, Left };
    public static PosState posState;

    public static Vector3 ZmoveVec;
    public static Vector3 XmoveVec;

    protected abstract void Move();
    protected abstract void PosCheck();

    public abstract IEnumerator SideMoving(PlayerMoving.PosState targetPosState);
    protected abstract Vector3 GetTargetPos(PlayerMoving.PosState targetPosState);

    protected float previousPosValue;

    protected AdventureManager adventureManager;

    protected static float totalDistance;

    public static bool isSideMoving;

    private void Awake()
    {
        isSideMoving = false;
        totalDistance = 0;
        adventureManager = AdventureManager.Instance;
    }
}
