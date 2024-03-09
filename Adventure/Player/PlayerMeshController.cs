using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    static SkinnedMeshRenderer[] skinnedMeshRenderer;
    static MeshRenderer[] meshRenderer;

    static Material[] skinnedOriginalMaterial;
    static Material[] originalMaterial;

    static Material boostMaterial;
    static Material damageMaterial;

    public static bool dmgState = false;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        meshRenderer = GetComponentsInChildren<MeshRenderer>();

        skinnedOriginalMaterial = new Material[skinnedMeshRenderer.Length];
        originalMaterial = new Material[meshRenderer.Length];

        for(int i =0; i<skinnedMeshRenderer.Length; i++)
        {
            skinnedOriginalMaterial[i] = skinnedMeshRenderer[i].material;
        }

        for (int i = 0; i < meshRenderer.Length; i++)
        {
            originalMaterial[i] = meshRenderer[i].material;
        }

        boostMaterial = Resources.Load("BoostMaterial", typeof(Material)) as Material;
        damageMaterial = Resources.Load("DefaultMaterial", typeof(Material)) as Material;
    }

    public void BoostModeOn()
    {
        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material = boostMaterial;
        }

        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material = boostMaterial;
        }
    }

    public void OriginalMaterialOn()
    {
        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material = skinnedOriginalMaterial[i];
        }

        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material = originalMaterial[i];
        }
    }

    public static IEnumerator DamageMaterialOn()
    {
        dmgState = true;

        while (dmgState == true && skinnedMeshRenderer[0].material == skinnedOriginalMaterial[0])
        {
            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = damageMaterial; 
            }
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = damageMaterial;
            }

            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = skinnedOriginalMaterial[i];
            }
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = originalMaterial[i];
            }

            yield return new WaitForSeconds(0.3f);
        }

        for (int i = 0; i < skinnedMeshRenderer.Length; i++)
        {
            skinnedMeshRenderer[i].material = skinnedOriginalMaterial[i];
        }
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material = originalMaterial[i];
        }
    }

    public static IEnumerator OneTimeDamageMaterialOn()
    {
        if (skinnedMeshRenderer[0].material == skinnedOriginalMaterial[0])
        {
            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = damageMaterial;
            }
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = damageMaterial;
            }

            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                skinnedMeshRenderer[i].material = skinnedOriginalMaterial[i];
            }
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = originalMaterial[i];
            }
        }

        yield return null;
    }
}
