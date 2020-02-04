using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyStatus { none, pressed, held, released, frozen };

public class InputManagerSuper : MonoBehaviour
{
    #region singleton
    private static InputManagerSuper instance;
    public static InputManagerSuper Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public string[] inputAxes = new string[]
        { "Horizontal", "Vertical", "Output", "Pickup", "Rotate", "Reset", "Cancel" };
    // true if frozen, false is free
    private Dictionary<string, bool> inputAxesFrozen = new Dictionary<string, bool> {
        { "Horizontal", false }, { "Vertical", false }, { "Output", false },
        { "Pickup", false }, { "Rotate", false }, { "Reset", false },
        { "Cancel", false } };
    private bool allFree = true;
    private Dictionary<string, float> inputAxesDurations = new Dictionary<string, float> {
        { "Horizontal", 0.0f }, { "Vertical", 0.0f }, { "Output", 0.0f },
        { "Pickup", 0.0f }, { "Rotate", 0.0f }, { "Reset", 0.0f },
        { "Cancel", 0.0f } };
    public Vector2 inputDir = Vector2.zero;
    public Direction dir = Direction.up;

    private void Update()
    {
        // Free frozen inputs
        if (!allFree)
        {
            allFree = true;
            foreach (string a in inputAxes)
            {
                if (inputAxesFrozen[a])
                {
                    if (!Input.GetButton(a))
                    {
                        inputAxesFrozen[a] = false;
                    }
                    else
                    {
                        allFree = false;
                    }
                }
            }
        }
        // Count time held of unfrozen inputs
        foreach (string a in inputAxes)
        {
            if (!inputAxesFrozen[a])
            {
                if (Input.GetButton(a))
                {
                    inputAxesDurations[a] += Time.deltaTime;
                }
                else if(!Input.GetButtonUp(a))
                {
                    inputAxesDurations[a] = 0.0f;
                }
            }
        }
        // Process movement (if unfrozen)
        if (!inputAxesFrozen["Horizontal"] && !inputAxesFrozen["Vertical"])
        {
            // Horizontal/vertical processing/response
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
        }
    }

    public bool AxisDown(string axis)
    {
        return !inputAxesFrozen[axis] && Input.GetButtonDown(axis);
    }

    public bool AxisReleased(string axis)
    {
        return !inputAxesFrozen[axis] && Input.GetButtonUp(axis);
    }

    public bool AxisHeld(string axis)
    {
        return !inputAxesFrozen[axis] && Input.GetButton(axis) && !(Input.GetButtonDown(axis) || Input.GetButtonUp(axis));
    }

    public float AxisDuration(string axis)
    {
        return inputAxesDurations[axis];
    }

    public void FreezeInputs()
    {
        foreach (string a in inputAxes)
        {
            inputAxesFrozen[a] = true;
            inputAxesDurations[a] = 0.0f;
        }
        allFree = false;
    }
}
