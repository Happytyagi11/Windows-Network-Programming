Assignment A01 
Requirements:  
message.txt Requirements: 
1. Connect to multiple client at the same time. 
2. Overwrite the output file (message.txt) every time the server starts. 
3. Write all the client messages into samefile (message.txt) 
4. Format for each message in the file:        Username:  -----         Date and time:--------- 
                                                Message: ------------------------------------------ 
                                                -------------------------------------------------------------- 
Separate each message with a black line for readability. 
5.stop all client when the file reaches 1kb in size. 
6. measure and print performance metrics on the server side : Number of messages 
received, time spend,   messages per second. 
 
Client Requirements: 
1. Prompt the user for a username when connecting. 
2. Ask the user to inter the messages he wants to send 
3. When file is full 90 %  display a warning : file is nearly full. 
4. Stop message when file is full 
5. Exit gracefully when requested.  
 
• Using TcpClinet class to connect to the server. 
• A background listener task continuously read messages form the server. 
• Client then enters an interactive loop where user can type messages and and 
send to server. 
• If user press enter without typing anything, then client exits. 
• If warning is received displays it . 
• It stop is received the client exit gracefully. 
 
Server Requirements:  
1. Using  TcpListener class to accept incoming connections. 
2. Handle each client connection by separate asynchronous task “HandleClientAsycn) 
3. At start , server overwrites message.txt to clean file.. 
4. First message received from each client Is username and stored in dictionary. 
And username is displayed to the server. 
5. Format each subsequent message as mentioned and wrote each message into 
message.txt. 
6. Monitors fie size using “FileInfo.Length. 
7. At 90% of the 1kb send a warning to all client. 
8. At 100% of the threshold, broadcast a stop signal and print performance metrics.
