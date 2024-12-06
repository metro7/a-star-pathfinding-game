using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node parentNode;
    public List<Node> neighbors;
    public float gScore;
    public float hScore;

    public float FScore()
    {
        return gScore + hScore;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        if(neighbors.Count > 0 )
        {
            for( int i = 0; i < neighbors.Count; i++)
            {
                Gizmos.DrawLine(transform.position, neighbors[i].transform.position);
            }
        }
    }

}
