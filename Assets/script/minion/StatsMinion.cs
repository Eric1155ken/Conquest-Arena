using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMinion : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    private float currentHealth;
    private float targetHealth;

    public float damageLerpDuration;
    private Coroutine damageCoroutine;

    private void Awake()
    {
        currentHealth = health;
        targetHealth = health;
    }


    // Update is called once per frame
    public void TakeDamage(GameObject target, float damageAmount)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        StatsMinion targetStats = target.GetComponent<StatsMinion>();
        targetStats.targetHealth -= damageAmount;

        if (targetStats.targetHealth <= 0)
        {
            PhotonNetwork.Destroy(target);
        }
        else if (targetStats.damageCoroutine == null)
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

        while (elapsedTime < damageLerpDuration)
        {
            currentHealth = Mathf.Lerp(currentHealth, targetHealth, elapsedTime / damageLerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentHealth = targetHealth;

        damageCoroutine = null;
    }
}
