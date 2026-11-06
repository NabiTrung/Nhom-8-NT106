
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientNhanVien
{
    public sealed class SocketClient : IDisposable
    {
        private TcpClient? client;
        private NetworkStream? stream;
        private StreamReader? reader;
        private StreamWriter? writer;

        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private const int PORT = 8888;

        public async Task ConnectAsync(string ip)
        {
            if (client != null)
                throw new InvalidOperationException("Socket đã được khởi tạo.");

            TcpClient newClient = new TcpClient();

            try
            {
                using var timeout = new CancellationTokenSource(
                    TimeSpan.FromSeconds(10));

                await newClient.ConnectAsync(ip, PORT, timeout.Token);

                NetworkStream newStream = newClient.GetStream();

                StreamReader newReader = new StreamReader(
                    newStream, Encoding.UTF8, false, 1024, true);

                StreamWriter newWriter = new StreamWriter(
                    newStream, new UTF8Encoding(false), 1024, true);

                newWriter.AutoFlush = true;

                client = newClient;
                stream = newStream;
                reader = newReader;
                writer = newWriter;
            }
            catch
            {
                newClient.Dispose();
                throw;
            }
        }

        public async Task<string> SendAsync(string request)
        {
            await semaphore.WaitAsync();

            try
            {
                if (client == null || reader == null || writer == null)
                    throw new IOException("Chưa kết nối đến Server.");

                using var timeout = new CancellationTokenSource(
                    TimeSpan.FromSeconds(15));

                await writer.WriteLineAsync(
                    request.AsMemory(), timeout.Token);

                string? response = await reader.ReadLineAsync(
                    timeout.Token);

                if (response == null)
                    throw new IOException("Server đã ngắt kết nối.");

                return response;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static string Encode(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        public static string Decode(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }

        public void Disconnect()
        {
            writer?.Dispose();
            reader?.Dispose();
            stream?.Dispose();
            client?.Dispose();

            writer = null;
            reader = null;
            stream = null;
            client = null;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
