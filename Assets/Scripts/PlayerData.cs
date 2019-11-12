using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    int timeAwaySeconds;
    long currentTimeAsTicks;
    DateTime lastSeenAsDateTime;

    public static PlayerData instance { get; set; }
    public PlayerDataVariables PlayerDataVariables { get; set; }

    public static event Action<int> timeAwayAward;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        PlayerDataVariables = JsonConvert.DeserializeObject<PlayerDataVariables>(Resources.Load<TextAsset>("JSON/PlayerData").ToString());
    }

    public void Start()
    {
        lastSeenAsDateTime = new DateTime(PlayerDataVariables.lastSeen);
        SetTimeAwayAsSeconds();

    }

    public void SetLastTime()
    {
        currentTimeAsTicks = DateTime.Now.Ticks;
        PlayerDataVariables.lastSeen = currentTimeAsTicks;
        SaveDatabaseState();
    }

    public void SaveDatabaseState()
    {
        var jsonData = JsonConvert.SerializeObject(PlayerDataVariables);
        System.IO.File.WriteAllText("Assets/Resources/JSON/PlayerData.json", jsonData);
    }

    public void SetTimeAwayAsSeconds()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSpan = currentTime - lastSeenAsDateTime;

        timeAwaySeconds = timeSpan.Seconds;

        if(timeAwayAward != null)
            timeAwayAward(timeAwaySeconds);

        //for now
        CurrencyInventory.instance.AddCurrentBalance(timeAwaySeconds);
    }

    public IEnumerator WaitforUI()
    {
        while (UIController.instance == null)
        {
            yield return null;
        }
        lastSeenAsDateTime = new DateTime(PlayerDataVariables.lastSeen);
        UIController.instance.ToggleOfflineReward(true, timeAwaySeconds);
        SetTimeAwayAsSeconds();
        yield return null;
    }

    public void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            StartCoroutine(WaitforUI());
        }
        else
        {
            SetLastTime();
        }
    }

    public void OnApplicationQuit()
    {
        SetLastTime();
    }

}

[Serializable]
public class PlayerDataVariables
{
    public long lastSeen;
}
