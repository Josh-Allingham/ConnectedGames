using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI main;

    [Header("Tutorial")]
    public CanvasGroup CG_Tutorial;
    public Sprite controlsTutorial;
    public Sprite jumpTutorial;
    public Sprite windmillTutorial;
    public Sprite cauldronTutorial;
    public Image tutorialUI;
    public Button hintButton;
    public Button closeButton;
    
    [Header("Dialogue")]
    [SerializeField] private CanvasGroup DialogueUI;
    [SerializeField] private TMP_Text DialogueUIText;
    [SerializeField] private TMP_Text HighlightText;
    void Start()
    {
        main = this;
        HighlightText.text = "";
        DialogueUI.alpha = 0f;
    }

    void Update()
    {
        
    }

    public void ShowDialogue(string dialogue)
    {
        DialogueUI.alpha = 1f;
        DialogueUIText.text = dialogue;
    }

    public void EndDialogue()
    {
        DialogueUI.alpha = 0f;
        DialogueUIText.text = "";
        HighlightText.text = "";
    }
    public void UpdateHighlightText(string text, Vector3 position, float alpha)
    {
        HighlightText.text = text;
        HighlightText.transform.position = Camera.main.WorldToScreenPoint(position) + Vector3.up;
        HighlightText.alpha = alpha;
    }

    public void SetTutorialImage(Sprite sprite)
    {
        tutorialUI.sprite = sprite;
    }
    public void ActivateHintButton(Sprite sprite)
    {
        SetTutorialImage(sprite);
        CG_Tutorial.alpha = 1;
    }

    public void DisableHintButton()
    {
        CG_Tutorial.alpha = 0;
        DisableAndHideButton(closeButton);
        DisableAndHideButton(hintButton);
    }
    public void ShowTutorialOverlay()
    {
        tutorialUI.gameObject.SetActive(true);
        EnableAndShowButton(closeButton);
        DisableAndHideButton(hintButton);
        
    }
    public void HideTutorialOverlay()
    {
        tutorialUI.gameObject.SetActive(false);
        DisableAndHideButton(closeButton);
        EnableAndShowButton(hintButton);
    }
    public void EnableAndShowButton(Button button)
    {
        
        button.enabled = true;
        button.gameObject.SetActive(true);
    }
    public void DisableAndHideButton(Button button) 
    {
        button.enabled = false;
        button.gameObject.SetActive(false);
    }
    
}
