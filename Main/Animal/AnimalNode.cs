using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class AnimalNode : MonoBehaviour
{
    public AnimalState nodeAnimalState;
    public Island nodeAnimalIsland;

    public Text nodeAnimalNameText;

    protected Image lockImage;
    protected RawImage animalProfileImage;

    private string animalName;


    public virtual void NodeSet()
    {
        nodeAnimalNameText = transform.Find("NodeAnimalName").GetComponent<Text>();
        lockImage = transform.Find("LockImage").GetComponent<Image>();
        animalProfileImage = transform.Find("ProfileAnimal").GetComponent<RawImage>();
    }
}
