```mermaid
  sequenceDiagram
    participant client as Client
    participant apiserver as APIServer
    participant apigameserver as API Game Server
    
    autonumber
    
    client ->>+ apiserver: 회원가입
    apiserver -->> apiserver: 가입한 유저의 데이터(ID, 패스워드, 솔트값) DB에 저장
    client ->>+ apigameserver: 토큰을 사용해 로그인 요청
    apigameserver ->> apiserver: redis에 없는 토큰일 경우 하이브 서버에 토큰 검증 요청
    apiserver ->>+ apigameserver: 검증 완료
    apigameserver -->> apigameserver: redis에 해당 토큰 저장 및 신규 유저일 경우 gamedata 생성

```
