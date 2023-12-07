using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform[] tableParent;
    [SerializeField] private Transform checkInPoint;
    public float moveSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PatrolRoutine()
    {
        while(true)
        {
        
        }
    }
}
