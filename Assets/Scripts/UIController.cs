using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public CanvasGroup managerPopUp;
    public CanvasGroup offlinePopUp;

    public Button managerButton;
    public Button closeManagerButton;

    public TMPro.TextMeshProUGUI offlineRewardtext;

    public void Start()
    {
        if (instance == null)
            instance = this;

        managerButton.onClick.AddListener(() =>ToggleManagerPopup(true));
        closeManagerButton.onClick.AddListener(() => ToggleManagerPopup(false));
    }

    public void ToggleManagerPopup(bool active)
	{
        managerPopUp.alpha = active ? 1f : 0f;
        managerPopUp.blocksRaycasts = active;
        managerPopUp.interactable = active;
    }

    public void ToggleOfflineReward(bool active, int offlineRewardValue = 0)
    {
        offlinePopUp.alpha = active ? 1f : 0f;
        offlinePopUp.blocksRaycasts = active;
        offlinePopUp.interactable = active;

        if(active)
            offlineRewardtext.text = offlineRewardValue.ToString();
    }

    public void CloseOfflineReward()
    {
        ToggleOfflineReward(false);
    }
}
