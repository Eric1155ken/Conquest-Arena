//Attribution Declaration: The script is adapted from this tutorial: 
//    https://www.youtube.com/watch?v=Dj_pSEDxs8o&list=PLuKMRhgr5rGkgABx8Sezws-ekSWIWRf4Q&index=7

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviourPun
{
    public float maxHealth;
    public float currentHealth;
    private float targetHealth;

    private float damageLarpDuration = 0.01f;
    private Coroutine damageCoroutine;

    private HealthUI healthUI;

    public GameObject bloodSplashPrefab;


    private void Awake()
    {
        healthUI = GetComponent<HealthUI>();
        currentHealth = maxHealth;
        targetHealth = maxHealth;
    }

    private void Start()
    {
        healthUI.StartSlider(maxHealth);
    }

    public void TakeDamage(GameObject target, float damageAmount)
    {
        if (damageAmount > 0)
        {
            Instantiate(bloodSplashPrefab, transform.position, Quaternion.identity);
        }

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        //PhotonNetwork.Instantiate("Blood Splash", transform.position, Quaternion.identity);

        Stats targetStats = target.GetComponent<Stats>();


        targetStats.targetHealth -= damageAmount;


        if (targetStats.targetHealth > targetStats.maxHealth)
        {
            targetStats.targetHealth = targetStats.maxHealth;
        }
        else if (targetStats.targetHealth <= 0) 
        {
            healthUI.Update2Dslider(0);
            healthUI.Update3DSlider(0);
            currentHealth = 0;
        }


        if (targetStats.damageCoroutine == null)
        {
            targetStats.StartLerpHealth();
        }
    }

    private void StartLerpHealth()
    {
        damageCoroutine = StartCoroutine(LerpHealth());
    }

    private IEnumerator LerpHealth()
    {
        float elapsedTime = 0;

        while(elapsedTime < damageLarpDuration)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, elapsedTime / damageLarpDuration);

            healthUI.Update2Dslider(currentHealth);
            healthUI.Update3DSlider(currentHealth);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentHealth = targetHealth;

        healthUI.Update2Dslider(currentHealth);
        healthUI.Update3DSlider(currentHealth);

        damageCoroutine = null;
    }
}
