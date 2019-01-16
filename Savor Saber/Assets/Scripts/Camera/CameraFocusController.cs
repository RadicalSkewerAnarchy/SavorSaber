using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    [SerializeField]
    GameObject Target;
    //
    [SerializeField]
    [Range(0f, 10f)]
    float Snap;
    //
    [SerializeField]
    [Range(0f, 10f)]
    int RadiusChain;
    //
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float xTo;
        float yTo;
        
        var leftRight = 1;
        var upDown = 1;
        var localPosition = transform.position;
      

        leftRight = DirectionX();
        upDown = DirectionY();
        xTo = Target.transform.position.x + leftRight * Mathf.Min((RadiusChain), Mathf.Abs(localPosition.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x));
        yTo = Target.transform.position.y + upDown * Mathf.Min((RadiusChain), Mathf.Abs(localPosition.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y));

        localPosition.x += (xTo - localPosition.x);
        localPosition.y += (yTo - localPosition.y);
        transform.position = Vector2.MoveTowards(transform.position, localPosition, Snap * Time.deltaTime);
    }
    int DirectionX()
    {
        var camCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(camCoord.x < Target.transform.position.x)
        {
            return -1;
        }
        return 1;
    }
    int DirectionY()
    {
        var camCoord = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (camCoord.y < Target.transform.position.y)
        {
            return -1;
        }
        return 1;
    }
}
