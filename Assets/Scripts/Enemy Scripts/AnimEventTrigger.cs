using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventTrigger : MonoBehaviour
{
    private EnemyAudio aud;
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponentInParent<EnemyAudio>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void playScream()
    {
        aud.Play_ScreamSound();
    }

    public void playWalk()
    {
        aud.Playwalk();
    }

    public void playAttack()
    {
        aud.Play_AttackSound();
    }


}
