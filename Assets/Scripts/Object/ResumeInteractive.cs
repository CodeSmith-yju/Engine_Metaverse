using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResumeInteractive : MonoBehaviour
{
    bool isCheck = false;
    Players player;
    public List<string> resume_Name = new List<string>();
    public List<GameObject> resume_Obj = new List<GameObject>();
    [SerializeField] private GameObject resume_Popup;
    public Transform resume_Parent;
    public GameObject resume_Prefab;

    private void Update()
    {
        if (isCheck)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (player.GetRole() == Role.Manager)
                {
                    Debug.Log("이력서 제출 명단 확인");
                    ResumeCheck();
                }
                else if (player.GetRole() == Role.Customer)
                {
                    Debug.Log("이력서 제출");
                    if (!player.resume_Done)
                    {
                        ResumeSubmit();
                    }
                    else
                    {
                        Debug.Log("이미 이력서를 제출했습니다.");
                        return;
                    }
                    
                }
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isCheck = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        player = collision.gameObject.GetComponent<Players>();
    }

    private void OnCollisionExit(Collision collision)
    {
        isCheck = false;
    }

    public void ResumeCheck()
    {
        Debug.Log("팝업 띄우기");
        if (!resume_Popup.activeSelf)
        {
            resume_Popup.SetActive(true);
        }
        else
        {
            resume_Popup.SetActive(false);
        }
    }


    public void ResumeSubmit()
    {
        resume_Name.Add(player.name);
        GameObject obj = Instantiate(resume_Prefab, resume_Parent);
        resume_Obj.Add(obj);

        foreach (GameObject resumes in resume_Obj)
        {
            ResumeObj resume = resumes.GetComponent<ResumeObj>();

            foreach (string text in resume_Name)
            {
                resume.nameText.text = text;
            }
            resume.submit.onClick.AddListener(() => player.SetRole(Role.Empolyee));
        }
        player.resume_Done = true;
    }


}
