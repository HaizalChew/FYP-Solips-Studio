using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDrawingEffect : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject maskObj;
    [SerializeField] private GameObject swordObj;
    [SerializeField] private Transform swordMaskBase;
    [SerializeField] private GameObject swordDrawVFX;

    private GameObject vfx;

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sheathing Sword") || animator.GetCurrentAnimatorStateInfo(0).IsName("Withdrawing Sword"))
        {
            maskObj.SetActive(true);
        }
        else
        {
            maskObj.SetActive(false);
        }

        if (vfx != null)
        {
            vfx.transform.position = swordMaskBase.position;
            vfx.transform.rotation = swordMaskBase.rotation;
        }
    }

    public void SheathingSwordStart()
    {
        vfx = Instantiate(swordDrawVFX, swordMaskBase.position, swordMaskBase.rotation);
        
    }

    public void WithdrawSwordStart()
    {
        vfx = Instantiate(swordDrawVFX, swordMaskBase.position, swordMaskBase.rotation);
        swordObj.SetActive(true);
    }

    public void SheathingSwordStop()
    {
        swordObj.SetActive(false);
    }
}
