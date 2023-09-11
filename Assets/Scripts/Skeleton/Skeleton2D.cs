using System;
using UnityEngine;

public class Skeleton2D : MonoBehaviour
{
    [SerializeField] private Bone2D[] bones;

    private void Awake()
    {
        foreach (var bone in bones)
            bone.Init();
    }

    public void SetOutfit(Bone2D.Type boneType, Sprite sprite)
    {
        Bone2D bone = Array.Find(bones, x => x.Id == boneType);

        if (bone != null)
            bone.Sprite = sprite;
    }
}

[System.Serializable]
public class Bone2D
{
    public enum Type
    {
        WristL = 0,
        WristR,
        ElbowL,
        ElbowR,
        ShoulderL,
        ShoulderR,
        WeaponL,
        WeaponR,
        Torso,
        Pelvis,
        LegL,
        LegR,
        BootL,
        BootR,
        Head,
        Face,
        Hood
    };

    public Type Id;
    public Transform Origin;
    public Sprite Sprite
    {
        get => m_spriteRenderer.sprite;
        set => m_spriteRenderer.sprite = value;
    }

    private SpriteRenderer m_spriteRenderer;

    public void Init()
    {
        m_spriteRenderer = Origin.GetComponent<SpriteRenderer>();
    }
}