# PPT Portfolio
https://1drv.ms/p/c/114382788941cec0/EcDOQYl4gkMggBFgfgAAAAAB9vmMTDPrmot8Dz7-YLa8DA?e=v8pvEm

---
# ForestRPG Portfolio

---
## 1. 소개
![Image](https://github.com/user-attachments/assets/a97806c4-2ba8-4f20-8862-2f88eeb1417f)
+ 백 뷰 시점으로 진행하는 RPG게임이며 퀘스트를
진행하고 몬스터를 처치하여 얻은 자원과 보상을 활용
해 장비를 강화하여 성장하며 던전을 탐험할 수 있
는 게임입니다.
+ 개발기간 : 2024.05.20 ~ 2025.03.11

---
## 2. 개발환경
+ Unity 2022.3.22f1 LST
+ C#
+ Windows 11

---
## 3. 사용기술

| 기술    | 설명                                    |
|:------:|:-----------------------------------------|
| 싱글톤 패턴 | 싱글톤 패턴을 사용하여 Manager관리 |
| 옵저버 패턴 | 옵저버 패턴을 사용하여 퀘스트 데이터 변경 시 업데이트 |
| FSM | FSM을 사용하여 플레이어 에너미 구현 |
| ObjectPool | 자주 사용하는 Object를 Pool에 관리하여 사용 |
| GoogleSeet | GoogleSeet를 사용하여 데이터 관리 |
| Save | 저장할 데이터를 Json으로 변환하여 관리 |

---
## 4. 구현기능
+ Object
   + 플레이어
   + 몬스터
      + 슬라임
      + 곰
   + 보스 몬스터
      + 분노한 곰 (패턴 : 스킬4개)
   + NPC
      + 상점 NPC
      + 퀘스트 NPC
      + 강화 NPC
      + 시민
   + 아이템
      + HP, MP물약
      + 무기, 방어구, 장식
      + 강화제료(광석)
+ UI
  + Scene
     + TitleScene
       게임 처음 진입 시 게임시작, 종료, 세이브, 옵션설정 버튼
  + Popup
     + 인벤토리창, 장비창, 스킬창, 퀘스트창, 상점창, 강화창, 퀵슬롯, 옵션창
     + 던전 결과창, 대화창, 미니 퀘스트 정보창, 상점 구매 개수 설정창, 부활창, 미니맵, SceneLoad창
  + WorldSpace
     + 피격 데미지, 퀘스트 진행 아이콘
---
## 5. 플레이 영상
<img src="https://github.com/user-attachments/assets/71ee021a-f11c-40ec-b180-aa2435bf3ad2" width="250" height="250" />

https://youtu.be/AfGTK478ias

---
## 6. 다운로드 링크
https://drive.google.com/file/d/1G4nheyP5hJNptM8vA2VQzD5BHVzSb3Px/view?usp=sharing

# LastPage Portfolio
---
## 1. 소개
![Image](https://github.com/user-attachments/assets/f2e94679-6759-48f5-95ad-7092d460d16b)
백 뷰 시점으로 진행하는 로그라이크 게임이
며 몬스터를 처치하거나 퀘스트를 클리어하여 성장하
며 보스 몬스터를 잡아 다음 스테이지로 계속 이동하는
게임입니다.
+ 개발기간 : 2023.07.01 ~ 2024.01.31

---
## 2. 개발환경
+ Unity 2021.3.24f1 LST
+ C#
+ Windows 11

---
## 3. 사용기술
| 기술    | 설명                                    |
|:------:|:-----------------------------------------|
| 싱글톤 패턴 | 싱글톤 패턴을 사용하여 Manager관리 |
| 옵저버 패턴 | 옵저버 패턴을 사용하여 퀘스트 데이터 변경 시 업데이트 |
| FSM | FSM을 사용하여 에너미 구현 |
| ObjectPool | 자주 사용하는 Object를 Pool에 관리하여 사용 |
| 데이터 관리 | GoogleSeet, ScriptObject를 사용하여 데이터 관리 |
| Save | 저장할 데이터를 Json으로 변환하여 관리 |

---
## 4. 구현기능
+ Object
   + 플레이어
   + 몬스터
      + 고블린
      + 거미
   + 보스 몬스터
      + 드래곤 (패턴 : 스킬 2개)
   + NPC
      + 상점 NPC
      + 퀘스트 NPC
   + 아이템
      + HP, MP물약
      + 아티펙트
+ UI
  + Scene
     + TitleScene
       게임 처음 진입 시 게임시작, 종료, 세이브, 옵션설정
  + Popup
     + 인벤토리창, 퀘스트창, 상점창
     + 상점 구매 개수 설정창, 부활창, SceneLoad창
  + WorldSpace
     + 피격 데미지, 퀘스트 진행 아이콘
---
## 5. 플레이 영상
<img src=https://github.com/user-attachments/assets/692bba87-e72f-4667-8612-16271e6713af width="250" height="250" />

https://youtu.be/goe68owyowc

---
## 6. 다운로드 링크
https://drive.google.com/file/d/1Dk5PRMGt9yXcmOjbiupmyL-QjNs_2-db/view?usp=sharing
