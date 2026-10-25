using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttendanceClientTest
{
    public partial class Form1 : Form
    {
        private TcpClient? _client;
        private NetworkStream? _stream;
        private StreamReader? _reader;
        private StreamWriter? _writer;

        public Form1()
        {
            InitializeComponent();

            btnConnect.Click += btnConnect_Click;
            btnDisconnect.Click += btnDisconnect_Click;
            btnCheckIn.Click += btnCheckIn_Click;

            btnDisconnect.Enabled = false;
            btnCheckIn.Enabled = false;

            FormClosing += Form1_FormClosing;

            Log("Client is not connected.");
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

        private async void btnConnect_Click(object? sender, EventArgs e)
        {
            btnConnect.Enabled = false;

            try
            {
                _client = new TcpClient();

                string serverIP = txtServerIP.Text.Trim();

                Log($"Connecting to {serverIP}:8888...");

                await _client.ConnectAsync(serverIP, 8888);

                _stream = _client.GetStream();

                _reader = new StreamReader(
                    _stream, Encoding.UTF8, false, 1024, true);

                _writer = new StreamWriter(
                    _stream, new UTF8Encoding(false), 1024, true)
                {
                    AutoFlush = true
                };

                Log("Connected to server successfully.");

                btnDisconnect.Enabled = true;
                btnCheckIn.Enabled = true;
            }
            catch (Exception ex)
            {
                Log($"Connection failed: {ex.Message}");

                CloseConnection();

                btnConnect.Enabled = true;
            }
        }

        private async void btnCheckIn_Click(object? sender, EventArgs e)
        {
            if (_client == null ||
                !_client.Connected ||
                _writer == null ||
                _reader == null)
            {
                Log("Please connect to the server first.");
                return;
            }

            btnCheckIn.Enabled = false;

            try
            {
                // Gửi lệnh CHECKIN đến server.
                await _writer.WriteLineAsync("CHECKIN");

                Log("Sent: CHECKIN");

                // Chờ phản hồi từ server.
                string? response = await _reader.ReadLineAsync();

                if (response == null)
                {
                    Log("Server disconnected.");

                    CloseConnection();
                    return;
                }

                Log($"Server response: {response}");
            }
            catch (IOException ex)
            {
                Log($"Communication error: {ex.Message}");
                CloseConnection();
            }
            catch (SocketException ex)
            {
                Log($"Socket error: {ex.Message}");
                CloseConnection();
            }
            catch (ObjectDisposedException)
            {
                Log("Connection is closed.");
                CloseConnection();
            }
            finally
            {
                btnCheckIn.Enabled =
                    _client != null && _client.Connected;
            }
        }

        private void btnDisconnect_Click(object? sender, EventArgs e)
        {
            CloseConnection();
            Log("Disconnected from server.");
        }

        private void CloseConnection()
        {
            try { _reader?.Dispose(); } catch { }
            try { _writer?.Dispose(); } catch { }
            try { _stream?.Dispose(); } catch { }
            try { _client?.Close(); } catch { }

            _reader = null;
            _writer = null;
            _stream = null;
            _client = null;

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnCheckIn.Enabled = false;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }
    }

}