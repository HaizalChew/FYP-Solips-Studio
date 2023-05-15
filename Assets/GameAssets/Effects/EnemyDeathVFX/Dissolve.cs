using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Material dissolveMat;
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.5f;
    [SerializeField] private float delay;
    //public float RefreshRate = 0.025f;

    private Material[] skinnedMaterials;
    private float counter = 0;
    private bool isResetMaterials;
    private bool startDissolve;

    private void Awake()
    {
        health = GetComponent<Health>();
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;
        }
    }

    //private void Start()
    //{
    //    StartCoroutine(Dissolve());
    //}

    //IEnumerator Dissolve()
    //{
    //    if (skinnedMaterials.Length > 0)
    //    {
    //        float counter = 0;

    //        while (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
    //        {
    //            counter += dissolveRate;

    //            for (int i = 0; i < skinnedMaterials.Length; i++)
    //            {
    //                skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
    //            }

    //            yield return new WaitForSeconds(RefreshRate);
    //        }
    //    }
    //}

    private void Start()
    {
        if (VFXGraph != null)
        {
            
        }
    }

    private void Update()
    {

        //if (skinnedMaterials.Length > 0 && health.isDead)
        //{
        //    if (skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
        //    {
        //        counter += dissolveRate * Time.deltaTime;

        //        for (int i = 0; i < skinnedMaterials.Length; i++)
        //        {
        //            skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
        //        }
        //    }
        //}

        if (!isResetMaterials && health.isDead)
        {
            isResetMaterials = true;
            Invoke(nameof(ResetMaterial), delay);
        }

        if (health.isDead && startDissolve)
        {
            if (skinnedMesh.material.GetFloat("_DissolveAmount") < 1)
            {
                counter += dissolveRate * Time.deltaTime;

                skinnedMesh.material.SetFloat("_DissolveAmount", counter);
            }

        }
    }

    private void ResetMaterial()
    {
        VFXGraph.Play();
        skinnedMesh.material = dissolveMat;
        startDissolve = true;
        
    }

}
