using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoom : MonoBehaviour
{
    public class mydata
    {
        public Vector3 pos;
        public Color col;
        public List<GameObject> gggg;
        public mydata(Vector3 pos, Color col, List<GameObject> gggg)
        {
            this.pos = pos;
            this.col = col;
            this.gggg = gggg;
        }
    }
    public GameObject prefab;

    public int allRoom;
    public int fightRoom;

    public int fightWay;

    Dictionary<Vector3, GameObject> prePos;

    List<mydata> datas;

    private void Awake()
    {
        datas = new List<mydata>();
        prePos = new Dictionary<Vector3, GameObject>();
        allRoom = allRoom == 0 ? Random.Range(3, 10) : allRoom;
        fightRoom = fightRoom == 0 ? Random.Range(1, allRoom) : fightRoom;

        fightWay = fightWay == 0 ? Random.Range(1, 7) : fightWay;

        Debug.Log(allRoom);
        Debug.Log(fightRoom);
        Debug.Log(fightWay);

        Create(Vector3.zero, Color.white);
        while (datas.Count > 0)
        {
            if (!Create(datas[0].pos, datas[0].col))
            {
                foreach (var item in datas[0].gggg)
                {
                    Destroy(item);
                }
            }
            datas.RemoveAt(0);
        }
    }

    bool Create(Vector3 pos, Color col)
    {
        if (prePos.ContainsKey(pos))
            return true;
        if (allRoom <= 0)
            return false;
        GameObject gg = Instantiate(prefab, pos, Quaternion.identity);
        gg.GetComponent<Renderer>().material.color = col;
        prePos.Add(pos, gg);

        int len;
        if (col == Color.white)
            len = 1;
        else
        {
            len = allRoom > 3 ? Random.Range(2, 4) : Random.Range(1, allRoom + 1);
            allRoom -= 1;
        }


        for (int i = 0; i < len; i++)
        {
            Vector3 ddd;
            do
            {
                Vector2 dir;
                int x = Random.Range(0, 2);
                if (x == 0)
                    dir = GetDir(Vector2.up);
                else
                    dir = GetDir(Vector2.left);
                ddd = new Vector3(dir.x, 0, dir.y);
            } while (prePos.ContainsKey(pos + ddd));

            List<GameObject> gggg = new List<GameObject>();
            for (int j = 0; j < 4; j++)
            {
                GameObject go = Instantiate(prefab, pos + ddd * (j + 1), Quaternion.identity);
                go.GetComponent<Renderer>().material.color = Color.gray;
                prePos.Add(pos + ddd * (j + 1), go);
                gggg.Add(go);
            }

            datas.Add(new mydata(pos + ddd * 5, Color.green, gggg));
        }
        return true;
    }

    Vector2 GetDir(Vector2 v2)
    {
        int s;

        s = Random.Range(0, 2);
        if (s == 0)
        {
            return -v2;
        }
        else
        {
            return v2;
        }
    }
}

