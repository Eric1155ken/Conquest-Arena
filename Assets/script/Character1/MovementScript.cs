using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;


public class MovementScript : MonoBehaviourPun
{
    private Animator _anim;
    private NavMeshAgent _agent;
    private bool Running = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {

            if (Physics.Raycast(ray, out hit, 100))
            {
                _agent.destination = hit.point;
            }
        }

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            Running = false;
        }
        else
        {
            Running = true;
        }
        _anim.SetBool("running", Running);
    }
}