public enum EVolume
{
    Master,         //마스터 볼륨
    SFX,            //효과음
    BGM,            //배경음
}

public enum ESensivity
{
    AxisXY,     //xy 전체 수치 변경
    AxisX,      //x 수치 변경
    AxisY,      //y 수치 변경
}

public enum EResolution
{
    HD,     //1280 x 720
    FHD,    //1920 x 1080
}

public class OptionData
{
    private float[] mfVolume;   //볼륨
    private float[] mfMouseSensivity;    //마우스 민감도
    private bool mbAxisYInverseStatu;    //Y축 반전 상태
    private bool mbAxisXYSensivityStatu;    //XY축 민감도 상태
    private EResolution meResolution;   //해상도
    private ESensivity meSensivityType;     //민감도 타입

    public void Copy(OptionData other)
    {
        //볼륨 값 저장
        for(int i = 0; i < mfVolume.Length; ++i)
        {
            mfVolume[i] = other.mfVolume[i];
        }

        //마우스 민감도 값 저장
        for(int i = 0; i < mfMouseSensivity.Length; ++i)
        {
            mfMouseSensivity[i] = other.mfMouseSensivity[i];
        }

        mbAxisYInverseStatu = other.mbAxisYInverseStatu;    //Y축 반전 상태
        mbAxisXYSensivityStatu = other.mbAxisXYSensivityStatu;      //XY축 민감도 상태
        meResolution = other.meResolution;    //해상도
        meSensivityType = other.meSensivityType;    //민감도 타입
    }   //데이터 복사

    public void Initialize()
    {
        //EVolume idx 위치의 볼륨 슬라이더 값
        if (mfVolume == null)
        {
            mfVolume = new float[3] { 1.0f, 1.0f, 1.0f };
        }

        //ESensivity idx 위치의 민감도 슬라이더 값
        if (mfMouseSensivity == null)
        {
            mfMouseSensivity = new float[3] { 50.0f, 50.0f, 50.0f };
        }

        ResetData();
    }

    public void ResetData()
    {
        //볼륨값 초기화
        for(int i = 0; i < mfVolume.Length; ++i)
        {
            mfVolume[i] = 1.0f;
        }

        //마우스 민감도 초기화
        for(int i = 0; i < mfMouseSensivity.Length; ++i)
        {
            mfMouseSensivity[i] = 50.0f;
        }

        mbAxisYInverseStatu = false;        //Y축 반전상태 초기화
        mbAxisXYSensivityStatu = false;      //XY축 민감도 상태 초기화

        meResolution = EResolution.FHD;    //화면 해상도 설정 초기화
        meSensivityType = ESensivity.AxisXY;    //XY축 민감도 값 초기화
    }   //옵션 데이터 초기화

    public EResolution Resolution
    {
        set { meResolution = value; }
        get { return meResolution; }
    }   //해상도 값

    public ESensivity SensivityType
    {
        set { meSensivityType = value; }
        get { return meSensivityType; }
    }   //마우스 민감도

    public bool AxisYInverseStatu
    {
        set { mbAxisYInverseStatu = value; }
        get { return mbAxisYInverseStatu; }
    }   //Y축 반전

    public bool AxisXYSensivityStatu
    {
        set { mbAxisXYSensivityStatu = value; }
        get { return mbAxisXYSensivityStatu; }
    }   //XY축 민감도 상태

    /// <summary>
    /// eVolumeType : 볼륨 타입
    /// fVolume : 볼륨 값
    /// </summary>
    public void SetVolume(EVolume eVolumeType, float fVolume)
    {
        mfVolume[(int)eVolumeType] = fVolume;
    }   //볼륨 타입의 값 설정

    /// <summary>
    /// eVolumeType : 볼륨 타입
    /// </summary>
    public float GetVolume(EVolume eVolumeType)
    {
        return mfVolume[(int)eVolumeType];
    }   //볼륨 타입의 값 반환

    /// <summary>
    /// eSnsivityType : 민감도 타입
    /// fSensivity : 민감도 값
    /// </summary>
    public void SetSensivity(ESensivity eSnsivityType, float fSensivity)
    {
        mfMouseSensivity[(int)eSnsivityType] = fSensivity;
    }   //민감도 값 설정

    /// <summary>
    /// eSnsivityType : 민감도 타입
    /// </summary>
    public float GetSensivity(ESensivity eSnsivityType)
    {
        return mfMouseSensivity[(int)eSnsivityType];
    }   //민감도 값 반환
}
