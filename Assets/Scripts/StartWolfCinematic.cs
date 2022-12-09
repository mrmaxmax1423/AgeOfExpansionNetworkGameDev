using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWolfCinematic : MonoBehaviour
{
    public Animator camAnimator;
    public Animator wolfAnimator;
    public bool wolfCutsceneTriggered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && wolfCutsceneTriggered == false)
        {
            camAnimator.SetBool("wolfCutscene", true);
            wolfCutsceneTriggered = true;
            Invoke(nameof(ShowAttack), 1f);
            Invoke(nameof(StopCutscene), 3f);
        }
    }

    void StopCutscene()
    {
        camAnimator.SetBool("wolfCutscene", false);
    }

    void ShowAttack()
    {
        wolfAnimator.SetBool("Attacking", true);
    }
}
