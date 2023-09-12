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

    public void SetOutfit(SkeletonOutfit[] outfits)
    {
        foreach (var outfit in outfits)
            SetOutfit(outfit);
    }

    public void SetOutfit(SkeletonOutfit outfit)
    {
        Bone2D bone = Array.Find(bones, x => x.Id == outfit.Type);

        if (bone != null)
            bone.Sprite = outfit.Sprite;
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

[System.Serializable]
public struct SkeletonOutfit
{
    public Bone2D.Type Type;
    public Sprite Sprite;
}