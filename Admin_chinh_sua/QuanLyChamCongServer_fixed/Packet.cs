namespace QuanLyChamCongServer
{
    public class Packet
    {
        public string Command { get; set; }
        public string Data { get; set; }

        public Packet(string command, string data)
        {
            Command = command;
            Data = data;
        }

        public string ToMessage()
        {
            return Command + "|" + Data;
        }

        public static Packet FromMessage(string message)
        {
            string[] parts = message.Split(
                new char[] { '|' }, 2);

            if (parts.Length != 2)
                return null;

            return new Packet(parts[0], parts[1]);
        }
    }
}