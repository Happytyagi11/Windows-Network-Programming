/*
 * File: Server.cs
 * Author: [Your Name]
 * Course: Windows Network Programming
 * Assignment: A01
 * Description: TCP/IP server using Tasks. Receives data from multiple clients,
 *              writes to message.txt with username, timestamp, and message.
 *              Warns clients when file is 90% full, stops at 1 KB.
 */

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /*
     * Class: Server
     * Description: Represents the TCP/IP server application. Accepts multiple clients,
     *              processes their messages, writes to a shared file, and manages warnings/stops.
     */
    class Server
    {
        // Dictionary to track connected clients and their usernames
        private static readonly ConcurrentDictionary<TcpClient, string> Clients = new();

        // File path and size threshold
        private static string filePath = "message.txt";
        private static long maxFileSize = 1024 * 1; // 1 KB threshold

        // Performance tracking
        private static DateTime startTime;
        private static int messageCount = 0;

        // Flag to ensure warning is only sent once 
        private static bool warningSent = false;

        /*
         * Method: Main
         * Description: Entry point for the server application. Initializes the listener,
         *              overwrites the file, and accepts incoming client connections.
         * Parameters: None
         * Returns: Task (async execution)
         */
        public static async Task Main()
        {
            // Overwrite file at startup
            File.WriteAllText(filePath, string.Empty);

            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Server started on port 5000");

            startTime = DateTime.Now;

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Task task = HandleClientAsync(client);
            }
        }

        /*
         * Method: HandleClientAsync
         * Description: Handles communication with a single client. Reads username,
         *              processes messages, writes them to file, and manages warnings/stops.
         * Parameters: TcpClient client - the connected client
         * Returns: Task (async execution)
         */
        private static async Task HandleClientAsync(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[2048];

            // First message from client is the username
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string username = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
            Clients.TryAdd(client, username);
            Console.WriteLine($"Client '{username}' connected");

            try
            {
                while (true)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                    string formattedMessage =
                        $"Username: {username}, Date and Time: {DateTime.Now}\nMessage: {message};\n\n";
                    // Extra blank line for clarity

                    File.AppendAllText(filePath, formattedMessage);
                    messageCount++;

                    FileInfo fi = new FileInfo(filePath);



                    // Warn clients when file is 90% full
                    if (fi.Length >= (maxFileSize * 0.9) && fi.Length < maxFileSize)
                    {
                        Console.WriteLine("Warning: File is 90% full.");
                        await BroadcastAsync("WARNING: File is almost full!");
                        warningSent = true; // ensure warning only sent once
                    }

                    // Stop clients when file reaches max size
                    if (fi.Length >= maxFileSize)
                    {
                        Console.WriteLine("Threshold reached. Stopping clients...");
                        await BroadcastAsync("STOP");
                        PrintPerformance();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {username}: {ex.Message}");
            }
            finally
            {
                Clients.TryRemove(client, out _);
                client.Close();
                Console.WriteLine($"Client '{username}' disconnected");
            }
            return;
        }

        /*
         * Method: BroadcastAsync
         * Description: Sends a message to all connected clients.
         * Parameters: string message - the message to broadcast
         * Returns: Task (async execution)
         */
        private static async Task BroadcastAsync(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var kvp in Clients)
            {
                try
                {
                    NetworkStream stream = kvp.Key.GetStream();
                    await stream.WriteAsync(data, 0, data.Length);
                }
                catch { }
            }
        }

        /*
         * Method: PrintPerformance
         * Description: Prints performance metrics including total messages received,
         *              time taken, and throughput.
         * Parameters: None
         * Returns: void
         */
        private static void PrintPerformance()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;
            Console.WriteLine($"Performance Report:");
            Console.WriteLine($"Messages received: {messageCount}");
            Console.WriteLine($"Time taken: {duration.TotalSeconds} seconds");
            Console.WriteLine($"Throughput: {messageCount / duration.TotalSeconds} msgs/sec");
        }
    }
}
