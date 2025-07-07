using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpirtsAdjust : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private bool isDrawing = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // 鼠标左键按下
        {
            if (!isDrawing)
            {
                isDrawing = true;
                points.Clear();
                lineRenderer.positionCount = 0;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;  // 确保是二维坐标
            points.Add(mousePos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        else if (Input.GetMouseButtonUp(0))  // 鼠标左键松开
        {
            
            isDrawing = false;
            DetectShape(points);

        }
    }

    // 检测输入的形状
    void DetectShape(List<Vector3> points)
    {
        List<Vector3> newPoints = new List<Vector3>();
        for(int i = 0;i<points.Count;i+=10)
        {
            newPoints.Add(points[i]);
        }
        // 这里可以加入具体的图形检测逻辑
        if (IsCircle(newPoints))
        {
            Debug.Log("检测到圆形");
        }
        else if (IsTriangle(newPoints))
        {
            Debug.Log("检测到三角形");
        }
        // 更多的形状检测...
    }

    // 示例：简单的圆形检测
    bool IsCircle(List<Vector3> points)
    {
        if (points.Count < 10) return false;

        // 计算路径的平均中心点
        Vector3 center = Vector3.zero;
        foreach (var point in points)
        {
            center += point;
        }
        center /= points.Count;
        // 检测所有点是否接近圆形
        float radius = Vector3.Distance(center, points[0]);
       
        foreach (var point in points)
        {           
            if (Mathf.Abs(Vector3.Distance(center, point) - radius) > .5f)
            {
                return false;
            }
        }

        return true;
    }

    public float Cross(Vector3 o, Vector3 a, Vector3 b)
    {
        return (a.x - o.x) * (b.y - o.y) - (a.y - o.y) * (b.x - o.x);
    }

    // 计算凸包
    public  List<Vector3> GetConvexHull(List<Vector3> points)
    {
        // 排序点集（按x，y排序）
        points.Sort((p1, p2) => p1.x == p2.x ? p1.y.CompareTo(p2.y) : p1.x.CompareTo(p2.x));

        // 构建下半凸包
        List<Vector3> lowerHull = new List<Vector3>();
        foreach (var p in points)
        {
            while (lowerHull.Count >= 2 && Cross(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], p) <= 0)
            {
                lowerHull.RemoveAt(lowerHull.Count - 1);
            }
            lowerHull.Add(p);
        }

        // 构建上半凸包
        List<Vector3> upperHull = new List<Vector3>();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            while (upperHull.Count >= 2 && Cross(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], points[i]) <= 0)
            {
                upperHull.RemoveAt(upperHull.Count - 1);
            }
            upperHull.Add(points[i]);
        }

        // 合并上半和下半凸包，去掉重复的点
        lowerHull.RemoveAt(lowerHull.Count - 1);
        upperHull.RemoveAt(upperHull.Count - 1);
        lowerHull.AddRange(upperHull);

        return lowerHull;
    }

    // 判断给定点是否形成三角形
    public bool IsTriangle(List<Vector3> points)
    {
        List<Vector3> convexHull = GetConvexHull(points);
        Debug.Log(convexHull.Count);
        return convexHull.Count == 3;
    }

}
