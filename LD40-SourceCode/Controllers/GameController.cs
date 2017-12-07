using System.Collections.Generic;
using UnityEngine;

public class GameController : UnitySingleton<GameController>
{
    public delegate void CoreCarryChange (bool carry);
    public static event CoreCarryChange OnCoreChange;

    public Transform Player;
    public enum Difficulty
    {
        Casual = 3,
        Normal = 6,
        Difficult = 9,
        Hardcore = 12
    }
    public Difficulty difficulty = Difficulty.Normal;
    public Transform ammoPickups;
    public Transform spiderSpawns;
    public GameObject spiderPrefab;

    List<Transform> spawns = new List<Transform>();
    List<Spider> spiders = new List<Spider>();
    List<PowerCoreHolder> powerCores = new List<PowerCoreHolder>();
    bool carryingCore;
    int remaining;

    public bool HasCore
    {
        get
        {
            return carryingCore;
        }
    }

    public int NumberOfCores
    {
        get
        {
            return powerCores.Count;
        }
    }

    public int NumberOfCoresRemaining
    {
        get
        {
            return remaining;
        }
    }

    protected override void Awake ()
    {
        base.Awake();

        MenuController.Instance.canvas.SetActive(false);
        switch(MenuController.Difficulty)
        {
            case 1:
                difficulty = Difficulty.Casual;
                break;
            case 2:
                difficulty = Difficulty.Normal;
                break;
            case 3:
                difficulty = Difficulty.Difficult;
                break;
            case 4:
                difficulty = Difficulty.Hardcore;
                break;
            default:
                difficulty = Difficulty.Normal;
                break;
        }
        print("Difficulty selected : " + difficulty);

        SortCores();
        carryingCore = false;

        foreach (Transform child in spiderSpawns)
        {
            spawns.Add(child);
        }

        SpawnSpiders(2);
    }

    public void CarryingCore(bool carrying)
    {
        carryingCore = carrying;

        if (OnCoreChange != null)
        {
            OnCoreChange(carryingCore);
        }

        if (!carryingCore)
        {
            remaining--;
        }
        else
        {
            SpawnSpiders((int)difficulty / 3);
        }
    }

    public void DestroySpiders()
    {
        foreach (var spider in spiders)
        {
            Destroy(spider.gameObject);
        }
    }

    public void RemoveSpider(Spider s)
    {
        spiders.Remove(s);
    }

    public void EndGame(bool win)
    {
        UIController.Instance.HideOxygenLabel();
        UIController.Instance.ShowEndGameScreen(win);
    }

    void SpawnSpiders(int toSpawn)
    {
        while (toSpawn > 0)
        {
            int index = Random.Range(0, spawns.Count);

            // Check distance to player
            if (Vector3.Distance(Player.position, spawns[index].position) < 30f)
            {
                continue;
            }

            // Check spawn distance to spiders
            foreach (var spider in spiders)
            {
                if (Vector3.Distance(Player.position, spider.transform.position) < 8f)
                {
                    continue;
                }
            }

            // Spawn if got this far
            GameObject g = Instantiate(spiderPrefab, spawns[index].position, Quaternion.identity);
            spiders.Add(g.GetComponent<Spider>());
            toSpawn--;
        }
    }

    void SortCores()
    {
        powerCores.AddRange(FindObjectsOfType<PowerCoreHolder>());
        List<PowerCoreHolder> temp = new List<PowerCoreHolder>();

        while (temp.Count < (int)Difficulty.Hardcore - (int)difficulty)
        {
            int index = Random.Range(0, powerCores.Count);
            if (!temp.Contains(powerCores[index]))
            {
                temp.Add(powerCores[index]);
            }
        }

        foreach (var tmp in temp)
        {
            powerCores.Remove(tmp);
            Destroy(tmp.gameObject);
        }

        remaining = NumberOfCores;
    }
}
