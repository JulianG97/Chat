using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ProtocolCreator
    {
        private static Random random = new Random();
        private static object locker = new object();

        public static Protocol RegistrationOk()
        {
            return new Protocol(ProtocolTypes.RegistrationOk, new byte[0]);
        }

        public static Protocol RegistrationInvalid()
        {
            return new Protocol(ProtocolTypes.RegistrationInvalid, new byte[0]);
        }

        public static byte[] GenerateSessionKey()
        {
            byte[] sessionKey = new byte[32];

            int number1;
            int number2;

            for (int i = 0; i < 32; i++)
            {
                lock (locker)
                {
                    number1 = random.Next(1, 4);
                }

                // Character will be a letter
                if (number1 == 1)
                {
                    lock (locker)
                    {
                        number2 = random.Next(1, 3);
                    }

                    // Letter will be upper case
                    if (number2 == 1)
                    {
                        lock (locker)
                        {
                            sessionKey[i] = (byte)random.Next(65, 91);
                        }
                    }
                    // Letter will be lower case
                    else if (number2 == 2)
                    {
                        lock (locker)
                        {
                            sessionKey[i] = (byte)random.Next(97, 123);
                        }
                    }
                }
                // Character will be a number
                else if (number1 == 2)
                {
                    lock (locker)
                    {
                        sessionKey[i] = (byte)random.Next(48, 58);
                    }
                }
                // Character will be a special character
                else if (number1 == 3)
                {
                    lock (locker)
                    {
                        sessionKey[i] = (byte)random.Next(33, 39);
                    }
                }
            }

            return sessionKey;
        }

        public static Protocol LoginOk()
        {
            return new Protocol(ProtocolTypes.LoginOk, GenerateSessionKey());
        }

        public static Protocol LoginInvalid()
        {
            return new Protocol(ProtocolTypes.LoginInvalid, new byte[0]);
        }

        public static Protocol PublishMessage(string username, char userGroup, string message)
        {
            DateTime time = DateTime.Now;

            byte[] userMessage = Encoding.ASCII.GetBytes(username + "-" + userGroup + "-" + time.ToString("HH:mm") + "-" + message);
            return new Protocol(ProtocolTypes.PublishMessage, userMessage);
        }
    }
}
