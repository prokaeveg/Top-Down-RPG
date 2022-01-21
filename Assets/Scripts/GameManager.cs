using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud.gameObject);
            Destroy(menu.gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        instance.onHitPointChanged();
    }

    // Ressourses

    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References

    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitPointBar;
    public Animator deathMenuAmin;
    public GameObject hud;
    public GameObject menu;


    // Logic
    public int dollards;
    public int experience;
    public int currentCharacterSprite;


    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Upgrade weapon
    public bool TryUpgradeWeapon()
    {
        // is the weapon max level right now
        if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

        if (dollards >= weaponPrices[weapon.weaponLevel])
        {
            dollards -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoint Bar
    public void onHitPointChanged()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoints;
        hitPointBar.localScale = new Vector3(ratio, 1, 1);
    }

    // Experience system
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) // Max level
            {
                return r;
            }
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            for (int i = currLevel; i < GetCurrentLevel(); i++)
            {
                OnLevelUp();
            }
    }

    public void OnLevelUp()
    {
        Debug.Log("Level Up!");
        player.OnLevelUp();
        onHitPointChanged();
    }

    
    // On Scene Loaded
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    // Death menu and respawn
    public void Respawn()
    {
        LoadState(SceneManager.GetSceneByName("MainScene"), LoadSceneMode.Single);
        SceneManager.LoadScene("MainScene");
        player.Respawn();
        Time.timeScale = 1f;
    }
    // Save state
    /*
     * INT preferedSkin
     * INT dollards
     * INT experience
     * INT weaponLevel
     */
    public void SaveState()
    {
        string s = "";

        //s += currentCharacterSprite.ToString() + "|";
        s += "0" + "|";
        s += dollards.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState with s = " + s);
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;
        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // change player skin TODO
        //instance.player.SwapSprite(int.Parse(data[0]));

        dollards = int.Parse(data[1]);
        // Experience
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log("Load state with " + data[0] + "|" + data[1] + "|" + data[2] + "|" + data[3]);
    }

}
