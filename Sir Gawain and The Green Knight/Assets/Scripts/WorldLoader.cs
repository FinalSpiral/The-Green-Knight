using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    [SerializeField]
    private WorldData wd;

    [SerializeField]
    private int pos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int i = 0;
            foreach(GameObject g in wd.Areas)
            {
                if(i==pos-1 || i==pos || i == pos + 1)
                {
                    g.SetActive(true);
                }
                else
                {
                    g.SetActive(false);
                }
                i++;
            }
        }
    }
}
