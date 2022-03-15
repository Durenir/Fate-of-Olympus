using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerName, dialogue, navButtonText;
    public Image speakerSprite;

    private static DialogueManager instance;
    private Conversation currentConvo;
    private int currentIndex;
    private Animator anim;
    private Coroutine typing;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            anim = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartConversation(Conversation convo)
    {
        instance.currentIndex = 0;
        instance.anim.SetBool("isOpen", true);
        instance.currentConvo = convo;
        instance.speakerName.text = "";
        instance.dialogue.text = "";
        instance.navButtonText.text = ">";

        instance.ReadNext();
    }

    public void ReadNext()
    {
        if(currentIndex > currentConvo.GetLength())
        {
            instance.anim.SetBool("isOpen", false);
            return;
        }
        speakerName.text = currentConvo.GetLineByIndex(currentIndex).speaker.GetName();
        if(typing == null)
        {
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));

        }
        else
        {
            instance.StopCoroutine(typing);
            typing = null;
            typing = instance.StartCoroutine(TypeText(currentConvo.GetLineByIndex(currentIndex).dialogue));
        }
        speakerSprite.sprite = currentConvo.GetLineByIndex(currentIndex).speaker.GetSprite();
        currentIndex++;
    }

    private IEnumerator TypeText(string text)
    {
        dialogue.text = "";
        bool complete = false;
        int index = 0;
        while (!complete)
        {
            dialogue.text += text[index];
            index++;
            yield return new WaitForSeconds(0.02f);

            if (index == text.Length)
                complete = true;
        }
        typing = null;
    }
}
