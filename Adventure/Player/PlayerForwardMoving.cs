using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForwardMoving : PlayerMoving
{ 
    private void Update()
    {
        adventureManager.ScoreUpdate((int)(totalDistance + transform.position.z - previousPosValue));
    }

    private void FixedUpdate()
    {
        Move();
        PosCheck();
    }

    private void OnEnable()
    {
        previousPosValue = transform.position.z;
    }

    protected override void Move()
    {
        transform.position += ZmoveVec = new Vector3(0, 0, speed * Time.deltaTime); 
    }

    protected override void PosCheck()
    {
        if (isSideMoving == false)
        {
            if (posState == PosState.Left)
            {
                transform.position = new Vector3(centerValue - sideMove, transform.position.y, transform.position.z);
            }
            else if (posState == PosState.Mid)
            {
                transform.position = new Vector3(centerValue, transform.position.y, transform.position.z);
            }
            else if (posState == PosState.Right)
            {
                transform.position = new Vector3(centerValue + sideMove, transform.position.y, transform.position.z);
            }
        }
    }

    public override IEnumerator SideMoving(PlayerMoving.PosState targetPosState)
    {
        isSideMoving = true;

        float time = 0.0f;
        Vector3 pos = transform.position;
        Vector3 targetPos;
        PlayerMoving.posState = targetPosState;

        while (time < sideMovingTime)
        {
            targetPos = GetTargetPos(targetPosState);
            Vector3 new_pos = Vector3.Lerp(pos, targetPos, time / sideMovingTime);
            transform.position = new_pos;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        isSideMoving = false;
    }

    protected override Vector3 GetTargetPos(PlayerMoving.PosState targetPosState)
    {
        Vector3 targetPos;


        if (targetPosState == PosState.Left)
        {
            targetPos = new Vector3(centerValue - sideMove, transform.position.y, transform.position.z);
        }
        else if (targetPosState == PosState.Mid)
        {
            targetPos = new Vector3(centerValue, transform.position.y, transform.position.z);
        }
        else
        {
            targetPos = new Vector3(centerValue + sideMove, transform.position.y, transform.position.z);
        }

        return targetPos;
    }

        private void OnDisable()
    {
        totalDistance += transform.position.z - previousPosValue;
    }
}
