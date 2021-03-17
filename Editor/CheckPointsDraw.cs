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
                        if (t.GetChild(i).GetChild(j).transform.name.Contains("jumpdir"))
                        {
                            t.GetChild(i).GetChild(j).transform.SetSiblingIndex(t.GetChild(i).childCount);
                        }
                    }
                    t.GetChild(k).transform.name = k + "";
                }
            }
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < t.childCount; i++)
        {
            //Debug.Log(i + "_");
            for (int j = 0; j < t.GetChild(i).childCount; j++)
            {
                // Debug.Log(i+"_"+j);
                if (!t.GetChild(i).GetChild(j).transform.name.Contains("jumpdir"))
                {
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
        Gizmos.color = Color.yellow;
        Transform t2 = GameObject.Find("CheckPoints_Ladders").transform;
        for (int i = 0; i < t2.childCount; i++)
        {
            string[] s = t2.GetChild(i).name.Split('_');
            Gizmos.DrawSphere(((t.GetChild(int.Parse(s[0])).position + t.GetChild(int.Parse(s[1])).position) / 2 + t.GetChild(int.Parse(s[0])).position) / 2, 0.2f);
        }

        Gizmos.color = Color.green;
        Transform t3 = GameObject.Find("CheckPoints_Jumps").transform;
        for (int i = 0; i < t3.childCount; i++)
        {
            string[] s = t3.GetChild(i).name.Split('_');
            Gizmos.DrawSphere(((t.GetChild(int.Parse(s[0])).position + t.GetChild(int.Parse(s[1])).position) / 2 + t.GetChild(int.Parse(s[0])).position) / 2, 0.2f);
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
                Transform tr = GameObject.Find("CheckPoints_Ladders").transform;
                for (int i = 0; i < tr.childCount; i++)
                {
                    string[] s = tr.GetChild(i).name.Split('_');
                    if (Selection.gameObjects[one].transform.name.Equals(s[0]) && Selection.gameObjects[two].transform.name.Equals(s[1]))
                    {
                        do_ladders = true;
                    }
                }
                if (!do_ladders)
                {
                    GameObject EmptyObj = new GameObject(Selection.gameObjects[one].transform.name + "_" + Selection.gameObjects[two].transform.name);
                    EmptyObj.transform.parent = GameObject.Find("CheckPoints_Ladders").transform;
                }
            }
            else if (jump)
            {
                bool do_jumps = false;
                Transform tr = GameObject.Find("CheckPoints_Jumps").transform;
                for (int i = 0; i < tr.childCount; i++)
                {
                    string[] s = tr.GetChild(i).name.Split('_');
                    if (Selection.gameObjects[one].transform.name.Equals(s[0]) && Selection.gameObjects[two].transform.name.Equals(s[1]))
                    {
                        do_jumps = true;
                    }
                }
                if (!do_jumps)
                {
                    GameObject EmptyObj = new GameObject(Selection.gameObjects[one].transform.name + "_" + Selection.gameObjects[two].transform.name);
                    EmptyObj.transform.parent = GameObject.Find("CheckPoints_Jumps").transform;
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