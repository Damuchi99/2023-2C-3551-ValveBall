﻿using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Platform;

public class Platform : Prefab
{
    public BoundingBox BoundingBox { get; set; }

    public Platform(Vector3 scale, Vector3 position, Material material) : base(scale, position, material)
    {
        BoundingBox = BoundingVolumesExtensions.FromMatrix(World);
    }
}