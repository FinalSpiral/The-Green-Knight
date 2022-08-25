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
    private List<Vector3Int> animations;

    private float time, frameSpan;

    private bool animate = true; 

    int i = 0, x, y, widthI, heightI, yI, xI;

    // Start is called before the first frame update
    void Start()
    {
        widthI = width / step; heightI = height / step;
       
        i = animations[animationIndex].x;

        xI = i - ((i / widthI) * widthI); yI = i / widthI;

        x = xI * step;
        y = height - (step * yI);

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
        if (animate)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                xI = i - ((i / widthI) * widthI); yI = i / widthI;

                x = xI * step;
                y = height - (step * yI);

                uv[0] = ConvertPixelsToUVCord(x, y - step, width, height);
                uv[1] = ConvertPixelsToUVCord(x + step, y - step, width, height);
                uv[2] = ConvertPixelsToUVCord(x, y, width, height);
                uv[3] = ConvertPixelsToUVCord(x + step, y, width, height);

                mesh.uv = uv;
                i++;

                if (i > animations[animationIndex].y)
                {
                    i = animations[animationIndex].x;
                    if (animations[animationIndex].z == 0)
                    {
                        animate = false;
                    }
                }

                time = frameSpan;
            }
        }
    }

    public void ChangeAnimation(int aI)
    {
        animationIndex = aI;
        i = animations[animationIndex].x;
        animate = true;
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
