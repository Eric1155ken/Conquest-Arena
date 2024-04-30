using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackCh3 : MonoBehaviourPun
{
    float projectileSpeed = 30f;
    public float waitTime;
    public float waitTime2;
    private Vector3 targetPosition;

    public int staminaCost;
    public int staminaCost2;
    public staminaStat myCharacterStamina;

    private int attackMode = 1;

    private bool isCooldown1 = false;
    public float cooldownTime1;
    private bool isCooldown2 = false;
    public float cooldownTime2;
    public GameObject canvas;

    public AudioClip normalAttackAudio;
    public AudioClip specialAttackAudio;

    public Animator _animator;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas");
    }
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            attackMode = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            attackMode = 1;
        }


        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.Alpha3))
        {
            CancelInvoke(nameof(Shoot));

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Map")))
            {
                targetPosition = hit.point;

                if (attackMode == 1 && !isCooldown1)
                {
                    if (myCharacterStamina.currentStamina >= staminaCost)
                    {
                        myCharacterStamina.deductStamina(staminaCost);
                        Invoke(nameof(Shoot), waitTime);
                    }
                }
                else if (attackMode == 2 && !isCooldown2)
                {
                    if (myCharacterStamina.currentStamina >= staminaCost2)
                    {
                        myCharacterStamina.deductStamina(staminaCost2);

                        PhotonNetwork.Instantiate("gatheringEffect", transform.position, Quaternion.identity);
                        
                        Invoke(nameof(Shoot), waitTime2);
                    }
                }
            }
        }
    }

    void Shoot()
    {
        _animator.SetTrigger("attack");

        Vector3 shootDirection = (targetPosition - transform.position).normalized;

        GameObject newProjectile = null;

        if (attackMode == 1)
        {
            newProjectile = PhotonNetwork.Instantiate("projectile1Ch3", transform.position, Quaternion.identity);
            newProjectile.GetComponent<projectileEffect>().photonView.RPC("SynchronizeVelocity", RpcTarget.All, photonView.ViewID);

            isCooldown1 = true;
            Invoke(nameof(ResetCooldown1), cooldownTime1);

            PhotonNetwork.Instantiate("HitEffect", transform.position, Quaternion.identity);

            soundEffectManager.instance.photonView.RPC("PlaySoundEffectClip", RpcTarget.All, "normalAttackAudio", transform.position, 0.5f);

            canvas.GetComponent<CoolDownShow>().StartCooldown1(cooldownTime1);
        }
        else if (attackMode == 2)
        {
            newProjectile = PhotonNetwork.Instantiate("projectile2Ch3", transform.position, Quaternion.identity);
            newProjectile.GetComponent<projectileEffect2>().photonView.RPC("SynchronizeVelocity", RpcTarget.All, photonView.ViewID);

            isCooldown2 = true;
            Invoke(nameof(ResetCooldown2), cooldownTime2);

            PhotonNetwork.Instantiate("hitEffect2", transform.position, Quaternion.identity);

            soundEffectManager.instance.photonView.RPC("PlaySoundEffectClip", RpcTarget.All, "specialAttackAudio", transform.position, 0.5f);

            canvas.GetComponent<CoolDownShow>().StartCooldown2(cooldownTime2);
        }

        newProjectile.GetComponent<Rigidbody>().velocity = shootDirection * projectileSpeed;


        //Collider myCollider = GetComponent<Collider>();

        //Collider projectileCollider = newProjectile.GetComponent<Collider>();

        //Physics.IgnoreCollision(myCollider, projectileCollider);
    }

    private void ResetCooldown1()
    {
        isCooldown1 = false;
    }

    private void ResetCooldown2()
    {
        isCooldown2 = false;
    }
}
