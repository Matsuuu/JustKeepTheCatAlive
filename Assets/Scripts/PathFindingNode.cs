using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingNode : MonoBehaviour
{

    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = false;
    }

    public void Show()
    {
        renderer.enabled = true;
    }
}
