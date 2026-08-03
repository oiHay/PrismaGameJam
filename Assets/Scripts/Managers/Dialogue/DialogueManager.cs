using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Suspeito atual")]
    public SuspectSO currentSuspect;

    [Header("UI - Opções")]
    public GameObject optionsPanel;
    public Transform optionButtonContainer;
    public Button optionButtonPrefab;

    [Header("UI - Conversa")]
    public GameObject conversationPanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;
    public Image portraitImage;
    public Button conversationClickArea; // botão invisível cobrindo a tela

    [Header("UI - Suspeitos")]
    public GameObject suspectsParent;

    [Header("Configuração")]
    public float charactersPerSecond = 40f;

    private DialogueOptionSO currentOption;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    private bool isTyping;

    void Start()
    {
        //Debug.Log("DialogueManager iniciou");
        conversationClickArea.onClick.AddListener(OnConversationClicked);
        //ShowOptions();
    }

    // ---------- TELA DE OPÇÕES ----------
    //Mostra as opções de dialogo
    //Destroi e recria toda vez, já atualizado com novas opçoes (se houverem)
    public void ShowOptions()
    {
        suspectsParent.SetActive(false);
        conversationPanel.SetActive(false);
        optionsPanel.SetActive(true);

        foreach (Transform child in optionButtonContainer)
            Destroy(child.gameObject);

        List<DialogueOptionSO> available = currentSuspect.GetAvailableOptions();

        foreach (var option in available)
        {
            Button btn = Instantiate(optionButtonPrefab, optionButtonContainer);
            btn.gameObject.SetActive(true);
            btn.GetComponentInChildren<TMP_Text>().text = option.displayText;

            DialogueOptionSO capturedOption = option;
            btn.onClick.AddListener(() => StartConversation(capturedOption));
        }
    }

    // ---------- CONVERSA ----------

    void StartConversation(DialogueOptionSO option)
    {
        currentOption = option;
        currentLineIndex = 0;

        optionsPanel.SetActive(false);
        conversationPanel.SetActive(true);

        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        DialogueLineSO line = currentOption.conversation[currentLineIndex];
        speakerNameText.text = line.speakerName;
        portraitImage.sprite = currentSuspect.GetSprite(line.emotion);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        float delay = 1f / charactersPerSecond;

        foreach (char c in fullText)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
    }

    void OnConversationClicked()
    {
        if (isTyping)
        {
            // completa o texto na hora
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentOption.conversation[currentLineIndex].text;
            isTyping = false;
        }
        else
        {
            AdvanceDialogue();
        }
    }

    void AdvanceDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex >= currentOption.conversation.Length)
        {
            EndConversation();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    void EndConversation()
    {
        foreach (string flag in currentOption.flagsToSetOnComplete)
        {
            InvestigationState.Instance.SetFlag(flag);
        }

        ShowOptions();
    }

    public void SetSuspect(SuspectSO suspect)
    {
        currentSuspect = suspect;
        ShowOptions();
    }

    public void PresentItem(ItemSO item)
    {
        DialogueOptionSO reaction = currentSuspect.GetReactionForItem(item);

        if (reaction == null)
        {
            Debug.LogWarning($"{currentSuspect.suspectName} não tem genericItemDialogue configurado!");
            return;
        }

        StartConversation(reaction);
    }

    public void ReturnToSuspects()
    {   
        optionsPanel.SetActive(false);
        conversationPanel.SetActive(false);
        suspectsParent.SetActive(true);
    }
}