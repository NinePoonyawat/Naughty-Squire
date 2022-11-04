using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSightGizmos : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyBase enemy;
    public Color meshColor = Color.red;
    Mesh mesh;

    void Start()
    {
        enemy = GetComponent<EnemyBase>();
        meshColor.a = 0.5f;
    }

    void OnValidate() {
        if(enemy != null) mesh = CreateWedgeMesh();
    }

    void OnDrawGizmos() {
        if (mesh) {
            Gizmos.color = meshColor;
            //transform.Rotate(enemy.transform.forward.x,enemy.transform.forward.y,enemy.transform.forward.z);
            Gizmos.DrawMesh(mesh,transform.position,transform.rotation);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, enemy.agent.destination);
        }
    }

    Mesh CreateWedgeMesh() {
        Mesh mesh = new Mesh();

        float fieldOfViewAngle = enemy.fieldOfViewAngle;
        float losRadius = enemy.losRadius;
        float height = enemy.height;

        int segments = 10;
        int numTriangles = (segments *4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -fieldOfViewAngle, 0) * Vector3.forward*losRadius;
        Vector3 bottomRight = Quaternion.Euler(0, fieldOfViewAngle, 0) * Vector3.forward*losRadius;

        Vector3 topCenter = bottomCenter + Vector3.up*height;
        Vector3 topRight = bottomRight + Vector3.up*height;
        Vector3 topLeft = bottomLeft + Vector3.up*height;

        int vert = 0;

        //left side
        vertices[vert++]  = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++]  = topCenter;
        vertices[vert++] = bottomCenter;
        //right side
        vertices[vert++]  = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++]  = bottomRight;
        vertices[vert++] = bottomCenter;
        
        float currentAngle = -fieldOfViewAngle;
        float deltaAngle = (fieldOfViewAngle *2) / segments;
        for (int i =0; i<segments; ++i) {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward*losRadius;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward*losRadius;

            topCenter = bottomCenter + Vector3.up*height;
            topRight = bottomRight + Vector3.up*height;
            topLeft = bottomLeft + Vector3.up*height;

            //far side 
            vertices[vert++]  = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++]  = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++]  = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++]  = bottomRight;
            vertices[vert++] = bottomLeft;    
            
            currentAngle += deltaAngle;
        }
        

        for (int i =0; i< numVertices; i++) {
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles  = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

}
