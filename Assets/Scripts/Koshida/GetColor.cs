using UnityEngine;
using UnityEngine.UI;

public class GetColor : MonoBehaviour
{
    public Color _groundColor;
    private Texture2D tex = null;
    [SerializeField]
    private Transform _groundPoint;

    void Start()
    {
        tex = new Texture2D(1, 1, TextureFormat.RGB24, false);
    }

    void OnPostRender()
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(GetComponent<Camera>(), _groundPoint.position);
        tex.ReadPixels(new Rect(pos.x, pos.y, 1, 1), 0, 0);
        _groundColor = tex.GetPixel(0, 0);
    }
}