using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private static TouchManager instance;

    public static TouchManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TouchManager>();
            }

            return instance;
        }
    }

    private Camera mainCamera;
    private Camera brainCamera;

    public AnimalState touchedAnimal;

    private float startX;
    private float endX;

    private bool touchState;


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        mainCamera = Camera.main;
        brainCamera = GameObject.Find("BrainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (UIManager.Instance.isPanelOn == false)
        {
            TouchCheck();
            DragCheck();
        }
    }

    void TouchCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = RaycastInfoGet();

            if (hit.collider != null)
            {
                HitColliderCheckAndResult(hit);
            }
        }
    }

    void DragCheck()
    {
        if (CameraManager.Instance.isFollow == false)
        {
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                startX = Input.touches[0].position.x;
                touchState = true;
            }

            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
            {
                if (touchState == true)
                {
                    endX = Input.touches[0].position.x;

                    if (startX - endX > 6f)
                    {
                        mainCamera.transform.RotateAround(CameraManager.Instance.lookAtIsland.transform.position, Vector3.up, (endX - startX) * 0.01f);
                    }
                    else if (startX - endX < -6f)
                    {
                        mainCamera.transform.RotateAround(CameraManager.Instance.lookAtIsland.transform.position, Vector3.up, (endX - startX) * 0.01f);
                    }
                }

            }
        }

    }

    public RaycastHit RaycastInfoGet()
    {
        RaycastHit hit;
        Ray touchRay;

        if (CameraManager.Instance.isFollow == false)
        {
            touchRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        }
        else
        {
            touchRay = brainCamera.ScreenPointToRay(Input.mousePosition);
        }

        Physics.Raycast(touchRay, out hit);

        return hit;
    }


    void HitColliderCheckAndResult(RaycastHit hit)
    {
        if (CameraManager.Instance.isFollow == false)
        {

            if (hit.collider.gameObject.CompareTag("Animal") == true)
            {
                touchedAnimal = hit.collider.gameObject.GetComponent<AnimalState>();
                CameraManager.Instance.BrainMode(touchedAnimal.gameObject);
            }

            else if (hit.collider.gameObject.CompareTag("Island") == true)
            {
                StartCoroutine(TouchCoinPool.Instance.TouchCoinGet(hit.point));
                StartCoroutine(ParticleManager.Instance.TouchEffect(hit.point));
                SoundManager.Instance.PlaySFXSound("MainCoinSound");
            }
        }

        else if (CameraManager.Instance.isFollow == true)
        {
            if (hit.collider.gameObject == touchedAnimal.gameObject)
            {
                touchedAnimal.IntimacyPlus();
                StartCoroutine(ParticleManager.Instance.LoveEffect(hit.point, 0.4f));
            }
        }
        else
        {
            return;
        }
    }
}

