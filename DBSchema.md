# HIVE Server

## Account DB

### Account Table
로그인과 관련된 유저의 계정 정보 저장

```sql
CREATE TABLE `account` (
  `AccountId` bigint NOT NULL AUTO_INCREMENT COMMENT '계정번호',
  `Email` varchar(50) NOT NULL COMMENT '이메일',
  `SaltValue` varchar(100) NOT NULL COMMENT '암호화 값',
  `HashedPassword` varchar(100) NOT NULL COMMENT '해싱된 비밀번호',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜',
  PRIMARY KEY (`AccountId`),
  UNIQUE KEY `Email` (`Email`)
)
```   

### UserGameData 
게임 플레이와 관련된 유저 정보 저장

```sql
CREATE TABLE `usergamedata` (
  `AccountId` bigint NOT NULL AUTO_INCREMENT COMMENT '계정번호',
  `Email` varchar(50) NOT NULL COMMENT '이메일',
  `LEVEL` int NOT NULL COMMENT '레벨',
  `EXP` int NOT NULL COMMENT '경험치',
  `WIN` int NOT NULL COMMENT '승리한 게임 수',
  `LOSE` int NOT NULL COMMENT '패배한 게임 수',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '생성 날짜',
  PRIMARY KEY (`AccountId`),
  UNIQUE KEY `Email` (`Email`)
)
```   
