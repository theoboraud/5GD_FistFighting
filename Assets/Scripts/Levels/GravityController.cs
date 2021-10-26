using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private Vector3 gravityDirection;
    private List<Vector2> directionList;
    private Vector2 gravity;

    void Start()
    {
        gravity = Physics2D.gravity;
        directionList = new List<Vector2>();
        directionList.Add(Vector2.up);
        directionList.Add(Vector2.down);
        directionList.Add(Vector2.left);
        directionList.Add(Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gravity = Physics2D.gravity;
        int ran = 0;

        while (gravity == Physics2D.gravity)
        {
            ran = Random.Range(0, directionList.Count);
            gravity = directionList[ran] * 9.8f;
        }
        switch (ran)
        {
            case 0:
                transform.Rotate(new Vector3(0,0,180));
                break;
            case 1:
                transform.Rotate(new Vector3(0, 0, 0));
                break;
            case 2:
                transform.Rotate(new Vector3(0, 0, -90));
                break;
            case 3:
                transform.Rotate(new Vector3(0, 0, 90));
                break;

            default:
                break;
        }

        Physics2D.gravity = gravity;
    }
}
