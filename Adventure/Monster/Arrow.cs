using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowPool arrowPool;
    public PlayerHealth playerHealth;
    private MeshRenderer meshRenderer;

    float speed = 20f;

    private int damage = 10;

    private void Awake()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * speed);
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnPool());
    }

    public IEnumerator ReturnPool()
    {
        yield return new WaitForSeconds(3f);
        arrowPool.ReturnObject(this);
    }

    public void MaterialChange(Material material)
    {
        meshRenderer.material = material;
    }

    public void MeshRendererSet()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            playerHealth.OneTimeDamage(damage);
        }
    }
}
