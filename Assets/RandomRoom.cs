using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public GameObject player;
    //模型预制体
    public GameObject prefab;

    public int allRoom;//所有房间数量
    public int fightRoom;//战斗房间数量

    public int fightWay;//战斗道路数量

    public Dictionary<Vector3, GameObject> prePos;//所有模型点位

    List<mydata> datas;

    List<GameObject> roomgo;
    List<GameObject> waygo;

    string[] lines;

    List<List<char>> json;

    int showAR, showFR, showFW;

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (showAR != 0)
        {
            GUILayout.TextField($"所有房间：{showAR}");
        }
        if (showFR != 0)
        {
            GUILayout.TextField($"战斗房间：{showFR}");
        }
        if (showFW != 0)
        {
            GUILayout.TextField($"战斗道路：{showFW}");
        }
        GUILayout.EndHorizontal();
        if (GUI.Button(new Rect(50, 50, 150, 50), "随机生成地图"))
        {
            showAR = 0;
            showFR = 0;
            showFW = 0;
            allRoom = 0;
            fightRoom = 0;
            fightWay = 0;
            RandomGeneration();
        }

        if (GUI.Button(new Rect(50, 100, 150, 50), "加载地图"))
        {
            showAR = 0;
            showFR = 0;
            showFW = 0;
            LoadMap();
        }
    }

    private void RandomGeneration()
    {
        Destroy(goo);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        roomgo = new List<GameObject>();
        waygo = new List<GameObject>();
        datas = new List<mydata>();
        prePos = new Dictionary<Vector3, GameObject>();

        allRoom = allRoom == 0 ? Random.Range(5, 10) : allRoom;
        fightRoom = fightRoom == 0 ? Random.Range(1, allRoom) : fightRoom;

        fightWay = fightWay == 0 ? Random.Range(1, 7) : fightWay;

        showAR = allRoom;
        showFR = fightRoom;
        showFW = fightWay;
        Create(Vector3.zero, Color.white);
        while (datas.Count > 0)
        {
            if (!Create(datas[0].pos, datas[0].col))
            {
                foreach (var item in datas[0].gos)
                {
                    waygo.Remove(item);
                    prePos.Remove(item.transform.position);
                    Destroy(item);
                }
            }
            datas.RemoveAt(0);
        }


        for (int i = 0; i < fightRoom; i++)
        {
            do
            {
                int index = Random.Range(0, roomgo.Count);
                if (roomgo[index].GetComponent<Renderer>().material.color == Color.green)
                {
                    roomgo[index].GetComponent<Renderer>().material.color = Color.blue;
                    break;
                }
            } while (true);
        }
        //foreach (var item in waygo)
        //{
        //    Debug.Log(item.GetComponent<Renderer>().material.color == Color.gray);
        //}
        for (int i = 0; i < fightWay; i++)
        {
            do
            {
                int index = Random.Range(0, waygo.Count);
                if (waygo[index].GetComponent<Renderer>().material.color == Color.gray)
                {
                    Debug.Log("set red");
                    waygo[index].GetComponent<Renderer>().material.color = Color.red;
                    break;
                }
            } while (true);
        }

        goo = Instantiate(player, new Vector3(0, 1.58f, 0), Quaternion.identity);
        goo.SetActive(true);
    }

    bool Create(Vector3 pos, Color col)
    {
        if (prePos.ContainsKey(pos) && pos == Vector3.zero)
            return false;
        if (prePos.ContainsKey(pos))
            return true;
        if (allRoom <= 0)
            return false;

        GameObject gg = CreateCube(pos, col);
        prePos.Add(pos, gg);

        int len;
        if (col == Color.white)
        { len = 1; }
        else
        {
            len = allRoom > 3 ? Random.Range(2, 4) : Random.Range(1, allRoom + 1);
            allRoom -= 1;
        }


        for (int i = 0; i < len; i++)
        {
            Vector3 ddd = GetDir(pos);

            if (ddd == Vector3.zero)
            {
                return true;
            }

            List<GameObject> gos = new List<GameObject>();
            for (int j = 0; j < 4; j++)
            {
                GameObject go = CreateCube(pos + ddd * (j + 1), Color.gray);
                prePos.Add(pos + ddd * (j + 1), go);
                gos.Add(go);
            }

            datas.Add(new mydata(pos + ddd * 5, Color.green, gos));
        }
        return true;
    }

    GameObject CreateCube(Vector3 pos, Color col)
    {
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        go.GetComponent<Renderer>().material.color = col;
        go.transform.SetParent(transform);

        if (col == Color.gray)
        {
            waygo.Add(go);
        }
        if (col == Color.green)
        {
            roomgo.Add(go);
        }
        if (col == Color.white)
        {
            player.transform.position = new Vector3(pos.x, 1.5f, pos.z);
        }
        return go;
    }

    Vector3 GetDir(Vector3 pos)
    {
        Vector3 ddd;
        List<Vector2> alldir = new List<Vector2>();

        alldir.Add(Vector2.up);
        alldir.Add(Vector2.down);
        alldir.Add(Vector2.left);
        alldir.Add(Vector2.right);

        do
        {
            if (alldir.Count == 0)
            {
                return Vector3.zero;
            }
            int x = Random.Range(0, alldir.Count);
            Vector2 dir = alldir[x];
            alldir.Remove(dir);

            ddd = new Vector3(dir.x, 0, dir.y);

        } while (prePos.ContainsKey(pos + ddd));
        return ddd;
    }

    void LoadMap()
    {
        Destroy(goo);
        prePos = new Dictionary<Vector3, GameObject>();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        json = new List<List<char>>();

        lines = System.IO.File.ReadAllLines(System.Environment.CurrentDirectory + "/Resources/map.txt");

        for (int i = 0; i < lines.Length; i++)
        {
            json.Add(new List<char>());
            foreach (var item in lines[i])
            {
                json[i].Add(item);
            }
        }


        System.IO.File.WriteAllText(System.Environment.CurrentDirectory + "/Resources/map.json",
            JsonConvert.SerializeObject(json));

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                Vector3 pos = new Vector3(i, 0, j);
                GameObject gg = Instantiate(prefab, pos, Quaternion.identity);
                prePos.Add(pos, gg);
                gg.transform.SetParent(transform);
                switch (lines[i][j])
                {
                    case ' ':
                        gg.GetComponent<Renderer>().enabled = false;
                        prePos.Remove(pos);
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

        goo = Instantiate(player, new Vector3(5, 1.58f, 0), Quaternion.identity);
        goo.SetActive(true);
    }
    GameObject goo;
}


public class mydata
{
    public Vector3 pos;
    public Color col;
    public List<GameObject> gos;
    public mydata(Vector3 pos, Color col, List<GameObject> gos)
    {
        this.pos = pos;
        this.col = col;
        this.gos = gos;
    }
}