using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehavior : MonoBehaviour
{
    private Transform container;
    private Transform craftItemTemplate;

    [SerializeField]
    public Text oreCounter, woodCounter;

    public int woodAmount;
    public int oreAmount;

    public string woods;

    public int woodCost = 0;
    public int oreCost = 0;

    public bool axeOwned = false;
    public bool shieldOwned = false;
    public bool bowOwned = false;

    public void OnAxeCraftClick()
    {
        woods = (oreCounter.text);
        oreAmount = PlayerControl.oreCount;

        if (woodAmount > woodCost && oreAmount > oreCost)
        {
            axeOwned = true;
        }
    }
    public void OnShieldCraftClick()
    {
        woodAmount = PlayerControl.woodCount;
        oreAmount = PlayerControl.oreCount;

        if (woodAmount > woodCost && oreAmount > oreCost)
        {
            shieldOwned = true;
        }
    }
    public void OnBowCraftClick()
    {
        woodAmount = PlayerControl.woodCount;
        oreAmount = PlayerControl.oreCount;

        if (woodAmount > woodCost && oreAmount > oreCost)
        {
            bowOwned = true;
        }
    }
}
