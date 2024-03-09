using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureSceneTutorial : MonoBehaviour
{
    TutorialManager tutorialManager;

    public GameObject inducePanel;

    public GameObject speechBubble;
    private Text speechBubbleText;
    LocalizeSrcript localText;

    bool screenTouchCheck;
    bool screenTouchStandBy;

    Transform adventureAnimal;

    public GameObject rightSideMoveInduceArrow;
    public GameObject leftSideMoveInduceArrow;
    public GameObject jumpInduceArrow;
    public GameObject dropInduceArrow;

    private Vector2 rightArroweOrigjnalRectPos;
    private Vector2 leftArrowOrigjnalRectPos;
    private Vector2 jumpArrowOriginalRectPos;
    private Vector2 downArrowOriginalRectPos;

    private float rightSideMoveInduceEndPosX;
    private float leftSideMoveInduceEndPosX;
    private float jumpInduceEndPosY;
    private float downInduceEndPosY;

    private bool isInduceMove;

    PlayerInput playerInput;
    PlayerMovement playerMovement;

    Vector2 fingerDownPos;
    Vector2 fingerUpPos;

    bool detectSwipeAfterRelease = false;
    float SWIPE_THRESHOLD = 20f;

    private Archer tutorialArcher;

    bool isHomeButClick;
    public Button homeButton;
    public Image homeButImage;

    public Canvas canvas;

    private void Start()
    {
        if (PlayerPrefs.GetInt("TutorialEnd") == 0)
        {
            rightSideMoveInduceEndPosX = 250f;
            leftSideMoveInduceEndPosX = -250f;
            jumpInduceEndPosY = 370f;
            downInduceEndPosY = 220f;

            rightArroweOrigjnalRectPos = rightSideMoveInduceArrow.GetComponent<RectTransform>().anchoredPosition;
            leftArrowOrigjnalRectPos = leftSideMoveInduceArrow.GetComponent<RectTransform>().anchoredPosition;
            jumpArrowOriginalRectPos = jumpInduceArrow.GetComponent<RectTransform>().anchoredPosition;
            downArrowOriginalRectPos = dropInduceArrow.GetComponent<RectTransform>().anchoredPosition;

            rightSideMoveInduceArrow.gameObject.SetActive(false);
            leftSideMoveInduceArrow.gameObject.SetActive(false);
            jumpInduceArrow.gameObject.SetActive(false);
            dropInduceArrow.gameObject.SetActive(false);

            tutorialManager = TutorialManager.Instance;
            tutorialManager.adventureSceneTutorial = this;

            speechBubbleText = speechBubble.GetComponentInChildren<Text>();
            speechBubble.gameObject.SetActive(true);
            screenTouchCheck = false;

            tutorialManager.PhaseClear();
            StartCoroutine(tutorialManager.TutorialProceed());
            screenTouchCheck = false;

            tutorialArcher = GameObject.Find("TutorialArcher").GetComponent<Archer>();
            tutorialArcher.enabled = false;

            playerInput = FindObjectOfType<PlayerInput>();
            playerInput.enabled = false;
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
        else
        {
            Destroy(speechBubble.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void TutorialProceed(string dialouge, string behavior)
    {
        if (dialouge == "Empty")
        {
            if (speechBubble.activeSelf == true)
            {
                speechBubble.gameObject.SetActive(false);
            }
        }
        else
        {
            if (speechBubble.activeSelf == false)
            {
                speechBubble.gameObject.SetActive(true);
            }

            localText.textKey = dialouge;
            localText.LocalizeChanged();
        }


        StartCoroutine(behavior);
    }

    private IEnumerator ScreenTouchStandBy()
    {
        inducePanel.gameObject.SetActive(true);

        Time.timeScale = 0f;
        screenTouchStandBy = true;
        screenTouchCheck = false;


        while (screenTouchCheck == false)
        {
            yield return null;
        }

        screenTouchCheck = false;
        screenTouchStandBy = false;

        tutorialManager.PhaseClear();
    }

    public void ScreenTouch()
    {
        if (screenTouchStandBy == true)
        {
            screenTouchCheck = true;
        }
    }

    private IEnumerator ObstacleAvoidRightInduce()
    {
        Time.timeScale = 1f;
        adventureAnimal = FindObjectOfType<PlayerHealth>().gameObject.transform;

        while (adventureAnimal.position.z < 21f)
        {
            yield return null;
        }

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = rightSideMoveInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = rightArroweOrigjnalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rightSideMoveInduceEndPosX,rectTransform.anchoredPosition.y), 1f,rightSideMoveInduceArrow));
        StartCoroutine(RightSideMoveInduce(PlayerMoving.PosState.Right));

        Time.timeScale = 0f;
    }

    private IEnumerator ObstacleAvoidLeftInduce()
    {
        while (adventureAnimal.position.z < 36.5f)
        {
            yield return null;
        }

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = leftSideMoveInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = leftArrowOrigjnalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(leftSideMoveInduceEndPosX, rectTransform.anchoredPosition.y), 1f, leftSideMoveInduceArrow));
        StartCoroutine(LeftSideMoveInduce(PlayerMoving.PosState.Mid));

        Time.timeScale = 0f;
    }

    private IEnumerator AnimalJumpInduce1()
    {
        while (adventureAnimal.position.z < 48f)
        {
            yield return null;
        }

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = jumpInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = jumpArrowOriginalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rectTransform.anchoredPosition.x, jumpInduceEndPosY), 1f, jumpInduceArrow));
        StartCoroutine(JumpInduce());

        Time.timeScale = 0f;
    }

    private IEnumerator AnimalJumpInduce2()
    {
        while (adventureAnimal.position.z < 77.5f)
        {
            yield return null;
        }


        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = jumpInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = jumpArrowOriginalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rectTransform.anchoredPosition.x, jumpInduceEndPosY), 1f, jumpInduceArrow));
        StartCoroutine(JumpInduce());

        Time.timeScale = 0f;
    }

    private IEnumerator DropWait()
    {
        yield return new WaitForSeconds(0.6f);

        tutorialManager.PhaseClear();
    }

    private IEnumerator AnimalDropInduce()
    {
        yield return null;

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = dropInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = downArrowOriginalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rectTransform.anchoredPosition.x,downInduceEndPosY), 1f, dropInduceArrow));
        StartCoroutine(DropInduce());
        
        Time.timeScale = 0f;
    }

    private IEnumerator ObstacleAvoidLeftInduce2()
    {
        if (Time.timeScale != 0)
        {
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 0f;
        }

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = leftSideMoveInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = leftArrowOrigjnalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(leftSideMoveInduceEndPosX,rectTransform.anchoredPosition.y), 1f, leftSideMoveInduceArrow));
        StartCoroutine(LeftSideMoveInduce(PlayerMoving.PosState.Left));
    }

    private IEnumerator AnimalCurveInduce()
    {
        while (adventureAnimal.position.z < 112.5f)
        {
            yield return null;
        }

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = rightSideMoveInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = rightArroweOrigjnalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rightSideMoveInduceEndPosX, rectTransform.anchoredPosition.y), 1f, rightSideMoveInduceArrow));
        StartCoroutine(RightCurveInduce());

        Time.timeScale = 0f;
    }

    private IEnumerator ArcherShot()
    {
        tutorialArcher.enabled = true;

        tutorialArcher.animator.speed = 2; 
        tutorialArcher.animator.SetTrigger("Shot");
        StartCoroutine(tutorialArcher.Shot());

        yield return new WaitForSeconds(1.5f);

        Time.timeScale = 0f;
        tutorialManager.PhaseClear();
    }

    private IEnumerator ObstacleAvoidRightInduce2()
    {
        yield return new WaitForSeconds(0.45f);

        inducePanel.gameObject.SetActive(false);
        RectTransform rectTransform = rightSideMoveInduceArrow.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = rightArroweOrigjnalRectPos;
        StartCoroutine(InduceArrowLerpMove(new Vector2(rightSideMoveInduceEndPosX, rectTransform.anchoredPosition.y), 1f, rightSideMoveInduceArrow));
        StartCoroutine(RightSideMoveInduce(PlayerMoving.PosState.Mid));

        Time.timeScale = 0f;
    }

    private IEnumerator ArcherdDamagedWait()
    {
        Debug.Log(Time.timeScale);

        while (tutorialArcher.boxCollider.enabled == true)
        {
            yield return null;
        }

        tutorialManager.PhaseClear();
    }

    private IEnumerator AnimalDamagedWait()
    {
        Debug.Log(Time.timeScale);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        while (playerHealth.Hp != 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        tutorialManager.PhaseClear();
    }

    private IEnumerator RewardPanelOnWait()
    {
        Time.timeScale = 1f;

        while (AdventureUIManager.Instance.rewardPanel.activeSelf != true)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3.3f);

        tutorialManager.PhaseClear();
    }

    private IEnumerator HomeButtonInduce()
    {
        Time.timeScale = 1f;

        canvas.gameObject.SetActive(false);

        isHomeButClick = false;
        homeButton.onClick.AddListener(HomeButClick);
        yield return null;

        Color basicOriginalColor = homeButImage.color;
        Color basicFadeColor = new Color(homeButImage.color.r, homeButImage.color.g, homeButImage.color.b, homeButImage.color.a * 0.5f);

        float time;
        while (isHomeButClick == false)
        {
            homeButImage.color = basicFadeColor;

            time = Time.time + tutorialManager.changeTimeInterval;
            while (isHomeButClick == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }

            homeButImage.color = basicOriginalColor;
            time = Time.time + tutorialManager.changeTimeInterval;
            while (isHomeButClick == false && Time.time <= time)
            {
                yield return tutorialManager.waitTime;
            }
        }
    }

    private void HomeButClick()
    {
        isHomeButClick = true;
    }


    IEnumerator InduceArrowLerpMove(Vector2 target, float targetTime, GameObject obj)
    {
        isInduceMove = false;

        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 pos = rectTransform.anchoredPosition;
        obj.gameObject.SetActive(true);

        while (isInduceMove == false)
        {
            float time = 0.0f;

            while (time < targetTime && isInduceMove == false)
            {
                Vector2 new_pos = Vector2.Lerp(pos, target, time / targetTime);
                rectTransform.anchoredPosition = new_pos;
                time += 0.05f;
                yield return new WaitForSecondsRealtime(0.05f);
            }

            rectTransform.anchoredPosition = pos;
        }

        obj.gameObject.SetActive(false);
        inducePanel.gameObject.SetActive(true);

        tutorialManager.PhaseClear();
    }

    IEnumerator RightSideMoveInduce(PlayerMoving.PosState posState)
    {
        while (isInduceMove == false)
        {
            if (TouchCheck())
            {
                RightDetectSwipe(false, posState);
            }
            yield return null;
        }
    }

    IEnumerator LeftSideMoveInduce(PlayerMoving.PosState targetPosState)
    {
        while (isInduceMove == false)
        {
            if (TouchCheck())
            {
                LeftDetectSwipe(targetPosState);
            }
            yield return null;
        }
    }

    IEnumerator JumpInduce()
    {
        while (isInduceMove == false)
        {
            if (TouchCheck())
            {
                JumpDetectSwipe();
            }
            yield return null;
        }
    }

    IEnumerator DropInduce()
    {
        while (isInduceMove == false)
        {
            if (TouchCheck())
            {
                DropDetectSwipe();
            }
            yield return null;
        }
    }

    IEnumerator RightCurveInduce()
    {
        while (isInduceMove == false)
        {
            if (TouchCheck())
            {
                RightDetectSwipe(true);
            }
            yield return null;
        }
    }



    private bool TouchCheck()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
                detectSwipeAfterRelease = false;
                return false;
            }
            //Detects Swipe while finger is still moving on screen
            else if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = touch.position;
                    detectSwipeAfterRelease = true;
                    return false;
                }
                return false;
            }
            //Detects swipe after finger is released from screen
            else if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                return true;
            }
            return false;
        }
        return false;
    }

    private void RightDetectSwipe(bool isCurve,PlayerMoving.PosState posState = PlayerMoving.PosState.Mid)
    {
        if (Mathf.Abs(fingerDownPos.x - fingerUpPos.x) > SWIPE_THRESHOLD)
        {
            if (fingerDownPos.x - fingerUpPos.x > 0)
            {
                isInduceMove = true;
                if(isCurve == false)
                {
                    StartCoroutine(playerMovement.onPlayerMoving.SideMoving(posState));
                }
                else
                {
                    playerMovement.VerticalToRightCurve(112.5f, PlayerMoving.PosState.Mid);
                }
                Time.timeScale = 1f;
            }
        }
    }

    private void LeftDetectSwipe(PlayerMoving.PosState targetPosState)
    {
        if (Mathf.Abs(fingerDownPos.x - fingerUpPos.x) > SWIPE_THRESHOLD)
        {
            if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                isInduceMove = true;
                StartCoroutine(playerMovement.onPlayerMoving.SideMoving(targetPosState));
                Time.timeScale = 1f;
            }
        }
    }

    private void JumpDetectSwipe()
    {
        if (Mathf.Abs(fingerDownPos.y - fingerUpPos.y) > SWIPE_THRESHOLD)
        {
            if (fingerDownPos.y - fingerUpPos.y > 0)
            {
                isInduceMove = true;
                playerMovement.Jump();
                Time.timeScale = 1f;
            }
        }
    }

    private void DropDetectSwipe()
    {
        if (Mathf.Abs(fingerDownPos.y - fingerUpPos.y) > SWIPE_THRESHOLD)
        {
            if (fingerDownPos.y - fingerUpPos.y < 0)
            {
                isInduceMove = true;
                playerMovement.Drop();
                Time.timeScale = 1f;
            }
        }
    }
}
