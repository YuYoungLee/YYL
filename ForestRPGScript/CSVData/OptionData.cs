public enum EVolume
{
    Master,         //������ ����
    SFX,            //ȿ����
    BGM,            //�����
}

public enum ESensivity
{
    AxisXY,     //xy ��ü ��ġ ����
    AxisX,      //x ��ġ ����
    AxisY,      //y ��ġ ����
}

public enum EResolution
{
    HD,     //1280 x 720
    FHD,    //1920 x 1080
}

public class OptionData
{
    private float[] mfVolume;   //����
    private float[] mfMouseSensivity;    //���콺 �ΰ���
    private bool mbAxisYInverseStatu;    //Y�� ���� ����
    private bool mbAxisXYSensivityStatu;    //XY�� �ΰ��� ����
    private EResolution meResolution;   //�ػ�
    private ESensivity meSensivityType;     //�ΰ��� Ÿ��

    public void Copy(OptionData other)
    {
        //���� �� ����
        for(int i = 0; i < mfVolume.Length; ++i)
        {
            mfVolume[i] = other.mfVolume[i];
        }

        //���콺 �ΰ��� �� ����
        for(int i = 0; i < mfMouseSensivity.Length; ++i)
        {
            mfMouseSensivity[i] = other.mfMouseSensivity[i];
        }

        mbAxisYInverseStatu = other.mbAxisYInverseStatu;    //Y�� ���� ����
        mbAxisXYSensivityStatu = other.mbAxisXYSensivityStatu;      //XY�� �ΰ��� ����
        meResolution = other.meResolution;    //�ػ�
        meSensivityType = other.meSensivityType;    //�ΰ��� Ÿ��
    }   //������ ����

    public void Initialize()
    {
        //EVolume idx ��ġ�� ���� �����̴� ��
        if (mfVolume == null)
        {
            mfVolume = new float[3] { 1.0f, 1.0f, 1.0f };
        }

        //ESensivity idx ��ġ�� �ΰ��� �����̴� ��
        if (mfMouseSensivity == null)
        {
            mfMouseSensivity = new float[3] { 50.0f, 50.0f, 50.0f };
        }

        ResetData();
    }

    public void ResetData()
    {
        //������ �ʱ�ȭ
        for(int i = 0; i < mfVolume.Length; ++i)
        {
            mfVolume[i] = 1.0f;
        }

        //���콺 �ΰ��� �ʱ�ȭ
        for(int i = 0; i < mfMouseSensivity.Length; ++i)
        {
            mfMouseSensivity[i] = 50.0f;
        }

        mbAxisYInverseStatu = false;        //Y�� �������� �ʱ�ȭ
        mbAxisXYSensivityStatu = false;      //XY�� �ΰ��� ���� �ʱ�ȭ

        meResolution = EResolution.FHD;    //ȭ�� �ػ� ���� �ʱ�ȭ
        meSensivityType = ESensivity.AxisXY;    //XY�� �ΰ��� �� �ʱ�ȭ
    }   //�ɼ� ������ �ʱ�ȭ

    public EResolution Resolution
    {
        set { meResolution = value; }
        get { return meResolution; }
    }   //�ػ� ��

    public ESensivity SensivityType
    {
        set { meSensivityType = value; }
        get { return meSensivityType; }
    }   //���콺 �ΰ���

    public bool AxisYInverseStatu
    {
        set { mbAxisYInverseStatu = value; }
        get { return mbAxisYInverseStatu; }
    }   //Y�� ����

    public bool AxisXYSensivityStatu
    {
        set { mbAxisXYSensivityStatu = value; }
        get { return mbAxisXYSensivityStatu; }
    }   //XY�� �ΰ��� ����

    /// <summary>
    /// eVolumeType : ���� Ÿ��
    /// fVolume : ���� ��
    /// </summary>
    public void SetVolume(EVolume eVolumeType, float fVolume)
    {
        mfVolume[(int)eVolumeType] = fVolume;
    }   //���� Ÿ���� �� ����

    /// <summary>
    /// eVolumeType : ���� Ÿ��
    /// </summary>
    public float GetVolume(EVolume eVolumeType)
    {
        return mfVolume[(int)eVolumeType];
    }   //���� Ÿ���� �� ��ȯ

    /// <summary>
    /// eSnsivityType : �ΰ��� Ÿ��
    /// fSensivity : �ΰ��� ��
    /// </summary>
    public void SetSensivity(ESensivity eSnsivityType, float fSensivity)
    {
        mfMouseSensivity[(int)eSnsivityType] = fSensivity;
    }   //�ΰ��� �� ����

    /// <summary>
    /// eSnsivityType : �ΰ��� Ÿ��
    /// </summary>
    public float GetSensivity(ESensivity eSnsivityType)
    {
        return mfMouseSensivity[(int)eSnsivityType];
    }   //�ΰ��� �� ��ȯ
}
