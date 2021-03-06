using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AMATypes // 相性
{
    Type_Transport, // 運搬
    Type_Cleanup,   // 浄化
    Type_Mobility,  // 機動
    Type_None,
}

// 辞書で表情参照時、Stringとして扱われる
public enum AMAs
{
    Higashi,    // 東シナ
    Gibraltar,  // ジブラルタル
    Arctic,     // 北極
    Polynesia,  // ポリネシア
    Panama,     // パナマ
    Mozambique, // モザンビーク
    Max,
}

[CreateAssetMenu(fileName = "AMA_", menuName = "AMA")]
public class AMASO : ScriptableObject
{
    public string ama;
    public Sprite icon;
    [TextArea(5, 20)] public string nature;
    public AMATypes type = AMATypes.Type_None;
    static public string[] features = { "できる事が多い", "汚染浄化", "足はやい", "" };
    [ReadOnly] public string feature = features[(int)AMATypes.Type_None];
    public float timePerGrid;
    public int energy;
    public Vector2 baseOffset = Vector2.zero;
    public Vector2 mainOffset = Vector2.zero;

    // Inspectorにある属性を編集するとEditorに反映してくれるコールバック
    void OnValidate()
    {
        feature = features[(int)type];
    }

    static public  CharacterTypes ConvertAMAsToCharacterTypes(AMAs AMA)
    {
        return (CharacterTypes)(AMA + 3);
    }
}
