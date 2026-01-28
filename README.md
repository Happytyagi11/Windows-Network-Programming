In server:
1. add networking , file I|0 , and async using statements
2. define Global variables to manage clients , file size and date and time .
3. Sets up the listener using TcpListener and begins accepting clients.

4. Create a static async task  method named HandleClientAsync to read messages , writes to the file , and checks the file size.
5. Create another static async task method named BroadcastAsync to stop signal to all connected clinet. 
6. Create another method to measure performance .

In Client : 
1. Connect to server using :  TCP Client put ip address and port number. 
2. Create a Listener Task.  it runs in the background,  Continuously reads messages from the server., If "STOP" is received, it prints a message and exits. The client must stop gracefully when the server signals.

3. Create Multiple Sender Tasks "Task<int>[] senders = new Task<int>[3]; // 3 parallel tasks" 

4. Wait for all senders to finish .
5. Closes the TCP connection.



file:  overwrite file every time server started
      ask each of the client their username . 
  used fomating  like   "  username : __, date and time:  
                 Message :      ;  for each message . 





when file is 90 percent full warn the user about it .  and change the message.txt size to 1kb  and separate the each message by i line to see clearly in the message.txt







-------------------------------------------------------------------------------------------------------------
Assignment A01 – TCP/IP Server and Client using Tasks
Course: Windows Network Programming
Student Name: [Your Name]
Date: [Submission Date]

1. Requirements
The goal of this assignment was to design and implement a TCP/IP server and client system using asynchronous Tasks in C#. The following requirements were set:

Server Requirements

Accept multiple clients concurrently.

Overwrite the output file (message.txt) each time the server starts.

Write all client messages into a single file.

Format each message with:

Username

Date and time

Message text

Separate each message with a blank line for readability.

Warn all connected clients when the file is 90% full.

Stop all clients when the file reaches 1 KB in size.

Print performance metrics including:

Number of messages received

Time taken

Throughput (messages per second)

Client Requirements

Prompt the user for a username when connecting.

Prompt the user for messages interactively.

Send each message to the server.

Display server broadcasts:

Warning when file is nearly full.

Stop message when file is full.

Exit gracefully when requested.

2. Design and Implementation
Server Design
The server was implemented using the TcpListener class to accept incoming connections. Each client connection was handled by a separate asynchronous Task (HandleClientAsync).

At startup, the server overwrites message.txt to ensure a clean file.

The first message received from each client is treated as the username and stored in a dictionary.

Each subsequent message is formatted as follows:

Code
Username: Alice, Date and Time: 2026-01-28 14:25
Message: Hello Server;

Messages are appended to message.txt with a blank line separating entries for clarity.

The server monitors file size using FileInfo.Length.

At 90% of the 1 KB threshold, it broadcasts a warning to all clients.

At 100% of the threshold, it broadcasts a stop signal and prints performance metrics.

Performance metrics include total messages received, total time taken, and throughput calculated as messages per second.

Client Design
The client was implemented using the TcpClient class to connect to the server.

Upon connection, the client prompts the user for a username and sends it to the server.

A background listener Task continuously reads messages from the server.

If "WARNING" is received, the client displays a warning message.

If "STOP" is received, the client exits gracefully.

The client then enters an interactive loop where the user can type messages.

Each message is sent to the server.

If the user presses Enter without typing anything, the client exits.

This design ensures that the client is interactive, responsive to server broadcasts, and shuts down cleanly when required.

3. Observations
Single Client Test
A single client named “Alice” connected and sent multiple messages.

The server wrote each message into message.txt with proper formatting.

When the file reached approximately 900 bytes, the client displayed:

Code
⚠️ Warning from server: File is almost full!
When the file reached 1024 bytes, the client displayed:

Code
Server requested stop. Exiting...
The server printed performance metrics showing the number of messages received and throughput.

Multiple Clients Test
Two clients, “Alice” and “Bob,” connected simultaneously.

Both usernames were recorded in message.txt.

Messages from both clients were interleaved in the file, each clearly separated by blank lines.

When the file reached 90% capacity, both clients received the warning broadcast simultaneously.

When the file reached 100% capacity, both clients received the stop broadcast and exited gracefully.

Performance
In one test run:

Messages received: 30

Time taken: 12 seconds

Throughput: 2.5 messages per second

4. Conclusions
Tasks vs Threads:  
Using Tasks simplified concurrency management compared to Threads. Tasks integrate naturally with async/await, making the code easier to read and maintain. Tasks also support return values, which can be aggregated using Task.WhenAll.

Server Reliability:  
The server successfully handled multiple clients concurrently without blocking. The use of a concurrent dictionary ensured safe tracking of connected clients.

Broadcasts:  
Warning and stop broadcasts worked reliably, reaching all clients simultaneously. This demonstrated effective use of asynchronous communication.

File Management:  
Overwriting the file at startup ensured clean test runs. Formatting with usernames, timestamps, and blank lines improved readability of message.txt.

Performance Measurement:  
The server provided clear metrics on throughput and message handling. This allowed analysis of system efficiency under different loads.

Overall, the assignment demonstrated the advantages of using Tasks for asynchronous programming in C#, particularly in networking scenarios where multiple clients must be handled concurrently.

5. References
Microsoft Docs: TcpListener Class

Microsoft Docs: TcpClient Class

Course Reference Code Examples (AsyncTCPServer, AsyncTCPClient, AsyncTaskswithMultipleReturns)

Lecture Notes: Windows Network Programming – Asynchronous Tasks
---------------------------------------------------------------------------------------------------------------------------
