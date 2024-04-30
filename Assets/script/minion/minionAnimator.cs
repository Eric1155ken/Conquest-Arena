using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class minionAnimator : MonoBehaviourPun
{
    private Animator _anim;
    private NavMeshAgent _agent;
    private bool Speed = false;
    private minionAttack _attack;
    private bool isAttacking = false;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _attack = GetComponent<minionAttack>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (_agent.speed == 0)
        {
            Speed = false;
        }
        else
        {
            Speed = true;
        }
        _anim.SetBool("Speed", Speed);

        if(_attack.isAttacking == false)
        {
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
        }
        _anim.SetBool("Attack", isAttacking);
    }
}
