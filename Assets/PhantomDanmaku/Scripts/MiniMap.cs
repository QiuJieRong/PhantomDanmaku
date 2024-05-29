using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Camera mapCamera;
    private RawImage m_Image;
    private RenderTexture m_RenderTexture;
    void Start()
    {
        m_Image = GetComponent<RawImage>();
        m_RenderTexture = new RenderTexture(1920, 1080, 1, new DefaultFormat());
        mapCamera.targetTexture = m_RenderTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
