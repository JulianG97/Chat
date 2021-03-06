﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public class InputWindow
    {
        private TcpListener listener = new TcpListener(IPAddress.Any, 90);
        private TcpClient outputWindowClient;
        private string username;
        private string sessionKey;

        public void Start()
        {
            Console.Title = "Enter a message";

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WindowHeight = 3;
            Console.WindowWidth = 120;

            ConnectToOutputWindow();

            while (true)
            {
                EnterMessage();
            }
        }

        public void ConnectToOutputWindow()
        {
            listener.Start();

            outputWindowClient = listener.AcceptTcpClient();

            GetSessionData(outputWindowClient);

            while (true)
            {
                EnterMessage();
            }
        }

        public void GetSessionData(TcpClient outputWindowClient)
        {
            string sessionDataProtocol = NetworkManager.ReadMessage(outputWindowClient, 48);

            char[] sessionDataArray = sessionDataProtocol.ToCharArray();

            if (sessionDataArray[0] == 'C' && sessionDataArray[1] == 'H' && sessionDataArray[2] == 'A' && sessionDataArray[3] == 'T' && sessionDataArray[4] == 'S' && sessionDataArray[5] == 'D')
            {
                string sessionDataString = string.Empty;

                for (int i = 6; i < sessionDataArray.Length; i++)
                {
                    sessionDataString = sessionDataString + sessionDataArray[i];
                }

                string[] sessionData = sessionDataString.Split('-');

                if (sessionData.Length == 2)
                {
                    this.username = sessionData[0];
                    this.sessionKey = sessionData[1];
                }
            }
        }

        public void EnterMessage()
        {
            Console.Clear();

            Console.Write(">> ");

            string message = Console.ReadLine();

            Protocol userMessage = ProtocolCreator.Message(this.username, message, this.sessionKey);
            NetworkManager.SendMessage(userMessage, outputWindowClient);
        }
    }
}
