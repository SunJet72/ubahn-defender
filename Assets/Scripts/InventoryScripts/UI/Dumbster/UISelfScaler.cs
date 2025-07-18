using UnityEngine;
using UnityEngine.UI;

public class UISelfScaler : MonoBehaviour
{
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(CountActiveChildren() * transform.parent.gameObject.GetComponent<RectTransform>().rect.width, transform.parent.gameObject.GetComponent<RectTransform>().rect.height);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void Rebuild()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(CountActiveChildren() * transform.parent.gameObject.GetComponent<RectTransform>().rect.width, transform.parent.gameObject.GetComponent<RectTransform>().rect.height);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private int CountActiveChildren()
    {
        int activeChildrenCount = 0;
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                ++activeChildrenCount;
            }
        }
        return activeChildrenCount;
    }

}
