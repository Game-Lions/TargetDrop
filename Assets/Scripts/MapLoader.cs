using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApplyTextureToGameObject : MonoBehaviour
{
    public string imageURL;  // URL of the image
    private Renderer objectRenderer;  // Reference to the Renderer of the GameObject

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();  // Get the Renderer component
        if (objectRenderer != null && !string.IsNullOrEmpty(imageURL))
        {
            StartCoroutine(DownloadAndApplyTexture(imageURL));
        }
    }

    IEnumerator DownloadAndApplyTexture(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D downloadedTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Check if the texture is too large
            if (downloadedTexture.width > 16384 || downloadedTexture.height > 16384)
            {
                Debug.Log("Resizing large texture to fit within Unity's limits.");

                // Create a smaller texture
                Texture2D resizedTexture = new Texture2D(16384, 16384, downloadedTexture.format, false);
                resizedTexture.Reinitialize(16384, 16384);
                resizedTexture.SetPixels(downloadedTexture.GetPixels(0, 0, 16384, 16384));
                resizedTexture.Apply();

                downloadedTexture = resizedTexture;  // Replace the large texture with the resized one
            }

            objectRenderer.material.mainTexture = downloadedTexture;  // Apply the texture to the material
            Debug.Log("Load Success");
        }
        else
        {
            Debug.LogError("Failed to download texture: " + request.error);
        }
    }
}
