using System.Collections.Generic;
using UI;
using UnityEngine;

public class SetStageMaskTransform : MonoBehaviour
{
    private const int CellWidthsBetweenStagePoints = 5;

    [Header("Settings")]
    [SerializeField] private GetStageWorldPosition.StagePoints[] _stageAnchors = new GetStageWorldPosition.StagePoints[2];
    [SerializeField] private int _cellsNumberToCoverWidth = 8;
    [SerializeField] private int _cellsNumberToCoverHeight = 1;

    private Dictionary<GetStageWorldPosition.StagePoints, Vector3> _stagePoints = new Dictionary<GetStageWorldPosition.StagePoints, Vector3>();
    private Vector3[] _currentMaskWorldPoints = new Vector3[2];

    private void OnEnable()
    {
        SetupTransform();
    }

    private void SetupTransform()
    {
        _stagePoints.Clear();
        _stagePoints = AppManager.TutorialManager.GetStagePoints();

        if (_stagePoints.Count == 0)
        {
            return;
        }

        for (int i = 0; i < 2; i++)
        {
            _currentMaskWorldPoints[i] = _stagePoints[_stageAnchors[i]];
        }

        float cellWidth = (
            (GetScreenPos(_stagePoints[GetStageWorldPosition.StagePoints.DownStageRight])) -
            (GetScreenPos(_stagePoints[GetStageWorldPosition.StagePoints.DownstageLeft]))).x
            / CellWidthsBetweenStagePoints;

        float cellHeight = (
            (GetScreenPos(_stagePoints[GetStageWorldPosition.StagePoints.DownStageRight])) -
            (GetScreenPos(_stagePoints[GetStageWorldPosition.StagePoints.BackstageRight]))).y;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        SetWidth(rectTransform, cellWidth * _cellsNumberToCoverWidth);
        SetHeight(rectTransform, cellHeight * _cellsNumberToCoverHeight);

       rectTransform.SetPositionAndRotation(new Vector3(0, GetScreenPos(_stagePoints[GetStageWorldPosition.StagePoints.BackstageLeft]).y, 0), Quaternion.identity);
    }

    private Vector3 GetScreenPos(Vector3 worldPos)
    {
        return Camera.main.WorldToScreenPoint(worldPos);
    }

    public void SetSize(RectTransform rectTransform, Vector2 newSize)
    {
        Vector2 oldSize = rectTransform.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        rectTransform.offsetMin = rectTransform.offsetMin - new Vector2(deltaSize.x * rectTransform.pivot.x, deltaSize.y * rectTransform.pivot.y);
        rectTransform.offsetMax = rectTransform.offsetMax + new Vector2(deltaSize.x * (1f - rectTransform.pivot.x), deltaSize.y * (1f - rectTransform.pivot.y));
    }

    public void SetWidth(RectTransform rectTransform, float newSize)
    {
        SetSize(rectTransform, new Vector2(newSize, rectTransform.rect.size.y));
    }

    public void SetHeight(RectTransform rectTransform, float newSize)
    {
        SetSize(rectTransform, new Vector2(rectTransform.rect.size.x, newSize));
    }

}