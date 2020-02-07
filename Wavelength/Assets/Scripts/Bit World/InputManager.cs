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

    private InputManagerSuper inSuper;

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
        MoveCall(Vector2.zero);
    }

    private void Start()
    {
        inSuper = InputManagerSuper.Instance;
    }

    private void Update()
    {
        if (!activeControls)
        {
            return;
        }

        // Horizontal/vertical processing/response
        Vector2 inputDir = inSuper.inputDir.normalized;
        // If input direction is un-sterile and the input is a zero vector, set sterile
        if (!inDirSterile && inputDir == Vector2.zero)
        {
            inDirSterile = true;
        }
        // If choosing a direction, and input vector is non-zero
        if (choosingDirection && inputDir != Vector2.zero && inDirSterile && SelectionCall != null)
        {
            Direction dir = inSuper.dir;

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
        else if (choosingDirection && !inDirSterile)
        {
            MoveCall(Vector2.zero);
        }

        // Axis response
        for (int i = 0; i < inSuper.inputAxes.Length; ++i)
        {
            string a = inSuper.inputAxes[i];
            switch (a)
            {
                case "Output":
                    HandleOutput(a);
                    break;
                case "Pickup":
                    HandlePickup(a);
                    break;
                case "Rotate":
                    HandleRotate(a);
                    break;
                case "Reset":
                    HandleReset(a);
                    break;
                case "Cancel":
                    HandleCancel(a);
                    break;
                default:
                    break;
            }
        }
    }

    // Response to output axis
    private void HandleOutput(string axis)
    {
        // If no functions to call, return
        if (ChangeBeaconCall == null)
        {
            return;
        }
        if (inSuper.AxisReleased(axis))
        {
            ChangeBeaconCall();
        }
    }
    // Response to pickup axis
    private void HandlePickup(string axis)
    {
        // If no functions to call, return
        if (PickupCall == null)
        {
            return;
        }
        if (inSuper.AxisDown(axis))
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
    bool rotationHoldResponse = false;
    private void HandleRotate(string axis)
    {
        if (inSuper.AxisReleased(axis))
        {
            if (inSuper.AxisDuration(axis) < rotateHoldDuration)
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
        else if (inSuper.AxisHeld(axis))
        {
            if (inSuper.AxisDuration(axis) >= rotateHoldDuration)
            {
                if (!rotationHoldResponse)
                {
                    bool isChoosing = false;
                    // Individually call delegate functions to see if one returns true
                    foreach (ChooseDirectionBeacon func in ChooseDirectionCall.GetInvocationList())
                    {
                        isChoosing |= func();
                    }
                    ChoosingDirection = isChoosing;
                    rotationHoldResponse = true;
                }
            }
            else if (rotationHoldResponse)
            {
                rotationHoldResponse = false;
            }
        }

    }
    // Response to reset axis
    bool resetHoldResponse = false;
    private void HandleReset(string axis)
    {
        // If no functions to call, return
        if (ResetCall == null)
        {
            return;
        }
        // If key is being held down
        if (inSuper.AxisHeld(axis))
        {
            // Reset if held for longer than a second
            if (inSuper.AxisDuration(axis) >= 1.0f)
            {
                if (!resetHoldResponse)
                {
                    ResetCall();
                    inSuper.FreezeInputs();
                    ChoosingDirection = false;
                    resetHoldResponse = true;
                }
            }
            else if (resetHoldResponse)
            {
                resetHoldResponse = false;
            }
        }
    }
    // Response to cancel axis
    private void HandleCancel(string axis)
    {
        if (inSuper.AxisReleased(axis))
        {
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }
}
