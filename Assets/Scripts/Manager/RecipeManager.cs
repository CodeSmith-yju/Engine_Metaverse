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


    // 딕셔너리를 이용해서 레시피 설정
    private void Awake()
    {
        recipe = new Dictionary<string, List<string>>();

        recipe["아이스 아메리카노"] = new List<string> { "얼음", "냉수", "에스프레소" };
        recipe["아메리카노"] = new List<string> { "에스프레소", "온수" };
        recipe["아이스 카페라떼"] = new List<string> { "얼음", "우유", "에스프레소" };
        recipe["아이스 카페모카"] = new List<string> { "초콜릿", "우유", "에스프레소", "믹서기", "얼음" };
        recipe["초코라떼"] = new List<string> { "초콜릿", "우유", "믹서기", "얼음" };
        recipe["딸기라떼"] = new List<string> { "딸기", "우유", "믹서기", "얼음" };
        //recipe["요거트 스무디"] = new List<string> { "요거트 파우더", "우유", "얼음", "믹서기" };
        recipe["딸기 요거트 스무디"] = new List<string> { "요거트 파우더", "우유", "딸기", "얼음", "믹서기" };

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

    // 요리를 하는 메서드 (주문한 메뉴 이름, 현재 컵에 들어있는 재료 리스트)
    public bool Cook(string dish, List<string> ingredient)
    {
        if (recipe.ContainsKey(dish))
        {
            List<string> recipeingredient = recipe[dish];
            if (CheckIngredients(recipeingredient, ingredient))
            {
                Debug.Log("요리 성공" + dish);
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
                Debug.Log("재료 순서가 맞지 않거나 재료가 부족합니다");
                return false;
            }
        }
        else
        {
            Debug.Log("해당 레시피가 없습니다");
            return false;
        }
    }

    private bool CheckIngredients(List<string> recipeIngredients, List<string> ingredients)
    {
        int recipeIndex = 0; // 현재 요리 과정의 인덱스를 추적하기 위한 변수

        // 입력된 재료를 순회하면서 요리 과정의 재료와 순서를 비교
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

            // 모든 요리 과정이 완료되면 요리를 완료 true 반환
            if (recipeIndex >= recipeIngredients.Count)
            {
                return true;
            }
        }

        // 모든 재료를 확인한 후에도 요리 과정이 완료되지 않았으면 false를 반환
        return false;
    }


}
