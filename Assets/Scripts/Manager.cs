using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject workingPlace;

    [SerializeField]
    private Sprite managerAvailable;

    [SerializeField]
    private Sprite managerUnavailable;

    public int managerCost;
    private bool managerCanBeBought = false;

    public event Action OnManagerBought;
    public event Action OnManagerActivated;

    public bool activated = false;
    public float managerBonus { get; set; }
    [SerializeField]
    private Sprite activeSprite;
    [SerializeField]
    private Sprite inactiveSprite;

    private SpriteRenderer spriteRenderer;

    public bool managerBought = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (managerBought && OnManagerActivated != null)
        {
            OnManagerActivated();
        }

        else if (!managerBought && managerCanBeBought && OnManagerBought != null)
        {
            GameCore.Instance.Data.earnedMoney -= managerCost;
            managerBought = true;
            OnManagerBought();
            spriteRenderer.sprite = inactiveSprite;
        }
    }

    public void CheckIfManagerAvailable(int earnedMoney)
    {
        if (managerCost <= earnedMoney)
        {
            managerCanBeBought = true;
            spriteRenderer.sprite = managerAvailable;
        }
        else
        {
            managerCanBeBought = false;
            spriteRenderer.sprite = managerUnavailable;
        }
    }
}
