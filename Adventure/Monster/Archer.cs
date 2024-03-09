using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Placement
{
    public int stage;
    public ArrowPool arrowPool;

    public Animator animator;

    private float reloadTime = 5f;
    private float time;

    private PlayerAnimation playerAnimation;
    public BoxCollider boxCollider;
    private PlayerHealth playerHealth;

    private bool isAttacked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Start()
    {
        playerAnimation = FindObjectOfType<PlayerAnimation>();
    }

    private void OnEnable()
    {
        isAttacked = false;
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (time + reloadTime < Time.time && isAttacked == false)
        {
            animator.SetTrigger("Shot");
            StartCoroutine(Shot());
            time = Time.time;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Animal"))
    //    {
    //        playerHealth.ObstacleDamage(damage);
    //    }
    //}

    public IEnumerator Shot()
    {
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.AimStraight")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isAttacked == false)
            {
                arrowPool.Shot(this);
                animator.SetTrigger("ShotEnd");
                break;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Animal") && isAttacked == false)
        {
            isAttacked = true;
            StartCoroutine(playerAnimation.AttackAnimation(this));
        }
    }

    public IEnumerator Death()
    {
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(0.5f);
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        ArcherSpawner.Instance.ReturnObject(this);
    }
}
