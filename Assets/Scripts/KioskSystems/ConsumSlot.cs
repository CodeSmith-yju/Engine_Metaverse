using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConsumSlot : MonoBehaviour
{
    public SelectedMenu selectedMenu;
    public TextMeshProUGUI selectedText;
    public void Init(SelectedMenu _selectMenu)
    {
        if (_selectMenu == null)
        {
            Debug.Log("is Null");
        }
        this.selectedMenu = _selectMenu;
        selectedText.text = selectedMenu.GetIndex().ToString();

        gameObject.SetActive(true);//생성자를 통해 생성되면 활성화되어 사용자에게 노출됨
    }

    public void GetOutWaitSlot()
    {
        this.selectedMenu = null;
        this.selectedText = null;

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
