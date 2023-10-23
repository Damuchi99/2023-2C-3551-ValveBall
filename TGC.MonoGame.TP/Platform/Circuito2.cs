using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TGC.MonoGame.TP.Collisions;

namespace TGC.MonoGame.TP.Platform;

public static class Circuito2
{
    public static readonly List<Matrix> PlatformMatrices =  new();
    public static readonly List<BoundingBox> PlatformAabb =  new();
    public static readonly List<MovingPlatform> MovingPlatforms =  new();
    
    public static void JumpingElevator()
    {
        // eliminar el "nivel 1"
            
        var height = 0f;
        const float heightIncrement = 35;
        var platformSize = new Vector3(5000f, 6f, 50f);
        for (int floors = 0; floors < 3; floors++) {
            CreatePlatform(platformSize, new Vector3(0f, -40f, 0f));
            height += heightIncrement;
            CreatePlatform(platformSize, new Vector3(50f, height, 0f));
            height += heightIncrement;
            CreatePlatform(platformSize, new Vector3(50f, height, 50f));
            height += heightIncrement;
            CreatePlatform(platformSize, new Vector3(0f, height, 50f));
            height += heightIncrement;
        }
            
        CreatePlatform(platformSize, new Vector3(0f, height, 0f)); // checkpoint
        
        AcceleratedJumps(height);
    }

    private static void CreateMovingPlatform(Vector3 scale, Vector3 position)
    {
        var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        var platformBoundingBox = BoundingVolumesExtensions.FromMatrix(platformWorld);
        var movingPlatform = new MovingPlatform(platformWorld, scale, position, platformBoundingBox);
        MovingPlatforms.Add(movingPlatform);
    }

    public static void UpdateMovingPlatforms()
    {
        foreach (var movingPlatform in MovingPlatforms)
        {
            movingPlatform.Update();
        }
    }
    
    public static void AcceleratedJumps(float height)
    {
        float separation = -80;
        var platformSize = new Vector3(50f, 6f, 50f);
            
        for (int jumps = 2; jumps < 8; jumps++) {
            CreatePlatform(platformSize, new Vector3(0f, height, separation));
            separation -= 40f * jumps;
        }
            
        CreatePlatform(platformSize, new Vector3(0f, height, separation)); // checkpoint
        
        ObstacleRace(separation - 75, height);
    }

    public static void ObstacleRace(float separation, float height)
    {
        var platformSize = new Vector3(100f, 6f, 100f);

        for (int platforms = 0; platforms < 10; platforms++) {
            CreatePlatform(platformSize, new Vector3(0f, height, separation - 100f * platforms));
            // agregar obstaculos fijos
        }
            
        for (int platforms = 10; platforms < 20; platforms++) {
            CreatePlatform(platformSize, new Vector3(0f, height, separation - 100f * platforms));
            // agregar obstaculos moviles
        }
    }
    
    private static void CreatePlatform(Vector3 scale, Vector3 position)
    {
        var platformWorld = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
        PlatformAabb.Add(BoundingVolumesExtensions.FromMatrix(platformWorld));
        PlatformMatrices.Add(platformWorld);
    }
}



