using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckPointsDraw : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool sort = false;
    public bool show = false;
    public bool ladder = false;
    public bool jump = false;
    private void OnDrawGizmos()
    {
        Transform t = GameObject.Find("CheckPoints").transform;
        if (sort)
        {
            for (int k = 0; k < t.childCount; k++)
            {
                for (int i = 0; i < t.childCount; i++)
                {
                    for (int j = 0; j < t.GetChild(i).childCount; j++)
                    {
                        if (t.GetChild(i).GetChild(j).transform.name.Equals(transform.GetChild(k).transform.name))
                        {
                            t.GetChild(i).GetChild(j).transform.name = k + "";
                            t.GetChild(k).transform.name = k + "";
                        }
                        if (t.GetChild(i).GetChild(j).transform.name.Contains("jumpdir") || t.GetChild(i).GetChild(j).transform.name.Contains("ladder"))
                        {
                            t.GetChild(i).GetChild(j).transform.SetSiblingIndex(t.GetChild(i).childCount);
                        }
                    }
                    t.GetChild(k).transform.name = k + "";
                }
            }
        }

        for (int i = 0; i < t.childCount; i++)
        {
            //Debug.Log(i + "_");
            for (int j = 0; j < t.GetChild(i).childCount; j++)
            {
                // Debug.Log(i+"_"+j);
                if (t.GetChild(i).GetChild(j).transform.name.Contains("jumpdir"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(((t.GetChild(i).transform.position + t.GetChild(int.Parse(t.GetChild(i).GetChild(j).GetChild(0).name)).position) / 2 + t.GetChild(i).transform.position) / 2, 0.2f);
                }
                else if (t.GetChild(i).GetChild(j).transform.name.Contains("ladder")) 
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(((t.GetChild(i).transform.position + t.GetChild(int.Parse(t.GetChild(i).GetChild(j).GetChild(0).name)).position) / 2 + t.GetChild(i).transform.position) / 2, 0.2f);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Transform tchild = t.Find(t.GetChild(i).GetChild(j).transform.name);
                    Gizmos.DrawLine(t.GetChild(i).position, tchild.position);
                    for (int k = 0; k < tchild.childCount; k++)
                    {
                        if (tchild.GetChild(k).transform.name.Equals(i.ToString()))
                        {
                            Gizmos.DrawSphere((t.GetChild(i).position + tchild.position) / 2, 0.2f);
                        }
                    }
                }
            }
        }

        if (Selection.gameObjects.Length == 2 && Selection.gameObjects[0].transform.parent.name.Equals("CheckPoints"))
        {
            bool do_it = false;
            int one = 0;
            int two = 1;
            if (Selection.gameObjects[0] == Selection.activeObject)
            {
                int tmp = one;
                one = two;
                two = tmp;
            }
            for (int i = 0; i < Selection.gameObjects[one].transform.childCount; i++)
            {
                if (Selection.gameObjects[one].transform.GetChild(i).transform.name.Equals(Selection.gameObjects[two].transform.name))
                {
                    do_it = true;
                }
            }
            if (!do_it && !ladder && !jump)
            {
                GameObject EmptyObj = new GameObject(Selection.gameObjects[two].transform.name);
                EmptyObj.transform.parent = Selection.gameObjects[one].transform;
            }
            else if (ladder)
            {
                bool do_ladders = false;
                for (int i = 0; i < t.childCount; i++)
                {
                   if (Selection.gameObjects[one].transform.GetChild(Selection.gameObjects[one].transform.childCount-1).name.Equals("ladder"))
                    {
                        do_ladders = true;
                    }
                }
                if (!do_ladders)
                {
                    GameObject EmptyObj = new GameObject("ladder");
                    EmptyObj.transform.parent = Selection.gameObjects[one].transform;
                    GameObject EmptyObj2 = new GameObject(Selection.gameObjects[two].transform.name);
                    EmptyObj2.transform.parent = EmptyObj.transform;
                }
            }
            else if (jump)
            {
                bool do_jumps = false;
                for (int i = 0; i < t.childCount; i++)
                {
                    if (Selection.gameObjects[one].transform.GetChild(Selection.gameObjects[one].transform.childCount - 1).name.Equals("jumpdir"))
                    {
                        do_jumps = true;
                    }
                }
                if (!do_jumps)
                {
                    GameObject EmptyObj = new GameObject("jumpdir");
                    EmptyObj.transform.parent = Selection.gameObjects[one].transform;
                    GameObject EmptyObj2 = new GameObject(Selection.gameObjects[two].transform.name);
                    EmptyObj2.transform.parent = EmptyObj.transform;
                }
            }
        }
        if (show)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                MeshRenderer mr = transform.GetChild(i).gameObject.GetComponent<MeshRenderer>();
                mr.enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                MeshRenderer mr = transform.GetChild(i).gameObject.GetComponent<MeshRenderer>();
                mr.enabled = false;
            }
        }
    }
}
