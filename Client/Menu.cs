using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Client
{
    public class Menu
    {
        private static Settings settings = new Settings();

        public static void DisplayMainMenu()
        {
            string[] menuItems = { "Login", "Register", "Exit" };

            bool exit = false;
            int menuPosition = 0;

            Console.CursorVisible = false;

            while (exit == false)
            {
                ShowHeader();

                Console.WriteLine();
                Console.WriteLine();

                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == menuPosition)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine("                 " + menuItems[i].ToUpper());

                    Console.WriteLine();

                    Console.ResetColor();
                }

                ConsoleKeyInfo cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.UpArrow)
                {
                    if (menuPosition - 1 < 0)
                    {
                        menuPosition = menuItems.Length - 1;
                    }
                    else
                    {
                        menuPosition--;
                    }
                }
                else if (cki.Key == ConsoleKey.DownArrow)
                {
                    if (menuPosition + 1 > menuItems.Length - 1)
                    {
                        menuPosition = 0;
                    }
                    else
                    {
                        menuPosition++;
                    }
                }
                else if (cki.Key == ConsoleKey.Enter)
                {
                    if (menuPosition == 0)
                    {
                        DisplayLoginMenu();
                    }
                    else if (menuPosition == 1)
                    {
                        DisplayRegisterMenu();
                    }
                    else if (menuPosition == 2)
                    {
                        exit = true;
                    }
                }

                Console.Clear();
            }
        }

        public static void DisplayLoginMenu()
        {
            Console.Clear();

            ShowHeader();

            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("[!] Enter your user information below to login or press [ESC] to go back!");

            Console.WriteLine();

            Console.WriteLine("             " + "Username: ");

            Console.WriteLine();

            Console.WriteLine("             " + "Password: ");

            try
            {
                bool exit = false;

                Console.SetCursorPosition(23, 9);

                Console.CursorVisible = true;

                string username = GetStringWithASpecificLength(10, 23, 9, out exit, true);

                if (exit == false)
                {
                    if (CheckIfLegalString(username, 3, 10) == false)
                    {
                        throw new ArgumentException("The username must not be empty and between 3 and 10 characters!");
                    }

                    Console.CursorVisible = false;

                    Console.SetCursorPosition(23, 11);

                    Console.CursorVisible = true;

                    string password = GetStringWithASpecificLength(12, 23, 11, out exit, false);

                    if (exit == false)
                    {
                        if (CheckIfLegalString(password, 6, 12) == false)
                        {
                            throw new ArgumentException("The password must not be empty and between 6 and 12 characters!");
                        }

                        settings.CheckConfigFile();

                        IPEndPoint server = new IPEndPoint(settings.ServerIP, settings.ServerPort);

                        TcpClient client = new TcpClient();

                        NetworkManager.Connect(server, client);

                        NetworkStream stream = client.GetStream();

                        Protocol loginRequest = ProtocolCreator.LoginRequest(username, password);

                        NetworkManager.SendMessage(loginRequest, stream);

                        string loginResponse = NetworkManager.ReadMessage(stream, 38);

                        if (loginResponse == "CHATLI")
                        {
                            throw new ArgumentException("The username or the password is invalid!");
                        }
                        else
                        {
                            char[] loginResponseArray = loginResponse.ToCharArray();

                            if (loginResponseArray[0] == 'C' && loginResponseArray[1] == 'H' && loginResponseArray[2] == 'A' && loginResponseArray[3] == 'T' && loginResponseArray[4] == 'L' && loginResponseArray[5] == 'O')
                            {
                                string sessionKey = string.Empty;

                                for (int i = 6; i < loginResponseArray.Length; i++)
                                {
                                    sessionKey = sessionKey + loginResponseArray[i];
                                }

                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine("You have logged in successfully!");
                                Console.WriteLine("Your session key is: {0}", sessionKey);
                                Console.WriteLine();
                                Console.WriteLine("Press any key to continue!");

                                Console.CursorVisible = false;

                                Console.ReadKey(true);

                                Process.Start("Client.exe", "\\OpenInputWindow");

                                OutputWindow outputWindow = new OutputWindow();
                                outputWindow.Start(username, sessionKey);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.SetCursorPosition(0, 12);

                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("Press any key to continue!");

                Console.CursorVisible = false;

                Console.ReadKey(true);

                DisplayLoginMenu();
            }

            Console.CursorVisible = false;

            Console.ResetColor();
        }

        public static void DisplayRegisterMenu()
        {
            Console.Clear();

            ShowHeader();

            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("[!] Enter a username and a password below to register or press [ESC] to go back!");
            Console.WriteLine("    If you have problems registering, press [F12] to open the help!");

            Console.WriteLine();

            Console.WriteLine("             " + "Username: ");

            Console.WriteLine();

            Console.WriteLine("             " + "Password: ");

            Console.WriteLine();
            Console.WriteLine("             " + "Confirm Password: ");

            Console.SetCursorPosition(23, 10);

            Console.CursorVisible = true;

            try
            {
                bool exit = false;

                string username = GetStringWithASpecificLength(10, 23, 10, out exit, true);

                if (exit == false)
                {
                    if (CheckIfLegalString(username, 3, 10) == false)
                    {
                        throw new ArgumentException("The username must not be empty and between 3 and 10 characters!");
                    }

                    Console.CursorVisible = false;

                    Console.SetCursorPosition(23, 12);

                    Console.CursorVisible = true;

                    string password = GetStringWithASpecificLength(12, 23, 12, out exit, false);

                    if (exit == false)
                    {
                        if (CheckIfLegalString(password, 6, 12) == false)
                        {
                            throw new ArgumentException("The password must not be empty and between 6 and 12 characters!");
                        }

                        Console.CursorVisible = false;

                        Console.SetCursorPosition(31, 14);

                        Console.CursorVisible = true;

                        string confirmPassword = GetStringWithASpecificLength(12, 31, 14, out exit, false);

                        if (exit == false)
                        {
                            if (CheckIfLegalString(confirmPassword, 6, 12) == false)
                            {
                                throw new ArgumentException("The confirmation password must not be empty and between 6 and 12 characters!");
                            }
                            else if (password != confirmPassword)
                            {
                                throw new ArgumentException("The password must match the confirmation password!");
                            }

                            settings.CheckConfigFile();

                            IPEndPoint server = new IPEndPoint(settings.ServerIP, settings.ServerPort);

                            TcpClient client = new TcpClient();

                            NetworkManager.Connect(server, client);

                            NetworkStream stream = client.GetStream();

                            Protocol registrationRequest = ProtocolCreator.RegistrationRequest(username, password);

                            NetworkManager.SendMessage(registrationRequest, stream);

                            string registrationResponse = NetworkManager.ReadMessage(stream, 6);

                            if (registrationResponse == "CHATRI")
                            {
                                throw new ArgumentException("The username already exists! Please try another one!");
                            }
                            else if (registrationResponse == "CHATRO")
                            {
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine("The registration was successful! You can login now!");
                                Console.WriteLine();
                                Console.WriteLine("Press any key to continue!");

                                Console.CursorVisible = false;

                                Console.ReadKey(true);
                            }
                        }
                    }                 
                }
            }
            catch (Exception e)
            {
                Console.SetCursorPosition(0, 15);

                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.WriteLine("Press any key to continue!");

                Console.CursorVisible = false;

                Console.ReadKey(true);

                DisplayRegisterMenu();
            }

            Console.CursorVisible = false;

            Console.ResetColor();
        }

        public static string GetStringWithASpecificLength(int maxLength, int cursorPosX, int cursorPosY, out bool exit, bool inputVisible)
        {
            string input = string.Empty;

            exit = false;

            int initCursorPosX = cursorPosX;

            ConsoleKey lastKey = ConsoleKey.A;

            for (int i = 0; lastKey != ConsoleKey.Enter; i++)
            {
                ConsoleKeyInfo cki = new ConsoleKeyInfo();

                lastKey = cki.Key;

                if (input.Length < maxLength)
                {
                    Console.CursorVisible = true;
                }
                else
                {
                    Console.CursorVisible = false;
                }

                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (cki.Key == ConsoleKey.Escape)
                {
                    exit = true;
                    break;
                }
                else if (cki.Key == ConsoleKey.Backspace && i > 0 && input.Length > 0)
                {
                    i -= 2;
                    cursorPosX--;

                    Console.SetCursorPosition(cursorPosX, cursorPosY);

                    Console.Write(" ");

                    Console.SetCursorPosition(cursorPosX, cursorPosY);

                    char[] splittedInput = input.ToCharArray();

                    char[] newInput = new char[splittedInput.Length - 1];

                    for (int z = 0; z < splittedInput.Length - 1; z++)
                    {
                        newInput[z] = splittedInput[z];
                    }

                    input = new string(newInput);
                }
                else if (cki.Key == ConsoleKey.Backspace && cursorPosX == initCursorPosX)
                {
                    Console.SetCursorPosition(initCursorPosX, cursorPosY);
                    i--;
                }
                else if (cki.Key != ConsoleKey.Backspace && input.Length < maxLength)
                {
                    if (CheckIfLegalCharacter(cki.KeyChar) == true)
                    {
                        if (inputVisible == true)
                        {
                            Console.Write(cki.KeyChar);
                        }
                        else
                        {
                            Console.Write('X');
                        }

                        input += cki.KeyChar;
                        cursorPosX++;
                    }
                }
            }

            return input;
        }

        public static bool CheckIfLegalCharacter(char character)
        {
            int charNumber = (int)character;

            // a-z
            if (charNumber >= 97 && charNumber <= 122)
            {
                return true;
            }
            // A-Z
            else if (charNumber >= 65 && charNumber <= 90)
            {
                return true;
            }
            // 0-9
            else if (charNumber >= 48 && charNumber <= 57)
            {
                return true;
            }
            // ü, Ü, ä, Ä, ö, Ö
            else if (charNumber == 252 || charNumber == 220 || charNumber == 228 || charNumber == 196 || charNumber == 246 || charNumber == 214)
            {
                return true;
            }
            // #, !, ?, $, %, &, ~, @
            else if (charNumber == 35 || charNumber == 33 || charNumber == 63 || charNumber == 36 || charNumber == 37 || charNumber == 38 || charNumber == 126 || charNumber == 64)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfLegalString(string text, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text) == true)
            {
                return false;
            }
            else if (text.Length < minLength || text.Length > maxLength)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ShowHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(" ______     __  __     ______     ______");
            Console.WriteLine("/\\  ___\\   /\\ \\_\\ \\   /\\  __ \\   /\\__  _\\");
            Console.WriteLine("\\ \\ \\____  \\ \\  __ \\  \\ \\  __ \\  \\/_/\\ \\/");
            Console.WriteLine(" \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\ \\_\\    \\ \\_\\");
            Console.WriteLine("  \\/_____/   \\/_/\\/_/   \\/_/\\/_/     \\/_/");

            Console.ResetColor();
        }
    }
}