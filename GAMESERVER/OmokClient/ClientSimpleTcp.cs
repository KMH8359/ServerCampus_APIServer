using System;
using System.Net.Sockets;
using System.Net;

namespace csharp_test_client
{
    public class ClientSimpleTcp
    {
        public Socket Sock = null;   
        public string LatestErrorMsg;
        

        //소켓연결        
        public bool Connect(string ip, int port)
        {
            try
            {
                IPAddress serverIP = IPAddress.Parse(ip);
                int serverPort = port;

                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Sock.Connect(new IPEndPoint(serverIP, serverPort));

                if (Sock == null || Sock.Connected == false)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LatestErrorMsg = ex.Message;
                return false;
            }
        }

        public Tuple<int,byte[]> Receive()
        {

            try
            {
                byte[] ReadBuffer = new byte[2048];
                var nRecv = Sock.Receive(ReadBuffer, 0, ReadBuffer.Length, SocketFlags.None);

                if (nRecv == 0)
                {
                    return null;
                }

                return Tuple.Create(nRecv,ReadBuffer);
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.Message;
            }

            return null;
        }

        public void Send(byte[] sendData)
        {
            try
            {
                if (Sock != null && Sock.Connected) 
                {
                    Sock.Send(sendData, 0, sendData.Length, SocketFlags.None);
                }
                else
                {
                    LatestErrorMsg = "먼저 채팅서버에 접속하세요!";
                }
            }
            catch (SocketException se)
            {
                LatestErrorMsg = se.Message;
            }
        }

        public void Close()
        {
            if (Sock != null && Sock.Connected)
            {
                Sock.Close();
            }
        }

        public bool IsConnected() { return (Sock != null && Sock.Connected) ? true : false; }
    }
}
