using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PathFinder : MonoBehaviour
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
        // HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        queue.Enqueue(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        bool hasRoute = false;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            // 找到目标位置
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            // 四个相邻方向
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经在队列中
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                // 把相邻方向格子加进队列
                queue.Enqueue(newPos);
                // newPos 是由于 current 添加进来的，因此current是newPos的父节点
                cameFrom[newPos] = current;
            }
        }
        // 如果找到，就把路径渲染出来
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
                // 通过mapComp修改sprite来进行表现
                mapComp.RenderRoute(p);
            }
        }
    }


    public void Dijkstra(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        StartCoroutine(IDijkstra(map, start, end));
    }

    public int manhattan(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    public IEnumerator IDijkstra(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        // Queue<Vector2Int> queue = new Queue<Vector2Int>();
        // HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        // SortedList<Vector2Int> queue = new S
        // 此处可以用优先队列
        List<Vector2Int> sortList = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        // queue.Enqueue(start);
        sortList.Add(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        bool hasRoute = false;
        while (sortList.Count > 0)
        {
            // 此处可以不排序，改为拿到最小值并移出即可
            sortList.Sort((Vector2Int a, Vector2Int b) =>
            {
                return manhattan(start, a) - manhattan(start, b);
            });
            Vector2Int current = sortList[0];
            sortList.RemoveAt(0);

            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            // 找到目标位置
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            // 四个相邻方向
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经在队列中
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                // 把相邻方向格子加进队列
                // queue.Enqueue(newPos);
                sortList.Add(newPos);
                // newPos 是由于 current 添加进来的，因此current是newPos的父节点
                cameFrom[newPos] = current;
            }
        }
        // 如果找到，就把路径渲染出来
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
                // 通过mapComp修改sprite来进行表现
                mapComp.RenderRoute(p);
            }
        }
    }
    public void AStar(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        StartCoroutine(IAStar(map, start, end));
    }
    public IEnumerator IAStar(List<List<Transform>> map, Vector2Int start, Vector2Int end)
    {
        // Queue<Vector2Int> queue = new Queue<Vector2Int>();
        // HashSet<Vector2Int> set = new HashSet<Vector2Int>();
        // SortedList<Vector2Int> queue = new S
        // 此处可以用优先队列
        List<Vector2Int> sortList = new List<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        // queue.Enqueue(start);
        sortList.Add(start);
        cameFrom[start] = new Vector2Int(-1, -1);
        bool hasRoute = false;
        while (sortList.Count > 0)
        {
            // 此处可以不排序，改为拿到最小值并移出即可
            sortList.Sort((Vector2Int a, Vector2Int b) =>
            {
                return (manhattan(start, a) + manhattan(end, a)) - (manhattan(start, b) + manhattan(end, b));
            });
            Vector2Int current = sortList[0];
            sortList.RemoveAt(0);

            yield return new WaitForSeconds(0.1f);
            mapComp.RenderBox(current);
            // 找到目标位置
            if (current == end)
            {
                hasRoute = true;
                break;
            }
            // 四个相邻方向
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int newPos = current + offset;
                // 超出边界
                if (newPos.x < 0 || newPos.y < 0 || newPos.x >= map.Count || newPos.y >= map[0].Count)
                {
                    continue;
                }
                // 已经在队列中
                if (cameFrom.ContainsKey(newPos))
                {
                    continue;
                }
                // 障碍物
                if (map[newPos.x][newPos.y].Find("Wallnut"))
                {
                    continue;
                }
                // 把相邻方向格子加进队列
                // queue.Enqueue(newPos);
                sortList.Add(newPos);
                // newPos 是由于 current 添加进来的，因此current是newPos的父节点
                cameFrom[newPos] = current;
            }
        }
        // 如果找到，就把路径渲染出来
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
                // 通过mapComp修改sprite来进行表现
                mapComp.RenderRoute(p);
            }
        }
    }
}