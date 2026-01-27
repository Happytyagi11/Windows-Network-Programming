# Windows-Network-Programming
Please  fetch or pull the code before push anything 

Requirements:
1.	a. Make only 1 server. 
b.  Run client on the same computer and on the other computer.
    2. Check if there is a  file named “Local File.txt” if does not exit create one.  All the data  coming from all the client should be stored in this file. 

3.	We have to define the size of “Local File.txt”  when file is about to full it should send the alert to client  “ file is about to full”  then when file is fulled  a  messeage is send to client and local tasks should be stopped peacefully.  
4.	Search how can be improve the performance .  for instance:   
 stop the  program( Server or Client )  after certain time if there is nothing to connect).  
   Note the name of the client and time of the messeage into the “ Local File.txt” 
5.	Use threading for optimize the proformance of the program. 




A1 -Tasks
The application must run on multiple computers. 
a. One computer will act as the server 
b. At least one other computer will act as a client and can be the source of multiple 
client threads 
2. The server must receive data from a client, and write it to a local file. The single file 
must contain all communications from the various clients. 
3. When the file reaches a certain size, it must notify the clients so they can stop sending 
data. It must then stop all local tasks. These stops must be graceful. 
4. Experiment with the performance of your solution. You must come up with your own 
metrics and one of them MUST be some form of valid time measurement. 
5. You will need to discuss the solution with your team and come up with additional 
detailed requirements. Make sure you validate your own requirements with the 
instructor during the labs that lead up to assignment submission. 
6. In a separate PDF document called A01Report.pdf, you must include: 
a. The full list of your requirements 
b. Observations made while measuring performance 
c. Your Conclusions based on your Observations
