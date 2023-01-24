using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


/*public class CubeMovement
{
    public Vector3 position;
    public CubeMovement (Vector3 newPosition)
    {
        position = newPosition;
    }
}*/


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour {

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    [Header("Grid Settings")]
    //need for 2 different cellSize: one for the x axis, one depending on cube's position
    public float cellSize=1; 
    public Vector3 gridOffset;
    [Range(1,99)]
    public int gridSizeX;
    //[Range(1, 99)]
    public int gridSizeZ = 0;

    [Space]
    //[Header("Cube")]
    public GameObject cube;
    Vector3 cubePos;
    Vector3 newCubePos;
    //Vector3 cubeOffset;
    List<Vector3> cubeMovement = new List<Vector3>();
    Vector3[] cM;

    // Use this for initialization
    void Awake () {
        mesh = GetComponent<MeshFilter>().mesh;
	}

    private void Start()
    {
        List<Vector3> cubeMovement = new List<Vector3>();
        cubePos = cube.transform.position;

        //tolerance to avoid small movements
        //cubeOffset = new Vector3(Mathf.Abs(2), Mathf.Abs(2), Mathf.Abs(2));
    }

    // Update is called once per frame
    void Update () {
        //SaveRowPosition();
        MakeContiguosProceduralGrid();
        UpdateMesh();
	}

    /*void SaveRowPosition()
    {
        //check if cube is moving
        newCubePos = cube.transform.position;
        if (Mathf.Round(newCubePos.z) > cubePos.z /*+ cubeOffset)
        {
            //save this position for the row
            cubeMovement.Add(newCubePos);
            cM = cubeMovement.ToArray();

            //add one row to the grid size
            gridSizeZ++;
            cubePos = newCubePos;
        }
        /*else if (Mathf.Round(newCubePos.z) < cubePos.z /*+ cubeOffset)
        {
            cubeMovement.Remove(newCubePos);
            cM = cubeMovement.ToArray();

            gridSizeZ--;
            cubePos = newCubePos;
        }
    }*/

    void MakeContiguosProceduralGrid()
    {
        //check if cube is moving
        newCubePos = cube.transform.position;
        if (Mathf.Round(newCubePos.z) != Mathf.Round(cubePos.z) /*+ cubeOffset*/)
        {
            //erase cM data


            //save this position for the row
            cubeMovement.Add(newCubePos);
            cM = cubeMovement.ToArray();
            Debug.Log(cM[1].y); //what is the reason why if I comment this line everything stops to work?!

            //add one row to the grid size
            gridSizeZ++;
            //Debug.Log(gridSizeZ);

            cubePos = newCubePos;
        }

        if (gridSizeZ == 0)
        {
            return;
        }
        else if (gridSizeZ>=0)
        {

            //set array size
            vertices = new Vector3[(gridSizeX + 1) * (gridSizeZ + 1)];
            triangles = new int[gridSizeX * gridSizeZ * 6];

            //set tracker integers
            int v = 0;
            int t = 0;

            //set vertex offset based on cell size
            float vertexOffset = cellSize * 0.5f;

            //create vertex grid
            for (int x = 0; x <= gridSizeX; x++)
            {
                for (int z = 0; z <= gridSizeZ; z++)
                {
                    //Debug.Log( cM[z].y);

                    vertices[v] = new Vector3((x * cellSize) - vertexOffset, (cM[z].y * cellSize) - vertexOffset, (cM[z].z * cellSize) - vertexOffset) + gridOffset;
                    v++;
                }
            }

            //reset vertex tracker
            v = 0;

            //setting each cell's triangles
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    triangles[t + 0] = v + 0;
                    triangles[t + 1] = triangles[t + 4] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + (gridSizeZ + 1);
                    triangles[t + 5] = v + (gridSizeZ + 1) + 1;

                    v++;
                    t += 6;
                }
                v++;
            }
        }
    }

    /*void MakeContiguosProceduralGrid()//"Original"
    {
        //set array size
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeZ + 1)];
        triangles = new int[gridSizeX * gridSizeZ * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset based on cell size
        float vertexOffset = cellSize * 0.5f;

        //create vertex grid
        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z <= gridSizeZ; z++)
            {
                vertices[v] = new Vector3((x * cellSize) - vertexOffset, 0, (z * cellSize) - vertexOffset)+gridOffset;
                v++;
            }
        }
        
        //reset vertex tracker
        v = 0;

        //setting each cell's triangles
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                triangles[t + 0] = v + 0;
                triangles[t + 1] = triangles[t + 4] = v + 1;
                triangles[t + 2] = triangles[t + 3] = v + (gridSizeZ+1);
                triangles[t + 5] = v + (gridSizeZ+1) +1;

                v++;
                t += 6;
            }
            v++;
        }
    }*/

    //non contiguos vertices
    void MakeDiscreteProceduralGrid()
    {
        //set array size
        vertices = new Vector3[gridSizeX * gridSizeZ * 4];
        triangles = new int[gridSizeX * gridSizeZ * 6];

        //set tracker int
        int v = 0;
        int t = 0;

        //set vertex offset based on cell size
        float vertexOffset = cellSize * 0.5f;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 cellOffset =new Vector3(x*cellSize, 0, z*cellSize);

                //populate the certices and triangles array
                vertices[v+0] = new Vector3(-vertexOffset, x+z, -vertexOffset) + gridOffset + cellOffset;
                vertices[v+1] = new Vector3(-vertexOffset, x+z, vertexOffset) + gridOffset + cellOffset;
                vertices[v+2] = new Vector3(vertexOffset, x+z, -vertexOffset) + gridOffset + cellOffset;
                vertices[v+3] = new Vector3(vertexOffset, x+z, vertexOffset) + gridOffset + cellOffset;

                triangles[t+0] = v+0;
                triangles[t+1] = triangles[t+4] = v+1;
                triangles[t+2] = triangles[t+3] = v+2;
                triangles[t+5] = v+3;

                v += 4;
                t += 6;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}