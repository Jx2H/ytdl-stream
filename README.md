# ytdl-stream

__본 코드는 단순 공부 및 연구 목적으로 코드 작성되었음을 알려드립니다.__    
__본 코드는 환경변수 PATH에 youtube-dl 프로그램을 향하도록 되어 있어야 합니다.__

게리모드에서 3D Radio 애드온을 사용하는데 파일 URL는 스트리밍 가능하다고 한다. 하지만 최근 게리모드는 크로미움을 지원하지 않아 HTML5 미지원하여 정상적으로 재생을 할 수 없다.    
그래서 직접 유튜브 링크만 알면 파일 URL 처럼 사용하고 싶어 이 코드를 작성하였다.
<br>
간단하게 정상 작동 될 수 있게 실행한 다음, http://127.0.0.1:424/?q=유튜브_비디오_ID 으로 요청하게 되면 프로그램은 ytdl를 실행시켜 3초후 sidout에서 스트리밍 데이터를 받게 된다. 이것을 이제 이용자에게 파일로 주는 것.
