using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//테스트용 UI (예시)
public class RoomSettingsUIExample : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private RoomSettingsBootstrap bootstrap;

    [Header("Settings")]
    //팀 당 플레이어 수
    [SerializeField]
    private Slider playerCountSlider;
    [SerializeField]
    private TMP_Text playerCountText;
    //게임 당 매치 수
    [SerializeField]
    private Slider matchCountSlider;
    [SerializeField]
    private TMP_Text matchCountText;
    //매치 당 라운드 수
    [SerializeField]
    private Slider roundCountSlider;
    [SerializeField]
    private TMP_Text roundCountText;
    //승부가 이미 결정난 상황에서도 총 라운드 수만큼 강제 진행
    [SerializeField]
    private Toggle fullRoundToggle;
    //입력 제한 시간
    [SerializeField]
    private Slider inputTimeSlider;
    [SerializeField]
    private TMP_Text inputTimeText;
    //키커 최대 파워 입력까지 걸리는 시간
    [SerializeField]
    private Slider kickerTimeSlider;
    [SerializeField]
    private TMP_Text kickerTimeText;
    //골키퍼 최대 속력 입력까지 걸리는 시간
    [SerializeField]
    private Slider goalkeeperTimeSlider;
    [SerializeField]
    private TMP_Text goalkeeperTimeText;
    //아이템 활성화 여부
    [SerializeField]
    private Toggle itemToggle;
    //아이템 선택 제한 시간
    [SerializeField]
    private Slider itemTimeSlider;
    [SerializeField]
    private TMP_Text itemTimeText;
    //관전자 허용 여부
    [SerializeField]
    private Toggle spectatorToggle;
    //테스트용 게임 시작 버튼
    [SerializeField]
    private Button gameStartButton;
    
    private RoomSettingsService service;

    private void Start()
    {
        service = bootstrap.Service;

        ConfigureControls();

        //이벤트 Listener 추가
        playerCountSlider.onValueChanged.AddListener(OnPlayerCountChange);
        matchCountSlider.onValueChanged.AddListener(OnMatchCountChange);
        roundCountSlider.onValueChanged.AddListener(OnRoundCountChange);
        fullRoundToggle.onValueChanged.AddListener(OnFullRoundToggle);
        inputTimeSlider.onValueChanged.AddListener(OnInputTimeChange);
        kickerTimeSlider.onValueChanged.AddListener(OnKickerTimeChange);
        goalkeeperTimeSlider.onValueChanged.AddListener(OnGoalkeeperTimeChange);
        itemToggle.onValueChanged.AddListener(OnItemToggle);
        itemTimeSlider.onValueChanged.AddListener(OnItemTimeChange);
        spectatorToggle.onValueChanged.AddListener(OnSpectatorToggle);
        gameStartButton.onClick.AddListener(OnGameStartButtonClick);

        service.OnSettingsChange += Refresh;

        Refresh(service.Current);
    }

    //Controls 설정
    private void ConfigureControls()
    {
        ConfigureSlider(playerCountSlider, RoomSettingsID.PlayersPerTeam);
        ConfigureSlider(matchCountSlider, RoomSettingsID.MatchesPerGame);
        ConfigureSlider(roundCountSlider, RoomSettingsID.RoundsPerMatch);
        ConfigureSlider(inputTimeSlider, RoomSettingsID.InputTimeLimit);
        ConfigureSlider(kickerTimeSlider, RoomSettingsID.KickerChargeSpeed);
        ConfigureSlider(goalkeeperTimeSlider, RoomSettingsID.GoalkeeperChargeSpeed);
        ConfigureSlider(itemTimeSlider, RoomSettingsID.ItemSelectionTimeLimit);
    }

    //Slider 설정
    private void ConfigureSlider(Slider slider, RoomSettingsID id)
    {
        //올바른 설정인지 검사
        if (!RoomSettingsCatalog.TryGet(id, out RoomSettingsDefinition definition))
        {
            return;
        }

        //최솟값, 최댓값 적용
        slider.minValue = definition.Min;
        slider.maxValue = definition.Max;

        //Slider 자체에는 Step 속성이 없어서 int형 값만 wholeNumbers 속성 적용
        slider.wholeNumbers = (definition.ValueType == RoomSettingsValueType.Int);
    }

    private void OnPlayerCountChange(float value)
    {
        RequestInt(RoomSettingsID.PlayersPerTeam, Mathf.RoundToInt(value));
    }

    private void OnMatchCountChange(float value)
    {
        RequestInt(RoomSettingsID.MatchesPerGame, Mathf.RoundToInt(value));
    }

    private void OnRoundCountChange(float value)
    {
        RequestInt(RoomSettingsID.RoundsPerMatch, Mathf.RoundToInt(value));
    }

    private void OnFullRoundToggle(bool value)
    {
        service.RequestChange(RoomSettingsChange.FromBool(RoomSettingsID.ForceFullRounds, value));
    }

    private void OnInputTimeChange(float value)
    {
        RequestInt(RoomSettingsID.InputTimeLimit, Mathf.RoundToInt(value));
    }

    private void OnKickerTimeChange(float value)
    {
        //Step에 맞춰서 값 변환
        float step = RoomSettingsCatalog.CatalogList.First(definition => definition.ID == RoomSettingsID.KickerChargeSpeed).Step;
        float rounded = Mathf.Round(value / step) * step;

        service.RequestChange(RoomSettingsChange.FromFloat(RoomSettingsID.KickerChargeSpeed, rounded));
    }

    private void OnGoalkeeperTimeChange(float value)
    {
        //Step에 맞춰서 값 변환
        float step = RoomSettingsCatalog.CatalogList.First(definition => definition.ID == RoomSettingsID.GoalkeeperChargeSpeed).Step;
        float rounded = Mathf.Round(value / step) * step;

        service.RequestChange(RoomSettingsChange.FromFloat(RoomSettingsID.KickerChargeSpeed, rounded));
    }

    private void OnItemToggle(bool value)
    {
        service.RequestChange(RoomSettingsChange.FromBool(RoomSettingsID.EnableItems, value));
    }

    private void OnItemTimeChange(float value)
    {
        RequestInt(RoomSettingsID.ItemSelectionTimeLimit, Mathf.RoundToInt(value));
    }

    private void OnSpectatorToggle(bool value)
    {
        service.RequestChange(RoomSettingsChange.FromBool(RoomSettingsID.AllowSpectators, value));
    }

    private void OnGameStartButtonClick()
    {
        bootstrap.StartGame();
        Refresh(service.Current);
    }

    //int형 값 변경 요청 (직접 변경하는 것이 아닌 요청 전송)
    private void RequestInt(RoomSettingsID id, int value)
    {
        service.RequestChange(RoomSettingsChange.FromInt(id, value));
    }

    //설정 데이터 새로 고침 (값 변경, 방장 변경, 게임 시작 등 상황에서 호출)
    private void Refresh(RoomSettingsData settings)
    {
        //표시되는 값 설정
        playerCountSlider.SetValueWithoutNotify(settings.PlayersPerTeam);
        matchCountSlider.SetValueWithoutNotify(settings.MatchesPerGame);
        roundCountSlider.SetValueWithoutNotify(settings.RoundsPerMatch);
        fullRoundToggle.SetIsOnWithoutNotify(settings.ShouldForceFullRounds);
        inputTimeSlider.SetValueWithoutNotify(settings.InputTimeLimitInSeconds);
        kickerTimeSlider.SetValueWithoutNotify(settings.KickerFullChargeTimeInSeconds);
        goalkeeperTimeSlider.SetValueWithoutNotify(settings.GoalkeeperFullChargeTimeInSeconds);
        itemToggle.SetIsOnWithoutNotify(settings.IsItemEnabled);
        itemTimeSlider.SetValueWithoutNotify(settings.ItemSelectionTimeLimitInSeconds);
        spectatorToggle.SetIsOnWithoutNotify(settings.AllowSpectators);

        //표시되는 텍스트 설정
        playerCountText.text = $"{settings.PlayersPerTeam}명";
        matchCountText.text = $"{settings.MatchesPerGame}회";
        roundCountText.text = $"{settings.RoundsPerMatch}명";
        inputTimeText.text = $"{settings.InputTimeLimitInSeconds}초";
        kickerTimeText.text = $"{settings.KickerFullChargeTimeInSeconds:0.00}초";
        goalkeeperTimeText.text = $"{settings.GoalkeeperFullChargeTimeInSeconds:0.00}초";
        itemTimeText.text = $"{settings.ItemSelectionTimeLimitInSeconds}초";

        //Controls 선택(값 편집) 가능 여부 설정
        bool editable = service.CanEdit;
        playerCountSlider.interactable = editable;
        matchCountSlider.interactable = editable;
        roundCountSlider.interactable = editable;
        fullRoundToggle.interactable = editable;
        inputTimeSlider.interactable = editable;
        kickerTimeSlider.interactable = editable;
        goalkeeperTimeSlider.interactable = editable;
        itemToggle.interactable = editable;
        itemTimeSlider.interactable = editable && itemToggle.isOn;
        spectatorToggle.interactable = editable;
        gameStartButton.interactable = editable;
    }

    private void OnDestroy()
    {
        if (service != null)
        {
            service.OnSettingsChange -= Refresh;
        }

        //이벤트 Listener 제거
        playerCountSlider.onValueChanged.RemoveListener(OnPlayerCountChange);
        matchCountSlider.onValueChanged.RemoveListener(OnMatchCountChange);
        roundCountSlider.onValueChanged.RemoveListener(OnRoundCountChange);
        fullRoundToggle.onValueChanged.RemoveListener(OnFullRoundToggle);
        inputTimeSlider.onValueChanged.RemoveListener(OnInputTimeChange);
        kickerTimeSlider.onValueChanged.RemoveListener(OnKickerTimeChange);
        goalkeeperTimeSlider.onValueChanged.RemoveListener(OnGoalkeeperTimeChange);
        itemToggle.onValueChanged.RemoveListener(OnItemToggle);
        itemTimeSlider.onValueChanged.RemoveListener(OnItemTimeChange);
        spectatorToggle.onValueChanged.RemoveListener(OnSpectatorToggle);
        gameStartButton.onClick.RemoveListener(OnGameStartButtonClick);
    }
}