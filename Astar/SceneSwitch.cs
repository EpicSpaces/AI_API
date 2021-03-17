using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //GameObject go = GameObject.Find("Cube");
        //SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);
        //SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName("SampleScene"));
        
    }

 
public SpriteRenderer renderer;
   // Update is called once per frame
    void Update()
    {
       // SceneManager.UnloadSceneAsync("Game_Menu");
         
    }
}
