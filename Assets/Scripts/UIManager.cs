using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _itemsMenuCanvas;
    [SerializeField] private GameObject _ARPositionMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnMainMenu += ActivateMainMenu;
        GameManager.Instance.OnItemsMenu += ActivateItemsMenu;
        GameManager.Instance.OnARPosition += ActivateARPositionMenu;
    }
    private void ActivateMainMenu()
    {
        MainMenuActiveDoTween();
        ItemsMenuDeactiveDoTween();
        ARPositionMenuDeactiveDoTween();
    }

    private void ActivateItemsMenu()
    {
        MainMenuDeactiveDoTween();
        ItemsMenuActiveDoTween();
        ARPositionMenuDeactiveDoTween();
    }


    private void ActivateARPositionMenu()
    {
        MainMenuDeactiveDoTween();
        ItemsMenuDeactiveDoTween();
        ARPositionMenuActiveDoTween();
    }

    private void MainMenuActiveDoTween()
    {
        _mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        _mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        _mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(1, 1, 1), 0.3f);

    }

    private void MainMenuDeactiveDoTween()
    {
        _mainMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        _mainMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        _mainMenuCanvas.transform.GetChild(2).transform.DOScale(new Vector3(0, 0, 0), 0.3f);

    }

    private void ItemsMenuActiveDoTween()
    {
        _itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.5f);
        _itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
    }

    private void ItemsMenuDeactiveDoTween()
    {
        _itemsMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.5f);
        _itemsMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
    }

    private void ARPositionMenuActiveDoTween()
    {

        _ARPositionMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
        _ARPositionMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(1, 1, 1), 0.3f);
    }

    private void ARPositionMenuDeactiveDoTween()
    {

        _ARPositionMenuCanvas.transform.GetChild(0).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
        _ARPositionMenuCanvas.transform.GetChild(1).transform.DOScale(new Vector3(0, 0, 0), 0.3f);
    }
}
