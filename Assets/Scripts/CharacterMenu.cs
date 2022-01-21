using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, dollarsText, upgradeCostText, xpText;

    // Logic
    [HideInInspector]
    public int currentCaracterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;


    // Character selection
    public void onArrowClick(bool right)
    {
        if (right)
        {
            currentCaracterSelection++;

            // if we went too far away

            if (currentCaracterSelection == GameManager.instance.playerSprites.Count)
                currentCaracterSelection = 0;

            OnSelectionChanged();
        }
        else
        {
            currentCaracterSelection--;

            // if we went too far away

            if (currentCaracterSelection < 0)
                currentCaracterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }

    public void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCaracterSelection];
        GameManager.instance.player.SwapSprite(currentCaracterSelection);
    }


    // Weapon upgrade
    public void onUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // Update the character information
    public void UpdateMenu()
    {
        //Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        // Meta
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        dollarsText.text = GameManager.instance.dollards.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

        //xp bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (GameManager.instance.GetCurrentLevel() == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total xp points"; // display total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }

    }
}
