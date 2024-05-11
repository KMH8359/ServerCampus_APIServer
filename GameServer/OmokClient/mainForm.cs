using CSCommon;
using MemoryPack;
using System;
using System.Collections.Generic;   
using System.Text;
using System.Collections.Concurrent;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;

#pragma warning disable CA1416

namespace csharp_test_client
{
    [SupportedOSPlatform("windows10.0.177630")]
    public partial class mainForm : Form
    {
        ClientSimpleTcp Network = new ClientSimpleTcp();

        bool IsNetworkThreadRunning = false;
        bool IsBackGroundProcessRunning = false;

        System.Threading.Thread NetworkReadThread = null;
        System.Threading.Thread NetworkSendThread = null;

        PacketBufferManager PacketBuffer = new PacketBufferManager();
        ConcurrentQueue<byte[]> RecvPacketQueue = new ConcurrentQueue<byte[]>();
        ConcurrentQueue<byte[]> SendPacketQueue = new ConcurrentQueue<byte[]>();

        System.Windows.Forms.Timer dispatcherUITimer;



        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            PacketBuffer.Init((8096 * 10), MemoryPackPacketHeaderInfo.HeadSize, 2048);

            IsNetworkThreadRunning = true;
            NetworkReadThread = new System.Threading.Thread(this.NetworkReadProcess);
            NetworkReadThread.Start();
            NetworkSendThread = new System.Threading.Thread(this.NetworkSendProcess);
            NetworkSendThread.Start();

            IsBackGroundProcessRunning = true;
            dispatcherUITimer = new System.Windows.Forms.Timer();
            dispatcherUITimer.Tick += new EventHandler(BackGroundProcess);
            dispatcherUITimer.Interval = 100;
            dispatcherUITimer.Start();

            btnDisconnect.Enabled = false;

            SetPacketHandler();


            Omok_Init();
            DevLog.Write("프로그램 시작 !!!", LOG_LEVEL.INFO);
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsNetworkThreadRunning = false;
            IsBackGroundProcessRunning = false;

            Network.Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string address = textBoxIP.Text;

            if (checkBoxLocalHostIP.Checked)
            {
                address = "127.0.0.1";
            }

            int port = Convert.ToInt32(textBoxPort.Text);

            if (Network.Connect(address, port))
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 중", DateTime.Now);
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;

                DevLog.Write($"서버에 접속 중", LOG_LEVEL.INFO);
            }
            else
            {
                labelStatus.Text = string.Format("{0}. 서버에 접속 실패", DateTime.Now);
            }

            PacketBuffer.Clear();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            HandleDisconnect();
            Network.Close();
        }

        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            await SendHttpRequestCreateAccount(sender, e);
        }

        private async void btnLoginHiveServer_Click(object sender, EventArgs e)
        {
            await SendHttpRequestLoginHiveServer(sender, e);
        }

        private async void btnLoginApiServer_Click(object sender, EventArgs e)
        {
            await SendHttpRequestLoginApiServer(sender, e);
        }
        private async Task SendHttpRequestCreateAccount(object sender, EventArgs e)
        {
            try
            {
                string ApiServerURL = textBoxHiveIP.Text;
                string id = textBoxHiveUserID.Text;
                string pw = textBoxHiveUserPW.Text;

                HttpClient client = new HttpClient();

                string url = $"http://{ApiServerURL}:5256/CreateAccount";

                string jsonContent = $"{{ \"Id\": \"{id}\", \"Password\": \"{pw}\" }}";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var createAccountResponse = JsonConvert.DeserializeObject<APIErrorCode>(responseString);
                    if (createAccountResponse == APIErrorCode.None) 
                    {
                        DevLog.Write($"계정 생성 성공:  {textBoxHiveUserID.Text}, {textBoxHiveUserPW.Text}");
                    }
                    else
                    {
                        DevLog.Write($"계정 생성 실패:  {createAccountResponse}");
                    }
                    
                }
                else
                {
                    DevLog.Write($"계정 생성 실패: {response.ReasonPhrase} ");
                }
            }
            catch (Exception ex)
            {
                DevLog.Write($"오류 발생: {ex.Message}");
            }
        }

        private async Task SendHttpRequestLoginHiveServer(object sender, EventArgs e)
        {
            try
            {
                string ApiServerURL = textBoxHiveIP.Text;
                string id = textBoxHiveUserID.Text;
                string pw = textBoxHiveUserPW.Text;

                HttpClient client = new HttpClient();

                string url = $"http://{ApiServerURL}:5256/Login";

                string jsonContent = $"{{ \"Id\": \"{id}\", \"Password\": \"{pw}\" }}";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginHiveServerResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                    if (loginHiveServerResponse.Result == APIErrorCode.None)
                    {
                        textBoxApiUserAuthToken.Text = loginHiveServerResponse.AuthToken;
                        DevLog.Write($"Hive 서버 로그인 성공:  {textBoxHiveUserID.Text}, {textBoxHiveUserPW.Text}, 인증 토큰 - {loginHiveServerResponse.AuthToken}");
                    }
                    else
                    {
                        DevLog.Write($"계정 생성 실패:  {loginHiveServerResponse.Result}");
                    }

                }
                else
                {
                    DevLog.Write($"계정 생성 실패: {response.ReasonPhrase} ");
                }
            }
            catch (Exception ex)
            {
                DevLog.Write($"오류 발생: {ex.Message}");
            }
        }

        private async Task SendHttpRequestLoginApiServer(object sender, EventArgs e)
        {
            try
            {
                string ApiServerURL = textBoxApiIP.Text;
                string id = textBoxApiUserID.Text;
                string pw = textBoxApiUserAuthToken.Text;

                HttpClient client = new HttpClient();

                string url = $"http://{ApiServerURL}:6525/Login";

                string jsonContent = $"{{ \"Id\": \"{id}\", \"AuthToken\": \"{pw}\" }}";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginApiServerResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                    if (loginApiServerResponse.Result == APIErrorCode.None)
                    {
                        DevLog.Write($"Api 서버 로그인 성공:  {textBoxApiUserID.Text}, {textBoxApiUserAuthToken.Text}");
                    }
                    else
                    {
                        DevLog.Write($"계정 생성 실패:  {loginApiServerResponse.Result}");
                    }

                }
                else
                {
                    DevLog.Write($"계정 생성 실패: {response.ReasonPhrase} ");
                }
            }
            catch (Exception ex)
            {
                DevLog.Write($"오류 발생: {ex.Message}");
            }
        }

        private void RespondToHeartbeat()
        {
            var responsePkt = new PKTResponse();

            var sendPacketData = MemoryPackSerializer.Serialize(responsePkt);

            PostSendPacket(PacketID.HEART_BEAT, sendPacketData);
        }
        

        void NetworkReadProcess()
        {
            while (IsNetworkThreadRunning)
            {
                if (Network.IsConnected() == false)
                {
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                var recvData = Network.Receive();

                if (recvData != null)
                {
                    PacketBuffer.Write(recvData.Item2, 0, recvData.Item1);

                    while (true)
                    {
                        var data = PacketBuffer.Read();
                        if (data == null)
                        {
                            break;
                        }

                        RecvPacketQueue.Enqueue(data);
                    }
                }
                else
                {
                    Network.Close();
                    HandleDisconnect();
                    DevLog.Write("서버와 접속 종료 !!!", LOG_LEVEL.INFO);
                }
            }
        }

        void NetworkSendProcess()
        {
            while (IsNetworkThreadRunning)
            {
                System.Threading.Thread.Sleep(1);

                if (Network.IsConnected() == false)
                {
                    continue;
                }

                
                if (SendPacketQueue.TryDequeue(out var packet)) // if packet is not NULL
                {
                    Network.Send(packet);
                }
            }
        }


        void BackGroundProcess(object sender, EventArgs e)
        {
            ProcessLog();

            try
            {
                byte[] packet = null;

                if(RecvPacketQueue.TryDequeue(out packet)) // if packet is not NULL
                {
                    PacketProcess(packet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("BackGroundProcess. error:{0}", ex.Message));
            }
        }

        private void ProcessLog()
        {
            // 너무 이 작업만 할 수 없으므로 일정 작업 이상을 하면 일단 패스한다.
            int logWorkCount = 0;

            while (IsBackGroundProcessRunning)
            {
                System.Threading.Thread.Sleep(1);

                string msg;

                if (DevLog.GetLog(out msg))
                {
                    ++logWorkCount;

                    if (listBoxLog.Items.Count > 512)
                    {
                        listBoxLog.Items.Clear();
                    }

                    listBoxLog.Items.Add(msg);
                    listBoxLog.SelectedIndex = listBoxLog.Items.Count - 1;
                }
                else
                {
                    break;
                }

                if (logWorkCount > 8)
                {
                    break;
                }
            }
        }


        public void HandleDisconnect()
        {
            if (btnConnect.Enabled == false)
            {
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }

            while (true)
            {
                if (SendPacketQueue.TryDequeue(out var temp) == false)
                {
                    break;
                }
            }

            listBoxRoomChatMsg.Items.Clear();
            listBoxRoomUserList.Items.Clear();
            OmokLogic.ClearBoard();
            승리플레이어Name = "";
            EndGame("");

            labelStatus.Text = "서버 접속이 끊어짐";
        }

        void PostSendPacket(UInt16 packetID, byte[] packetData)
        {
            if (Network.IsConnected() == false)
            {
                DevLog.Write("서버 연결이 되어 있지 않습니다", LOG_LEVEL.ERROR);
                return;
            }

            var header = new MemoryPackPacketHeaderInfo();
            header.Id = packetID;
            header.Type = 0;

            if (packetData != null)
            {
                header.TotalSize = (UInt16)packetData.Length;
                
                header.Write(packetData);
            }
            else
            {
                packetData = header.Write(); // 헤더만 있는 빈 패킷
            }

            SendPacketQueue.Enqueue(packetData);
        }

        
        void AddRoomUserList(string userID)
        {
            listBoxRoomUserList.Items.Add(userID);
        }

        void ClearRoomUserList()
        {
            listBoxRoomUserList.Items.Clear();
        }

        void RemoveRoomUserList(string userID)
        {
            object removeItem = null;

            foreach( var user in listBoxRoomUserList.Items)
            {
                if ((string)user == userID)
                {
                    removeItem = user;
                    break;
                }
            }

            if (removeItem != null)
            {
                listBoxRoomUserList.Items.Remove(removeItem);
            }
        }

        string GetOtherPlayers(string myName)
        {
            if(listBoxRoomUserList.Items.Count != 2)
            {
                return null;
            }

            var firstName = (string)listBoxRoomUserList.Items[0];
            if (firstName != myName)
            {
                return firstName;
            }
            else 
            {
                return (string)listBoxRoomUserList.Items[1];
            }
        }


        static public string ToReadableByteArray(byte[] bytes)
        {
            return string.Join(", ", bytes);
        }

        // 로그인 요청
        private void btn_Login_Click(object sender, EventArgs e)
        {
            var loginReq = new PKTReqLogin();
            loginReq.UserID = textBoxUserID.Text;
            loginReq.AuthToken = textBoxUserPW.Text;

            var packet = MemoryPackSerializer.Serialize(loginReq);
                        
            PostSendPacket(PacketID.REQ_LOGIN, packet);            
            DevLog.Write($"로그인 요청:  {textBoxUserID.Text}, {textBoxUserPW.Text}");
            DevLog.Write($"로그인 요청: {ToReadableByteArray(packet)}");
        }

        private void btn_RoomEnter_Click(object sender, EventArgs e)
        {
            var requestPkt = new PKTReqRoomEnter();
            requestPkt.RoomNumber = textBoxRoomNumber.Text.ToInt32();

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

            PostSendPacket(PacketID.REQ_ROOM_ENTER, sendPacketData);
            DevLog.Write($"방 입장 요청:  {textBoxRoomNumber.Text} 번");
        }

        private void btn_RoomLeave_Click(object sender, EventArgs e)
        {
            PostSendPacket(PacketID.REQ_ROOM_LEAVE, new byte[MemoryPackPacketHeaderInfo.HeadSize]);
            DevLog.Write($"방 퇴장 요청:  {textBoxRoomNumber.Text} 번");
        }

        private void btn_RoomChat_Click(object sender, EventArgs e)
        {
            if(textBoxRoomSendMsg.Text.IsEmpty())
            {
                MessageBox.Show("채팅 메시지를 입력하세요");
                return;
            }

            var requestPkt = new PKTReqRoomChat();
            requestPkt.ChatMessage = textBoxRoomSendMsg.Text;

            var sendPacketData = MemoryPackSerializer.Serialize(requestPkt);

            PostSendPacket(PacketID.REQ_ROOM_CHAT, sendPacketData);
            DevLog.Write($"방 채팅 요청");
        }

        private void btn_Matching_Click(object sender, EventArgs e)
        {
            DevLog.Write($"매칭 요청 - 미구현");
        }

        
        private void listBoxRoomChatMsg_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // 게임 시작 요청
        private void btn_Ready_Click(object sender, EventArgs e)
        {
            PostSendPacket(PacketID.REQ_READY_OMOK, new byte[MemoryPackPacketHeaderInfo.HeadSize]);

            DevLog.Write($"게임 준비 완료 요청");
        }

        void SendPacketOmokPut(int x, int y)
        {
            var requestPkt = new PKTReqPutMok
            {
                PosX = x,
                PosY = y
            };

            var packet = MemoryPackSerializer.Serialize(requestPkt);
            PostSendPacket(PacketID.REQ_PUT_MOK, packet);

            DevLog.Write($"put stone 요청 : x  [ {x} ], y: [ {y} ] ");
        }

        private void btn_GameStart_Click(object sender, EventArgs e)
        {
            //PostSendPacket(PACKET_ID.GAME_START_REQ, null);
            //StartGame(true, "My", "Other");
        }

        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            // AddUser("test1");
            // AddUser("test2");
        }

        void AddUser(string userID)
        {
            // var value = new PvPMatchingResult
            // {
            //     IP = "127.0.0.1",
            //     Port = 32451,
            //     RoomNumber = 0,
            //     Index = 1,
            //     Token = "123qwe"
            // };
            // var saveValue = MessagePackSerializer.Serialize(value);

            // var key = "ret_matching_" + userID;

            // var redisConfig = new CloudStructures.RedisConfig("omok", "127.0.0.1");
            // var RedisConnection = new CloudStructures.RedisConnection(redisConfig);

            // var v = new CloudStructures.Structures.RedisString<byte[]>(RedisConnection, key, null);
            // var ret = v.SetAsync(saveValue).Result;
        }

        // [MessagePackObject]
        // public class PvPMatchingResult
        // {
        //     [Key(0)]
        //     public string IP;
        //     [Key(1)]
        //     public UInt16 Port;
        //     [Key(2)]
        //     public Int32 RoomNumber;
        //     [Key(3)]
        //     public Int32 Index;
        //     [Key(4)]
        //     public string Token;
        // }
    }
}
