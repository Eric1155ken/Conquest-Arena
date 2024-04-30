using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SkillCh3 : MonoBehaviourPun
{
    public Stats myHealth;
    public GameObject projectilePrefab1;
    public staminaStat myStamina;
    public GameObject canvas;

    public float duration;
    public int staminaCost;
    public int newDamageValue;

    private int originalDamage;

    private bool isSkillLaunching = false;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }
    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (!isSkillLaunching && myStamina.currentStamina >= staminaCost)
            {
                myStamina.deductStamina(staminaCost);
                LaunchSkill();
            }
        }
    }

    public void LaunchSkill()
    {
        isSkillLaunching = true;

        PhotonNetwork.Instantiate("Explode", transform.position, Quaternion.identity);

        canvas.GetComponent<skillTakeEffectShow>().StartLaunching(duration);

        originalDamage = projectilePrefab1.GetComponent<projectileEffect>().damageAmount;

        myHealth.TakeDamage(gameObject, -myHealth.currentHealth);
        projectilePrefab1.GetComponent<projectileEffect>().damageAmount = newDamageValue;

        Invoke(nameof(ResetParameter), duration);
    }

    private void ResetParameter()
    {
        projectilePrefab1.GetComponent<projectileEffect>().damageAmount = originalDamage;

        isSkillLaunching = false;
    }
}
