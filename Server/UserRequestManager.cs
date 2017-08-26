using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    public class UserRequestManager
    {
        public void HandleUserRequest(string userRequest, TcpClient client, string userPath)
        {
            char[] splittedUserRequest = userRequest.ToCharArray();

            if (splittedUserRequest[0] == 'C' && splittedUserRequest[1] == 'H' && splittedUserRequest[2] == 'A' && splittedUserRequest[3] == 'T')
            {
                string username = string.Empty;
                string password = string.Empty;

                for (int i = 6; i < splittedUserRequest.Length; i++)
                {
                    if (splittedUserRequest[i] == '-')
                    {
                        for (int j = i + 1; j < splittedUserRequest.Length; j++)
                        {
                            password = password + splittedUserRequest[j];
                        }

                        break;
                    }
                    else
                    {
                        username = username + splittedUserRequest[i];
                    }
                }

                if (splittedUserRequest[4] == 'R' && splittedUserRequest[5] == 'R')
                {
                    CreateNewUser(username, password, client, userPath);
                }
                else if (splittedUserRequest[4] == 'L' && splittedUserRequest[5] == 'R')
                {
                    // Login Request
                }
            }
        }

        public static void CreateNewUser(string username, string password, TcpClient client, string userPath)
        {
            string path = userPath + @"\" + username + ".txt";

            if (File.Exists(path) == true)
            {
                Protocol registrationInvalid = ProtocolCreator.RegistrationInvalid();

                NetworkManager.SendMessage(registrationInvalid, client);
            }
            else
            {
                if (CheckIfLegalUserData(username, password) == false)
                {
                    Protocol registrationInvalid = ProtocolCreator.RegistrationInvalid();

                    NetworkManager.SendMessage(registrationInvalid, client);
                }
                else
                {
                    string[] userFile = new string[3];

                    userFile[0] = "Username: " + username;
                    userFile[1] = "Password: " + password;
                    userFile[2] = "User Group: User";

                    File.WriteAllLines(path, userFile);

                    Protocol registrationOk = ProtocolCreator.RegistrationOk();

                    NetworkManager.SendMessage(registrationOk, client);
                }
            }
        }

        public static bool CheckIfLegalUserData(string username, string password)
        {
            char[] splittedUsername = username.ToCharArray();
            char[] splittedPassword = password.ToCharArray();

            // Checks the length of the username and password
            if (CheckIfLegalString(username, 3, 10) == false)
            {
                return false;
            }
            else if (CheckIfLegalString(password, 6, 12) == false)
            {
                return false;
            }

            // Checks the characters of the username and password
            for (int i = 0; i < splittedUsername.Length; i++)
            {
                if (CheckIfLegalCharacter(splittedUsername[i]) == false)
                {
                    return false;
                }
            }

            for (int i = 0; i < splittedPassword.Length; i++)
            {
                if (CheckIfLegalCharacter(splittedPassword[i]) == false)
                {
                    return false;
                }
            }

            return true;
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
    }
}
