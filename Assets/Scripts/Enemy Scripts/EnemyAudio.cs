using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour {

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip scream_Clip, walk;

    [SerializeField]
    private AudioClip attack_Clips;

    // Use this for initialization
    void Awake () {
        audioSource = GetComponent<AudioSource>();
	}

    public void Play_ScreamSound() {
        audioSource.clip = scream_Clip;
        audioSource.Play();
    }

    public void Play_AttackSound() {
        audioSource.clip = attack_Clips;
        audioSource.Play();
    }

    public void Playwalk() {
        audioSource.clip = walk;
        audioSource.Play();
    }

} // class


































