using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class AnimationScript : MonoBehaviour
{
    /// <summary>
    /// Inputs:
    /// Mesh
    /// AnimationData[]
    /// Math
    /// ChangeAnimation
    /// StartAnimation
    /// 
    /// Outputs:
    /// FrameNum
    /// Finished
    /// AnimationData[]
    /// Direction
    /// </summary>

    /*widthI = width / stepX; heightI = height / stepY;
    i = animations[animationIndex].from;
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

    mesh.uv = uv;*/

    //Input variables
    private Mesh mesh;

    [SerializeField]
    private int width, height, stepX, stepY, startAnimationIndex;

    public List<AnimationData> animations;

    //Utility variables
    private int framesPerSecond, animationIndex;

    private Vector2[] uv = new Vector2[4];

    private int i = 0, x, y, widthI, heightI, yI, xI;

    private bool finished = false, animate = true, draw = true;

    private float time = 0;

    private void Awake()
    {
        //Declarations
        mesh = GetComponent<MeshFilter>().mesh;
        i = animations[startAnimationIndex].from;
        animationIndex = startAnimationIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (animate)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                if (i > animations[animationIndex].to)
                {
                    if (animations[animationIndex].loop)
                    {
                        i = animations[animationIndex].from;
                        draw = true;
                        finished = false;
                    }
                    else
                    {
                        animate = false;
                        draw = false;
                        finished = true;
                        Debug.Log("Finnished: " + i);
                    }
                }

                if (draw)
                {
                    Debug.Log(i);
                    //Do math
                    widthI = width / stepX; heightI = height / stepY;

                    xI = i - ((i / widthI) * widthI); yI = i / widthI;

                    x = xI * stepX;
                    y = height - (stepY * yI);

                    //Draw frame
                    uv[0] = ConvertPixelsToUVCord(x + 1, y - stepY, width, height);
                    uv[1] = ConvertPixelsToUVCord(x + stepX - 1, y - stepY, width, height);
                    uv[2] = ConvertPixelsToUVCord(x + 1, y - 1, width, height);
                    uv[3] = ConvertPixelsToUVCord(x + stepX - 1, y - 1, width, height);

                    mesh.uv = uv;
                }

                //Time
                time = 1f / animations[animationIndex].framesPerSecond;
                i++;
            }
        }
    }

    //Utility functions
    public Vector2 ConvertPixelsToUVCord(int x, int y, int textureWidth, int textureHeight)
    {
        return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    }

    public Vector2 ConvertPosToUVCord(int x, int y, int size)
    {
        return new Vector2();
    }

    //Input functions
    public void ChangeAnimation(int aI)
    {        
        i = animations[aI].from;
        animationIndex = aI;
        time = 0;
        animate = true;
        draw = true;
        finished = false;
    }

    //Output functions
    public int getCurrentAnimationIndex()
    {
        return animationIndex;
    }

    public AnimationData getCurrentAnimationData()
    {
        return animations[animationIndex];
    }

    public int getCurrentFrameImage()
    {
        return i-1;
    }

    public int getCurrentDirection()
    {
        return -1;
    }

    public bool animationFinished()
    {
        //Debug.Log(finished);
        return finished;
    }
}
