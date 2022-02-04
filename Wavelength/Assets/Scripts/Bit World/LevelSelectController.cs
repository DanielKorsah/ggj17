using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    [SerializeField]
    LevelListSO Levels;
    [SerializeField]
    Transform LevelDetailsPrefab;
    [SerializeField]
    RectTransform ContentParent;
    [SerializeField]
    Text Flavour;

    [SerializeField]
    Scrollbar Scroller;

    int SidePadding = 400;
    int MidPadding = 300;

    int CurrentScrollIndex = 0;
    int ObjCount;
    float ScrollTarget = 0f;
    float MinSpeed;
    float MinSpeedDist;
    float ScrollSizePadding;

    List<LevelDetailsDisplay> LevelDetailsObjs = new List<LevelDetailsDisplay>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Levels.Levels.Count; ++i)
        {
            LevelDetailsDisplay ldd = Instantiate(LevelDetailsPrefab, ContentParent).GetComponent<LevelDetailsDisplay>();
            ldd.transform.localPosition = new Vector3(SidePadding + i * MidPadding, 0, 0);
            ldd.SetDetails(Levels.Levels[i]);
            LevelDetailsObjs.Add(ldd);
        }

        ObjCount = LevelDetailsObjs.Count;
        //Rect rec = ContentParent.rect;
        //rec.width = LevelDetailsObjs[LevelDetailsObjs.Count - 1].transform.localPosition.x + SidePadding;
        //ContentParent.rect.Set(rec.x, rec.y, rec.width, rec.height);
        ContentParent.sizeDelta = new Vector2(LevelDetailsObjs[ObjCount - 1].transform.localPosition.x + SidePadding, ContentParent.sizeDelta.y);
        ScrollSizePadding = 1.0f / (ObjCount - 1);
        MinSpeed = ScrollSizePadding * 2.0f;
        MinSpeedDist = ScrollSizePadding * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            // Go right
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                MoveScrollIndex(1);
            }
            // Go left
            else
            {
                MoveScrollIndex(-1);
            }
        }

        if (ScrollTarget != Scroller.value)
        {
            float diff = ScrollTarget - Scroller.value;
            float dist = Mathf.Sign(diff) * MinSpeed * Time.deltaTime;
            float rapidExtra = Mathf.Abs(diff) - MinSpeedDist;
            if (rapidExtra > 0)
            {
                float percentExtra = rapidExtra / MinSpeedDist;
                float modifiedPercent = 1f + percentExtra;
                modifiedPercent *= modifiedPercent;
                dist = Mathf.Sign(diff) * MinSpeed * modifiedPercent * Time.deltaTime;
            }

            if (Mathf.Abs(dist) > Mathf.Abs(diff))
            {
                dist = diff;
            }

            Scroller.value += dist;
        }
    }

    private void MoveScrollIndex(int change)
    {
        int before = CurrentScrollIndex;
        CurrentScrollIndex = Mathf.Clamp(CurrentScrollIndex + change, 0, ObjCount - 1);
        // If unchanged
        if (before == CurrentScrollIndex)
        {
            return;
        }

        ScrollTarget = (LevelDetailsObjs[CurrentScrollIndex].transform.localPosition.x - SidePadding) / (MidPadding * (ObjCount - 1));
        //Scroller.value = (LevelDetailsObjs[CurrentScrollIndex].transform.localPosition.x - SidePadding) / (MidPadding * (ObjCount - 1));
    }
}
