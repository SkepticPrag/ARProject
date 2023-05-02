using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private GameObject _buttonContainer;
    [SerializeField] private ItemButtonManager _itemButtonManager;

    private void Start()
    {
        GameManager.Instance.OnItemsMenu += CreateButtons;
    }

    private void CreateButtons()
    {
        foreach (var item in _items)
        {
            ItemButtonManager itemButton;
            itemButton = Instantiate(_itemButtonManager, _buttonContainer.transform);
            itemButton.ItemName = item.ItemName;
            itemButton.ItemImage = item.ItemImage;
            itemButton.ItemDescription = item.ItemDescription;
            itemButton.ItemModel = item.ItemModel;
            itemButton.name = item.ItemName;
        }

        GameManager.Instance.OnItemsMenu -= CreateButtons;
    }
}
