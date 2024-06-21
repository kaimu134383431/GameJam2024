using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Dictionary<Item.ItemType, int> items = new Dictionary<Item.ItemType, int>();
    private int Score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeItems();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void InitializeItems()
    {
        Score = 0;
        foreach (Item.ItemType itemType in System.Enum.GetValues(typeof(Item.ItemType)))
        {
            items[itemType] = 0;
        }
    }

    public void AddItem(Item.ItemType itemType, int amount)
    {
        if (items.ContainsKey(itemType))
        {
            items[itemType] += amount;
        }
        else
        {
            items[itemType] = amount;
        }
    }

    public int GetItemAmount(Item.ItemType itemType)
    {
        if (items.ContainsKey(itemType))
        {
            return items[itemType];
        }
        return 0;
    }

    public void RemoveItem(Item.ItemType itemType, int amount)
    {
        if (items.ContainsKey(itemType))
        {
            items[itemType] -= amount;
            if (items[itemType] < 0)
            {
                items[itemType] = 0; // Å’á0ŒÂ‚ÉÝ’è‚·‚é
            }
        }
    }

    public int GetScore()
    {
        return Score;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public Dictionary<Item.ItemType, int> GetAllItems()
    {
        return new Dictionary<Item.ItemType, int>(items);
    }
}
