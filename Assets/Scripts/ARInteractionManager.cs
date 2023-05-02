using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera _arCamera;
    private ARRaycastManager _arRaycastManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _arPointer;

    private bool _isInitialPosition;
    private bool _isOverUI;

    private GameObject _itemModel;

    public GameObject ItemModel
    {
        set
        {
            _itemModel = value;
            _itemModel.transform.position = _arPointer.transform.position;
            _itemModel.transform.parent = _arPointer.transform;
            _isInitialPosition = true;

        }
        get { return _itemModel; }
    }

    private void Start()
    {
        _arPointer = transform.GetChild(0).gameObject;
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        GameManager.Instance.OnMainMenu += SetItemPosition;
    }

    private void Update()
    {
        if (_isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            _arRaycastManager.Raycast(middlePointScreen, _hits, TrackableType.Planes);
            if (_hits.Count > 0)
            {
                transform.position = _hits[0].pose.position;
                transform.rotation = _hits[0].pose.rotation;
                _arPointer.SetActive(true);
                _isInitialPosition = false;
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);
            if (touchOne.phase == TouchPhase.Began)
            {
                var touchPosition = touchOne.position;
                _isOverUI = isTapOverUI(touchPosition);
            }

            if (touchOne.phase == TouchPhase.Moved)
            {
                if (_arRaycastManager.Raycast(touchOne.position, _hits, TrackableType.Planes))
                {
                    Pose hitPose = _hits[0].pose;
                    if (!_isOverUI)
                        transform.position = hitPose.position;
                }
            }
        }
    }

    private bool isTapOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touchPosition.x, touchPosition.y);

        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);

        return result.Count > 0;
    }

    private void SetItemPosition()
    {
        if (_itemModel != null)
        {
            _itemModel.transform.parent = null;
            _arPointer.SetActive(false);
            _itemModel = null;
        }
    }

    public void DeleteItem()
    {
        Destroy(_itemModel);
        _arPointer.SetActive(false);
        GameManager.Instance.MainMenu();
    }
}
