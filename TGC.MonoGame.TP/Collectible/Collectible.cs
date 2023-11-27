﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Collectible;

public abstract class Collectible
{
    public BoundingBox BoundingBox { get; private set; }
    private bool CanInteract { get; set; }
    public bool ShouldDraw { get; private set; }
    protected Vector3 Position { get; set; }
    protected float Scale { get; init; }
    public Matrix World { get; protected set; }
    public Model Model { get; set; }
    public Effect Shader { get; set; }
    public SoundEffect Sound { get; set; }
    
    private float _totalElapsedTime;
    private const float Amplitude = 0.15f;
    private const float VerticalSpeed = 2f;
    private const float RotationSpeed = 1.25f;
    
    protected Collectible(BoundingBox boundingBox)
    {
        BoundingBox = boundingBox;
        CanInteract = true;
        ShouldDraw = true;
        Model = null;
        _totalElapsedTime = 0f;
    }

    public virtual void Update(GameTime gameTime, Player.Player player)
    {
        HandleCollection(player);
        UpdateAnimation(gameTime);
    }
    
    public abstract void Draw(GameTime gameTime, Camera camera, GraphicsDevice graphicsDevice);

    protected void DrawGizmos()
    {
        if (!TGCGame.Gizmos.Enabled) return;
        var center = BoundingVolumesExtensions.GetCenter(BoundingBox);
        var extents = BoundingVolumesExtensions.GetExtents(BoundingBox);
        TGCGame.Gizmos.DrawCube(center, extents * 2f, Color.Orange);
    }

    protected virtual void UpdateAnimation(GameTime gameTime)
    {
        var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _totalElapsedTime += elapsedSeconds;
        var verticalOffset = Amplitude * (float)Math.Cos(_totalElapsedTime * VerticalSpeed);
        var rotationAngle = _totalElapsedTime * RotationSpeed;
        Position = new Vector3(Position.X, Position.Y + verticalOffset, Position.Z);
        BoundingBox = new BoundingBox(BoundingBox.Min + Vector3.Up * verticalOffset, BoundingBox.Max + Vector3.Up * verticalOffset);
        World = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(rotationAngle) * Matrix.CreateTranslation(Position);
    }
    
    protected virtual void StopDrawing()
    {
        ShouldDraw = false;
    }

    private void HandleCollection(Player.Player player)
    {
        if (!CanInteract || !player.BoundingSphere.Intersects(BoundingBox)) return;
        Sound?.Play();
        OnCollected(player);
        StopDrawing();
        CanInteract = false;
    }

    protected abstract void OnCollected(Player.Player player);
}
