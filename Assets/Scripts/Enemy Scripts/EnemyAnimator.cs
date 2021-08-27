using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour {

    private Animator anim;

	void Awake () {
        anim = GetComponentInChildren<Animator>();	
	}
	
    public void Idle(bool walk) {
        anim.SetBool("Idle", walk);
    }

    public void Run(bool walk)
    {
        anim.SetBool("Run", walk);
    }

    public void Hit(bool walk)
    {
        anim.SetBool("Hit", walk);
    }


    public void IdleToWalk()
    { 
        anim.SetFloat("WalkSpeed", 1.1f);

    }

    public void WalkToIdle()
    {
        anim.SetFloat("WalkSpeed",0.1f) ;

    }

    public void WalkToRun()
    {
        Run(true);
        anim.SetFloat("WalkSpeed", 2.1f);

    }



} // class































