using UnityEngine;
using Photon.Pun;
using Unity.Collections;
public class GroundTextureGenerator : MonoBehaviour
{
    [SerializeField] public Texture2D texture;
    public int width = 32;
    public int height = 32;
    void Start()
    {
        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.mainTexture = texture;
        GenerateMap();
    }

    void Update()
    {
        
    }

    public void DrawAt(int x, int y, int brushRadius, Vector4 colour)
    {
        texture.GetPixels();
        for (int i = -brushRadius; i <= brushRadius; i++)
        {
            for (int j = -brushRadius; j <= brushRadius; j++)
            {
                ChangePixelColour(x + i, y + j, colour);
                
            }
        }
        texture.Apply();
    }

    
    void ChangePixelColour(int x, int y, Vector3 colour)
    {
        //Change pixel
        Color newColour = new Color(colour.x, colour.y, colour.z);
        texture.SetPixel(x, y, newColour);

       
    }
    void GenerateMap()
    {
        texture.GetPixels();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(i, j, new Color((float)i / (float)width, (float)j / (float)height, 0, 1));
                
                texture.Apply();
            }
        }
    }

    public Color GetColorAt(int x, int y)
    {
        return texture.GetPixel(x, y);
    }

}
