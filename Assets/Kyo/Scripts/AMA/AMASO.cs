using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AMATypes // ����
{
    Type_Transport, // �^��
    Type_Cleanup,   // ��
    Type_Mobility,  // �@��
    Type_None,
}

// �����ŕ\��Q�Ǝ��AString�Ƃ��Ĉ�����
public enum AMAs
{
    Higashi,    // ���V�i
    Gibraltar,  // �W�u�����^��
    Arctic,     // �k��
    Polynesia,  // �|���l�V�A
    Panama,     // �p�i�}
    Mozambique, // ���U���r�[�N
    Max,
}

[CreateAssetMenu(fileName = "AMA_", menuName = "AMA")]
public class AMASO : ScriptableObject
{
    public string ama;
    public Sprite icon;
    [TextArea(5, 20)] public string nature;
    public AMATypes type = AMATypes.Type_None;
    static public string[] features = { "�ł��鎖������", "������", "���͂₢", "" };
    [ReadOnly] public string feature = features[(int)AMATypes.Type_None];
    public float timePerGrid;
    public int energy;

    // Inspector�ɂ��鑮����ҏW�����Editor�ɔ��f���Ă����R�[���o�b�N
    void OnValidate()
    {
        feature = features[(int)type];
    }

    static public  CharacterTypes ConvertAMAsToCharacterTypes(AMAs AMA)
    {
        return (CharacterTypes)(AMA + 3);
    }
}
