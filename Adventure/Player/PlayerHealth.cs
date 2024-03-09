using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerAnimation playerAnimation;

    private int maxHp;
    
    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    private int hp;

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    

    public void MaxHpSet(int maxHpValue)
    {
        maxHp = maxHpValue;
        hp = maxHp;
        StartCoroutine(AdventureUIManager.Instance.HpBarSet(maxHp));
    }

    public bool HpMinus(int minusHp)
    {
        if(playerMovement.IsBoost == false)
        {
            if (hp > minusHp)
            {
                hp -= minusHp;
                AdventureUIManager.Instance.HpMinusUpdate(minusHp);
                return true;
            }
            else
            {
                hp = 0;
                AdventureUIManager.Instance.HpMinusUpdate(minusHp);
                StartCoroutine(Die());
                return false;
            }
        }

        return false;
        
    }

    public IEnumerator Die()
    {
        this.enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        playerMovement.onPlayerMoving.enabled = false;
        playerAnimation.DeathAnimation();
        //SoundManager.Instance.PlaySFXSound("GameOverSound");
        yield return new WaitForSeconds(3f);

        AdventureManager.Instance.GameOver();
    }

    public void ObstacleDamage(int damage)
    {
        if (HpMinus(damage))
        {
            StartCoroutine(PlayerMeshController.DamageMaterialOn());
            playerAnimation.DmgAnimation(PlayerMoving.posState);
            playerMovement.onPlayerMoving.enabled = false;
            AdventureParticleManager.Instance.StunEffectPlay(new Vector3(transform.position.x - 0.25f,transform.position.y + 1f, transform.position.z));
        }
    }

    public void OneTimeDamage(int damage)
    {
        if (HpMinus(damage))
        {
            StartCoroutine(PlayerMeshController.OneTimeDamageMaterialOn());
        }
    }

    public void ObstacleDamageOut()
    {
        PlayerMeshController.dmgState = false;
        playerMovement.onPlayerMoving.enabled = true;
        playerAnimation.AnimationReStart();
    }
}
