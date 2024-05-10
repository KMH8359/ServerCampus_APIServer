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
            this.btnLoginAPIServer = new System.Windows.Forms.Button();
            this.btnLoginGameAPIServer = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBoxAPI = new System.Windows.Forms.GroupBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBoxLocalHostIP = new System.Windows.Forms.CheckBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.textBoxHiveIP = new System.Windows.Forms.TextBox();
            this.textBoxGameApiIP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHiveUserID = new System.Windows.Forms.TextBox();
            this.textBoxHiveUserPW = new System.Windows.Forms.TextBox();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.textBoxUserPW = new System.Windows.Forms.TextBox();
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
            this.btn_RoomLeave = new System.Windows.Forms.Button();
            this.btn_RoomEnter = new System.Windows.Forms.Button();
            this.textBoxRoomNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            //this.button1 = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.groupBoxAPI.SuspendLayout();
            this.Room.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDisconnect.Location = new System.Drawing.Point(410, 44);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(88, 26);
            this.btnDisconnect.TabIndex = 29;
            this.btnDisconnect.Text = "접속 끊기";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConnect.Location = new System.Drawing.Point(410, 16);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(88, 26);
            this.btnConnect.TabIndex = 28;
            this.btnConnect.Text = "접속하기";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCreateAccount.Location = new System.Drawing.Point(220, 45);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(70, 26);
            this.btnCreateAccount.TabIndex = 28;
            this.btnCreateAccount.Text = "계정생성";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // btnLoginAPIServer
            // 
            this.btnLoginAPIServer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoginAPIServer.Location = new System.Drawing.Point(300, 45);
            this.btnLoginAPIServer.Name = "btnLoginAPIServer";
            this.btnLoginAPIServer.Size = new System.Drawing.Size(80, 26);
            this.btnLoginAPIServer.TabIndex = 28;
            this.btnLoginAPIServer.Text = "API로그인";
            this.btnLoginAPIServer.UseVisualStyleBackColor = true;
            this.btnLoginAPIServer.Click += new System.EventHandler(this.btnLoginApiServer_Click);
            // 
            // btnLoginGameAPIServer
            // 
            this.btnLoginGameAPIServer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoginGameAPIServer.Location = new System.Drawing.Point(390, 45);
            this.btnLoginGameAPIServer.Name = "btnLoginGameAPIServer";
            this.btnLoginGameAPIServer.Size = new System.Drawing.Size(110, 26);
            this.btnLoginGameAPIServer.TabIndex = 28;
            this.btnLoginGameAPIServer.Text = "게임API로그인";
            this.btnLoginGameAPIServer.UseVisualStyleBackColor = true;
            this.btnLoginGameAPIServer.Click += new System.EventHandler(this.btnLoginApiServer_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.textBoxPort);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.checkBoxLocalHostIP);
            this.groupBox5.Controls.Add(this.textBoxIP);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.btnConnect);
            this.groupBox5.Controls.Add(this.btnDisconnect);
            this.groupBox5.Controls.Add(this.button2);
            this.groupBox5.Controls.Add(this.textBoxUserPW);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.textBoxUserID);
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
            this.groupBoxAPI.Controls.Add(this.textBoxGameApiIP);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveIP);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveUserID);
            this.groupBoxAPI.Controls.Add(this.textBoxHiveUserPW);
            this.groupBoxAPI.Controls.Add(this.label4);
            this.groupBoxAPI.Controls.Add(this.label5);
            this.groupBoxAPI.Controls.Add(this.btnCreateAccount);
            this.groupBoxAPI.Controls.Add(this.btnLoginAPIServer);
            this.groupBoxAPI.Controls.Add(this.btnLoginGameAPIServer);
            this.groupBoxAPI.Location = new System.Drawing.Point(12, 5);
            this.groupBoxAPI.Name = "groupBoxAPI";
            this.groupBoxAPI.Size = new System.Drawing.Size(503, 80);
            this.groupBoxAPI.TabIndex = 27;
            this.groupBoxAPI.TabStop = false;
            this.groupBoxAPI.Text = "API 서버 로그인";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(225, 20);
            this.textBoxPort.MaxLength = 6;
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(51, 21);
            this.textBoxPort.TabIndex = 18;
            this.textBoxPort.Text = "32452";
            this.textBoxPort.WordWrap = false;
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
            // checkBoxLocalHostIP
            // 
            this.checkBoxLocalHostIP.AutoSize = true;
            this.checkBoxLocalHostIP.Checked = true;
            this.checkBoxLocalHostIP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLocalHostIP.Location = new System.Drawing.Point(282, 18);
            this.checkBoxLocalHostIP.Name = "checkBoxLocalHostIP";
            this.checkBoxLocalHostIP.Size = new System.Drawing.Size(103, 16);
            this.checkBoxLocalHostIP.TabIndex = 15;
            this.checkBoxLocalHostIP.Text = "localhost 사용";
            this.checkBoxLocalHostIP.UseVisualStyleBackColor = true;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(68, 18);
            this.textBoxIP.MaxLength = 20;
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(87, 21);
            this.textBoxIP.TabIndex = 11;
            this.textBoxIP.Text = "127.0.0.1";
            this.textBoxIP.WordWrap = false;
            // 
            // textBoxHiveIP
            // 
            this.textBoxHiveIP.Location = new System.Drawing.Point(100, 20);
            this.textBoxHiveIP.MaxLength = 20;
            this.textBoxHiveIP.Name = "textBoxHiveIP";
            this.textBoxHiveIP.Size = new System.Drawing.Size(87, 21);
            this.textBoxHiveIP.TabIndex = 11;
            this.textBoxHiveIP.Text = "127.0.0.1";
            this.textBoxHiveIP.WordWrap = false;
            // 
            // textBoxGameApiIP
            // 
            this.textBoxGameApiIP.Location = new System.Drawing.Point(120, 47);
            this.textBoxGameApiIP.MaxLength = 20;
            this.textBoxGameApiIP.Name = "textBoxGameApiIP";
            this.textBoxGameApiIP.Size = new System.Drawing.Size(87, 21);
            this.textBoxGameApiIP.TabIndex = 11;
            this.textBoxGameApiIP.Text = "127.0.0.1";
            this.textBoxGameApiIP.WordWrap = false;
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
            this.label11.Size = new System.Drawing.Size(61, 12);
            this.label11.TabIndex = 10;
            this.label11.Text = "Hive 서버 주소:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "API 게임 서버 주소:";
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
            // textBoxUserID
            // 
            this.textBoxUserID.Location = new System.Drawing.Point(62, 67);
            this.textBoxUserID.MaxLength = 15;
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(87, 21);
            this.textBoxUserID.TabIndex = 43;
            this.textBoxUserID.Text = "test1";
            this.textBoxUserID.WordWrap = false;
            // 
            // textBoxUserPW
            // 
            this.textBoxUserPW.Location = new System.Drawing.Point(230, 68);
            this.textBoxUserPW.MaxLength = 20;
            this.textBoxUserPW.Name = "textBoxUserPW";
            this.textBoxUserPW.Size = new System.Drawing.Size(87, 21);
            this.textBoxUserPW.TabIndex = 45;
            this.textBoxUserPW.Text = "123qwe";
            this.textBoxUserPW.WordWrap = false;
            // 
            // textBoxHiveUserID
            // 
            this.textBoxHiveUserID.Location = new System.Drawing.Point(220, 20);
            this.textBoxHiveUserID.MaxLength = 15;
            this.textBoxHiveUserID.Name = "textBoxHiveUserID";
            this.textBoxHiveUserID.Size = new System.Drawing.Size(87, 21);
            this.textBoxHiveUserID.TabIndex = 43;
            this.textBoxHiveUserID.Text = "test1";
            this.textBoxHiveUserID.WordWrap = false;
            // 
            // textBoxHiveUserPW
            // 
            this.textBoxHiveUserPW.Location = new System.Drawing.Point(370, 20);
            this.textBoxHiveUserPW.MaxLength = 20;
            this.textBoxHiveUserPW.Name = "textBoxHiveUserPW";
            this.textBoxHiveUserPW.Size = new System.Drawing.Size(87, 21);
            this.textBoxHiveUserPW.TabIndex = 45;
            this.textBoxHiveUserPW.Text = "123qwe";
            this.textBoxHiveUserPW.WordWrap = false;
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
            this.label4.Location = new System.Drawing.Point(200, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 44;
            this.label4.Text = "ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 12);
            this.label5.TabIndex = 44;
            this.label5.Text = "PassWord:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.Location = new System.Drawing.Point(337, 67);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 26);
            this.button2.TabIndex = 46;
            this.button2.Text = "Login";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_Login_Click);
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
            this.Room.Controls.Add(this.btn_RoomLeave);
            this.Room.Controls.Add(this.btn_RoomEnter);
            this.Room.Controls.Add(this.textBoxRoomNumber);
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
            this.btn_RoomLeave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_RoomLeave.Location = new System.Drawing.Point(216, 18);
            this.btn_RoomLeave.Name = "btn_RoomLeave";
            this.btn_RoomLeave.Size = new System.Drawing.Size(66, 26);
            this.btn_RoomLeave.TabIndex = 48;
            this.btn_RoomLeave.Text = "Leave";
            this.btn_RoomLeave.UseVisualStyleBackColor = true;
            this.btn_RoomLeave.Click += new System.EventHandler(this.btn_RoomLeave_Click);
            // 
            // btn_RoomEnter
            // 
            this.btn_RoomEnter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_RoomEnter.Location = new System.Drawing.Point(144, 18);
            this.btn_RoomEnter.Name = "btn_RoomEnter";
            this.btn_RoomEnter.Size = new System.Drawing.Size(66, 26);
            this.btn_RoomEnter.TabIndex = 47;
            this.btn_RoomEnter.Text = "Enter";
            this.btn_RoomEnter.UseVisualStyleBackColor = true;
            this.btn_RoomEnter.Click += new System.EventHandler(this.btn_RoomEnter_Click);
            // 
            // textBoxRoomNumber
            // 
            this.textBoxRoomNumber.Location = new System.Drawing.Point(98, 20);
            this.textBoxRoomNumber.MaxLength = 6;
            this.textBoxRoomNumber.Name = "textBoxRoomNumber";
            this.textBoxRoomNumber.Size = new System.Drawing.Size(38, 21);
            this.textBoxRoomNumber.TabIndex = 44;
            this.textBoxRoomNumber.Text = "0";
            this.textBoxRoomNumber.WordWrap = false;
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
        private System.Windows.Forms.Button btnLoginAPIServer;
        private System.Windows.Forms.Button btnLoginGameAPIServer;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBoxAPI;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBoxLocalHostIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.TextBox textBoxHiveIP;
        private System.Windows.Forms.TextBox textBoxGameApiIP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHiveUserID;
        private System.Windows.Forms.TextBox textBoxHiveUserPW;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.TextBox textBoxUserPW;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox Room;
        private System.Windows.Forms.Button btn_RoomLeave;
        private System.Windows.Forms.Button btn_RoomEnter;
        private System.Windows.Forms.TextBox textBoxRoomNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRoomChat;
        private System.Windows.Forms.TextBox textBoxRoomSendMsg;
        private System.Windows.Forms.ListBox listBoxRoomChatMsg;
        private System.Windows.Forms.ListBox listBoxRoomUserList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        // private System.Windows.Forms.Button btnMatching;
        // private System.Windows.Forms.Button GameStartBtn;
        private System.Windows.Forms.Panel panel1;
        //private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ReadyButton;
    }
}

