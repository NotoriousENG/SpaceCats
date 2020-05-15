using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_OnRailsScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject meshParent;
    public Material material;
    public List<Transform> Vertices = new List<Transform>();
    Vector3 topLeft, topRight, bottomLeft, bottomRight;
    public List<Vector3[]> rectPointsArr = new List<Vector3[]>();
    List<Collider> boxColliders;
    List<Vector3[]> verticesList = new List<Vector3[]>();
    
    void Start()
    {
        meshParent = new GameObject("Rails", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

        Vertices.Clear();
        verticesList.Clear();
        getChildren();
        for (int i = 0; i < rectPointsArr.Count - 1; i++)
        {
            addVerticesToList(rectPointsArr[i], rectPointsArr[i+1]);
        }
        foreach (var vGroup in verticesList)
        {
            createMesh(vGroup);
        }
        combineMesh();
    }
    void getChildren()
    {
        rectPointsArr.Clear();
        Vertices.Clear();
        foreach (Transform child in transform)
        {
            if (Vertices.Count == 0 || !Vertices.Contains(child))
            {
                Vertices.Add(child);
                rectPointsArr.Add(getRectPoints(child));
            }
        }
    }

    void addVerticesToList(Vector3[] a, Vector3[] b)
    {
        var arr = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            if (i == 3)
            {
                arr = new Vector3[4] {a[i], b[i], b[0], a[0]};
                verticesList.Add(arr);
                break;
            }
            arr = new Vector3[4] {a[i], b[i], b[i+1], a[i+1]};
            verticesList.Add(arr);
        }
    }
    Vector3[] getRectPoints(Transform transform)
    {
        if (transform.TryGetComponent<M_Rectangle>(out var rectangle))
        {
            var diagonalLength = rectangle.diagonalLength;
            Vector3 pos = transform.position;
            var length = Mathf.Sqrt(diagonalLength);
            // Debug.Log("Length: " + length);

            topRight = pos += new Vector3(1,1,0) * length/2;
            topLeft = topRight - Vector3.right * length;

            bottomRight = topRight - Vector3.up * length;
            bottomLeft = topLeft - Vector3.up * length;

            Vector3[] rectPoints = new Vector3[4] {topLeft, topRight, bottomRight, bottomLeft};
            return rectPoints;
        }
        return null;
    }
    void DrawFrustum(Vector3[] startRect, Vector3[] endRect)
    {
        drawRect(startRect);
        drawRect(endRect);
        for (int i = 0; i < startRect.Length; i++)
        {
            Gizmos.DrawLine(startRect[i], endRect[i]);
        }
    }

    /// <summary>
    /// take in 4 vertices to create mesh (Top Left -> Bottom Left (counter clockwise))
    ///</summary>
    void createMesh(Vector3[] Vertices)
    {
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6] {0,3,2,2,1,0};

        for (int i = 0; i < 4; i++)
        {
            // Debug.Log("Array element"+ i + ": "+ Vertices[i]);
            uv[i] = (Vector2) Vertices[i];
        }
        
        Mesh mesh = new Mesh();

        mesh.vertices = Vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        GameObject obj = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
        obj.transform.SetParent(meshParent.transform);
        // Debug.Log(obj.name);
        obj.GetComponent<MeshFilter>().mesh = mesh;
        obj.GetComponent<MeshRenderer>().material = material;
    }
    void combineMesh()
    {
        MeshFilter[] meshFilters = meshParent.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        var mesh = meshParent.transform.GetComponent<MeshFilter>().mesh;
        mesh = new Mesh();
        
        mesh.CombineMeshes(combine);
        meshParent.transform.gameObject.SetActive(true);

        var mC = meshParent.GetComponent<MeshCollider>();
        mC.convex = false;
        mC.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;
        mC.sharedMesh = mesh;

        meshParent.GetComponent<MeshRenderer>().material = material;
    }
    void drawRect(Vector3[] rect)
    {
        for (int i = 0; i < rect.Length -1; i++)
        {
            Gizmos.DrawLine(rect[i], rect[i+1]);
            if (i >= rect.Length -2)
            {
                Gizmos.DrawLine(rect[i+1], rect[0]);
            }
        }
    }
    public List<Transform> getVectors()
    {
        return Vertices;
    }
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        getChildren();
        for (int i = 0; i < Vertices.Count - 1; i++)
        {
            Gizmos.DrawLine(Vertices[i].position, Vertices[i+1].position);
            // Debug.Log(rectPointsArr);
            DrawFrustum(rectPointsArr[i], rectPointsArr[i+1]);
        }
    }
}
