using TMPro;
using UnityEngine;

public class ConsumSlot : MonoBehaviour
{
    public SelectedMenu selectedMenu;
    public TextMeshProUGUI selectedText;
    public void Init(SelectedMenu _selectMenu)
    {
        this.selectedMenu = _selectMenu;

        gameObject.SetActive(true);//�����ڸ� ���� �����Ǹ� Ȱ��ȭ�Ǿ� ����ڿ��� �����
        selectedText.text = selectedMenu.GetIndex().ToString();
    }
}
