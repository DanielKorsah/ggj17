using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    #region singleton
    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    // Spin beacon clockwise
    public delegate bool RotateBeacon();
    public RotateBeacon RotateBeaconCall;
    // Choose beacon direction, returns true if choosing direction for a beacon
    public delegate bool ChooseDirectionBeacon();
    public ChooseDirectionBeacon ChooseDirectionCall;
    // Change beacon output
    public delegate void ChangeBeacon();
    public ChangeBeacon ChangeBeaconCall;
    // Pickup interaction, returns true if choosing pickup for a beacon
    public delegate bool PickupInput();
    public PickupInput PickupCall;
    // The choice from up, down, left, and right... returns true if successful
    public delegate bool SelectionDirection(Direction choice);
    public SelectionDirection SelectionCall;
    // Move the player
    public delegate void MoveDirection(Vector2 direction);
    public MoveDirection MoveCall;
    // Reset the level
    public delegate void ResetLevel();
    public ResetLevel ResetCall;

    enum KeyStatus { none, pressed, held, released };
    struct AxisToStatus
    {
        public string axis;
        public KeyStatus status;
        public float duration;

        public AxisToStatus(string a)
        {
            axis = a;
            status = KeyStatus.none;
            duration = 0.0f;
        }
    };
    AxisToStatus[] inputStatuses = new AxisToStatus[]
    {
        new AxisToStatus("Output"), new AxisToStatus("Pickup"), new AxisToStatus("Rotate"),
        new AxisToStatus("Reset"), new AxisToStatus("Cancel")
    };

    float rotateHoldDuration = 0.2f;

    bool choosingDirection = false;
    bool ChoosingDirection
    {
        set
        {
            if (value != choosingDirection)
            {
                choosingDirection = value;
                inDirSterile = false;
            }
        }
    }

    bool inDirSterile = true;
    bool activeControls = false;

    // Reset the delegates related to non-permanent entities
    public void ResetManager()
    {
        RotateBeaconCall = null;
        ChooseDirectionCall = null;
        ChangeBeaconCall = null;
        PickupCall = null;
        SelectionCall = null;
        ResetCall = null;
    }
    // Sets the players controls active/inactive as appropriate
    public void PlayerControlsActive(bool state)
    {
        activeControls = state;
        MoveCall(new Vector2());
    }

    private void Update()
    {
        if (!activeControls)
        {
            return;
        }
        // Axis list processing
        for (int i = 0; i < inputStatuses.Length; ++i)
        {
            ref AxisToStatus inStat = ref inputStatuses[i];
            // If released last frame, now: none
            if (inStat.status == KeyStatus.released)
            {
                inStat.status = KeyStatus.none;
            }
            // If the axis is registering
            if (Input.GetAxisRaw(inStat.axis) > 0)
            {
                // If status none, now: pressed  (reset hold duration)
                if (inStat.status == KeyStatus.none)
                {
                    inStat.status = KeyStatus.pressed;
                    inStat.duration = 0;
                }
                // Else if pushed last frame, now: held
                else if (inStat.status == KeyStatus.pressed)
                {
                    inStat.status = KeyStatus.held;
                }
                // If held, increment duration for
                if (inStat.status == KeyStatus.held)
                {
                    inStat.duration += Time.deltaTime;
                }
            }
            // If the axis isn't registering
            else
            {
                // If previously pushed, now: released
                if (inStat.status == KeyStatus.pressed || inStat.status == KeyStatus.held)
                {
                    inStat.status = KeyStatus.released;
                }
            }
        }

        // Horizontal/vertical processing/response
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        // If input direction is un-sterile and the input is a zero vector, set sterile
        if (!inDirSterile && inputDir == Vector2.zero)
        {
            inDirSterile = true;
        }
        // If choosing a direction, and input vector is non-zero
        if (choosingDirection && inputDir != Vector2.zero && inDirSterile && SelectionCall != null)
        {
            Direction dir = Direction.up;
            // If horizontal input is stronger than vertical
            if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y))
            {
                // If input is to right
                if (inputDir.x > 0)
                {
                    dir = Direction.right;
                }
                // If input is to left
                else
                {
                    dir = Direction.left;
                }
            }
            // If vertical input is stronger than horizontal
            else
            {
                // If input direction is up
                if (inputDir.y > 0)
                {
                    dir = Direction.up;
                }
                // If input direction is down
                else
                {
                    dir = Direction.down;
                }
            }

            bool choiceOngoing = false;
            // Individually call delegate functions to see if one returns true
            foreach (SelectionDirection func in SelectionCall.GetInvocationList())
            {
                choiceOngoing |= func(dir);
            }
            // If any selections were successful, set back to moving
            if (!choiceOngoing)
            {
                // Set back to moving
                ChoosingDirection = false;
            }
        }
        // If currently taking movement input
        else if (!choosingDirection && inDirSterile && MoveCall != null)
        {
            // Inform of movement
            MoveCall(inputDir);
        }
        else if(choosingDirection && !inDirSterile)
        {
            MoveCall(Vector2.zero);
        }

        // Axis response
        for (int i = 0; i < inputStatuses.Length; ++i)
        {
            switch (inputStatuses[i].axis)
            {
                case "Output":
                    HandleOutput(inputStatuses[i]);
                    break;
                case "Pickup":
                    HandlePickup(inputStatuses[i]);
                    break;
                case "Rotate":
                    HandleRotate(ref inputStatuses[i]);
                    break;
                case "Reset":
                    HandleReset(ref inputStatuses[i]);
                    break;
                case "Cancel":
                    HandleCancel(inputStatuses[i]);
                    break;
                default:
                    break;
            }
        }
    }

    // Response to output axis
    private void HandleOutput(AxisToStatus inStat)
    {
        // If no functions to call, return
        if (ChangeBeaconCall == null)
        {
            return;
        }
        if (inStat.status == KeyStatus.released)
        {
            ChangeBeaconCall();
        }
    }
    // Response to pickup axis
    private void HandlePickup(AxisToStatus inStat)
    {
        // If no functions to call, return
        if (PickupCall == null)
        {
            return;
        }
        if (inStat.status == KeyStatus.pressed)
        {
            bool isChoosing = false;
            // Individually call delegate functions to see if one returns true
            foreach (PickupInput func in PickupCall.GetInvocationList())
            {
                isChoosing |= func();
            }
            ChoosingDirection = isChoosing;
        }
    }
    // Response to rotate axis
    private void HandleRotate(ref AxisToStatus inStat)
    {
        if (inStat.status == KeyStatus.released)
        {
            if (inStat.duration < rotateHoldDuration)
            {
                bool isChoosing = false;
                // Individually call delegate functions to see if one returns true
                foreach (RotateBeacon func in RotateBeaconCall.GetInvocationList())
                {
                    isChoosing |= func();
                }
                ChoosingDirection = isChoosing;
            }
        }
        else if (inStat.status == KeyStatus.held)
        {
            if (inStat.duration >= rotateHoldDuration && inStat.duration < 2.0f)
            {
                bool isChoosing = false;
                // Individually call delegate functions to see if one returns true
                foreach (ChooseDirectionBeacon func in ChooseDirectionCall.GetInvocationList())
                {
                    isChoosing |= func();
                }
                ChoosingDirection = isChoosing;
                inStat.duration += 100.0f;
            }
        }

    }
    // Response to reset axis
    private void HandleReset(ref AxisToStatus inStat)
    {
        // If no functions to call, return
        if (ResetCall == null)
        {
            return;
        }
        // If key is being held down
        if (inStat.status == KeyStatus.held)
        {
            // Reset if held between 1 and 2 seconds
            if (inStat.duration > 1.0f && inStat.duration < 2.0f)
            {
                ResetCall();
                // Set duration above threshold to avoid repeated use
                inStat.duration = 100.0f;
                ChoosingDirection = false;
            }
        }
    }
    // Response to cancel axis
    private void HandleCancel(AxisToStatus inStat)
    {
        if (inStat.status == KeyStatus.held && inStat.duration > 1.5f)
        {
            SceneManager.LoadScene("Welcome");
        }
    }
}
