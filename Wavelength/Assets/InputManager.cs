using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public delegate void RotateBeacon();
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
    // The choice from up, down, left, and right
    public delegate void SelectionDirection(Direction choice);
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
    AxisToStatus[]inputStatuses = new AxisToStatus[]
    {
        new AxisToStatus("Output"), new AxisToStatus("Pickup"), new AxisToStatus("Rotate"), new AxisToStatus("Reset")
    };

    bool choosingDirection = false;
    bool disabledInput = false;

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

    private void Update()
    {
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
        // If choosing a direction, and input vector is non-zero
        if (choosingDirection && inputDir != Vector2.zero && SelectionCall != null)
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
            // Inform about direction choices
            SelectionCall(dir);
            // Set back to moving
            choosingDirection = false;
        }
        // If currently taking movement input
        else if (!choosingDirection && MoveCall != null)
        {
            // Inform of movement
            MoveCall(inputDir);
        }

        // Axis response
        foreach (AxisToStatus inStat in inputStatuses)
        {
            switch (inStat.axis)
            {
                case "Output":
                    HandleOutput(inStat);
                    break;
                case "Pickup":
                    HandlePickup(inStat);
                    break;
                case "Rotate":
                    HandleRotate(inStat);
                    break;
                case "Reset":
                    HandleReset(inStat);
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
        if (inStat.status == KeyStatus.released)
        {
            // Individually call delegate functions to see if one returns true
            foreach (PickupInput func in PickupCall.GetInvocationList())
            {
                if (func())
                {
                    choosingDirection = true;
                }
            }
        }
    }
    // Response to rotate axis
    private void HandleRotate(AxisToStatus inStat)
    {
        if (inStat.status == KeyStatus.released)
        {
            if (inStat.duration < 0.4f)
            {
                // If no functions to call, return
                if (RotateBeaconCall == null)
                {
                    return;
                }
                RotateBeaconCall();
            }
            else
            {
                // Individually call delegate functions to see if one returns true
                foreach (ChooseDirectionBeacon func in ChooseDirectionCall.GetInvocationList())
                {
                    if (func())
                    {
                        choosingDirection = true;
                    }
                }
            }
        }
    }
    // Response to reset axis
    private void HandleReset(AxisToStatus inStat)
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
            }
        }
    }
}
