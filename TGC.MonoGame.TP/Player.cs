using System;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TGC.MonoGame.TP;

public class Player
{
    public Vector3 SpherePosition;
    public BoundingSphere SphereCollider { get; set; }
    public float Yaw { get; private set; }
    private readonly Matrix _sphereScale;
    private float _pitch;
    private float _roll;
    private float _speed;
    private float _pitchSpeed; 
    private float _yawSpeed;
    private float _jumpSpeed;
    private bool _isJumping;

    public Player(Matrix sphereScale, Vector3 spherePosition)
    {
        _sphereScale = sphereScale;
        SpherePosition = spherePosition;
    }

    private const float MaxSpeed = 180f;
    private const float PitchMaxSpeed = 15f;
    private const float YawMaxSpeed = 5.8f;
    private const float Acceleration = 60f;
    private const float PitchAcceleration = 5f;
    private const float YawAcceleration = 5f;
    private const float Gravity = 175f;
    private const float MaxJumpHeight = 35f;

    public Matrix Update(float time, KeyboardState keyboardState)
    {
        HandleJumping(time, keyboardState);

        HandleYaw(time, keyboardState);

        var rotationY = Matrix.CreateRotationY(Yaw);
        var forward = rotationY.Forward;

        HandleMovement(time, keyboardState, forward);

        //SphereCollider.Center = SpherePosition;
            
        var rotationX = Matrix.CreateRotationX(_pitch);
        var translation = Matrix.CreateTranslation(SpherePosition);
            
        return _sphereScale * rotationX * rotationY * translation;
    }

    private void HandleMovement(float time, KeyboardState keyboardState, Vector3 forward)
    {
        if (keyboardState.IsKeyDown(Keys.W))
        {
            _speed += Acceleration * time;
            _pitchSpeed -= PitchAcceleration * time;
        }
        else if (keyboardState.IsKeyDown(Keys.S))
        {
            _speed -= Acceleration * time;
            _pitchSpeed += PitchAcceleration * time;
        }
        else
        {
            var decelerationDirection = Math.Sign(_speed) * -1;
            var pitchDecelerationDirection = Math.Sign(_pitchSpeed) * -1;
            _speed += Acceleration * time * decelerationDirection;
            _pitchSpeed += PitchAcceleration * time * pitchDecelerationDirection;
        }

        _pitchSpeed = MathHelper.Clamp(_pitchSpeed, -PitchMaxSpeed, PitchMaxSpeed);
        _speed = MathHelper.Clamp(_speed, -MaxSpeed, MaxSpeed);
        SpherePosition += forward * time * _speed;
        _pitch += _pitchSpeed * time;
    }

    private void HandleYaw(float time, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.A))
        {
            _yawSpeed += YawAcceleration * time;
        }
        else if (keyboardState.IsKeyDown(Keys.D))
        {
            _yawSpeed -= YawAcceleration * time;
        }
        else
        {
            DecelerateYaw(time);
        }

        _yawSpeed = MathHelper.Clamp(_yawSpeed, -YawMaxSpeed, YawMaxSpeed);
        Yaw += _yawSpeed * time;
    }

    private void DecelerateYaw(float time)
    {
        var yawDecelerationDirection = Math.Sign(_yawSpeed) * -1;
        _yawSpeed += YawAcceleration * time * yawDecelerationDirection;
    }

    private void HandleJumping(float time, KeyboardState keyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Space) && !_isJumping)
        {
            StartJump();
        }

        if (_isJumping)
        {
            SpherePosition = CalculateFallPosition(time);
            if (SpherePosition.Y <= 0)
            {
                EndJump();
            }
        }
    }

    private void EndJump()
    {
        _isJumping = false;
        _jumpSpeed = 0;
    }

    private void StartJump()
    {
        _isJumping = true;
        _jumpSpeed = CalculateJumpSpeed();
    }

    private Vector3 CalculateFallPosition(float time)
    {
        _jumpSpeed -= Gravity * time;
        var newYPosition = SpherePosition.Y + _jumpSpeed * time;
        var newPosition = new Vector3(SpherePosition.X, newYPosition, SpherePosition.Z);
        return newPosition;
    }

    private static float CalculateJumpSpeed()
    {
        return (float)Math.Sqrt(2 * MaxJumpHeight * Math.Abs(Gravity));
    }
}