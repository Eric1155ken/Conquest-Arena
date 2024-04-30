using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class minionAttack : MonoBehaviourPun, IPunObservable
{
    private float attackRange = 1.0f;
    private float attackDamage = 10.0f;
    public float attackCooldown = 1.0f;

    private float lastAttackTime;
    public bool isAttacking = false;

    private minionAIScript2222 minionAI;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        minionAI = GetComponent<minionAIScript2222>();

        attackRange = minionAI.stopDistance + 0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (isAttacking)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                isAttacking = false;
            }
        }

        if (CanAttack())
        {
            Attack();
        }
    }

    private bool CanAttack()
    {
        if(!isAttacking && minionAI.currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, minionAI.currentTarget.position);
            return distanceToTarget <= attackRange;
        }
        return false;
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        isAttacking = true;

        if (minionAI.currentTarget.CompareTag("RedMinion") || minionAI.currentTarget.CompareTag("BlueMinion"))
        {
            minionAI.currentTarget.GetComponent<StatsMinion>().TakeDamage(minionAI.currentTarget.gameObject, attackDamage);
        }
        if (minionAI.currentTarget.CompareTag("Character")) {
            photonView.RPC("AttackCharacter", RpcTarget.All, minionAI.currentTarget.gameObject.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    public void AttackCharacter(int viewID)
    {
        GameObject targetCharacter = null;

        GameObject[] Characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject Character in Characters)
        {
            if (Character.GetPhotonView().ViewID == viewID)
            {
                targetCharacter = Character;
                break;
            }
        }

        targetCharacter.GetComponent<Stats>().TakeDamage(targetCharacter, attackDamage);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
}
