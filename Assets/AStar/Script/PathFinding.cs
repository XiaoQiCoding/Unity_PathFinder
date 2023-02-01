using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathFinding : MonoBehaviour
{
    private Vector2Int[] offsets;
    private MapComp mapComp;

    private void Awake()
    {
        offsets = new Vector2Int[] { new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1) };
        mapComp = GetComponent<MapComp>();
    }
    public void BreadFirst(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        StartCoroutine(IBreadFirst(map, start, end));
    }

    public IEnumerator IBreadFirst(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        bool hasRoute = false;
        queue.Enqueue(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            // 画出来
            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经走过
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 遇到障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                queue.Enqueue(newPos);
                cameFrom[newPos] = current;

            }
        }
        if (hasRoute)
        {
            Stack<Vector2Int> trace = new Stack<Vector2Int>();
            Vector2Int pos = end;
            while (cameFrom.ContainsKey(pos))
            {
                trace.Push(pos);
                pos = cameFrom[pos];
            }
            while (trace.Count > 0)
            {
                Vector2Int p = trace.Pop();
                yield return new WaitForSeconds(0.5f);
                mapComp.RenderRoute(p);
            }
        }

    }
    int manhattan(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
    }

    public void Dijkstra(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        StartCoroutine(IDijkstra(map, start, end));
    }

    public IEnumerator IDijkstra(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        print("innnnnnnnnnnnnnnnnnnnnnnnnnnn");
        List<Vector2Int> sortList = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        bool hasRoute = false;
        sortList.Add(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        while (sortList.Count > 0)
        {
            print(sortList.Count);
            sortList.Sort((Vector2Int a, Vector2Int b) =>
            {
                return manhattan(start, a) - manhattan(start, b);
            });
            Vector2Int current = sortList[0];
            sortList.RemoveAt(0);
            // 画出来
            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经走过
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 遇到障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                sortList.Add(newPos);
                cameFrom[newPos] = current;
 
            }
        }
        if (hasRoute)
        {
            Stack<Vector2Int> trace = new Stack<Vector2Int>();
            Vector2Int pos = end;
            while (cameFrom.ContainsKey(pos))
            {
                trace.Push(pos);
                pos = cameFrom[pos];
            }
            while (trace.Count > 0)
            {
                Vector2Int p = trace.Pop();
                yield return new WaitForSeconds(0.5f);
                mapComp.RenderRoute(p);
            }
        }

    }

    public void AStar(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        StartCoroutine(IAstar(map, start, end));
    }

    public IEnumerator IAstar(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> sortList = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        bool hasRoute = false;
        sortList.Add(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        while (sortList.Count > 0)
        {
            print(sortList.Count);
            sortList.Sort((Vector2Int a, Vector2Int b) =>
            {
                return (manhattan(start, a) + manhattan(end, a)) - (manhattan(start, b) + manhattan(end,b));
            });
            Vector2Int current = sortList[0];
            sortList.RemoveAt(0);
            // 画出来
            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经走过
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 遇到障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                sortList.Add(newPos);
                cameFrom[newPos] = current;
            }
        }
        if (hasRoute)
        {
            Stack<Vector2Int> trace = new Stack<Vector2Int>();
            Vector2Int pos = end;
            while (cameFrom.ContainsKey(pos))
            {
                trace.Push(pos);
                pos = cameFrom[pos];
            }
            while (trace.Count > 0)
            {
                Vector2Int p = trace.Pop();
                yield return new WaitForSeconds(0.5f);
                mapComp.RenderRoute(p);
            }
        }

    }

}
