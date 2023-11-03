using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    transition,
    forcedReading,
    unforcedReading,
    idle,
    walking
}

public enum PlayerDirection
{
    up,
    down,
    left,
    right
}

public class PlayerAttributes : MonoBehaviour
{
    public List<GameObject> collidingTriggers;
    Rigidbody2D rb;
    Animator anim;
    public float walkingSpeed = 5f;
    private float speed;
    public Flashlight flashlight;

    public bool canMoveHorizontal = true;
    public bool canMoveVertical = true;

    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canMoveUp = true;
    public bool canMoveDown = true;
    public PlayerState currentState;

    public bool isMovingDiagonally;
    public PlayerDirection currentDirection;
    public KeyCode lastMovementKeyHit;

    public bool updateFacing = true;

    // Start is called before the first frame update
    void Start()
    {
        speed = walkingSpeed;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flashlight = GameObject.FindGameObjectWithTag("Flashlight").GetComponent<Flashlight>();
        ChangeState(PlayerState.idle);
        currentDirection = PlayerDirection.down;
    }

    public void SetToIdle()
    {
        currentState = PlayerState.idle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentState == PlayerState.transition)
        {
            UpdateAnimation(Vector3.zero);
            return;
        }
        // Creates a Vector3 value change with the x and y of the player's input
        Vector3 change = Vector3.zero;
        if (canMoveHorizontal)
        {
            change.x = Input.GetAxisRaw("Horizontal");
        }
        if (canMoveVertical)
        {
            change.y = Input.GetAxisRaw("Vertical");
        }
        change = RestrictMovement(change);
        // Normalizes the Vector3 so that it conveys just the direction, not distance
        change.Normalize();
        if (updateFacing)
        {
            UpdateDirection(change);
            flashlight.UpdateFlashlightDirection();
        }
        // Moves the player's position in the direction of the change
        if (currentState != PlayerState.forcedReading)
        {
            UpdateAnimation(change);
            Vector3 newPosition = this.transform.position + (speed * Time.deltaTime * change);
            rb.MovePosition(newPosition);
            if (updateFacing)
            {
                //flashlight.UpdateFlashlightDirection();
            }
        }
    }
    void Update()
    {
        if (currentState == PlayerState.transition)
        {
            return;
        }

        if (updateFacing)
        {
            LogLastMovementKey();
        }

        bool inRangeOfTrigger = collidingTriggers.Count > 0;
        bool isReading = currentState == PlayerState.unforcedReading || currentState == PlayerState.forcedReading;
        if (Input.GetKeyDown(KeyCode.Space) && inRangeOfTrigger && !isReading)
        {
            IInteractable currentTrigger = collidingTriggers[0].GetComponent<IInteractable>();
            if (currentTrigger != null)
            {
                currentTrigger.Interact();
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.ToggleFlashLight();
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    Vector3 RestrictMovement(Vector3 change)
    {
        if (!canMoveLeft)
        {
            if (change.x < 0)
            {
                change.x = 0;
            }
        }
        if (!canMoveRight)
        {
            if (change.x > 0)
            {
                change.x = 0;
            }
        }
        if (!canMoveUp)
        {
            if (change.y > 0)
            {
                change.y = 0;
            }
        }
        if (!canMoveDown)
        {
            if (change.y < 0)
            {
                change.y = 0;
            }
        }
        return change;
    }

    public void ChangeState(PlayerState state)
    {
        if (state != PlayerState.walking)
        {
            anim.SetBool("Walking", false);
        }
        currentState = state;
    }

    void UpdateAnimation(Vector3 change)
    {
        if (change != Vector3.zero)
        {
            anim.SetBool("Walking", true);

        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (currentDirection == PlayerDirection.left)
        {
            anim.SetFloat("MoveX", -1f);
            anim.SetFloat("MoveY", 0f);
        }
        else if (currentDirection == PlayerDirection.right)
        {
            anim.SetFloat("MoveX", 1f);
            anim.SetFloat("MoveY", 0f);
        }
        else if (currentDirection == PlayerDirection.up)
        {
            anim.SetFloat("MoveY", 1f);
            anim.SetFloat("MoveX", 0f);
        }
        else if (currentDirection == PlayerDirection.down)
        {
            anim.SetFloat("MoveY", -1f);
            anim.SetFloat("MoveX", 0f);
        }
    }

    void UpdateDirection(Vector3 change)
    {
        CheckForDiagonalMovement(change);
        if (!isMovingDiagonally)
        {
            if (change.x < 0)
            {
                SetDirection(PlayerDirection.left);
            }
            else if (change.x > 0)
            {
                SetDirection(PlayerDirection.right);
            }
            else if (change.y < 0)
            {
                SetDirection(PlayerDirection.down);
            }
            else if (change.y > 0)
            {
                SetDirection(PlayerDirection.up);
            }
        }
        else
        {
            if (lastMovementKeyHit.Equals(KeyCode.A) || lastMovementKeyHit.Equals(KeyCode.LeftArrow))
            {
                SetDirection(PlayerDirection.left);
            }
            else if (lastMovementKeyHit.Equals(KeyCode.D) || lastMovementKeyHit.Equals(KeyCode.RightArrow))
            {
                SetDirection(PlayerDirection.right);
            }
            else if (lastMovementKeyHit.Equals(KeyCode.S) || lastMovementKeyHit.Equals(KeyCode.DownArrow))
            {
                SetDirection(PlayerDirection.down);
            }
            else if (lastMovementKeyHit.Equals(KeyCode.W) || lastMovementKeyHit.Equals(KeyCode.UpArrow))
            {
                SetDirection(PlayerDirection.up);
            }
        }
    }

    void LogLastMovementKey()
    {
        if (Input.GetKeyDown(KeyCode.W)) {
            lastMovementKeyHit = KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastMovementKeyHit = KeyCode.S;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lastMovementKeyHit = KeyCode.D;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastMovementKeyHit = KeyCode.A;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastMovementKeyHit = KeyCode.UpArrow;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastMovementKeyHit = KeyCode.DownArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastMovementKeyHit = KeyCode.LeftArrow;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastMovementKeyHit = KeyCode.RightArrow;
        }
    }

    void CheckForDiagonalMovement(Vector3 change)
    {
        if (change.x != 0 && change.y != 0)
        {
            isMovingDiagonally = true;
        }
        else
        {
            isMovingDiagonally = false;
        }
    }

    public void SetDirection(PlayerDirection direction)
    {
        currentDirection = direction;
    }

    public void AddTrigger(GameObject trigger)
    {
        collidingTriggers.Add(trigger);
    }

    public void RemoveTrigger(GameObject trigger)
    {
        collidingTriggers.Remove(trigger);
    }
}

