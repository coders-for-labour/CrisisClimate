using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TapTimerController : MonoBehaviour
{
    public static TapTimerController instance { get; set; }
    public List<TapTimer> AllDatabaseItems { get; set; }
    public List<TapTimer> PopulatedItems { get; set; } = new List<TapTimer>();

    public Transform contentTransform;

    public GameObject itemPrefab;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        AllDatabaseItems = JsonConvert.DeserializeObject<List<TapTimer>>(Resources.Load<TextAsset>("JSON/TapTimer").ToString());

        SpawnItems();
    }

    public void SpawnItems()
    {
        PopulatedItems.Clear();
        for (int i = 0; i < AllDatabaseItems.Count; i++)
        {
            var itemToSpawn = Instantiate(itemPrefab, contentTransform);
            var tapTimerRef = itemToSpawn.GetComponent<TapTimer>();

            tapTimerRef.ConstructItem(
                AllDatabaseItems[i].itemName,
                 AllDatabaseItems[i].itemDescription,
                  AllDatabaseItems[i].imageName,
                   AllDatabaseItems[i].baseTimerLength,
                    AllDatabaseItems[i].numberOwned,
                    AllDatabaseItems[i].locked,
                     AllDatabaseItems[i].baseCost,
                      AllDatabaseItems[i].valueToAddOnCompletion,
                      AllDatabaseItems[i].itemID,
                      AllDatabaseItems[i].hasManager

                );

            itemToSpawn.name = AllDatabaseItems[i].itemName;

            PopulatedItems.Add(tapTimerRef);
        }
    }

    public void SaveDatabaseState()
    {
        var jsonData = JsonConvert.SerializeObject(AllDatabaseItems);
        System.IO.File.WriteAllText("Assets/Resources/JSON/TapTimer.json", jsonData);
    }

    public TapTimer GetItem(string itemName)
    {
        foreach (TapTimer item in AllDatabaseItems)
        {
            if (item.titleText.text == itemName)
                return item;
        }
        Debug.Log("Couldn't find item: " + itemName);
        return null;
    }
}
