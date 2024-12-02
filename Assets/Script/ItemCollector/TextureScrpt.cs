using UnityEngine;

public class TextureScrpt : MonoBehaviour
{
    public static Texture2D Screenshot(Camera camera)
    {
        if (camera == null) return null;

        var renderText = camera.targetTexture;
        if (renderText == null)
        {
            renderText = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24, RenderTextureFormat.ARGB32);
            camera.targetTexture = renderText;
            camera.Render();
        }
        var texture = new Texture2D(renderText.width, renderText.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(renderText, texture);
        return texture;
    }
}
