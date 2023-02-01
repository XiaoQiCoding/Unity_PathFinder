using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapComp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MapParent;
    public List<List<Transform>> map;
    private PathFinding pathFinder;
    public GameObject zombie;
    public GameObject sunFlower;
    public Sprite ZombieSprite;
    public Sprite originSprite;

    private void Start()
    {
        pathFinder = GetComponent<PathFinding>();
        // 初始化地图（5行9列）
        map = new List<List<Transform>>();
        for (int i = 0; i < 5; i++)
        {
            Transform line = MapParent.transform.Find("line" + i);
            List<Transform> MapLine = new List<Transform>();
            for (int j = 0; j < 9; j++)
            {
                // print(i + "" + j);
                Transform box = line.Find("box" + j);
                MapLine.Add(box);
            }
            map.Add(MapLine);
        }
    }

    public void BreadFirst()
    {
        Vector2Int[] startAndEnd = GetStartAndEnd();
        pathFinder.BreadFirst(map, startAndEnd[0], startAndEnd[1]);
    }

    public void Dijkstra()
    {
        Vector2Int[] startAndEnd = GetStartAndEnd();
        pathFinder.Dijkstra(map, startAndEnd[0], startAndEnd[1]);
    }
    public void Astar()
    {
        Vector2Int[] startAndEnd = GetStartAndEnd();
        pathFinder.AStar(map, startAndEnd[0], startAndEnd[1]);
    }

    // 清理墙体
    public void ClearWall()
    {
        pathFinder.StopAllCoroutines();
        foreach (List<Transform> mapLine in map)
        {
            foreach (Transform box in mapLine)
            {
                box.gameObject.GetComponent<SpriteRenderer>().sprite = originSprite;
                box.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1,1,0.5f);
                // if (box.childCount > 0 && box.Find("Wallnut"))
                // {
                //     Destroy(box.Find("Wallnut").gameObject);
                // }
            }
        }
    }

    // 清理墙体
    public Vector2Int[] GetStartAndEnd()
    {
        Vector2Int[] startAndEnd = new Vector2Int[2];
        for (int i = 0; i < map.Count; i++)
        {
            List<Transform> mapLine = map[i];
            for (int j = 0; j < mapLine.Count; j++)
            {
                Transform box = mapLine[j];
                if (box.childCount > 0 && box.Find("Zombie"))
                {
                    startAndEnd[0] = new Vector2Int(i, j);
                }
                if (box.childCount > 0 && box.Find("Sunflower"))
                {
                    startAndEnd[1] = new Vector2Int(i, j);
                }
            }
        }
        return startAndEnd;
    }

    public void RenderBox(Vector2Int pos)
    {
        map[pos.x][pos.y].gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void RenderRoute(Vector2Int pos)
    {
        map[pos.x][pos.y].gameObject.GetComponent<SpriteRenderer>().sprite = ZombieSprite;
    }

}
