# Chat
A simple C# chat for the console

## Protocol
```
|  CHAT  |   TP   |  Content   |
| Header |  Type  |  Content   |
| 4 byte | 2 byte | 0 - n byte |
```

### Protocol Types Client-Server

| Type | Full Name | Value | Length | Description |
| ---- | --------- | ----- | ------ | ----------- |
| RR | Registration Request | Username-Password | 9 - 22 (+ 6) | Registration message sent from the client to the server (Username Length: 3 - 10 byte; Password Length: 6 - 12 byte) |
| RO | Registration OK | - | 0 (+ 6) | Registration OK message sent from the server to the client |
| RI | Registration Invalid | - | 0 (+ 6) | Registration failed message sent from the server to the client |
| LR | Login Request | Username-Password | 9 - 22 (+ 6) | Login message sent from the client to the server (Username Length: 3 - 10 byte; Password Length: 6 - 12 byte) |
| LO | Login OK | Sessionkey | 32 (+ 6) | Login OK message with session key sent from the server to the client |
| LI | Login Invalid | - | 0 (+ 6) | Login failed message sent from the server to the client |
| ME | Message | Username-Message-Sessionkey | 36 - 296 (+ 6) | Message forwarded from the output window to the server (Username Length: 3 - 10 byte; Message Length: 1 - 254 byte) |

### Protocol Types InputWindow-OutputWindow

| Type | Full Name | Value | Length | Description |
| ---- | --------- | ----- | ------ | ----------- |
| SD | Session Data | Username-Sessionkey | 35 - 42 (+ 6) | Session data sent from the output window to the input window (Username Length: 3 - 10 byte) |
| ME | Message | Username-Message-Sessionkey | 36 - 296 (+ 6) | Message written by the user sent from the input window to the output window (Username Length: 3 - 10 byte; Message Length: 1 - 254 byte) |
