using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera _arCamera;
    private ARRaycastManager _arRaycastManager;
    private List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private GameObject _arPointer;

    private bool _isInitialPosition;
    private bool _isOverUI;
    private bool _isOverItemModel;

    private Vector2 _initialTouchPosition;

    private GameObject _selectedItem;
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
                _isOverUI = IsTapOverUI(touchPosition);
                _isOverItemModel = IsOverItemModel(touchPosition);
            }

            if (touchOne.phase == TouchPhase.Moved)
            {
                if (_arRaycastManager.Raycast(touchOne.position, _hits, TrackableType.Planes))
                {
                    Pose hitPose = _hits[0].pose;

                    if (!_isOverUI && _isOverItemModel)
                        transform.position = hitPose.position;
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                    _initialTouchPosition = touchTwo.position - touchOne.position;

                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPosition = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(_initialTouchPosition, currentTouchPosition);
                    _itemModel.transform.rotation = Quaternion.Euler(0, _itemModel.transform.eulerAngles.y - angle, 0);
                    _initialTouchPosition = currentTouchPosition;
                }

                if(_isOverItemModel && _itemModel == null && !_isOverUI)
                {
                    GameManager.Instance.ARPosition();
                    _itemModel = _selectedItem;
                    _selectedItem = null;
                    _arPointer.SetActive(true);
                    transform.position = _itemModel.transform.position;
                    _itemModel.transform.parent = _arPointer.transform;
                }
            }
        }
    }

    private bool IsOverItemModel(Vector2 touchPosition)
    {
        Ray ray = _arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hitItemModel))
        {
            if (hitItemModel.collider.CompareTag("Item"))
            {
                _selectedItem = hitItemModel.transform.gameObject;
                return true;
            }
        }
        return false;
    }

    private bool IsTapOverUI(Vector2 touchPosition)
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
