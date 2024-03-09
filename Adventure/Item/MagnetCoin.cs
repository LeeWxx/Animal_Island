using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCoin : MonoBehaviour
{
    AdventureCoin coinScript;
    public static Transform playerTransform;
    private static float moveSpeed = 17f;

    // Start is called before the first frame update
    void Start()
    {
        coinScript = GetComponent<AdventureCoin>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            playerTransform.position, moveSpeed * Time.deltaTime);
    }
}
