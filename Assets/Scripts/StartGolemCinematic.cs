using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGolemCinematic : MonoBehaviour
{
    public Animator camAnimator;
    public Animator golemAnimator;
    public bool golemCutsceneTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && golemCutsceneTriggered == false)
        {
            camAnimator.SetBool("golemCutscene", true);
            golemCutsceneTriggered = true;
            Invoke(nameof(ShowAttack), 1f);
            Invoke(nameof(StopCutscene), 3f);
        }
    }

    void StopCutscene()
    {
        camAnimator.SetBool("golemCutscene", false);
    }

    void ShowAttack()
    {
        golemAnimator.SetBool("Attacking", true);
    }
}
