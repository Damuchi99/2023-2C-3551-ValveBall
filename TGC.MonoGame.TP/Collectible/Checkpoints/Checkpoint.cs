using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Collectible.Checkpoints;

public class Checkpoint : Collectible
{
    private const float DefaultScale = 0.1f;
    private const float DefaultXRotation = -MathHelper.PiOver2;

    public Checkpoint(Vector3 position) 
        : base(new BoundingBox(new Vector3(-4, -8, -6) + position, new Vector3(4, 8, 6) + position))
    {
        Position = position;
        Scale = DefaultScale;
        World = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(DefaultXRotation) * Matrix.CreateTranslation(position);
    }
    
    protected override void UpdateAnimation(GameTime gameTime)
    { }

    protected override void OnCollected(Player player)
    {
        player.ChangeRestartPosition(player.SpherePosition);
    }
}