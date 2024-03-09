using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;

    public static CameraManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraManager>();
            }

            return instance;
        }
    }

    private Camera mainCam;
    private Camera brainCam;

    private Cinemachine.CinemachineVirtualCamera virtualCam;
    CinemachineTransposer transposer;

    private SkyDome skyDome;
    public Island lookAtIsland;
    public bool isFollow;

    Dictionary<Island, Vector3> islandCamPosDic = new Dictionary<Island, Vector3>();

    public Quaternion camFirstRotation;

    private void Awake()
    {
        skyDome = GameObject.Find("SkyDome").GetComponent<SkyDome>();
        virtualCam = GameObject.Find("VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
        brainCam = GameObject.Find("BrainCamera").GetComponent<Camera>();
        mainCam = Camera.main;
        camFirstRotation = Quaternion.Euler(new Vector3(4, -180, 0));

        isFollow = false;
    }

    private void Start()
    {
        for (int i = 0; i < IslandManager.Instance.IslandArray.Length; i++)
        {
            islandCamPosDic.Add(IslandManager.Instance.IslandArray[i], new Vector3(i * IslandManager.Instance.islandInterval, 6, 45));
        }

        lookAtIsland = IslandManager.Instance.IslandArray[0];

        CamPosSet();
        
    }


    public void CamPosSet(Island island = null)
    {
        if(island != null)
        {
            lookAtIsland = island;
        }

        if (islandCamPosDic.TryGetValue(lookAtIsland, out Vector3 value))
        { 
            mainCam.transform.position = value;
        }

        mainCam.transform.rotation = camFirstRotation;

        skyDome.XMove(lookAtIsland.islandNum * IslandManager.Instance.islandInterval);

        UIManager.Instance.ArrowSet();
    }

    public void LeftMoveIsland()
    {
       if(lookAtIsland.islandNum != 0)
       {
            lookAtIsland = IslandManager.Instance.IslandArray[lookAtIsland.islandNum-1];
       }

        CamPosSet();
    }

    public void RightMoveIsland()
    {
        if (lookAtIsland.islandNum != 3)
        {
            lookAtIsland = IslandManager.Instance.IslandArray[lookAtIsland.islandNum + 1];
        }

        CamPosSet();
    }

    public void BrainMode(GameObject touchedAnimal)
    {
        virtualCam.Follow = touchedAnimal.transform;
        virtualCam.LookAt = touchedAnimal.transform;

        if (AnimalManager.Instance.animalFollowDic.TryGetValue(touchedAnimal, out Vector3 value))
        {
            transposer.m_FollowOffset = value;
        }
        
        brainCam.enabled = true;
        mainCam.enabled = false;

        isFollow = true;

        UIManager.Instance.FollowModeUIControl(true,touchedAnimal.GetComponent<AnimalState>());
        StartCoroutine(DampingSet(true));
    }

    private IEnumerator DampingSet(bool isSet)
    {
        yield return null;

        if(isSet == true)
        {
            transposer.m_XDamping = 10;
            transposer.m_YDamping = 10;
            transposer.m_ZDamping = 10;

            transposer.m_YawDamping = 20;
        }
        else
        {
            transposer.m_XDamping = 0;
            transposer.m_YDamping = 0;
            transposer.m_ZDamping = 0;

            transposer.m_YawDamping = 0;
        }
    }

    public void MainMode()
    {
        brainCam.enabled = false;
        mainCam.enabled = true;

        isFollow = false;

        UIManager.Instance.FollowModeUIControl(false);

        StartCoroutine(DampingSet(false));
    }
}
