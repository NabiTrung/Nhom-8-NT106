using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendanceServer
{
    public partial class Form1 : Form
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _serverCts;

        private readonly ConcurrentDictionary<TcpClient, byte> _clients = new();

        public Form1()
        {
            InitializeComponent();

            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;

            btnStop.Enabled = false;

            FormClosing += Form1_FormClosing;

            Log("Server is stopped.");
        }

        private void Log(string message)
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => Log(message)));
                return;
            }

            txtLog.AppendText(
                $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}"
            );
        }

        private async void btnStart_Click(object? sender, EventArgs e)
        {
            btnStart.Enabled = false;

            try
            {
                _serverCts = new CancellationTokenSource();

                _listener = new TcpListener(IPAddress.Any, 8888);
                _listener.Start();

                btnStop.Enabled = true;

                Log("Server started on port 8888.");
                Log("Waiting for clients...");

                await AcceptClientsAsync(_serverCts.Token);
            }
            catch (ObjectDisposedException)
            {
                // Listener đã bị đóng khi dừng server.
            }
            catch (SocketException ex)
            {
                Log($"Socket error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Log($"Server error: {ex.Message}");
            }
            finally
            {
                _listener?.Stop();
                _listener = null;

                _serverCts?.Dispose();
                _serverCts = null;

                btnStart.Enabled = true;
                btnStop.Enabled = false;

                Log("Server stopped.");
            }
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            TcpListener listener = _listener!;

            while (!token.IsCancellationRequested)
            {
                TcpClient client = await listener.AcceptTcpClientAsync(token);

                _clients.TryAdd(client, 0);

                string remoteIp =
                    client.Client.RemoteEndPoint?.ToString() ?? "Unknown";

                Log($"Client connected: {remoteIp}");

                // Mỗi client được xử lý độc lập.
                _ = Task.Run(() => HandleClientAsync(client, token));
            }
        }

        private async Task HandleClientAsync(
            TcpClient client,
            CancellationToken token)
        {
            string remoteIp =
                client.Client.RemoteEndPoint?.ToString() ?? "Unknown";

            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(
                    stream, Encoding.UTF8, false, 1024, true))
                using (StreamWriter writer = new StreamWriter(
                    stream, new UTF8Encoding(false), 1024, true)
                {
                    AutoFlush = true
                })
                {
                    while (!token.IsCancellationRequested)
                    {
                        string? message = await reader.ReadLineAsync(token);

                        if (message == null)
                            break;

                        Log($"Received from {remoteIp}: {message}");

                        string response;

                        if (message == "CHECKIN")
                        {
                            response = "CHECKIN_RECEIVED";
                        }
                        else
                        {
                            response = "UNKNOWN_COMMAND";
                        }

                        await writer.WriteLineAsync(response);

                        Log($"Sent to {remoteIp}: {response}");
                    }
                }
            }
            catch (IOException ex)
            {
                Log($"Connection error ({remoteIp}): {ex.Message}");
            }
            catch (SocketException ex)
            {
                Log($"Socket error ({remoteIp}): {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                // Server đang dừng.
            }
            catch (ObjectDisposedException)
            {
                // Kết nối đã được đóng.
            }
            finally
            {
                _clients.TryRemove(client, out _);
                Log($"Client disconnected: {remoteIp}");
            }
        }

        private void btnStop_Click(object? sender, EventArgs e)
        {
            btnStop.Enabled = false;

            _serverCts?.Cancel();
            _listener?.Stop();

            // Đóng các kết nối để giải phóng những client đang chờ đọc.
            foreach (TcpClient client in _clients.Keys)
            {
                client.Close();
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _serverCts?.Cancel();
            _listener?.Stop();

            foreach (TcpClient client in _clients.Keys)
            {
                client.Close();
            }
        }
    }
}