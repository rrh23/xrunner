using UnityEngine;
public class Parallax : MonoBehaviour
{
    public Material mat;
    public float offsetX; // accumulated offset

    [Range(0f, 0.7f)]
    public float speed;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offsetX += speed * Time.deltaTime;
        offsetX %= 1f;

        mat.mainTextureOffset = new Vector2(offsetX, 0);
    }
}
