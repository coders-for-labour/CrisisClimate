using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ManagerManager : MonoBehaviour
{
    public static ManagerManager instance { get; set; }
    public List<ManagerItem> AllItems { get; set; }

    public Transform contentTransform;

    public GameObject itemPrefab;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        AllItems = JsonConvert.DeserializeObject<List<ManagerItem>>(Resources.Load<TextAsset>("JSON/ManagerItems").ToString());

        SpawnItems();
    }

    public void SpawnItems()
    {
        for (int i = 0; i < AllItems.Count; i++)
        {
            var itemToSpawn = Instantiate(itemPrefab, contentTransform);
            var tapTimerRef = itemToSpawn.GetComponent<ManagerItem>();

            tapTimerRef.ConstructItem(
                 AllItems[i].itemDescription,
                  AllItems[i].imageName,
                    AllItems[i].locked,
                     AllItems[i].baseCost,
                     AllItems[i].itemID
                );

            //itemToSpawn.name = AllItems[i].itemName;
        }
    }

    public void SaveDatabaseState()
    {
        var jsonData = JsonConvert.SerializeObject(AllItems);
        System.IO.File.WriteAllText("Assets/Resources/JSON/ManagerItems.json", jsonData);
    }
}
