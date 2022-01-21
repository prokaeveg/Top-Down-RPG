using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int dollarsAmount = 5;
    protected override void OnCollect()
    {
        if (!collected)
        {
            //temp
            GameManager.instance.dollards += dollarsAmount;

            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.ShowText("+" + dollarsAmount + " dollars!", 25, Color.yellow, transform.position, Vector3.up * 50, 1.0f);
        }
    }
}
