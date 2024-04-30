using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoolDownShow : MonoBehaviour
{
    [Header("Basic Attack")]
    public Image Image1;
    public bool isCooldown1 = false;
    public float cooldownTime1;

    [Header("Special Attack")]
    public Image Image2;
    public bool isCooldown2 = false;
    public float cooldownTime2;

    void Start()
    {
        Image1.fillAmount = 1;
        Image2.fillAmount = 1;
    }

    private void Update()
    {
        if (isCooldown1)
        {
            BasicAttackCooldownUI();
        }
        if (isCooldown2)
        {
            SpecialAttackCooldownUI();
        }
    }


    public void StartCooldown1(float cooldownTime)
    {
        isCooldown1 = true;
        cooldownTime1 = cooldownTime;
    }

    public void StartCooldown2(float cooldownTime)
    {
        isCooldown2 = true;
        cooldownTime2 = cooldownTime;
    }

    public void BasicAttackCooldownUI()
    {
        Image1.fillAmount -= 1 / cooldownTime1 * Time.deltaTime;

        if(Image1.fillAmount <= 0)
        {
            Image1.fillAmount = 1;
            isCooldown1 = false;
        }
    }
    public void SpecialAttackCooldownUI()
    {
        Image2.fillAmount -= 1 / cooldownTime2 * Time.deltaTime;

        if (Image2.fillAmount <= 0)
        {
            Image2.fillAmount = 1;
            isCooldown2 = false;
        }
    }
}
