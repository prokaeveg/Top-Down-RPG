using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Mover
{
    private SpriteRenderer SpriteRenderer;
    private bool isAlive = true;
    protected override void Start()
    {
        base.Start();
        SpriteRenderer = GetComponent<SpriteRenderer>();

    }
    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;
        base.ReceiveDamage(dmg);
        GameManager.instance.onHitPointChanged();
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        xSpeed = 1;
        ySpeed = 1;

        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinId)
    {
        GameManager.instance.currentCharacterSprite = skinId;
        SpriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
    }


    public void OnLevelUp()
    {
        maxHitpoints++;
        hitpoint = maxHitpoints;
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
            OnLevelUp();
        }
    }

    protected override void Death()
    {
        GameManager.instance.deathMenuAmin.SetTrigger("Show");
        Time.timeScale = 0f;
    }

    public void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoints)
            return;
        
        hitpoint += healingAmount;
        if (hitpoint > maxHitpoints)
            hitpoint = maxHitpoints;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.onHitPointChanged();
    }

    public void Respawn()
    {
        hitpoint = maxHitpoints;
        GameManager.instance.onHitPointChanged();
        isAlive = true;
        lastImmune = Time.time;
        pushDirection = Vector3.zero;
    }
}
