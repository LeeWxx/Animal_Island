using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager instance;

    public static ParticleManager Instance
    {
        get
        { 
            if(instance == null)
            {
              instance =  GameObject.FindObjectOfType<ParticleManager>();
            }

            return instance;
        }
    }

    ParticleSystem animalSpawnParticle;
    ParticleSystem islandSpawnParticle;
    ParticleSystem loveParticle;
    ParticleSystem levelUpParticle;
    ParticleSystem touchParticle;

    public ParticleSystem fantasyMergeParticle;
    public ParticleSystem modernMergeParticle;
    public ParticleSystem boxOpenParticle;

    private void Awake()
    {

        if(Instance != this)
        {
            Destroy(this);
        }

        animalSpawnParticle = GameObject.Find("AnimalSpawnParticle").GetComponent<ParticleSystem>();
        islandSpawnParticle = GameObject.Find("IslandSpawnParticle").GetComponent<ParticleSystem>();
        loveParticle = GameObject.Find("LoveParticle").GetComponent<ParticleSystem>();
        levelUpParticle = GameObject.Find("LevelUpParticle").GetComponent<ParticleSystem>();
        touchParticle = GameObject.Find("CoinGetParticle").GetComponent<ParticleSystem>();
    }

    public IEnumerator AnimalSpawnEffect(Vector3 Pos,float playTime)
    {
        animalSpawnParticle.transform.position = Pos;
        animalSpawnParticle.Play();
        yield return new WaitForSeconds(playTime);
        animalSpawnParticle.Stop();
    }

    public IEnumerator IslandSpawnEffect(Vector3 Pos, float playTime)
    {
        islandSpawnParticle.transform.position = Pos;
        islandSpawnParticle.Play();
        yield return new WaitForSeconds(playTime);
        islandSpawnParticle.Stop();
    }

    public IEnumerator LoveEffect(Vector3 Pos, float playTime)
    {
        loveParticle.transform.position = Pos;
        loveParticle.Play();
        yield return new WaitForSeconds(playTime);
        loveParticle.Stop();
    }

    public IEnumerator LevelUpEffect(Vector3 Pos, float playTime)
    {
        Pos.y = Pos.y += 0.5f;
        levelUpParticle.transform.position = Pos;
        levelUpParticle.Play();
        yield return new WaitForSeconds(playTime);
        levelUpParticle.Stop();
    }

    public IEnumerator TouchEffect(Vector3 Pos)
    {
        touchParticle.transform.position = Pos;
        touchParticle.Play();
        yield return null;
        levelUpParticle.Stop();
    }

    public IEnumerator MergeEffect(Vector3 Pos, float playTime, int phase)
    {
        int scaleValue = phase % 3;
        Vector3 scaleVector = new Vector3(scaleValue, scaleValue, scaleValue);
        if(phase <= 3)
        {
            fantasyMergeParticle.transform.position = Pos;
            fantasyMergeParticle.Play();
            yield return new WaitForSeconds(playTime);
            fantasyMergeParticle.Stop();
            fantasyMergeParticle.transform.localScale = scaleVector;
        }
        else
        {
            modernMergeParticle.transform.position = Pos;
            modernMergeParticle.Play();
            yield return new WaitForSeconds(playTime);
            modernMergeParticle.Stop();
            fantasyMergeParticle.transform.localScale = scaleVector;
        }
    }

    public IEnumerator BoxOpenEffect(Vector3 Pos, float playTime)
    {
        boxOpenParticle.transform.position = Pos;
        boxOpenParticle.Play();
        yield return new WaitForSeconds(playTime);
        boxOpenParticle.Stop();
    }


}
