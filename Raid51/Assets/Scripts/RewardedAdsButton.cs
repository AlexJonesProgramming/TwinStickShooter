using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

#if UNITY_IOS
    private string gameId = "3252514";
#elif UNITY_ANDROID
    private string gameId = "3252515";
#endif

    Button myButton;
    private string myPlacementId = "rewardedVideo";

    public AfterLevel afterLevel;
    public GameObject afterDeathUI;
    public enum AdType
    {
        unlockCharacter,
        respawn
    }
    public AdType adType;

    void Start()
    {

        if (adType == AdType.respawn)
            myPlacementId = "RespawnWithBonusHealth";
        if (adType == AdType.unlockCharacter)
            myPlacementId = "UnlockCharacter";


        myButton = GetComponent<Button>();
        // Set interactivity to be dependent on the Placement’s status:
        myButton.interactable = Advertisement.IsReady(myPlacementId);

        // Map the ShowRewardedVideo function to the button’s click listener:
        if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

        // Initialize the Ads listener and service:
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    // Implement a function for showing a rewarded video ad:
    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if (adType == AdType.respawn)
            {
                afterLevel.Respawn();
                afterDeathUI.SetActive(false);
            }
            if (adType == AdType.unlockCharacter)
                afterLevel.UnlockCharacter();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            if (adType == AdType.respawn)
            {
                afterLevel.Respawn();
                afterDeathUI.SetActive(false);
            }
            if (adType == AdType.unlockCharacter)
                afterLevel.UnlockCharacter();
        }
        else if (showResult == ShowResult.Failed)
        {
            if (adType == AdType.respawn)
            {
                afterLevel.Respawn();
                afterDeathUI.SetActive(false);
            }
            if (adType == AdType.unlockCharacter)
                afterLevel.UnlockCharacter();
        }

        
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}