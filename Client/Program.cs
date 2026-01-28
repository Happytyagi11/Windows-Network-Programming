/*
 * File: Client.cs
 * Author: [Your Name]
 * Course: Windows Network Programming
 * Assignment: A01
 * Description: Interactive TCP/IP client. Prompts user for username and messages,
 *              sends them to the Server. Displays warnings and stop signals from Server.
 */

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    /*
     * Class: Client
     * Description: Represents the TCP/IP client application. Connects to the server,
     *              sends user messages, and listens for server broadcasts.
     */
    class Client
    {
        /*
         * Method: Main
         * Description: Entry point for the client application. Connects to the server,
         *              prompts user for username and messages, and handles server responses.
         * Parameters: None
         * Returns: Task (async execution)
         */
        public static async Task Main()
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);
            Console.WriteLine("Connected to Server");

            NetworkStream stream = client.GetStream();

            // Ask for username
            Console.Write("Enter your username: ");
            string? username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username)) username = "Anonymous";

            byte[] userData = Encoding.UTF8.GetBytes(username);
            await stream.WriteAsync(userData, 0, userData.Length);

            /*
             * Task: listener
             * Description: Background task that continuously listens for messages from the server.
             *              Displays warnings when file is nearly full and exits when STOP is received.
             */
            Task listener = Task.Run(async () =>
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (response.Contains("STOP"))
                    {
                        Console.WriteLine("Server requested stop. Exiting...");
                        break;
                    }
                    else if (response.Contains("WARNING"))
                    {
                        Console.WriteLine("## Warning from server: File is almost full!##");
                    }
                }
            });

            /*
             * Loop: Interactive message input
             * Description: Prompts the user to enter messages. Sends each message to the server.
             *              Exits when user enters a blank line.
             */
            bool done = false;
            while (!done)
            {
                Console.Write("Enter message (blank to quit): ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    done = true;
                }
                else
                {
                    byte[] data = Encoding.UTF8.GetBytes(input);
                    await stream.WriteAsync(data, 0, data.Length);
                }
            }

            client.Close();
            return;
        }
    }
}
