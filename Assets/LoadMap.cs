using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LoadMap : MonoBehaviour
{
    public GameObject prefab;
    string[] lines;

    List<List<char>> json;

    private void Awake()
    {


        json = new List<List<char>>();

        lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "/Resources/map.txt");


        //for (int i = 0; i < lines.Length; i++)
        //{
        //    json.Add(new List<char>());
        //    foreach (var item in lines[i])
        //    {
        //        json[i].Add(item);
        //    }
        //}


        //System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "/Resources/map.json",
        //    JsonConvert.SerializeObject(json));

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                GameObject gg = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                switch (lines[i][j])
                {
                    case ' ':
                        gg.GetComponent<Renderer>().enabled = false;
                        break;
                    case '0':
                        gg.GetComponent<Renderer>().material.color = Color.white;
                        break;
                    case '1':
                        gg.GetComponent<Renderer>().material.color = Color.gray;
                        break;
                    case '2':
                        gg.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case '3':
                        gg.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case '4':
                        gg.GetComponent<Renderer>().material.color = Color.blue;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void Update()
    {
        Debug.Log(Random.Range(0, 2));
    }

}
