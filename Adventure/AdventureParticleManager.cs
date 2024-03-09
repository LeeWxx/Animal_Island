using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureParticleManager : MonoBehaviour
{
    private static AdventureParticleManager instance;

    public static AdventureParticleManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = FindObjectOfType<AdventureParticleManager>();
            }

            return instance;
        }
    }

    public ParticleSystem stunEffect;
    public ParticleSystem sandEffect;

    public ParticleSystem lavaFogEffect;
    public ParticleSystem lavaSparkleEffect;

    public ParticleSystem fantasyEffect;
    public ParticleSystem modernEffect;

    public ParticleSystem fantasyRewardBoxEffect;
    public ParticleSystem modernRewardBoxEffect;
    public ParticleSystem noneRewardBoxEffect;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ParticleSet();
    }

    private void ParticleSet()
    {
        Transform animalTransform = FindObjectOfType<PlayerHealth>().transform;

        stunEffect.transform.SetParent(animalTransform);
        sandEffect.transform.SetParent(animalTransform);
        lavaFogEffect.transform.SetParent(animalTransform);
        lavaSparkleEffect.transform.SetParent(animalTransform);
    }

    public void SandEffectPlay()
    {
        sandEffect.Play();
    }

    public void SandEffectStop()
    {
        sandEffect.Stop();
    }

    public void LavaEffectPlay()
    {
        lavaFogEffect.Play();
        lavaSparkleEffect.Play();
    }

    public void StunEffectPlay(Vector3 Pos)
    {
        stunEffect.transform.position = Pos;
        stunEffect.Play();
    }

    public void StunEffectStop()
    {
        stunEffect.Stop();
    }


    public void FantasyEffectPlay()
    {
        fantasyEffect.Play();
    }

    public void ModernEffectPlay()
    {
        modernEffect.Play();
    }

    public IEnumerator RewardBoxEffectPlay(int boxIndex,Image afterGoldBoxImage)
    {
        yield return null;
        //afterGoldBoxImage.enabled = false;

        //if(boxIndex == 0)
        //{
        //    noneRewardBoxEffect.Play();
        //    yield return new WaitForSeconds(1.5f);
        //    noneRewardBoxEffect.Stop();
        //}
        //else if (boxIndex < 4)
        //{
        //    fantasyRewardBoxEffect.Play();
        //    yield return new WaitForSeconds(3f);
        //    fantasyRewardBoxEffect.Stop();
        //    afterGoldBoxImage.enabled = true;
        //}
        //else
        //{
        //    modernRewardBoxEffect.Play();
        //    yield return new WaitForSeconds(3f);
        //    modernRewardBoxEffect.Stop();
        //    afterGoldBoxImage.enabled = true;
        //}

    }

}
