using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelBehaviour : MonoBehaviour
{
    public Camera cameraObject;

    Mesh mesh;

    /*
     * Number of segments to render along the length of the tunnel.
     */   
    public int numSegments;

    /**
     * Number of sectors to render around the tunnel.
     */
    public int numSectors;

    public float length;

    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        if (meshFilter != null)
        {
            print("Mesh filter found");
            meshFilter.mesh = mesh;
        }

        Vector3[] vertices = new Vector3[numSegments * numSectors];
        int vertexIndex = 0;
        float sectorAngle = (2 * Mathf.PI) / numSectors;
        for (int r = 0; r < numSegments; r++)
        {
            float z = (((float)r / (float)numSegments) - 0.5f) * length;
            for (int c = 0; c < numSectors; c++)
            {
                float theta = sectorAngle * c;
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                vertices[vertexIndex] = new Vector3(x, y, z);
                vertexIndex++;
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[numSegments * numSectors * 6];
        int triangleIndex = 0;
        for (int r = 0; r < (numSegments - 1); r++)
        {
            for (int c = 0; c < numSectors; c++)
            {
                triangles[triangleIndex++] = r * numSectors + (c % numSectors);
                triangles[triangleIndex++] = (r + 1) * numSectors + (c % numSectors);
                triangles[triangleIndex++] = r * numSectors + ((c + 1) % numSectors);

                triangles[triangleIndex++] = r * numSectors + ((c + 1) % numSectors);
                triangles[triangleIndex++] = (r + 1) * numSectors + (c % numSectors);
                triangles[triangleIndex++] = (r + 1) * numSectors + ((c + 1) % numSectors);
            }
        }
        print("Created " + triangleIndex + " triangles");
        mesh.triangles = triangles;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
