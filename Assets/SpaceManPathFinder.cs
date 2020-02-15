using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class SpaceManPathFinder : MonoBehaviour
{
    public struct NodeWithPosition
    {
        public PathFindingNode NodeScript { get; set; }
        public Vector3 Position { get; set; }

        public NodeWithPosition(PathFindingNode nodeScript, Vector3 position)
        {
            this.NodeScript = nodeScript;
            this.Position = position;
        }
        
        
    }
    public List<NodeWithPosition> nodes = new List<NodeWithPosition>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject node in GameObject.FindGameObjectsWithTag("PathFindingNode"))
        {
            nodes.Add(new NodeWithPosition(node.GetComponent<PathFindingNode>(), node.transform.position));
            Debug.Log(node.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ListenForMouseClick();
    }

    private void ListenForMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePoisitionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);

            NodeWithPosition closestNode = FindClosestNode(mousePoisitionInWorld);
            closestNode.NodeScript.Show();
        }
    }

    private NodeWithPosition FindClosestNode(Vector3 pos)
    {
        NodeWithPosition closest = nodes[0];
        float closestDistance = float.MaxValue;
        foreach (NodeWithPosition node in nodes)
        {
            float distance = Vector2.Distance(pos, node.Position);
            if (distance < closestDistance)
            {
                closest = node;
                closestDistance = distance;
            }
        }
        return closest;
    }
}
