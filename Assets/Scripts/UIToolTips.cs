using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToolTips : MonoBehaviour
{
    public GameObject Player;

    public GameObject GolemTipTrigger;
    public GameObject GolemTips;

    public GameObject WolfTipTrigger;
    public GameObject WolfTips;

    public float distanceFromGolemTip;
    public float distanceFromWolfTip;

    void FixedUpdate()
    {
        //makes tips display when character is near trigger
        distanceFromGolemTip = Vector3.Distance(Player.transform.position, GolemTipTrigger.transform.position);
        distanceFromWolfTip = Vector3.Distance(Player.transform.position, WolfTipTrigger.transform.position);


        if (distanceFromGolemTip < 4)
        {
            GolemTips.SetActive(true);
        }
        else
        {
            GolemTips.SetActive(false);
        }

        if (distanceFromWolfTip < 4)
        {
            WolfTips.SetActive(true);
        }
        else
        {
            WolfTips.SetActive(false);
        }
    }
}
