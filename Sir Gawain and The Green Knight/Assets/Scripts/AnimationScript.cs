using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Mesh mesh;

    private Vector2[] uv = new Vector2[4];

    [SerializeField]
    private int framesPerSecond, width, height, step, animationIndex;
    [SerializeField]
    private List<Vector2Int> animations;

    private float time, frameSpan;

    int i = 0, x, y;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        y = height;

        frameSpan = 1f / framesPerSecond;
        time = frameSpan;
        mesh = GetComponent<MeshFilter>().mesh;

        uv[0] = ConvertPixelsToUVCord(x, y - step, width, height);
        uv[1] = ConvertPixelsToUVCord(x + step, y - step, width, height);
        uv[2] = ConvertPixelsToUVCord(x, y, width, height);
        uv[3] = ConvertPixelsToUVCord(x + step, y, width, height);

        mesh.uv = uv;        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            uv[0] = ConvertPixelsToUVCord(x, y - step, width, height);
            uv[1] = ConvertPixelsToUVCord(x + step, y - step, width, height);
            uv[2] = ConvertPixelsToUVCord(x, y, width, height);
            uv[3] = ConvertPixelsToUVCord(x + step, y, width, height);

            mesh.uv = uv;
            i++;
            x += step;
            if (!(x < width))
            {
                x = 0;
                y -= step;
                if (!(y > 0))
                {
                    y = height;
                    i = 0;
                }
            }
            time = frameSpan;
        }
    }

    public Vector2 ConvertPixelsToUVCord(int x, int y, int textureWidth, int textureHeight)
    {
        return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    }

    public Vector2 ConvertPosToUVCord(int x, int y, int size)
    {
        return new Vector2();
    }
}
