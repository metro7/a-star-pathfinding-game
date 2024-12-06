using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this; 
    }

    public List<Node> GeneratePath(Node startNode, Node endNode)
    {
        List<Node> openSet = new List<Node>();

        foreach(Node n in FindObjectsOfType<Node>())
        {
            n.gScore = float.MaxValue;
        }
        startNode.gScore = 0;
        startNode.hScore = Vector2.Distance(startNode.transform.position, endNode.transform.position);
        openSet.Add(startNode);
        while(openSet.Count > 0)
        {
            int lowestF = default;
            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);
            if(currentNode == endNode)
            {
                List<Node> path = new List<Node>();
                path.Insert(0, endNode);

                while(currentNode != startNode)
                {
                    currentNode = currentNode.parentNode;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach(Node neighborNode in currentNode.neighbors)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, neighborNode.transform.position);

                if(heldGScore < neighborNode.gScore)
                {
                    neighborNode.parentNode = currentNode;
                    neighborNode.gScore = heldGScore;
                    neighborNode.hScore = Vector2.Distance(neighborNode.transform.position, endNode.transform.position);

                    if (!openSet.Contains(neighborNode))
                    {
                        openSet.Add(neighborNode);
                    }
                }

            }

        }

        return null;
    }

}
