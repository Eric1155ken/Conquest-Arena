using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Die : MonoBehaviourPun
{   
    
    public Stats myStats;
    public GameObject canvas;
    GameObject losePanel;
    GameObject image1;
    GameObject image2;
    GameObject image3;
    GameObject healthBar;
    GameObject staminaBar;
    GameObject miniMap;

    GameEndController gameEndController;
    public Info myInfo;

    public AudioClip dieAudio;

    public Animator _animator;

    private void Awake()
    {
        gameEndController = GameObject.Find("GameManager").GetComponent<GameEndController>();
        canvas = GameObject.Find("Canvas");
        losePanel = canvas.transform.Find("losePanel").gameObject;
        image1 = canvas.transform.Find("BasicAttack").gameObject;
        image2 = canvas.transform.Find("SpecialAttack").gameObject;
        image3 = canvas.transform.Find("Skill").gameObject;
        healthBar = canvas.transform.Find("HealthBar2D").gameObject;
        staminaBar = canvas.transform.Find("StaminaBar2D").gameObject;
        miniMap = canvas.transform.Find("Minimap").gameObject;

        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (myStats.currentHealth <= 0 || transform.position.y < 7)
        {
            gameEndController.photonView.RPC("updateTeamMember", RpcTarget.All, gameObject.name, myInfo.TeamNum);

            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                ;
            }
            else
            {
                losePanel.SetActive(true);
                image1.SetActive(false);
                image2.SetActive(false);
                image3.SetActive(false);
                staminaBar.SetActive(false);
                healthBar.SetActive(false);
                miniMap.SetActive(false);

                _animator.SetTrigger("die");
                soundEffectManager.instance.photonView.RPC("PlaySoundEffectClip", RpcTarget.All, "dieAudio", transform.position, 0.5f);

                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
    
}
