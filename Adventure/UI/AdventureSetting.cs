using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureSetting : MonoBehaviour
{
    private static AdventureSetting instance;

    public static AdventureSetting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdventureSetting>();
            }

            return instance;
        }
    }

    public static string animalName;
    private static int index;
    private static int maxHp;

    List<Dictionary<string, object>> data_Animal;



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        data_Animal = AnimalManager.Instance.Data_Animal;
    }

    public void AdventureAnimalSet(string name)
    {
        animalName = name;

        for (int i = 0; i < data_Animal.Count; i++)
        {
            if (data_Animal[i]["Name"].ToString() == animalName)
            {
                index = i;
                maxHp = AnimalManager.Instance.animalArray[index].GetComponent<AnimalState>().MaxHp;
                return;
            }
        }
    }

    public void AdventureSet()
    {
        GameObject adventureAnimal = Resources.Load<GameObject>("AnimalPrefab/" + animalName);
        adventureAnimal = Instantiate(adventureAnimal);

        adventureAnimal.name = animalName;

        DontDestroyOnLoad(adventureAnimal);

        adventureAnimal.tag = "Animal";
        Utill.SetLayerRecursively(adventureAnimal.transform, "Animal");
        BoxCollider boxCollider = adventureAnimal.AddComponent<BoxCollider>();
        adventureAnimal.AddComponent<PlayerMeshController>();
        Rigidbody rigidbody = adventureAnimal.AddComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        adventureAnimal.AddComponent<PlayerAnimation>();
        adventureAnimal.AddComponent<PlayerForwardMoving>();
        adventureAnimal.AddComponent<PlayerRightMoving>();
        adventureAnimal.AddComponent<PlayerLeftMoving>();

        PlayerMovement playerMovement = adventureAnimal.AddComponent<PlayerMovement>();

        PlayerHealth playerHealth = adventureAnimal.AddComponent<PlayerHealth>();
        playerHealth.MaxHpSet(maxHp);

        PlayerInput playerInput = adventureAnimal.AddComponent<PlayerInput>();

        adventureAnimal.AddComponent<PlayerItemController>();

        Animator animator = adventureAnimal.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("AnimatorController/" + data_Animal[index]["Type"].ToString() +
            "_Adventure", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;

        animator.applyRootMotion = false;

        AdventureCamera adventureCamera = FindObjectOfType<AdventureCamera>();

        boxCollider.size = new Vector3(float.Parse(data_Animal[index]["AdventureColliderSizeX"].ToString()),
               float.Parse(data_Animal[index]["AdventureColliderSizeY"].ToString()), float.Parse(data_Animal[index]["AdventureColliderSizeZ"].ToString()));

        boxCollider.center = new Vector3(float.Parse(data_Animal[index]["AdventureColliderCenterX"].ToString()),
               float.Parse(data_Animal[index]["AdventureColliderCenterY"].ToString()), float.Parse(data_Animal[index]["AdventureColliderCenterZ"].ToString()));

        adventureAnimal.transform.localScale = new Vector3(float.Parse(data_Animal[index]["AdventureScaleX"].ToString()),
               float.Parse(data_Animal[index]["AdventureScaleY"].ToString()), float.Parse(data_Animal[index]["AdventureScaleZ"].ToString()));

        adventureAnimal.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        //Vector3 cameraPos= new Vector3(float.Parse(data_Animal[index]["AdventureCameraPosX"].ToString()),
        //      float.Parse(data_Animal[index]["AdventureCameraPosY"].ToString()), float.Parse(data_Animal[index]["AdventureCameraPosZ"].ToString()));

        //Vector3 cameraRotation = new Vector3(float.Parse(data_Animal[index]["AdventureCameraRotationX"].ToString()),
        //       float.Parse(data_Animal[index]["AdventureCameraRotationY"].ToString()), float.Parse(data_Animal[index]["AdventureCameraRotationZ"].ToString()));

        Vector3 cameraPos = new Vector3(0, 1.7f + playerMovement.startPosY, -2.25f);
        Vector3 cameraRotation = new Vector3(20f,0f,0f);

        adventureCamera.CameraSet(cameraPos,Quaternion.Euler(cameraRotation),adventureAnimal.transform);

        SpinZone.playerInput = playerInput;
        SpinZone.playerMovement = playerMovement;
    }
}
