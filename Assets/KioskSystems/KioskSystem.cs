using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KioskSystem : MonoBehaviour
{
    public static KioskSystem single;
    [Header("Kiosk Object")]
    public List<GameObject> kioskScene;//Ű����ũ�� ǥ�õ� ȭ��, ����Ʈ�� �����ϸ� ���ҵ�?

    public Dictionary<int, string> menus = new();// �ֹ����� �޴� ������ �����ϰԵ� ����, player��ũ��Ʈ���� �ٸ� ������Ʈ���� ��ȣ�ۿ������� ������ ���� ���� �� ���־�� �ϹǷ� public
    [SerializeField] private List<int> ticketNumbers = new();//�ֹ����� ��ȣ���� ����, 
    [SerializeField] private int ticketNum = 0;//�ֹ����� �޴� �ε���, ������ Key��, ��� ���� �ٲ��� �� ��, private
    public string menuName = "";// ������ Value��, ��ư��ȣ�ۿ��� ���Ͽ� �ش� ��ư������Ʈ�� �̸��� �����Ұ��̹Ƿ� public

    // ���� ȭ��
    public Image kioskBuyPanel;

    // Ƽ�� ��� ȭ��
    [SerializeField] private Image tiketIssuance;// ���� ȭ���� OK��ư Ŭ�� ��, ��µ� �ֹ���ȣ ��� ȭ��
    [SerializeField] private TextMeshProUGUI textNumberKiosk;// Ű����ũ�� ǥ�õ� ���÷��� �ؽ�Ʈ

    [Header("SellerDisplay Object")]
    public TextMeshProUGUI textNumberellerDisPlay;// �Ǹ��� �����ǿ� ǥ�õ� ���÷��� �ؽ�Ʈ

    [Header("ConsumerDisplay Object")]
    public TextMeshProUGUI textNumberConsumerDisPlay;// �Һ��� �����ǿ� ǥ�õ� ���÷��� �ؽ�Ʈ

    public int kioIndex = 0;
    public bool buyCheck = false;

    public Button btnBuyOK;

    //�ֹ��� ��ǰ ���۵Ǿ����� �ֹ��� �մ� ȣ��
    [Header("Order")]
    public Image imgOrder;
    public TextMeshProUGUI textOrder;
    private int removeCount = 0;

    public Button btnQuiteKiosk;
    
    private void Awake()
    {
        single = this;
        KioskStart();

    }
    public void KioskSceneChange()
    {
        switch (kioIndex)
        {
            case 0:
                Debug.Log("case 0");
                Debug.Log(kioIndex);
                kioskScene[0].SetActive(false);
                kioskScene[1].SetActive(true);
                kioIndex++;
                Debug.Log(kioIndex);
                break;
            case 1:
                if (buyCheck == true)
                {
                    Debug.Log("case 1");
                    Debug.Log(kioIndex);
                    kioskScene[0].SetActive(false);
                    kioskScene[1].SetActive(false);
                    //IssuanceOK()
                }
                break;
            default:
                break;
        }

    }
    public void TakeTicket()
    {
        if (ticketNum >= 999) // ��ȣǥ �ʱ�ȭ
        {
            ticketNumbers.Clear();
            ticketNum = 0;
        }
        ticketNumbers.Add(++ticketNum);
        menus.Add(ticketNum, menuName);

        //Ű����ũ�� ǥ�õ� ���ڰ� Update��.
        KioskUpdate();

        //Update�� ���ڰ� �����ǿ� ǥ�õ�
        SellerDisplayUpdate();
        ConsumerDisplayUpdate();
    }

    private void KioskUpdate()
    {
        string kioskText = "";
        if (menus.ContainsKey(ticketNum))
        {
            if (ticketNum < 10)
            {
                kioskText += "�ֹ��Ͻ� �޴�: " + menus[ticketNum] + "\n �ֹ� ��ȣ: \n 00" + ticketNum.ToString();
            }
            else if (ticketNum < 100)
            {
                kioskText += "�ֹ��Ͻ� �޴�: " + menus[ticketNum] + "\n �ֹ� ��ȣ: \n 0" + ticketNum.ToString();
            }
            else
            {
                kioskText += "�ֹ��Ͻ� �޴�: " + menus[ticketNum] + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
            }
        }
        else
        {
            kioskText += "�ֹ��Ͻ� �޴�: " + menuName + "\n �ֹ� ��ȣ: \n" + ticketNum.ToString();
        }

        textNumberKiosk.text = kioskText;
    }

    private void SellerDisplayUpdate()
    {
        string displayText = "";

        List<int> sortedTicketNumbers = ticketNumbers.OrderBy(num => num).ToList();

        for (int i = 0; i < sortedTicketNumbers.Count; i++)
        {
            displayText += menus[sortedTicketNumbers[i]] + sortedTicketNumbers[i];
            if (sortedTicketNumbers.Count > 1 && i < sortedTicketNumbers.Count - 1)
            {
                displayText += ", ";
            }
        }

        textNumberellerDisPlay.text = displayText;
    }

    private void ConsumerDisplayUpdate()
    {
        string displayText = string.Join(", ", ticketNumbers);
        textNumberConsumerDisPlay.text = displayText;
    }

    public void RemoveNum()
    {
        if (removeCount == 0)
        {
            imgOrder.gameObject.SetActive(true);

            if (menus.Count > 0)
            {
                if (ticketNumbers.Count > 0)
                {
                    textOrder.text = ticketNumbers[0].ToString();
                }
            }
            removeCount = 1;
        }
        else
        {
            imgOrder.gameObject.SetActive(false);
            if (menus.Count > 0)
            {
                if (ticketNumbers.Count > 0)
                {
                    int ticketToRemove = ticketNumbers[0]; // ù ��° ��� ��������
                    ticketNumbers.RemoveAt(0); // ����Ʈ���� ù ��° ��� ����
                    menus.Remove(ticketToRemove); // ���������� �ش� Ƽ�� ����

                    //Ű����ũ�� ǥ�õ� ���ڰ� Update��.
                    KioskUpdate();

                    //Update�� ���ڰ� �����ǿ� ǥ�õ�
                    SellerDisplayUpdate();
                    ConsumerDisplayUpdate();
                    removeCount = 0;
                }
            }
        }

    }

    private void KioskStart()
    {
        //���� ���ʷ� ���۵Ǿ����� Ű����ũ�� �⺻ȭ������ �ʱ�ȭ�Ǿ�� �Ѵ�. �̶� �ٸ� Ű����ũ�� �ɼǵ鵵 ���� �ʱ�ȭ�� �شٸ� ���� ��.
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
            btnQuiteKiosk.gameObject.SetActive(false);
        }


    }

    public void OnBuyMenue()
    {
        buyCheck = true;
        if (buyCheck == true)
        {
            Debug.Log("BuyChek OK");
            TakeTicket();
            tiketIssuance.gameObject.SetActive(buyCheck);
            KioskSceneChange();
            kioIndex++;
        }
    }
    public void IssuanceOK()
    {
        buyCheck = false;
        tiketIssuance.gameObject.SetActive(buyCheck);
        kioIndex = 0;
        btnQuiteKiosk.gameObject.SetActive(false);
    }
    public void OnQuiteKiosk()
    {
        for (int i = 0; i < kioskScene.Count; i++)
        {
            kioskScene[i].SetActive(false);
        }

        kioskBuyPanel.gameObject.SetActive(false);
        tiketIssuance.gameObject.SetActive(false);
        
        btnQuiteKiosk.gameObject.SetActive(false);
        kioIndex = 0;
    }

}
