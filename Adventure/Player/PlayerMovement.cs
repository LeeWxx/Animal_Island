using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isOnBridge;

    PlayerAnimation playerAnimation;

    PlayerForwardMoving playerForwardMoving;
    PlayerRightMoving playerRightMoving;
    PlayerLeftMoving playerLeftMoving;

    public PlayerMoving onPlayerMoving;

    private Quaternion forwardRotate;
    private Quaternion rightRotate;
    private Quaternion leftRotate;

    public Rigidbody rigidBody;

    public float jumpPower = 5f;
    public bool jumping = false;

    float forceGravity = 1200f;

    public float startPosY = 2f;
    private float[] speedArray = new float[4] { 12.5f, 13.5f, 14.5f, 15.5f};

    public static int movingStage;

    private bool isBoost;

    private GoldBoxSliderController goldBoxSliderController;

    public float animalPosY;

    public PlayerInput playerInput;

    public bool IsBoost
    {
        get { return isBoost; }
        set { isBoost = value; }
    }

    public static int boostSpeed = 4;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bridge") == true && collision.contacts[0].normal.y >= 0.5)
        {
            jumping = false;
        }
    }

    private void Awake()
    {
        isOnBridge = true;
        isBoost = false;

        movingStage = 1;
        SpeedSet();

        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimation.playerMovement = this;

        playerForwardMoving = GetComponent<PlayerForwardMoving>();
        playerRightMoving = GetComponent<PlayerRightMoving>();
        playerLeftMoving = GetComponent<PlayerLeftMoving>();

        rigidBody = GetComponent<Rigidbody>();

        playerLeftMoving.enabled = false;
        playerRightMoving.enabled = false;
        onPlayerMoving = playerForwardMoving;

        PlayerMoving.posState = PlayerMoving.PosState.Mid;

        PlayerMoving.centerValue = 0f;
        transform.position = new Vector3(PlayerMoving.centerValue, startPosY, 0f);

        forwardRotate = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        rightRotate = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        leftRotate = Quaternion.Euler(new Vector3(0f, -90f, 0f));

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        goldBoxSliderController = FindObjectOfType<GoldBoxSliderController>();
    }

    private void Start()
    {
        playerAnimation.RunAnimation(true);
    }

    public void Jump()
    {
        if(jumping != true)
        {
            transform.position += transform.up * jumpPower;
            //SoundManager.Instance.PlaySFXSound("JumpSound");
            playerAnimation.JumpAnimation();
            jumping = true;
            goldBoxSliderController.ValueUp();
        }
    }

    public void Drop()
    {
        rigidBody.AddForce(Vector3.down * forceGravity);
        goldBoxSliderController.ValueUp();
    }

    public void RightSideMove()
    {
        goldBoxSliderController.ValueUp();

        if (PlayerMoving.posState == PlayerMoving.PosState.Mid)
        {
           StartCoroutine(onPlayerMoving.SideMoving(PlayerMoving.PosState.Right));
        }

        else if (PlayerMoving.posState == PlayerMoving.PosState.Left)
        {
            StartCoroutine(onPlayerMoving.SideMoving(PlayerMoving.PosState.Mid));
        }

        SoundManager.Instance.PlaySFXSound("SideMoveSound");
    }

    public void LeftSideMove()
    {
        goldBoxSliderController.ValueUp();

        if (PlayerMoving.posState == PlayerMoving.PosState.Mid)
        {
            StartCoroutine(onPlayerMoving.SideMoving(PlayerMoving.PosState.Left));
        }

        else if (PlayerMoving.posState == PlayerMoving.PosState.Right)
        {
            StartCoroutine(onPlayerMoving.SideMoving(PlayerMoving.PosState.Mid));
        }

        SoundManager.Instance.PlaySFXSound("SideMoveSound");
    }

    public void VerticalToRightCurve(float newCenterValue, PlayerMoving.PosState _posState)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        PlayerMoving.posState = _posState;

        PlayerMoving.centerValue = newCenterValue;


        playerForwardMoving.enabled = false;
        StartCoroutine(Curve(rightRotate));
        playerRightMoving.enabled = true;
        onPlayerMoving = playerRightMoving;

        //if (isBoost == true)
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}
        //else
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void LeftToVerticalCurve(float newCenterValue, PlayerMoving.PosState _posState)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        PlayerMoving.posState = _posState;

        PlayerMoving.centerValue = newCenterValue;


        playerLeftMoving.enabled = false;
        StartCoroutine(Curve(forwardRotate));
        playerForwardMoving.enabled = true;
        onPlayerMoving = playerForwardMoving;

        //if (isBoost == true)
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation ;
        //}
        //else
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void VerticalToLeftCurve(float newCenterValue, PlayerMoving.PosState _posState)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        PlayerMoving.posState = _posState;

        PlayerMoving.centerValue = newCenterValue;

        if (onPlayerMoving == playerForwardMoving)
        {
            playerForwardMoving.enabled = false;
            StartCoroutine(Curve(leftRotate));
            playerLeftMoving.enabled = true;
            onPlayerMoving = playerLeftMoving;
        }

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        //if (isBoost == true)
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}
        //else
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}
    }

    public void RightToVerticalCurve(float newCenterValue, PlayerMoving.PosState _posState)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        PlayerMoving.posState = _posState;

        PlayerMoving.centerValue = newCenterValue;


        playerRightMoving.enabled = false;
        StartCoroutine(Curve(forwardRotate));
        playerForwardMoving.enabled = true;
        onPlayerMoving = playerForwardMoving;

        rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //if (isBoost == true)
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}
        //else
        //{
        //    rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //}
    }

    public IEnumerator Curve(Quaternion endQuaternion)
    {
        goldBoxSliderController.ValueUp();
        bool end = false;
        playerInput.spinPossibe = PlayerInput.SpinPossible.None;

        while (!end)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endQuaternion, Time.deltaTime * 10f);

            float angle = Quaternion.Angle(transform.rotation, endQuaternion);

            if (angle <= 0)
            {
                end = true;
            }

            yield return null;
        }

        SpeedSet();
        playerInput.spinPossibe = PlayerInput.SpinPossible.None;
    }

    public void SpinSpeedDecline()
    {
       PlayerMoving.speed = speedArray[movingStage - 1] / 3;
    }

    public void SpeedSet()
    {
        if (isBoost == false)
        {
            PlayerMoving.speed = speedArray[movingStage - 1];
        }
        else
        {
            PlayerMoving.speed = speedArray[movingStage - 1] * boostSpeed;
        }
    }

    public void RevivePosSet(BridgeEndZone endZone)
    {
        PlayerMoving.posState = PlayerMoving.PosState.Mid;

        if(onPlayerMoving == playerForwardMoving)
        {

            transform.position = new Vector3(PlayerMoving.centerValue, 7f, endZone.transform.position.z + 2f);
        }
        else if(onPlayerMoving == playerLeftMoving)
        {
            transform.position = new Vector3(endZone.transform.position.x - 2f, 7f, PlayerMoving.centerValue);
        }
        else if(onPlayerMoving == playerRightMoving)
        {
            transform.position = new Vector3(endZone.transform.position.x + 2f, 7f, PlayerMoving.centerValue);
        }
    }


    private void Update()
    {
        Debug.Log(PlayerMoving.posState);
    }
}
