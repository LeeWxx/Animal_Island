using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftMoving : PlayerMoving
{
    private void Update()
    {
        adventureManager.ScoreUpdate((int)(totalDistance + previousPosValue - transform.position.x));
    }

    private void FixedUpdate()
    {
        Move();
        PosCheck();
    }

    private void OnEnable()
    {
        previousPosValue = transform.position.x;
    }

    protected override void Move()
    {
        transform.position -= XmoveVec = new Vector3(speed * Time.deltaTime, 0, 0); 
    }

    protected override void PosCheck()
    {
        if (isSideMoving == false)
        {
            if (posState == PosState.Left)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, centerValue - sideMove);
            }
            else if (posState == PosState.Mid)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, centerValue);
            }
            else if (posState == PosState.Right)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, centerValue + sideMove);
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
            targetPos = new Vector3(transform.position.x, transform.position.y, centerValue - sideMove);
        }
        else if (targetPosState == PosState.Mid)
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, centerValue);
        }
        else
        {
            targetPos = new Vector3(transform.position.x, transform.position.y, centerValue + sideMove);
        }

        return targetPos;
    }


        private void OnDisable()
    {
        totalDistance += previousPosValue - transform.position.x;
    }
}
