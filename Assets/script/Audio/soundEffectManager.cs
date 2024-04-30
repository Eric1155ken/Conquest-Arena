using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class soundEffectManager : MonoBehaviourPun, IPunObservable
{
    public static soundEffectManager instance;

    public AudioSource soundEffectPrefab;

    public AudioClip normalAttackAudio;
    public AudioClip specialAttackAudio;
    public AudioClip dieAudio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [PunRPC]
    public void PlaySoundEffectClip(string audioClipName, Vector3 spawnPosition, float volume)
    {
        AudioSource audioSourceObject = Instantiate(soundEffectPrefab, spawnPosition, Quaternion.identity);

        if (audioClipName == "normalAttackAudio")
        {
            audioSourceObject.clip = normalAttackAudio;
        }
        else if (audioClipName == "specialAttackAudio")
        {
            audioSourceObject.clip = specialAttackAudio;
        }
        else if (audioClipName == "dieAudio")
        {
            audioSourceObject.clip = dieAudio;
        }

        audioSourceObject.volume = volume;

        audioSourceObject.Play();

        float clipLength = audioSourceObject.clip.length;

        Destroy(audioSourceObject.gameObject, clipLength);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        ;
    }
}
