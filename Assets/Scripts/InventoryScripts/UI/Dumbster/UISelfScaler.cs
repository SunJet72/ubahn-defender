using UnityEngine;
using UnityEngine.UI;

public class UISelfScaler : MonoBehaviour
{
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(2 * transform.parent.gameObject.GetComponent<RectTransform>().rect.width, transform.parent.gameObject.GetComponent<RectTransform>().rect.height);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

}
