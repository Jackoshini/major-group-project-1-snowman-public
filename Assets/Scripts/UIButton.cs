using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [Header("Interactivity")]
    [SerializeField]
    private Color normalColor = Color.white;
    [SerializeField]
    private Color hoverColor;

    [Header("Audio Feedback")]
    [SerializeField]
    private AudioClip hoverSound;
    [SerializeField]
    private AudioClip clickSound;
    [SerializeField]
    [Range(0, 1)]
    private float volume;

    private Image border;
    private TextMeshProUGUI text;
    private AudioSource audioController;

    void Start()
    {
        border = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        audioController = FindObjectOfType<AudioSource>();
    }

    public void OnClick()
    {
        audioController.PlayOneShot(clickSound, volume);
    }

    public void OnMouseHover()
    {
        border.color = hoverColor;

        if (text != null)
        {
            text.color = hoverColor;
        }

        audioController.PlayOneShot(hoverSound, volume);
    }

    public void OnMouseExit()
    {
        border.color = normalColor;

        if (text != null)
        {
            text.color = normalColor;
        }
    }
}
