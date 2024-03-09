using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator playerAnimator;
    float animatorSpeed;

    Material dmgMaterial;
    Material originalMaterial;

    public PlayerMovement playerMovement;
    public PlayerInput playerInput;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        animatorSpeed = playerAnimator.speed;

        originalMaterial =  gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        dmgMaterial = Resources.Load("DefaultMaterial", typeof(Material)) as Material;
    }

    public void RunAnimation(bool parameterBool)
    {
        playerAnimator.SetBool("Run", parameterBool);
    }

    public void DmgAnimation(PlayerMoving.PosState posState)
    {
        if (posState == PlayerMoving.PosState.Left)
        {
            playerAnimator.SetTrigger("LeftDmg");
        }
        else if (posState == PlayerMoving.PosState.Mid)
        {
            int num = Random.Range(1, 100);

            if (num <= 50)
            {
                playerAnimator.SetTrigger("LeftDmg");
            }

            else
            {
                playerAnimator.SetTrigger("RightDmg");
            }
        }
        else if (posState == PlayerMoving.PosState.Right)
        {
            playerAnimator.SetTrigger("RightDmg");
        }

        StartCoroutine(AnimationStop());
    }

    public void JumpAnimation()
    {
        playerAnimator.SetTrigger("Jump");
    }

    public void DeathAnimation()
    {
        int num = Random.Range(1, 100);

        if (num <= 50)
        {
            playerAnimator.SetTrigger("Death1");
        }

        else
        {
            playerAnimator.SetTrigger("Death2");
        }
    }

    public IEnumerator AttackAnimation(Archer dmgArcher)
    {
        playerMovement.enabled = false;
        playerInput.enabled = false;
        playerAnimator.SetTrigger("Attack");

        while (true)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack") == true
                && playerAnimator.GetAnimatorTransitionInfo(0).normalizedTime >= 0.3f)
            {
                //SoundManager.Instance.PlaySFXSound("HitSound");
                StartCoroutine(dmgArcher.Death());
                //playerMovement.RotationSet();
                playerMovement.enabled = true;
                playerInput.enabled = true;
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator AnimationStop()
    {
        playerAnimator.SetBool("Run", false);

        while (PlayerMeshController.dmgState == true)
        {
            animatorSpeed = Mathf.Lerp(animatorSpeed, 0, Time.deltaTime);
            playerAnimator.speed = animatorSpeed;
            yield return null;
        }
    }
    public void AnimationReStart()
    {
        playerAnimator.SetBool("Run", true);
        animatorSpeed = 1f;
        playerAnimator.speed = animatorSpeed;
    }
}
    
