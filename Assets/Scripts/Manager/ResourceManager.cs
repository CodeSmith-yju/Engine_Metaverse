using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager single;

    public Dictionary<string, Sprite> menuResource = new();
    public List<Sprite> menuIcons = new();

    private void Awake()
    {
        single = this;
    }

    private void Start()
    {
        menuResource.Clear();
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[0].ToString(), menuIcons[0]);
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[1].ToString(), menuIcons[1]);
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[2].ToString(), menuIcons[2]);
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[3].ToString(), menuIcons[3]);
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[4].ToString(), menuIcons[4]);
        menuResource.Add(GameMgr.Instance.recipe.recipe_Name[5].ToString(), menuIcons[5]);
        /*menuResource.Add("아이스 카페모카", menuIcons[3]);
        menuResource.Add("딸기라떼", menuIcons[4]);
        menuResource.Add("딸기 요거트 스무디", menuIcons[5]);*/

    }

}
