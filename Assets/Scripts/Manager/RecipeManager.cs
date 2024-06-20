using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private Dictionary<string, List<string>> recipe;
    public List<string> recipe_Name;
    public Dictionary<string, GameObject> recipe_Food;
    public List<GameObject> food_Obj_List;


    // ��ųʸ��� �̿��ؼ� ������ ����
    private void Awake()
    {
        recipe = new Dictionary<string, List<string>>();

        recipe["���̽� �Ƹ޸�ī��"] = new List<string> { "����", "�ü�", "����������" };
        recipe["�Ƹ޸�ī��"] = new List<string> { "����������", "�¼�" };
        recipe["���̽� ī���"] = new List<string> { "����", "����", "����������" };
        recipe["���̽� ī���ī"] = new List<string> { "���ݸ�", "����", "����������", "�ͼ���", "����" };
        recipe["���ڶ�"] = new List<string> { "���ݸ�", "����", "�ͼ���", "����" };
        recipe["�����"] = new List<string> { "����", "����", "�ͼ���", "����" };
        //recipe["���Ʈ ������"] = new List<string> { "���Ʈ �Ŀ��", "����", "����", "�ͼ���" };
        recipe["���� ���Ʈ ������"] = new List<string> { "���Ʈ �Ŀ��", "����", "����", "����", "�ͼ���" };

        recipe_Food = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        recipe_Name = new List<string>();
        int index = 0;

        foreach (string key in recipe.Keys)
        {
            recipe_Name.Add(key);

            if (index < food_Obj_List.Count)
            {
                recipe_Food[key] = food_Obj_List[index];
                index++;
            }
        }
    }

    // �丮�� �ϴ� �޼��� (�ֹ��� �޴� �̸�, ���� �ſ� ����ִ� ��� ����Ʈ)
    public bool Cook(string dish, List<string> ingredient)
    {
        if (recipe.ContainsKey(dish))
        {
            List<string> recipeingredient = recipe[dish];
            if (CheckIngredients(recipeingredient, ingredient))
            {
                Debug.Log("�丮 ����" + dish);
                /*GameObject food = Instantiate(recipe_Food[dish]);
                Player player = FindObjectOfType<Player>();

                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    food.transform.position = player.transform.position; 
                }*/
                
                return true;
            }
            else
            {
                Debug.Log("��� ������ ���� �ʰų� ��ᰡ �����մϴ�");
                return false;
            }
        }
        else
        {
            Debug.Log("�ش� �����ǰ� �����ϴ�");
            return false;
        }
    }

    private bool CheckIngredients(List<string> recipeIngredients, List<string> ingredients)
    {
        int recipeIndex = 0; // ���� �丮 ������ �ε����� �����ϱ� ���� ����

        // �Էµ� ��Ḧ ��ȸ�ϸ鼭 �丮 ������ ���� ������ ��
        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i] == recipeIngredients[recipeIndex])
            {
                recipeIndex++;
            }
            else
            {
                return false;
            }

            // ��� �丮 ������ �Ϸ�Ǹ� �丮�� �Ϸ� true ��ȯ
            if (recipeIndex >= recipeIngredients.Count)
            {
                return true;
            }
        }

        // ��� ��Ḧ Ȯ���� �Ŀ��� �丮 ������ �Ϸ���� �ʾ����� false�� ��ȯ
        return false;
    }


}
