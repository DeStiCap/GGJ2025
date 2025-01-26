using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    void Update()
    {
        Vector2 relatedPostion = Camera.main.WorldToScreenPoint(target.transform.position);
        ((RectTransform)transform).anchoredPosition = new Vector2(
            (relatedPostion.x - Screen.width/2) / 4 + offset.x, 
            (relatedPostion.y - Screen.height/2) / 4 + offset.y);
    }
}
