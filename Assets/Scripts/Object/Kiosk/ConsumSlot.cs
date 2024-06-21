using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConsumSlot : MonoBehaviour
{
    public SelectedMenu selectedMenu;
    public TextMeshProUGUI selectedText;
    public void Init(SelectedMenu _selectMenu)
    {
        if (gameObject.transform.position.z != 0)//Debuging Code
        {
            Debug.Log("My Position: " + gameObject.transform.position.z);

            Vector3 np = gameObject.transform.localPosition;
            np.z = 0;
            gameObject.transform.localPosition = np;

            Debug.Log("My after Position: " + gameObject.transform.position.z);
        }


        if (_selectMenu == null)
        {
            Debug.Log("is Null");
        }
        this.selectedMenu = _selectMenu;
        selectedText.text = selectedMenu.GetIndex().ToString();

        gameObject.SetActive(true);//�����ڸ� ���� �����Ǹ� Ȱ��ȭ�Ǿ� ����ڿ��� �����

    }

    public void GetOutWaitSlot()
    {
        this.selectedMenu = null;
        this.selectedText = null;

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
