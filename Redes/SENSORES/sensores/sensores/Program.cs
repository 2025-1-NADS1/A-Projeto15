using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace SmartHomeDevices
{
    class DeviceSimulator
    {
        private static string serverIP = "127.0.0.1";
        private static int serverPort = 12345;
        private static Random random = new Random();
        private static bool running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("ESCOLHA O DISPOSITIVO:");
            Console.WriteLine("1 - Quarto 1 (Sensor)");
            Console.WriteLine("2 - Quarto 2 (Sensor)");
            Console.WriteLine("3 - Sala (Sensor)");
            Console.WriteLine("4 - Cozinha (Sensor)");
            Console.WriteLine("5 - Piscina (Sensor)");
            Console.WriteLine("6 - Controlador");
            Console.WriteLine("7 - Sair");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SimulateSensorInteractively(1);
                    break;
                case "2":
                    SimulateSensorInteractively(2);
                    break;
                case "3":
                    SimulateSensorInteractively(3);
                    break;
                case "4":
                    SimulateSensorInteractively(4);
                    break;
                case "5":
                    SimulateSensorInteractively(5);
                    break;
                case "6":
                    SimulateControllerInteractively();
                    break;
                case "7":
                    running = false;
                    Console.WriteLine("Saindo...");
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.WriteLine("Pressione [ENTER] para sair.");
            Console.ReadLine();
            running = false; // Garante que qualquer thread pendente termine (melhoria seria usar um token de cancelamento)
        }

        static void SimulateSensorInteractively(int sensorId)
        {
            string[] sensorNames = { "", "Quarto 1", "Quarto 2", "Sala", "Cozinha", "Piscina" };
            try
            {
                using (TcpClient client = new TcpClient(serverIP, serverPort))
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine($"Simulando Sensor {sensorId} ({sensorNames[sensorId]}) - Pressione ENTER para enviar dados, 'sair' para encerrar.");
                    while (running)
                    {
                        string? input = Console.ReadLine()?.ToLower();
                        if (input == "sair")
                            break;

                        var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                        double temperatura = Math.Round(random.Next(15, 36) + random.NextDouble(), 1);
                        int umidade = random.Next(30, 91);
                        int movimento = random.Next(0, 2);

                        var sensorData = new
                        {
                            TimeStamp = timestamp,
                            ID_Sensor = sensorId,
                            Temperatura = temperatura,
                            Umidade = umidade,
                            Movimento = movimento
                        };

                        string jsonString = JsonSerializer.Serialize(sensorData);
                        byte[] data = Encoding.UTF8.GetBytes(jsonString + Environment.NewLine);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine($"Sensor {sensorId} enviou: {jsonString}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Sensor {sensorId}: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"Sensor {sensorId} desconectado.");
            }
        }

        static void SimulateControllerInteractively()
        {
            try
            {
                using (TcpClient client = new TcpClient(serverIP, serverPort))
                using (NetworkStream stream = client.GetStream())
                {
                    Console.WriteLine("Simulando Controlador - Digite um comando (ex: LIGAR Sala, DESLIGAR Quarto 1) ou 'sair':");
                    while (running)
                    {
                        Console.Write("Comando: ");
                        string? command = Console.ReadLine()?.ToUpper();
                        if (command?.ToLower() == "sair")
                            break;

                        if (!string.IsNullOrEmpty(command))
                        {
                            byte[] data = Encoding.UTF8.GetBytes(command + Environment.NewLine);
                            stream.Write(data, 0, data.Length);
                            byte[] responseBuffer = new byte[1024];
                            int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead).Trim();
                            Console.WriteLine($"Servidor respondeu: {response}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no Controlador: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Controlador desconectado.");
            }
        }
    }
}