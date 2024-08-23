using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject particle0;
    public GameObject particle1;
    List<GameObject> particleList = new List<GameObject>();

    private void Start()
    {
        InitBlockParticle();
    }
    void InitBlockParticle()
    {
        for (int i = 0; i < 200; i++)
        {
            if ( i % 2 ==0 )
            {
                GameObject temp = Instantiate(particle0) as GameObject;
                temp.gameObject.SetActive(false);

                particleList.Add(temp);
                particleList[i].transform.SetParent(this.transform);
            }

            else
            {
                GameObject temp = Instantiate(particle1) as GameObject;
                temp.gameObject.SetActive(false);

                particleList.Add(temp);
                particleList[i].transform.SetParent(this.transform);
            }
        }
    }

    public void ParticleEffect(GameObject block)
    {
        Color blockColor = block.GetComponent<SpriteRenderer>().color;

        for (int i = 0; i <= particleList.Count; i++)
        {
            int actIdx = 0;
            foreach(var item  in particleList)
            {
                if(item.activeSelf)
                {
                    actIdx++;
                }
            }

            if( actIdx == particleList.Count)
            {
                Debug.LogError("particle pool error, idx = "+actIdx);
                break;
            }

            else if(!particleList[i].activeSelf)
            {
                Vector2 particleSpot 
                    = new Vector2(Random.Range(block.transform.position.x-1.0f, block.transform.position.x+1.0f) 
                                , block.transform.position.y - 0.3f);

                particleList[i].GetComponent<SpriteRenderer>().color = blockColor;
                particleList[i].transform.position = particleSpot;
                particleList[i].SetActive(true);

                break;
            }
        }   
    }
}
