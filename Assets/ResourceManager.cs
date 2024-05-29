using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public Dictionary<string, Sprite> menuResource = new();
    public List<Sprite> menuIcons = new();

    private void Awake()
    {
        menuResource.Clear();
        menuResource.Add("¾Æ¾Æ", menuIcons[0]);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
