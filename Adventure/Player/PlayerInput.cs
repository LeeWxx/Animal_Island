using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;

    public bool jumpPossible;

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;

    public bool detectSwipeAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;


    public enum SpinPossible { VerticalToRight, VerticalToLeft, LeftToVertical, RightToVertical, None };
    public SpinPossible spinPossibe;

    public float spinCenterValue;
    public PlayerMoving.PosState spinPosState;


    private void Awake()
    {
        GetComponent<PlayerAnimation>().playerInput = this;

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMovement.playerInput = this;
        jumpPossible = true;
        spinPossibe = SpinPossible.None;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
                detectSwipeAfterRelease = false;
            }
            //Detects Swipe while finger is still moving on screen
            else if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                    detectSwipeAfterRelease = true;
                }
            }
            //Detects swipe after finger is released from screen
            else if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        if (PlayerMeshController.dmgState == true)
        {
            playerHealth.ObstacleDamageOut();
            AdventureParticleManager.Instance.StunEffectStop();
        }

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            if (fingerDownPos.y - fingerUpPos.y > 0)
            {
                OnSwipeUp();
            }
            else if (fingerDownPos.y - fingerUpPos.y < 0)
            {
                OnSwipeDown();
            }
            fingerUpPos = fingerDownPos;

        }
        else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            if (fingerDownPos.x - fingerUpPos.x > 0)
            {
                OnSwipeRight();
            }
            else if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                OnSwipeLeft();
            }
            fingerUpPos = fingerDownPos;

        }
        else
        {
            Debug.Log("No Swipe Detected!");
        }
    }

    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    void OnSwipeUp()
    {
        if(jumpPossible == true)
        {
            playerMovement.Jump();
        }
    }

    void OnSwipeDown()
    {
        if(playerMovement.jumping == true)
        {
            playerMovement.Drop();
        }
    }

    void OnSwipeLeft()
    {
        if (spinPossibe == SpinPossible.VerticalToLeft)
        {
            playerMovement.VerticalToLeftCurve(spinCenterValue, spinPosState);
        }

        else if (spinPossibe == SpinPossible.RightToVertical)
        {
            playerMovement.RightToVerticalCurve(spinCenterValue, spinPosState);
        }

        else if (PlayerMoving.posState != PlayerMoving.PosState.Left)
        {
            playerMovement.LeftSideMove();
        }
    }

    void OnSwipeRight()
    {
        if (spinPossibe == SpinPossible.VerticalToRight)
        {
            playerMovement.VerticalToRightCurve(spinCenterValue, spinPosState);
        }

        else if (spinPossibe == SpinPossible.LeftToVertical)
        {
            playerMovement.LeftToVerticalCurve(spinCenterValue, spinPosState);
        }
        else if (PlayerMoving.posState != PlayerMoving.PosState.Right)
        {
            playerMovement.RightSideMove();
        }
    }
}
