using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using TMPro;


namespace TS1989
{
    public class ConnectionController : M2MqttUnityClient
    {
        [Header("User Interface")]
        public TMP_InputField addressInputField;
        public TMP_InputField usernameInputField;
        public TMP_InputField passwordInputField;
        public Button connectButton;

        private List<Message> receivedMessages = new List<Message>();
        public static List<Message> ToBePublished = new List<Message>();

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
        }

        public void Publish(Message message)
        {
            client.Publish(message.topic, System.Text.Encoding.UTF8.GetBytes(message.message),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("PUBLISHED: " + message.topic + ", " + message.message);
        }

        public void UpdateBrokerAddress(string address)
        {
            this.brokerAddress = address;
        }
        
        public void UpdateUsername(string username)
        {
            this.mqttUserName = username;
        }
        
        public void UpdatePassword(string password)
        {
            this.mqttPassword = password;
        }

        public new void Connect()
        {
            HourglassController.Status =
                "Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "\n"
                + "Username: " + mqttUserName + "\nPassword: " + new string('*', mqttPassword.Length);
            Navigation.LoadScene(SceneNames.Connecting);
            base.Connect();
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + ", "
                      + "Username: " + mqttUserName + ", Password: " + mqttPassword);
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected to broker on " + brokerAddress + "\n");
            SubscribeTopics();
            Navigation.LoadScene(SceneNames.Dashboard);
        }
        
        protected override void OnConnectionFailed(string errorMessage)
        {
            ErrorController.Title = "Connection Failed";
            ErrorController.ErrorMessage = errorMessage;
            Navigation.LoadScene(SceneNames.Error);
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }
        
        protected override void OnConnectionLost()
        {
            ErrorController.Title = "Connection Lost";
            ErrorController.ErrorMessage = "Unexpectedly lost connection to " + brokerAddress;
            Navigation.LoadScene(SceneNames.Error);
            Debug.Log("CONNECTION LOST!");
        }

        protected override void SubscribeTopics()
        {
            if (brokerAddress == "mqttserver.tk")
            {
                client.Subscribe(new string[] { "/bkiot/1852346/status" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
            else
            {
                client.Subscribe(new string[] { "testTopic" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            }
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
        }

        protected override void OnDisconnected()
        {
            ErrorController.Title = "Disconnected";
            ErrorController.ErrorMessage = "You have been disconnected from " + brokerAddress;
            Navigation.LoadScene(SceneNames.Error);
            Debug.Log("Disconnected.");
        }

        protected override void Start()
        {
            Debug.Log("Ready.");
            base.Start();

            addressInputField.text = brokerAddress;
            usernameInputField.text = mqttUserName;
            passwordInputField.text = mqttPassword;
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg + ", topic: " + topic);
            receivedMessages.Add(new Message(topic, msg));
        }

        private void ProcessMessage(Message msg)
        {
            DashboardController.ProcessMessage(msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (receivedMessages.Count > 0)
            {
                foreach (Message msg in receivedMessages)
                {
                    ProcessMessage(msg);
                }
                receivedMessages.Clear();
            }
            
            if (ToBePublished.Count > 0)
            {
                foreach (Message message in ToBePublished)
                {
                    Publish(message);
                }
                ToBePublished.Clear();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
        }
    }

    public struct Message
    {
        public string topic;
        public string message;
        
        public Message(string topic, string message)
        {
            this.topic = topic;
            this.message = message;
        }
    }
}
