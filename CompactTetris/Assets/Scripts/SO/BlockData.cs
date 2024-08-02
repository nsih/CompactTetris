using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlockData", menuName = "BlockData")]
public class BlockData : ScriptableObject
{
    [SerializeField] private List<GameObject> blockShapes;
    [SerializeField] private List<Sprite> blockSprites;
    [SerializeField] private Color[] colorPalette;


    public GameObject Shape { get; private set; } 
    public Sprite BlockSprite { get; private set; }
    public Color RandomColor { get; private set; }



    public void InitRandomBlockData()
    {
        int idx = Random.Range(0, blockShapes.Count);

        Shape = blockShapes[idx];
        BlockSprite = blockSprites[idx];

        RandomColor = colorPalette[Random.Range(0, colorPalette.Length)];
    }
}
