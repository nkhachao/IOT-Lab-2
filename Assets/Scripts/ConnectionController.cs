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
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        public bool autoTest = false;
        [Header("User Interface")]
        public TMP_InputField addressInputField;
        public TMP_InputField usernameInputField;
        public TMP_InputField passwordInputField;
        public Button connectButton;

        private List<string> eventMessages = new List<string>();
        public static List<PublishTask> PublishTasks = new List<PublishTask>();
        private bool updateUI = false;

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
        }

        public void Publish(PublishTask task)
        {
            client.Publish(task.topic, System.Text.Encoding.UTF8.GetBytes(task.message),
                MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("PUBLISHED: " + task.topic + ", " + task.message);
        }

        public void UpdateBrokerAddress(string _)
        {
            if (addressInputField && !updateUI)
            {
                this.brokerAddress = addressInputField.text;
            }
        }
        
        public void UpdateUsername(string _)
        {
            if (usernameInputField && !updateUI)
            {
                this.mqttUserName = usernameInputField.text;
            }
        }
        
        public void UpdatePassword(string _)
        {
            if (passwordInputField && !updateUI)
            {
                this.mqttPassword = passwordInputField.text;
            }
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
            Navigation.LoadScene(SceneNames.Dashboard);

            if (autoTest)
            {
                TestPublish();
            }
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
            client.Subscribe(new string[] { "M2MQTT_Unity/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        private void UpdateUI()
        {
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                }
            }
            else
            {
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }

            addressInputField.interactable = connectButton.interactable;
            addressInputField.text = brokerAddress;

            usernameInputField.interactable = connectButton.interactable;
            usernameInputField.text = mqttUserName;
            
            passwordInputField.interactable = connectButton.interactable;
            passwordInputField.text = mqttPassword;
            
            updateUI = false;
        }

        protected override void Start()
        {
            Debug.Log("Ready.");
            updateUI = true;
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
            if (topic == "M2MQTT_Unity/test")
            {
                if (autoTest)
                {
                    autoTest = false;
                    Disconnect();
                }
            }
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            DashboardController.ProcessMessage(msg);
            Debug.Log("Received: " + msg);
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            
            if (PublishTasks.Count > 0)
            {
                foreach (PublishTask publishTask in PublishTasks)
                {
                    Publish(publishTask);
                }
                PublishTasks.Clear();
            }
            
            if (updateUI)
            {
                UpdateUI();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            if (autoTest)
            {
                autoConnect = true;
            }
        }
    }

    public struct PublishTask
    {
        public string topic;
        public string message;
        
        public PublishTask(string topic, string message)
        {
            this.topic = topic;
            this.message = message;
        }
    }
}
