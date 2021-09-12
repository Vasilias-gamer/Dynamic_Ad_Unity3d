using UnityEngine;
using UnityEngine.UI;

public class Ad_setup : MonoBehaviour
{
    [SerializeField]
    private InputField url;
    [SerializeField]
    private InputField user_input;
    [SerializeField]
    private json_controller controller;
    [SerializeField]
    private GameObject ad_panal;
    [SerializeField]
    private Text ad_text;
    [HideInInspector]
    public ad_layers panal_data;
    [HideInInspector]
    public ad_layers text_data;
    [SerializeField]
    public Text error;
    [SerializeField]
    private Sprite default_sprite;
    public string display_text;
    private void Start()
    {
        ad_panal.SetActive(false);
        error.gameObject.SetActive(false);
	url.text="http://lab.greedygame.com/arpit-dev/unity-assignment/templates/frame_only.json";
    	user_input.text="ORDER NOW!!!";
    }

    public void show_ad()
    {
        error.gameObject.SetActive(true);
        error.text = "Loading";
        controller.began_ad_setup(url.text, user_input.text);
    }
    public void close_ad()
    {
        setup_panal_default();
        setup_text_default();
        ad_panal.SetActive(false);
        error.gameObject.SetActive(false);
    }

    public void setup_panal(ad_layers frame)
    {
        RectTransform Rtrans = ad_panal.GetComponent<RectTransform>();
        Image image = ad_panal.GetComponent<Image>();

        Rtrans.localPosition = new Vector3(frame.placement[0].position.x, frame.placement[0].position.y, ad_panal.GetComponent<RectTransform>().localPosition.z);
        Rtrans.sizeDelta = new Vector2(frame.placement[0].position.width, frame.placement[0].position.height);
        if(frame.texture != null)
            image.sprite = Sprite.Create(frame.texture, new Rect(0, 0, frame.texture.width, frame.texture.height), Vector2.one / 2);


        if (frame.operations.Count > 0)
        {
            switch (frame.operations[0].name)
            {
                case "color":
                    Color newCol;

                    if (ColorUtility.TryParseHtmlString(frame.operations[0].argument, out newCol))
                    {
                        image.color = newCol;
                    }
                    else
                    {
                        error.gameObject.SetActive(true);
                        error.text = "invalid color code";
                        Debug.Log("Invalid color code.");
                    }
                    break;

                default:
                    error.gameObject.SetActive(true);
                    error.text = "argument " + frame.operations[0].name + " not found.";
                    Debug.Log("argument " + frame.operations[0].name + " not found.");
                    break;
            }

        }
        ad_panal.SetActive(true);
        error.text = "Done";
    }

    public void setup_panal_default()
    {
        RectTransform Rtrans = ad_panal.GetComponent<RectTransform>();
        Image image = ad_panal.GetComponent<Image>();

        Rtrans.localPosition = Vector3.zero;
        Rtrans.sizeDelta = new Vector2(200, 200);
        image.sprite = default_sprite;
        image.color = Color.white;
        ad_panal.SetActive(true);
        error.text = "Done";
    }

    public void setup_text(ad_layers text)
    {
        ad_text.text = display_text;
        RectTransform Rtrans = ad_text.rectTransform;
        Rtrans.localPosition= new Vector3(text.placement[0].position.x, text.placement[0].position.y, Rtrans.localPosition.z);
        Rtrans.sizeDelta = new Vector2(text.placement[0].position.width, text.placement[0].position.height);
       
        if (text.operations.Count>0)
        {
            switch (text.operations[0].name)
            {
                case "color":
                    Color newCol;

                    if (ColorUtility.TryParseHtmlString(text.operations[0].argument, out newCol))
                    {
                        ad_text.color = newCol;
                    }
                    else
                    {
                        error.gameObject.SetActive(true);
                        error.text = "invalid color code";
                        Debug.Log("Invalid color code.");
                    }
                    break;

                default:
                    error.gameObject.SetActive(true);
                    error.text = "argument " + text.operations[0].name + " not found.";
                    Debug.Log("argument " + text.operations[0].name + " not found.");
                    break;
            }
            
        }
    }

    public void setup_text_default()
    {
        RectTransform Rtrans = ad_text.rectTransform;
        Rtrans.localPosition = Vector3.zero;
        Rtrans.sizeDelta = new Vector2(200,200);
        ad_text.text = display_text;
        ad_text.color = Color.black;
    }



}
