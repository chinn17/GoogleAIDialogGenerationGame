using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   
    public int maxMessages = 25;
    public string userName;
    
    public GameObject chatPanel, textObject;
    public TMP_InputField chatBox;
    public Color playerMessage, info;
   // public SocketListener socket;
    public Client client;
   // public MyNetworkManager network;
    public Button pay;
    [SerializeField] List<Message> messageList = new List<Message>();
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(userName + ": " + chatBox.text);//, Message.MessageType.playerMessage);
                chatBox.text = "";
//                network.SetupLocalClient();
            }

        }
        else
        {
            if (!chatBox.isFocused && (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter)))
            {
                chatBox.ActivateInputField();
            }

        }

       
    }

    public void SendMessageToChat(string text)//, Message.MessageType messageType)
    {
        if (messageList.Count >= 25)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
//        newMessage.textObject.color = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }
    
    public void SendMessageToChat(TMP_InputField temp)
    {
       
        if (messageList.Count >= 25)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = this.userName+": " + temp.text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
//        newMessage.textObject.color = MessageTypeColor(messageType);
        messageList.Add(newMessage);
    }
    
}
[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
    public MessageType messageType;
    
    public enum MessageType
    {
        playerMessage,
        info
    }
}
 