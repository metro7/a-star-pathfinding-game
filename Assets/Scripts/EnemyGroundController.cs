using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGroundController : MonoBehaviour
{
    public GameObject nodeParent;
    public Node currentNode;
    public List<Node> path = new List<Node>();

    public Animator animator;

    public float moveSpeed;

    private bool isAlive = true;

    private Vector3 previousPosition;
    private Vector3 direction;

    private void Update()
    {
        if (isAlive)
        {
            CreatePath();
        }

    }


    public void CreatePath()
    {
        if(path.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y), moveSpeed * Time.deltaTime);
            
            if (previousPosition != transform.position)
            {
                direction = (previousPosition - transform.position).normalized;
                previousPosition = transform.position;
            }
            if(direction.x > 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
        else
        {
            Node[] nodes = FindObjectsOfType<Node>();
            while(path == null || path.Count == 0)
            {
                path = AStarManager.instance.GeneratePath(currentNode, nodes[Random.Range(0, nodes.Length)]);
            }
        }
    }


    public void TakeHit()
    {
        
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        isAlive = false;
        animator.Play("Death");
        Destroy(nodeParent);
        Destroy(gameObject, 1f);
        
    }

}
