using System;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    [SerializeField] float tresholdX = 60f;
    [SerializeField] int maxCardShiftX = 10;
    [SerializeField] float k = 0.02f;
    
    private Vector3 mouseUpPosition;
    private Vector3 mouseDownPosition;
    public event Action<bool> Swipe;
    public event Action<Vector3> DragPosUpdate;
    public event Action EndDrag;
    bool swipeEnabled = true;
    CardView currentCardView = null;

    private void OnEnable()
    {
        DragPosUpdate += CardPosUpdate;
        EndDrag += ResetCardPosition;
    }
    
    private void Update()
    {
        if (swipeEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDownPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                OnDragPosUpdate(Input.mousePosition - mouseDownPosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                float deltaX = (Input.mousePosition - mouseDownPosition).x;
                OnEndDrag();
                TrySwipe(deltaX);
            }
        }
    }

    bool TrySwipe(float delta)
    {
        if (Mathf.Abs(delta) > tresholdX)
        {
            if (delta > 0)
            {
                OnSwipe(true);
            }
            else
            {
                OnSwipe(false);
            }
            return true;
        }
        else
            return false;
    }

    Vector3 initialPosition;
    public void AttachCardViewdToSwipe(CardView cardView)
    {
        initialPosition = cardView.transform.position;
        currentCardView = cardView;
    }
    
    void CardPosUpdate(Vector3 direction)
    {
        if(currentCardView != null)
        {
            float x = direction.x;
            int dirSign = x > 0 ? 1 : -1;
            Vector3 newPosition = (initialPosition + new Vector3(x * k, 0, 0));
            Vector3 maxShiftPosition = (initialPosition + new Vector3(maxCardShiftX * dirSign, 0, 0));
            currentCardView.transform.position = Mathf.Abs(newPosition.x) < maxCardShiftX ? newPosition : maxShiftPosition;
        }
    }

    void ResetCardPosition()
    {
        if (currentCardView != null)
        {
            currentCardView.transform.position = initialPosition;
        }
    }

    void OnSwipe(bool direction)
    {
        Swipe?.Invoke(direction);
    }

    void OnDragPosUpdate(Vector3 direction)
    {
        DragPosUpdate?.Invoke(direction);
    }

    void OnEndDrag()
    {
        EndDrag?.Invoke();
    }

    private void OnDisable()
    {
        DragPosUpdate -= CardPosUpdate;
        EndDrag -= ResetCardPosition;
    }
}
