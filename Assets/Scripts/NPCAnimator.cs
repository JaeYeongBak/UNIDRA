using System.Collections;
using UnityEngine;

public class NPCAnimator : MonoBehaviour
{
    Animator animator;
    float WaitingTime = 0.0f;

    bool isTalk = false;

    public void setIsTalk()
    {
        if(isTalk)
        {
            isTalk = false;
        }
        else
        {
            isTalk = true;
            animator.SetTrigger("isTalk");
            WaitingTime = 0.0f;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTalk)
        {
            WaitingTime += Time.deltaTime;

            animator.SetFloat("WaitingTime", WaitingTime);

            if (WaitingTime > 4.0f)
                WaitingTime -= 4.0f;
        }
    }
}
