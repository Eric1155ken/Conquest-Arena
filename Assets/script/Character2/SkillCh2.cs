using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class SkillCh2 : MonoBehaviourPun, IPunObservable
{
    GameObject selectedCharacter;

    public Stats myStats;
    public staminaStat myStamina;

    public float duration = 1.0f;
    public int staminaCost = 40;
    public float lifeStealValue = 20f;

    private float originalSpeed;

    bool isSkillLaunching = false;

    public GameObject canvas;

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
                photonView.RPC("LaunchSkill", RpcTarget.All);

                PhotonNetwork.Instantiate("Explode", transform.position, Quaternion.identity);

                canvas.GetComponent<skillTakeEffectShow>().StartLaunching(duration);
            }
        }
    }

    [PunRPC]
    public void LaunchSkill()
    {
        GameObject[] Characters = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject Character in Characters)
        {
            if (Character.GetComponent<Info>().TeamNum != GetComponent<Info>().TeamNum)
            {
                selectedCharacter = Character;
                break;
            }
        }

        isSkillLaunching = true;

        //originalSpeed = selectedCharacter.GetComponent<NavMeshAgent>().speed;

        selectedCharacter.GetComponent<NavMeshAgent>().isStopped = true;
        selectedCharacter.GetComponent<Stats>().TakeDamage(selectedCharacter, lifeStealValue);
        myStats.TakeDamage(gameObject, -lifeStealValue);

        Invoke(nameof(ResetParameter), duration);
    }

    [PunRPC]
    private void ResetParameter()
    {
        //selectedCharacter.GetComponent<NavMeshAgent>().speed = originalSpeed;
        selectedCharacter.GetComponent<NavMeshAgent>().isStopped = false;

        isSkillLaunching = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
}
