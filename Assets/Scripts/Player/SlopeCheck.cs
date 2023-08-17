using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    GameplayPlayerController gpc;
    Rigidbody2D rb;

    [SerializeField] float slopeCheckDistance;
    [SerializeField] float maxSlopeAngle;

    bool slopeHit;


    // Start is called before the first frame update
    void Start()
    {
        gpc = GetComponentInParent<GameplayPlayerController>();
        gpc.slopeNormalPerp = Vector2.right;
        gpc.isOnSlope = false;

        rb = GetComponentInParent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //gpc.isOnSlope = false;
        CheckVertical();
        //CheckHorizontal();
    }


    void CheckVertical()
    {
        Vector2 test = new Vector2(transform.position.x + .2f, transform.position.y + .2f);

        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D frontDownHit = Physics2D.Raycast(test, Vector2.down, slopeCheckDistance + .2f, LayerMask.GetMask("Ground"));
        RaycastHit2D downHit = Physics2D.Raycast(transform.parent.position, Vector2.down, slopeCheckDistance, LayerMask.GetMask("Ground"));
        RaycastHit2D backHit = Physics2D.Raycast(-transform.position, -transform.right, slopeCheckDistance, LayerMask.GetMask("Ground"));


        Debug.DrawRay(test, Vector3.down * (slopeCheckDistance+.2f), Color.cyan);

        /*if(rb.velocity.y > 0)
        {


        }*/

        if(frontHit)
        {
            slopeHit = true;
            gpc.slopeNormalPerp = -Vector2.Perpendicular(frontHit.normal).normalized;

            Debug.DrawRay(frontHit.point, frontHit.normal, Color.blue);
            Debug.DrawRay(frontHit.point, -gpc.slopeNormalPerp, Color.yellow);
        }else if (frontDownHit)
        {
            slopeHit = true;
            gpc.slopeNormalPerp = -Vector2.Perpendicular(frontDownHit.normal).normalized;

            Debug.DrawRay(frontDownHit.point, frontDownHit.normal, Color.green);
            Debug.DrawRay(frontDownHit.point, -gpc.slopeNormalPerp, Color.red);

        } else if (downHit)
        {
            slopeHit = true;
            gpc.slopeNormalPerp = -Vector2.Perpendicular(downHit.normal).normalized;

            Debug.DrawRay(downHit.point, downHit.normal, Color.green);
            Debug.DrawRay(downHit.point, -gpc.slopeNormalPerp, Color.red);
        } else if (backHit)
        {
            slopeHit = true;
            gpc.slopeNormalPerp = -Vector2.Perpendicular(backHit.normal).normalized;

            Debug.DrawRay(backHit.point, backHit.normal, Color.cyan);
            Debug.DrawRay(backHit.point, -gpc.slopeNormalPerp, Color.magenta);
        } else 
        {
            slopeHit = false;
        }

        if(slopeHit && Mathf.Abs(gpc.slopeNormalPerp.y) < .8f)
        {
            gpc.isOnSlope = true;
        } else
        {
            gpc.isOnSlope = false;
        }
    }



}
