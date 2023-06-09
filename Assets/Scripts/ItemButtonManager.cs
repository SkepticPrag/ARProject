using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButtonManager : MonoBehaviour
{
    [SerializeField] private string _itemName;

    public string ItemName { set { _itemName = value; } get { return _itemName; } }

    [SerializeField] private Sprite _itemImage;

    public Sprite ItemImage { set { _itemImage = value; } get { return _itemImage; } }

    [SerializeField] private string _itemDescription;

    public string ItemDescription { set { _itemDescription = value; } get { return _itemDescription; } }

    [SerializeField] private GameObject _itemModel;

    public GameObject ItemModel { set { _itemModel = value; } get { return _itemModel; } }

    private ARInteractionManager _arInteractionManager;

    private void Start()
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = _itemName;
        transform.GetChild(1).GetComponent<RawImage>().texture = _itemImage.texture;
        transform.GetChild(2).GetComponent<TMP_Text>().text = _itemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.Instance.ARPosition);
        button.onClick.AddListener(Create3DModel);

        _arInteractionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
        _arInteractionManager.ItemModel = Instantiate(_itemModel);
    }
}
