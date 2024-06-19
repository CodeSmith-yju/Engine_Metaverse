using UnityEngine;

public class ChairUI : MonoBehaviour
{
    public GameObject sitPrompt; // F Ű ���� ������Ʈ

    private void Start()
    {
        if (sitPrompt != null)
        {
            sitPrompt.SetActive(false); // ó������ ��Ȱ��ȭ
        }
    }

    public void ShowSitPrompt(bool show)
    {
        if (sitPrompt != null)
        {
            sitPrompt.SetActive(show);
        }
    }
}
