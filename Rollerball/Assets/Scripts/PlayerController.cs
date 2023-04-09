using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    // this GameObject *MUST* be capped
    public GameObject winTextObject;

    
    public float jForce; // make a public jump force and set its default
    // number of times we can jump as specified by the gamedev
    public uint jumpQuant = 2;  // double jump by default

    private Rigidbody rb;
    private uint ctr;   // this is how we declare an unsiged int
    private float movx;
    private float movy;

    // a counter to count the number of jumps
    // make a double jump
    private uint jumpCtr;
    private bool isGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ctr = 0;
        jumpCtr = 0;
        SetCountText();
        
        winTextObject.SetActive(false);
    }


    //don't need the update funct for this project
    // actually yes, we're using an update funct
    // this works reeeeeealy well
    // thanks Unity chatboard @nefisto

    //CAVEAT: the jumping is a little finnicky if you're moving about
    // sometimes it just doesn't hop, but I think it's b/c of the fact
    // the jumping is in the update and the movement is in the FixedUpdate
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){

            // can we jump?
            if (jumpCtr < jumpQuant){
                jumpCtr++;
            } else {
                jumpCtr = 0;
                isGround = false;
            }

            // actually jump
            if (isGround){
                Jump();
            }
        }
    }

    // this MUST be OnMove; probably a keyfunct we're manipulating
    void OnMove(InputValue movementValue){
        Vector2 movVect = movementValue.Get<Vector2>();

        movx = movVect.x;
        movy = movVect.y;
    }

    void SetCountText(){
        countText.text = "Count: " + ctr.ToString();

        if(ctr >= 12){
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate(){
        Vector3 movement = new Vector3(movx, 0.0f, movy);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other){
        // u should use is kinematic and a rigidbody to make a better collectable that rotates

        // check gameObject tag
        if (other.gameObject.CompareTag("PickUp")){
            // deactivate the game object we collide with if tag is PickUp
            other.gameObject.SetActive(false);
            ctr++;
            SetCountText();
        }
    }

    private void OnCollisionEnter (Collision other)
    {
        if (other.gameObject.CompareTag("Ground")){ // check if player is touching ground via tagcheck
            isGround = true;
        }
    }

    private void Jump()
    // the => is a lambda expression
    // DO NOT USE {} TO CLOSE THIS EXPRESSION

    /*we take advantage of the lambda expression to streamline our jumping
    despite not knowing what vector we'd be jumping on
    also, the ForceMode.Impulse is something I'd look into, too, since lots of users
    recommended it / used it in potential solutions
    this is something I'm gonna have to look into more since udk much abt C# lambdas */
        => rb.AddForce(Vector3.up * jForce, ForceMode.Impulse);

}
