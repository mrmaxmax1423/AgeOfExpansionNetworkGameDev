using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UICraft : MonoBehaviour
{
    public Sprite axe1;
    public string axe1Name;
    public int axe1WoodCost;
    public int axe1OreCost;


    public Sprite bow1;
    public string bow1Name;
    public int bow1WoodCost;
    public int bow1OreCost;


    private Transform container;
    private Transform craftItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        craftItemTemplate = container.Find("craftItemTemplate");
        craftItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateCraftButton(axe1, axe1Name, axe1WoodCost, axe1OreCost, 0);
        CreateCraftButton(bow1, bow1Name, bow1WoodCost, bow1OreCost, 1);
    }

    private void CreateCraftButton(Sprite itemSprite, string itemName, int itemWoodCost, int itemOreCost, int positionIndex)
    {
        Transform craftItemTransform = Instantiate(craftItemTemplate, container);
        craftItemTransform.gameObject.SetActive(true);
        RectTransform craftItemRectTransform = craftItemTransform.GetComponent<RectTransform>();

        float craftItemHeight = 30f;
        craftItemRectTransform.anchoredPosition = new Vector2(0, -craftItemHeight * positionIndex);

        craftItemTransform.Find("itemName").GetComponent<Text>().text = itemName;
        craftItemTransform.Find("woodCostText").GetComponent<Text>().text = "x" + itemWoodCost.ToString();
        craftItemTransform.Find("oreCostText").GetComponent<Text>().text = "x" + itemOreCost.ToString();

        craftItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

    }

}
