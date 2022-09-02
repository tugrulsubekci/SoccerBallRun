using Google.Play.Review;
using System.Collections;
using UnityEngine;

public class InAppReview : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    private void Start()
    {
        _reviewManager = new ReviewManager();
    }
    public IEnumerator Review()
    {
        // REQUEST

        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        // LAUNCH

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            DataManager.Instance.isInAppReviewShown = true;
            DataManager.Instance.Save();
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
    public void StartReview()
    {
        StartCoroutine(nameof(Review));
    }
}
