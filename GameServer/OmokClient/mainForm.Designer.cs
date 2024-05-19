namespace csharp_test_client
{
    partial class mainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.btnLoginHiveServer = new System.Windows.Forms.Button();
            this.btnLoginApiServer = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBoxAPI = new System.Windows.Forms.GroupBox();
            this.gameServerPortNumber = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.gameServerIP = new System.Windows.Forms.Label();
            this.textBoxHiveIP = new System.Windows.Forms.TextBox();
            this.textBoxApiIP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHiveUserID = new System.Windows.Forms.TextBox();
            this.textBoxHiveUserPW = new System.Windows.Forms.TextBox();
            this.textBoxApiUserID = new System.Windows.Forms.TextBox();
            this.textBoxApiUserAuthToken = new System.Windows.Forms.TextBox();
            this.gameUserID = new System.Windows.Forms.Label();
            this.gameUserAuthToken = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.Room = new System.Windows.Forms.GroupBox();
            this.ReadyButton = new System.Windows.Forms.Button();
            // this.btnMatching = new System.Windows.Forms.Button();
            // this.GameStartBtn = new System.Windows.Forms.Button();
            this.btnRoomChat = new System.Windows.Forms.Button();
            this.textBoxRoomSendMsg = new System.Windows.Forms.TextBox();
            this.listBoxRoomChatMsg = new System.Windows.Forms.ListBox();
            this.listBoxRoomUserList = new System.Windows.Forms.ListBox();
            //this.btn_RoomLeave = new System.Windows.Forms.Button();
            //this.btn_RoomEnter = new System.Windows.Forms.Button();
            this.RoomNumber = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            //this.button1 = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.groupBoxAPI.SuspendLayout();
            this.Room.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDisconnect
            // 
            //this.btnDisconnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.btnDisconnect.Location = new System.Drawing.Point(410, 44);
            //this.btnDisconnect.Name = "btnDisconnect";
            //this.btnDisconnect.Size = new System.Drawing.Size(88, 26);
            //this.btnDisconnect.TabIndex = 29;
            //this.btnDisconnect.Text = "접속 끊기";
            //this.btnDisconnect.UseVisualStyleBackColor = true;
            //this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            //this.btnConnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.btnConnect.Location = new System.Drawing.Point(410, 16);
            //this.btnConnect.Name = "btnConnect";
            //this.btnConnect.Size = new System.Drawing.Size(88, 26);
            //this.btnConnect.TabIndex = 28;
            //this.btnConnect.Text = "접속하기";
            //this.btnConnect.UseVisualStyleBackColor = true;
            //this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCreateAccount.Location = new System.Drawing.Point(410, 5);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(90, 20);
            this.btnCreateAccount.TabIndex = 28;
            this.btnCreateAccount.Text = "계정생성";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // btnLoginHiveServer
            // 
            this.btnLoginHiveServer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoginHiveServer.Location = new System.Drawing.Point(410, 30);
            this.btnLoginHiveServer.Name = "btnLoginAPIServer";
            this.btnLoginHiveServer.Size = new System.Drawing.Size(90, 20);
            this.btnLoginHiveServer.TabIndex = 28;
            this.btnLoginHiveServer.Text = "HIVE로그인";
            this.btnLoginHiveServer.UseVisualStyleBackColor = true;
            this.btnLoginHiveServer.Click += new System.EventHandler(this.btnLoginHiveServer_Click);
            // 
            // btnLoginApiServer
            // 
            this.btnLoginApiServer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoginApiServer.Location = new System.Drawing.Point(410, 55);
            this.btnLoginApiServer.Name = "btnLoginGameAPIServer";
            this.btnLoginApiServer.Size = new System.Drawing.Size(90, 20);
            this.btnLoginApiServer.TabIndex = 28;
            this.btnLoginApiServer.Text = "API로그인";
            this.btnLoginApiServer.UseVisualStyleBackColor = true;
            this.btnLoginApiServer.Click += new System.EventHandler(this.btnLoginApiServer_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gameServerPortNumber);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.gameServerIP);
            this.groupBox5.Controls.Add(this.label9);
            //this.groupBox5.Controls.Add(this.btnConnect);
            //this.groupBox5.Controls.Add(this.btnDisconnect);
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Controls.Add(this.gameUserAuthToken);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.gameUserID);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Location = new System.Drawing.Point(12, 100);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(503, 100);
            this.groupBox5.TabIndex = 27;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "게임 서버 로그인";
            // 
            // groupBoxAPI
            // 
            this.groupBoxAPI.Controls.Add(this.label11);
            this.groupBoxAPI.Controls.Add(this.label12);
            this.groupBoxAPI.Controls.Add(this.textBoxApiIP);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveIP);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveUserID);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveUserPW);
            this.groupBoxAPI.Controls.Add(this.textBoxApiUserID);
            this.groupBoxAPI.Controls.Add(this.textBoxApiUserAuthToken);
            this.groupBoxAPI.Controls.Add(this.label4);
            this.groupBoxAPI.Controls.Add(this.label5);
            this.groupBoxAPI.Controls.Add(this.label6);
            this.groupBoxAPI.Controls.Add(this.label7);
            this.groupBoxAPI.Controls.Add(this.btnCreateAccount);
            this.groupBoxAPI.Controls.Add(this.btnLoginHiveServer);
            this.groupBoxAPI.Controls.Add(this.btnLoginApiServer);
            this.groupBoxAPI.Location = new System.Drawing.Point(12, 5);
            this.groupBoxAPI.Name = "groupBoxAPI";
            this.groupBoxAPI.Size = new System.Drawing.Size(503, 80);
            this.groupBoxAPI.TabIndex = 27;
            this.groupBoxAPI.TabStop = false;
            this.groupBoxAPI.Text = "API 서버 로그인";
            // 
            // gameServerPortNumber
            // 
            this.gameServerPortNumber.Location = new System.Drawing.Point(225, 22);
            //this.gameServerPortNumber.MaxLength = 6;
            this.gameServerPortNumber.Name = "gameServerPortNumber";
            this.gameServerPortNumber.Size = new System.Drawing.Size(51, 21);
            this.gameServerPortNumber.TabIndex = 18;
            this.gameServerPortNumber.Text = "";
            //this.gameServerPortNumber.WordWrap = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(163, 24);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "포트 번호:";
            // 
            // gameServerIP
            // 
            this.gameServerIP.Location = new System.Drawing.Point(68, 22);
            //this.gameServerIP.MaxLength = 20;
            this.gameServerIP.Name = "gameServerIP";
            this.gameServerIP.Size = new System.Drawing.Size(87, 21);
            this.gameServerIP.TabIndex = 11;
            this.gameServerIP.Text = "";
            //this.gameServerIP.WordWrap = false;
            // 
            // textBoxHiveIP
            // 
            this.textBoxHiveIP.Location = new System.Drawing.Point(70, 20);
            this.textBoxHiveIP.MaxLength = 20;
            this.textBoxHiveIP.Name = "textBoxHiveIP";
            this.textBoxHiveIP.Size = new System.Drawing.Size(87, 21);
            this.textBoxHiveIP.TabIndex = 11;
            this.textBoxHiveIP.Text = "127.0.0.1";
            this.textBoxHiveIP.WordWrap = false;
            // 
            // textBoxApiIP
            // 
            this.textBoxApiIP.Location = new System.Drawing.Point(70, 47);
            this.textBoxApiIP.MaxLength = 20;
            this.textBoxApiIP.Name = "textBoxApiIP";
            this.textBoxApiIP.Size = new System.Drawing.Size(87, 21);
            this.textBoxApiIP.TabIndex = 11;
            this.textBoxApiIP.Text = "127.0.0.1";
            this.textBoxApiIP.WordWrap = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "서버 주소:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 12);
            this.label11.TabIndex = 210;
            this.label11.Text = "Hive 주소:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "API 주소:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(18, 645);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(111, 12);
            this.labelStatus.TabIndex = 40;
            this.labelStatus.Text = "서버 접속 상태: ???";
            // 
            // listBoxLog
            // 
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.ItemHeight = 12;
            this.listBoxLog.Location = new System.Drawing.Point(12, 488);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(496, 148);
            this.listBoxLog.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 12);
            this.label1.TabIndex = 42;
            this.label1.Text = "UserID:";
            // 
            // gameUserID
            // 
            this.gameUserID.Location = new System.Drawing.Point(62, 70);
            //this.gameUserID.MaxLength = 15;
            this.gameUserID.Name = "gameUserID";
            this.gameUserID.Size = new System.Drawing.Size(87, 21);
            this.gameUserID.TabIndex = 43;
            this.gameUserID.Text = "";
            //this.gameUserID.WordWrap = false;
            // 
            // gameUserAuthToken
            // 
            this.gameUserAuthToken.Location = new System.Drawing.Point(230, 70);
            //this.gameUserAuthToken.MaxLength = 20;
            this.gameUserAuthToken.Name = "gameUserAuthToken";
            this.gameUserAuthToken.Size = new System.Drawing.Size(87, 21);
            this.gameUserAuthToken.TabIndex = 45;
            this.gameUserAuthToken.Text = "";
            //this.gameUserAuthToken.WordWrap = false;
            // 
            // textBoxHiveUserID
            // 
            this.textBoxHiveUserID.Location = new System.Drawing.Point(180, 20);
            this.textBoxHiveUserID.MaxLength = 15;
            this.textBoxHiveUserID.Name = "textBoxHiveUserID";
            this.textBoxHiveUserID.Size = new System.Drawing.Size(70, 21);
            this.textBoxHiveUserID.TabIndex = 43;
            this.textBoxHiveUserID.Text = "test1";
            this.textBoxHiveUserID.WordWrap = false;
            // 
            // textBoxHiveUserPW
            // 
            this.textBoxHiveUserPW.Location = new System.Drawing.Point(310, 20);
            this.textBoxHiveUserPW.MaxLength = 20;
            this.textBoxHiveUserPW.Name = "textBoxHiveUserPW";
            this.textBoxHiveUserPW.Size = new System.Drawing.Size(70, 21);
            this.textBoxHiveUserPW.TabIndex = 45;
            this.textBoxHiveUserPW.Text = "123qwe";
            this.textBoxHiveUserPW.WordWrap = false;
            // 
            // textBoxApiUserID
            // 
            this.textBoxApiUserID.Location = new System.Drawing.Point(180, 47);
            this.textBoxApiUserID.MaxLength = 15;
            this.textBoxApiUserID.Name = "textBoxApiUserID";
            this.textBoxApiUserID.Size = new System.Drawing.Size(70, 21);
            this.textBoxApiUserID.TabIndex = 43;
            this.textBoxApiUserID.Text = "test1";
            this.textBoxApiUserID.WordWrap = false;
            // 
            // textBoxApiUserAuthToken
            // 
            this.textBoxApiUserAuthToken.Location = new System.Drawing.Point(320, 47);
            this.textBoxApiUserAuthToken.MaxLength = 50;
            this.textBoxApiUserAuthToken.Name = "textBoxApiUserAuthToken";
            this.textBoxApiUserAuthToken.Size = new System.Drawing.Size(70, 21);
            this.textBoxApiUserAuthToken.TabIndex = 45;
            this.textBoxApiUserAuthToken.Text = "123qwe";
            this.textBoxApiUserAuthToken.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(162, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 44;
            this.label2.Text = "AuthToken:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(160, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 44;
            this.label4.Text = "ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(250, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 12);
            this.label5.TabIndex = 44;
            this.label5.Text = "PassWord:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(160, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 12);
            this.label6.TabIndex = 44;
            this.label6.Text = "ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(250, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 12);
            this.label7.TabIndex = 44;
            this.label7.Text = "AuthToken:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.Location = new System.Drawing.Point(380, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 26);
            this.button2.TabIndex = 46;
            this.button2.Text = "Matching";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_Matching_Click);
            // 
            // Room
            // 
           // this.Room.Controls.Add(this.button1);
            this.Room.Controls.Add(this.ReadyButton);
            // this.Room.Controls.Add(this.btnMatching);
            //this.Room.Controls.Add(this.GameStartBtn);
            this.Room.Controls.Add(this.btnRoomChat);
            this.Room.Controls.Add(this.textBoxRoomSendMsg);
            this.Room.Controls.Add(this.listBoxRoomChatMsg);
            this.Room.Controls.Add(this.listBoxRoomUserList);
            //this.Room.Controls.Add(this.btn_RoomLeave);
            //this.Room.Controls.Add(this.btn_RoomEnter);
            this.Room.Controls.Add(this.RoomNumber);
            this.Room.Controls.Add(this.label3);
            this.Room.Location = new System.Drawing.Point(14, 220);
            this.Room.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Room.Name = "Room";
            this.Room.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Room.Size = new System.Drawing.Size(495, 264);
            this.Room.TabIndex = 47;
            this.Room.TabStop = false;
            this.Room.Text = "Room";
            // 
            // button3
            // 
            this.ReadyButton.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ReadyButton.Location = new System.Drawing.Point(391, 18);
            this.ReadyButton.Name = "ready";
            this.ReadyButton.Size = new System.Drawing.Size(91, 28);
            this.ReadyButton.TabIndex = 57;
            this.ReadyButton.Text = "Game Ready";
            this.ReadyButton.UseVisualStyleBackColor = true;
            this.ReadyButton.Click += new System.EventHandler(this.btn_Ready_Click);
            // 
            // btnMatching
            // 
            //this.btnMatching.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.btnMatching.Location = new System.Drawing.Point(296, 18);
            //this.btnMatching.Name = "btnMatching";
            //this.btnMatching.Size = new System.Drawing.Size(78, 28);
            //this.btnMatching.TabIndex = 54;
            //this.btnMatching.Text = "Matching";
            //this.btnMatching.UseVisualStyleBackColor = true;
            //this.btnMatching.Click += new System.EventHandler(this.btn_Matching_Click);
            // 
            // GameStartBtn
            // 
            //this.GameStartBtn.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.GameStartBtn.Location = new System.Drawing.Point(341, 223);
            //this.GameStartBtn.Name = "GameStartBtn";
            //this.GameStartBtn.Size = new System.Drawing.Size(148, 28);
            //this.GameStartBtn.TabIndex = 55;
            //this.GameStartBtn.Text = "dummy - GameStart";
            //this.GameStartBtn.UseVisualStyleBackColor = true;
            //this.GameStartBtn.Click += new System.EventHandler(this.btn_GameStart_Click);
            // 
            // btnRoomChat
            // 
            this.btnRoomChat.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRoomChat.Location = new System.Drawing.Point(432, 191);
            this.btnRoomChat.Name = "btnRoomChat";
            this.btnRoomChat.Size = new System.Drawing.Size(50, 26);
            this.btnRoomChat.TabIndex = 53;
            this.btnRoomChat.Text = "chat";
            this.btnRoomChat.UseVisualStyleBackColor = true;
            this.btnRoomChat.Click += new System.EventHandler(this.btn_RoomChat_Click);
            // 
            // textBoxRoomSendMsg
            // 
            this.textBoxRoomSendMsg.Location = new System.Drawing.Point(7, 192);
            this.textBoxRoomSendMsg.MaxLength = 32;
            this.textBoxRoomSendMsg.Name = "textBoxRoomSendMsg";
            this.textBoxRoomSendMsg.Size = new System.Drawing.Size(419, 21);
            this.textBoxRoomSendMsg.TabIndex = 52;
            this.textBoxRoomSendMsg.Text = "test1";
            this.textBoxRoomSendMsg.WordWrap = false;
            // 
            // listBoxRoomChatMsg
            // 
            this.listBoxRoomChatMsg.FormattingEnabled = true;
            this.listBoxRoomChatMsg.ItemHeight = 12;
            this.listBoxRoomChatMsg.Location = new System.Drawing.Point(137, 51);
            this.listBoxRoomChatMsg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxRoomChatMsg.Name = "listBoxRoomChatMsg";
            this.listBoxRoomChatMsg.Size = new System.Drawing.Size(349, 136);
            this.listBoxRoomChatMsg.TabIndex = 51;
            this.listBoxRoomChatMsg.SelectedIndexChanged += new System.EventHandler(this.listBoxRoomChatMsg_SelectedIndexChanged);
            // 
            // listBoxRoomUserList
            // 
            this.listBoxRoomUserList.FormattingEnabled = true;
            this.listBoxRoomUserList.ItemHeight = 12;
            this.listBoxRoomUserList.Location = new System.Drawing.Point(8, 51);
            this.listBoxRoomUserList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBoxRoomUserList.Name = "listBoxRoomUserList";
            this.listBoxRoomUserList.Size = new System.Drawing.Size(123, 136);
            this.listBoxRoomUserList.TabIndex = 49;
            // 
            // btn_RoomLeave
            // 
            //this.btn_RoomLeave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.btn_RoomLeave.Location = new System.Drawing.Point(216, 18);
            //this.btn_RoomLeave.Name = "btn_RoomLeave";
            //this.btn_RoomLeave.Size = new System.Drawing.Size(66, 26);
            //this.btn_RoomLeave.TabIndex = 48;
            //this.btn_RoomLeave.Text = "Leave";
            //this.btn_RoomLeave.UseVisualStyleBackColor = true;
            //this.btn_RoomLeave.Click += new System.EventHandler(this.btn_RoomLeave_Click);
            //// 
            //// btn_RoomEnter
            //// 
            //this.btn_RoomEnter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.btn_RoomEnter.Location = new System.Drawing.Point(144, 18);
            //this.btn_RoomEnter.Name = "btn_RoomEnter";
            //this.btn_RoomEnter.Size = new System.Drawing.Size(66, 26);
            //this.btn_RoomEnter.TabIndex = 47;
            //this.btn_RoomEnter.Text = "Enter";
            //this.btn_RoomEnter.UseVisualStyleBackColor = true;
            //this.btn_RoomEnter.Click += new System.EventHandler(this.btn_RoomEnter_Click);
            // 
            // RoomNumber
            // 
            this.RoomNumber.Location = new System.Drawing.Point(98, 20);
            //this.RoomNumber.MaxLength = 6;
            this.RoomNumber.Name = "RoomNumber";
            this.RoomNumber.Size = new System.Drawing.Size(38, 21);
            this.RoomNumber.TabIndex = 44;
            this.RoomNumber.Text = "";
            //this.RoomNumber.WordWrap = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "Room Number:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Peru;
            this.panel1.Location = new System.Drawing.Point(521, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 657);
            this.panel1.TabIndex = 57;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // button1
            // 
            //this.button1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //this.button1.Location = new System.Drawing.Point(183, 223);
            //this.button1.Name = "button1";
            //this.button1.Size = new System.Drawing.Size(136, 28);
            //this.button1.TabIndex = 58;
            //this.button1.Text = "Dummy 유저 등록";
            //this.button1.UseVisualStyleBackColor = true;
            //this.button1.Click += new System.EventHandler(this.btn_AddUser_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 680);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Room);
            //this.Controls.Add(this.button2);
            //this.Controls.Add(this.textBoxUserPW);
            //this.Controls.Add(this.label2);
            //this.Controls.Add(this.textBoxUserID);
            //this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.listBoxLog);
            //this.Controls.Add(this.btnDisconnect);
            //this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBoxAPI);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "mainForm";
            this.Text = "네트워크 테스트 클라이언트";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBoxAPI.ResumeLayout(false);
            this.groupBoxAPI.PerformLayout();
            this.Room.ResumeLayout(false);
            this.Room.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnLoginHiveServer;
        private System.Windows.Forms.Button btnLoginApiServer;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBoxAPI;
        private System.Windows.Forms.Label gameServerPortNumber;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label gameServerIP;
        private System.Windows.Forms.TextBox textBoxHiveIP;
        private System.Windows.Forms.TextBox textBoxApiIP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHiveUserID;
        private System.Windows.Forms.TextBox textBoxHiveUserPW;
        private System.Windows.Forms.TextBox textBoxApiUserID;
        private System.Windows.Forms.TextBox textBoxApiUserAuthToken;
        private System.Windows.Forms.Label gameUserID;
        private System.Windows.Forms.Label gameUserAuthToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox Room;
        //private System.Windows.Forms.Button btn_RoomLeave;
        //private System.Windows.Forms.Button btn_RoomEnter;
        private System.Windows.Forms.Label RoomNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRoomChat;
        private System.Windows.Forms.TextBox textBoxRoomSendMsg;
        private System.Windows.Forms.ListBox listBoxRoomChatMsg;
        private System.Windows.Forms.ListBox listBoxRoomUserList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        // private System.Windows.Forms.Button btnMatching;
        // private System.Windows.Forms.Button GameStartBtn;
        private System.Windows.Forms.Panel panel1;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ReadyButton;
    }
}

