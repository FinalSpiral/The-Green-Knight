using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Mesh mesh;

    private Vector2[] uv = new Vector2[4];

    [SerializeField]
    private int framesPerSecond, width, height, stepX, stepY, animationIndex;
    [SerializeField]
    private List<AnimationData> animations;
    [SerializeField]
    private List<Vector2Int> frameCol;
    [SerializeField]
    private bool copyMode = false;
    [SerializeField]
    private AnimationScript copy;

    public bool forward = true, change = false;

    [HideInInspector]
    public int ic = 0;

    private float time, frameSpan;

    private bool animate = true;
    
    private bool finished = false;

    public bool Finished { get { return finished; } }

    [HideInInspector]
    public bool loop;

    int i = 0, x, y, widthI, heightI, yI, xI;

    private int i2 = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (copyMode)
        {
            widthI = width / stepX; heightI = height / stepY;
            i = copy.ic;
            forward = copy.forward;
            xI = i - ((i / widthI) * widthI); yI = i / widthI;

            x = xI * stepX;
            y = height - (stepY * yI);

            mesh = GetComponent<MeshFilter>().mesh;
            if (forward)
            {
                uv[0] = ConvertPixelsToUVCord(x, y - stepY, width, height);
                uv[1] = ConvertPixelsToUVCord(x + stepX, y - stepY, width, height);
                uv[2] = ConvertPixelsToUVCord(x, y, width, height);
                uv[3] = ConvertPixelsToUVCord(x + stepX, y, width, height);
            }
            else
            {
                uv[1] = ConvertPixelsToUVCord(x, y - stepY, width, height);
                uv[0] = ConvertPixelsToUVCord(x + stepX, y - stepY, width, height);
                uv[3] = ConvertPixelsToUVCord(x, y, width, height);
                uv[2] = ConvertPixelsToUVCord(x + stepX, y, width, height);
            }

            mesh.uv = uv;
        }
        else
        {           
            framesPerSecond = animations[animationIndex].framesPerSecond;
            widthI = width / stepX; heightI = height / stepY;

            i = animations[animationIndex].from;

            xI = i - ((i / widthI) * widthI); yI = i / widthI;

            x = xI * stepX;
            y = height - (stepY * yI);

            if (!animations[animationIndex].loop)
            {
                loop = false;
            }
            else
            {
                loop = true;
            }

            frameSpan = 1f / framesPerSecond;
            time = frameSpan;
            mesh = GetComponent<MeshFilter>().mesh;
            if (forward)
            {
                ic = i;
                uv[0] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                uv[1] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                uv[2] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                uv[3] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
            }
            else
            {
                ic = i;
                uv[1] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                uv[0] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                uv[3] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                uv[2] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
            }

            mesh.uv = uv;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (copyMode)
        {
            i = copy.ic;
            if (i2 != i)
            {
                widthI = width / stepX; heightI = height / stepY;
                i2 = i;
                forward = copy.forward;
                xI = i - ((i / widthI) * widthI); yI = i / widthI;

                x = xI * stepX;
                y = height - (stepY * yI);

                mesh = GetComponent<MeshFilter>().mesh;
                if (forward)
                {
                    uv[0] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                    uv[1] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                    uv[2] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                    uv[3] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                }
                else
                {
                    uv[1] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                    uv[0] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                    uv[3] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                    uv[2] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                }

                mesh.uv = uv;
            }
        }
        else
        {
            if (animate)
            {
                if (!animations[animationIndex].loop)
                {
                    loop = false;
                }
                else
                {
                    loop = true;
                }
                framesPerSecond = animations[animationIndex].framesPerSecond;
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    xI = i - ((i / widthI) * widthI); yI = i / widthI;

                    x = xI * stepX;
                    y = height - (stepY * yI);

                    if (forward)
                    {
                        ic = i;
                        uv[0] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                        uv[1] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                        uv[2] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                        uv[3] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                    }
                    else
                    {
                        ic = i;
                        uv[1] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                        uv[0] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                        uv[3] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                        uv[2] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                    }

                    mesh.uv = uv;
                    i++;

                    if (i > animations[animationIndex].to)
                    {
                        i = animations[animationIndex].from;
                        if (!animations[animationIndex].loop)
                        {
                            animate = false;
                            finished = true;
                        }
                    }

                    frameSpan = 1f / framesPerSecond;
                    time = frameSpan;
                }
                if (change)
                {
                    if (forward)
                    {
                        ic = i;
                        uv[0] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                        uv[1] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                        uv[2] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                        uv[3] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                    }
                    else
                    {
                        ic = i;
                        uv[1] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                        uv[0] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                        uv[3] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                        uv[2] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);
                    }
                    mesh.uv = uv;
                    change = false;
                }
            }
        }
    }

    public void ChangeAnimation(int aI)
    {
        animationIndex = aI;
        i = animations[animationIndex].from;
        animate = true;
        finished = false;
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
