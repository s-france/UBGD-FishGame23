using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer sr;
    Rigidbody2D rb;

    [SerializeField] public Sprite[] groundSprites;
    [SerializeField] public Sprite[] airSprites;

    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isSwinging;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();

        isWalking = false;
        isSwinging = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HidePlayer()
    {
        sr.enabled = false;
    }

    public void ShowPlayer()
    {
        sr.enabled = true;
    }

    public void ChangeSprite(Sprite sprite)
    {
        sr.sprite = sprite;
    }

    public void FlipX(int direction)
    {
        Vector3 right = new Vector3(1,gameObject.transform.localScale.y,1);
        Vector3 left = new Vector3(-1,gameObject.transform.localScale.y,1);

        if(direction > 0)
        {
            transform.parent.localScale = right;
        } else if (direction < 0)
        {
            transform.parent.localScale = left;
        }
    }

    public void FlipY(int direction)
    {
        Vector3 up = new Vector3(gameObject.transform.localScale.x,1,1);
        Vector3 down = new Vector3(gameObject.transform.localScale.x,-1,1);

        if(direction > 0)
        {
            transform.parent.localScale = up;
        } else if (direction < 0)
        {
            transform.parent.localScale = down;
        }
    }

    public void SetSpriteRotation(Vector2 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction.normalized, Vector2.up);
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void ResetSpriteRotation() //lazy ik
    {
        SetSpriteRotation(Vector2.right);
    }

    public IEnumerator WalkCycle()
    {
        //yield return new WaitForFixedUpdate();

        int sprite = 1;
        float walkspeed;

        if(!isWalking)
        {
            isWalking = true;

            while (isWalking)
            {
                walkspeed = Mathf.Abs(rb.velocity.x);
                //animate
                ChangeSprite(groundSprites[sprite]);

                sprite++;
                if(sprite > 4) {sprite = 1;}
                
                if (Mathf.Abs(rb.velocity.x) > 4)
                {
                    yield return new WaitForSeconds(1/walkspeed);
                    
                } else 
                {
                    yield return new WaitForSeconds(.25f);
                }
            }
            ChangeSprite(groundSprites[0]);
        }
    }

    //air grapple swinging
    public IEnumerator Swing( Transform hook)
    {
        if(!isSwinging)
        {
            isSwinging = true;
            ChangeSprite(airSprites[0]); //TEMPORARY - replace with final swinging sprite/animation

            while(isSwinging)
            {
                Vector2 direction = hook.position - transform.position;
                direction = Vector2.Perpendicular(direction);
                
                SetSpriteRotation(direction);

                //Quaternion rotation = Quaternion.LookRotation(direction, transform.TransformDirection(Vector2.up));
                //transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

                yield return null;
            }

            ResetSpriteRotation();
        }
    }
}
