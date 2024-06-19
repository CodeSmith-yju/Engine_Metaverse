using UnityEngine;

public class ChairUI : MonoBehaviour
{
    public GameObject sitPrompt; // F 키 문구 오브젝트

    private void Start()
    {
        if (sitPrompt != null)
        {
            sitPrompt.SetActive(false); // 처음에는 비활성화
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
