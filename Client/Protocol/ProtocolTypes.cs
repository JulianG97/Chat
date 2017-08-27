using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public static class ProtocolTypes
    {
        /// <summary>
        /// The Registration Request Protocol Type.
        /// <para>Value: Username-Password</para>
        /// <para>Length: 9 - 22 (+ 6)</para>
        /// <para>Description: Registration message sent from the client to the server.</para>
        /// </summary>
        public static byte[] RegistrationRequest = { 82, 82 };

        /// <summary>
        /// The Registration OK Protocol Type.
        /// <para>Value: -</para>
        /// <para>Length: 0 (+ 6)</para>
        /// <para>Description: Registration OK message sent from the server to the client.</para>
        /// </summary>
        public static byte[] RegistrationOk = { 82, 79 };

        /// <summary>
        /// The Registration Invalid Protocol Type.
        /// <para>Value: -</para>
        /// <para>Length: 0 (+ 6)</para>
        /// <para>Description: Registration failed message sent from the server to the client.</para>
        /// </summary>
        public static byte[] RegistrationInvalid = { 82, 73 };

        /// <summary>
        /// The Login Request Protocol Type.
        /// <para>Value: Username-Password</para>
        /// <para>Length: 9 - 22 (+ 6)</para>
        /// <para>Description: Login message sent from the client to the server.</para>
        /// </summary>
        public static byte[] LoginRequest = { 76, 82 };

        /// <summary>
        /// The Login OK Protocol Type.
        /// <para>Value: Sessionkey</para>
        /// <para>Length: 32 (+ 6)</para>
        /// <para>Description: Login OK message with session key sent from the server to the client.</para>
        /// </summary>
        public static byte[] LoginOk = { 76, 79 };

        /// <summary>
        /// The Login Invalid Protocol Type.
        /// <para>Value: -</para>
        /// <para>Length: 0 (+ 6)</para>
        /// <para>Description: Login failed message sent from the server to the client.</para>
        /// </summary>
        public static byte[] LoginInvalid = { 76, 73 };

        /// <summary>
        /// The Session Data Protocol Type.
        /// <para>Value: Username-Sessionkey</para>
        /// <para>Length: 35 - 42 (+ 6)</para>
        /// <para>Description: Session data sent from the output window to the input window.</para>
        /// </summary>
        public static byte[] SessionData = { 83, 68 };
    }
}