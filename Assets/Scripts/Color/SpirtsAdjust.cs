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
        if (Input.GetMouseButton(0))  // ����������
        {
            if (!isDrawing)
            {
                isDrawing = true;
                points.Clear();
                lineRenderer.positionCount = 0;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;  // ȷ���Ƕ�ά����
            points.Add(mousePos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        else if (Input.GetMouseButtonUp(0))  // �������ɿ�
        {
            
            isDrawing = false;
            DetectShape(points);

        }
    }

    // ����������״
    void DetectShape(List<Vector3> points)
    {
        List<Vector3> newPoints = new List<Vector3>();
        for(int i = 0;i<points.Count;i+=10)
        {
            newPoints.Add(points[i]);
        }
        // ������Լ�������ͼ�μ���߼�
        if (IsCircle(newPoints))
        {
            Debug.Log("��⵽Բ��");
        }
        else if (IsTriangle(newPoints))
        {
            Debug.Log("��⵽������");
        }
        // �������״���...
    }

    // ʾ�����򵥵�Բ�μ��
    bool IsCircle(List<Vector3> points)
    {
        if (points.Count < 10) return false;

        // ����·����ƽ�����ĵ�
        Vector3 center = Vector3.zero;
        foreach (var point in points)
        {
            center += point;
        }
        center /= points.Count;
        // ������е��Ƿ�ӽ�Բ��
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

    // ����͹��
    public  List<Vector3> GetConvexHull(List<Vector3> points)
    {
        // ����㼯����x��y����
        points.Sort((p1, p2) => p1.x == p2.x ? p1.y.CompareTo(p2.y) : p1.x.CompareTo(p2.x));

        // �����°�͹��
        List<Vector3> lowerHull = new List<Vector3>();
        foreach (var p in points)
        {
            while (lowerHull.Count >= 2 && Cross(lowerHull[lowerHull.Count - 2], lowerHull[lowerHull.Count - 1], p) <= 0)
            {
                lowerHull.RemoveAt(lowerHull.Count - 1);
            }
            lowerHull.Add(p);
        }

        // �����ϰ�͹��
        List<Vector3> upperHull = new List<Vector3>();
        for (int i = points.Count - 1; i >= 0; i--)
        {
            while (upperHull.Count >= 2 && Cross(upperHull[upperHull.Count - 2], upperHull[upperHull.Count - 1], points[i]) <= 0)
            {
                upperHull.RemoveAt(upperHull.Count - 1);
            }
            upperHull.Add(points[i]);
        }

        // �ϲ��ϰ���°�͹����ȥ���ظ��ĵ�
        lowerHull.RemoveAt(lowerHull.Count - 1);
        upperHull.RemoveAt(upperHull.Count - 1);
        lowerHull.AddRange(upperHull);

        return lowerHull;
    }

    // �жϸ������Ƿ��γ�������
    public bool IsTriangle(List<Vector3> points)
    {
        List<Vector3> convexHull = GetConvexHull(points);
        Debug.Log(convexHull.Count);
        return convexHull.Count == 3;
    }

}
