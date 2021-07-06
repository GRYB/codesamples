using UnityEngine;

namespace UI
{
    public class GetStageWorldPosition : MonoBehaviour
    {
        public enum StagePoints
        {
            BackstageLeft,
            BackstageRight,
            DownstageLeft,
            DownStageRight
        }

        [SerializeField] StagePoints _stagePoint;

        private void OnEnable()
        {
            AppManager.TutorialManager.RegisterStagePoint(_stagePoint, transform.position);
        }
    }
}