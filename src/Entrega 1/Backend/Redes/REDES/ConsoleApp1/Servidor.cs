using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace SmartHomeSimulator
{
    class Server
    {
        private static TcpListener tcpServer;
        private static bool fim = false;

        static void Main(string[] args)
        {
            string myIP = "127.0.0.1";
            int port = 12345;
            tcpServer = new TcpListener(IPAddress.Parse(myIP), port);
            tcpServer.Start();
            Console.WriteLine($"Servidor escutando em {myIP}:{port}");

            Thread serverThread = new Thread(ServerListener);
            serverThread.Start();

            Console.WriteLine("Pressione [ENTER] para terminar o servidor...");
            Console.ReadLine();
            fim = true;
            tcpServer.Stop();
        }

        static void ServerListener()
        {
            while (!fim)
            {
                try
                {
                    TcpClient client = tcpServer.AcceptTcpClient();
                    Console.WriteLine($"Conexão recebida de {client.Client.RemoteEndPoint}");
                    Thread clientHandler = new Thread(() => HandleClient(client));
                    clientHandler.Start();
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    // Servidor sendo parado
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao aceitar cliente: {ex.Message}");
                }
            }
            Console.WriteLine("Servidor finalizado.");
        }

        static void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Servidor recebeu de {client.Client.RemoteEndPoint}: {message}");

                    // Simulação de processamento de comandos
                    if (message.StartsWith("LIGAR") || message.StartsWith("DESLIGAR"))
                    {
                        Console.WriteLine($"Servidor processou comando: {message}");
                        byte[] responseBytes = Encoding.UTF8.GetBytes($"OK: {message}");
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    else if (message.StartsWith("{")) // Assumindo que dados de sensores são JSON
                    {
                        try
                        {
                            JsonDocument.Parse(message); // Tenta parsear como JSON para validar
                            Console.WriteLine($"Servidor recebeu dados de sensor: {message}");
                        }
                        catch (JsonException)
                        {
                            byte[] responseBytes = Encoding.UTF8.GetBytes("Erro: Formato de dados inválido.");
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                    else
                    {
                        byte[] responseBytes = Encoding.UTF8.GetBytes("Comando Desconhecido.");
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao lidar com o cliente {client.Client.RemoteEndPoint}: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine($"Conexão com {client.Client.RemoteEndPoint} encerrada.");
            }
        }
    }
}