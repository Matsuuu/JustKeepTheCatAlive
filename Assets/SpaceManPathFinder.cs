using System;
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

    public enum SideOfShip
    {
        Left,
        Right
    };

    private Camera mainCamera;
    private Transform centerOfShip;
    public bool inARoom = false;
    public float movementSpeed = 10f;
    public List<NodeWithPosition> nodes = new List<NodeWithPosition>();

    public bool movingTowardsTarget;
    public NodeWithPosition targetNode;

    public NodeWithPosition nextNodeOnPath;
    public bool hasReachedNextNodeOnPath;

    public float distanceToTarget;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.gravity = Vector2.zero;
        centerOfShip = GameObject.FindGameObjectWithTag("CenterOfShip").transform;
        mainCamera = Camera.main;
        foreach (GameObject node in GameObject.FindGameObjectsWithTag("PathFindingNode"))
        {
            nodes.Add(new NodeWithPosition(node.GetComponent<PathFindingNode>(), node.transform.position));
        }
    }

    // Update is called once per frame
    void Update()
    {
        ListenForMouseClick();
        HandleMovement();
    }

    private void ListenForMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePositionInWorld = mainCamera.ScreenToWorldPoint(mousePosition);

            NodeWithPosition closestNode = FindClosestNode(mousePositionInWorld);
            //closestNode.NodeScript.Show();
            targetNode = closestNode;
            movingTowardsTarget = true;
            distanceToTarget = GetDistanceToTarget();
            hasReachedNextNodeOnPath = true;
        }
    }

    private void HandleMovement()
    {
        if (movingTowardsTarget)
        {
            distanceToTarget = GetDistanceToTarget();
            if (distanceToTarget < 1)
            {
                movingTowardsTarget = false;
                return;
            }

            Vector3 playerPosition = transform.position;
            float distanceToNextNode = Vector2.Distance(playerPosition, nextNodeOnPath.Position);
            
            //Vector3 targetHeading = targetNode.Position - playerPosition;
            //Debug.DrawRay(playerPosition, targetHeading);
            //Debug.DrawRay(playerPosition, nextNodeOnPath.Position - playerPosition, Color.blue);
            transform.position = Vector2.MoveTowards(playerPosition, nextNodeOnPath.Position, movementSpeed * Time.deltaTime);

            if (distanceToNextNode < 0.2f)
            {
                hasReachedNextNodeOnPath = true;
            }
            if (hasReachedNextNodeOnPath)
            {
                GetNextNode(playerPosition);
            }
        }
    }

    private void GetNextNode(Vector3 playerPosition)
    {
        if (inARoom)
        {
            // Find the nodes on the same Horizontal level and get the ones in the hallway
            List<NodeWithPosition> closestHallwayNodes = nodes.FindAll(node => Math.Abs(node.Position.x - centerOfShip.position.x) < 0.4f && Math.Abs(node.Position.y - playerPosition.y) < 0.2f);
            closestHallwayNodes.Sort((a, b) => Math.Abs(a.Position.x - playerPosition.x) > Math.Abs(b.Position.x - playerPosition.x) ? 1 : -1);
            nextNodeOnPath = closestHallwayNodes[0];
            hasReachedNextNodeOnPath = false;
        }

        if (IsInHallway())
        {
            // Get the hallway node on the right vertical height
            List<NodeWithPosition> nodesInTheSameLevelAsTheTarget = nodes.FindAll(node =>
                Math.Abs(node.Position.x - playerPosition.x) < 0.2f &&
                Math.Abs(node.Position.y - targetNode.Position.y) < 0.2f);

            nextNodeOnPath = nodesInTheSameLevelAsTheTarget[0];
            hasReachedNextNodeOnPath = false;
        }

        if (IsInSameLevelAsTarget())
        {
            nextNodeOnPath = targetNode;
            hasReachedNextNodeOnPath = false;
        }
    }

    private float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, targetNode.Position);
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

    private SideOfShip GetSideOfShip()
    {
        return transform.position.x < centerOfShip.position.x ? SideOfShip.Left : SideOfShip.Right;
    }

    private bool IsInHallway()
    {
        float playerX = transform.position.x;
        float centerOfShipX = centerOfShip.position.x;
        return Math.Abs(Math.Max(playerX, centerOfShipX) - Math.Min(playerX, centerOfShipX)) < 0.4f;
    }

    private bool IsInSameLevelAsTarget()
    {
        float playerY = transform.position.y;
        float targetNodeY = targetNode.Position.y;
        return Math.Abs(Math.Max(playerY, targetNodeY) - Math.Min(playerY, targetNodeY)) < 0.4f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            inARoom = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Room"))
        {
            inARoom = false;
        }
    }
}
