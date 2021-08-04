using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class json_controller : MonoBehaviour
{
    private Ad_data UI_data;
    [SerializeField]
    private Ad_setup UI_setup;
    

    // Start is called before the first frame update
    public void began_ad_setup(string _json_URL,string user_text)
    {
        UI_setup.display_text = user_text;
        StartCoroutine(download_data(_json_URL));
    }

    IEnumerator download_data(string json_URL)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(json_URL))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = json_URL.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Internet connection isshu.");//pages[page] + ": Error: " + webRequest.error);
                    UI_setup.error.gameObject.SetActive(true);
                    UI_setup.error.text = "Internet connection isshu.";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("Server isshu");// pages[page] + ": HTTP Error: " + webRequest.error);
                    UI_setup.error.gameObject.SetActive(true);
                    UI_setup.error.text = "Server isshu";
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    process_json_data(webRequest.downloadHandler.text);
                    break;
            }
        }
        
    }


    private void process_json_data(string URL_data)
    {
        UI_data = JsonUtility.FromJson<Ad_data>(URL_data);
        bool frame_layer = true;
        bool text_layer = true;

        foreach(ad_layers layer in UI_data.layers)
        {
            if (layer.type.Equals("frame") && frame_layer)
            {
                if (placement_valitation(layer.placement[0]))
                {
                    if (layer.path != null)
                    {
                        StartCoroutine(GetTexture(layer.path, layer));
                    }
                }
                else
                {
                    UI_setup.error.gameObject.SetActive(true);
                    UI_setup.error.text = "invalid placement";
                    Debug.Log("invalid placement");
                }
                frame_layer = false;
            }
            else if(layer.type.Equals("text") && text_layer)
            {
                if (placement_valitation(layer.placement[0]))
                    UI_setup.setup_text(layer);
                else
                {
                    UI_setup.error.gameObject.SetActive(true);
                    UI_setup.error.text = "invalid placement";
                    Debug.Log("invalid placement");
                }
                text_layer = false;
            }
        }

        if (frame_layer && !text_layer)
        {
            UI_setup.setup_panal_default();
            frame_layer = false;
        }
        if (!frame_layer && text_layer)
        {
            UI_setup.setup_text_default();
            text_layer = false;
        }
        
        if(frame_layer && text_layer)
        {
            UI_setup.error.gameObject.SetActive(true);
            UI_setup.error.text = "valid layer not found!!";
            Debug.Log("valid layer not found!!");
        }
    }

    IEnumerator GetTexture(string URL , ad_layers frame_layer)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            UI_setup.error.gameObject.SetActive(true);
            UI_setup.error.text = "invalid image path.";
            Debug.Log("invalid image path.");
        }
        else
        {
            frame_layer.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            
            if (texture_validation(frame_layer.texture.GetRawTextureData()))
            {
                UI_setup.setup_panal(frame_layer);
            }
            else
            {
                UI_setup.error.gameObject.SetActive(true);
                UI_setup.error.text = "invalid texture";
                Debug.Log("invalid texture");
            }

        }
    }

    bool texture_validation(byte[] texture)
    {
        byte[] invalid_texture = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 0, 0, 255, 255, 255, 255, 255, 255 };

        bool valide = false;

        for(int i=0;i<invalid_texture.Length;i++)
        {
            if (texture[i] != invalid_texture[i])
            {
                valide = true;
            }
        }
        //Debug.Log(valide);
        return valide;
    }

    bool placement_valitation(layer_placement placement)
    {
        bool valid = true;
        //if (placement.position.x < 0 || placement.position.y < 0)
            //valid = false;
        if (placement.position.width < 0 || placement.position.height < 0)
            valid = false;
        return valid;
    }

}
