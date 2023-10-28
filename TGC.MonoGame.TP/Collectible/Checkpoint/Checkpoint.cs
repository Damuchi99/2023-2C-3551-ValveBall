using Microsoft.Xna.Framework;

namespace TGC.MonoGame.TP.Collectible.Checkpoint;

public class Checkpoint : Collectible
{
    private const float DefaultScale = 0.1f;

    public Checkpoint(Vector3 position) 
        : base(new BoundingBox(new Vector3(-4, -8, -6) + position, new Vector3(4, 8, 6) + position))
    {
        Position = position;
        Scale = DefaultScale;
    }

    protected override void OnCollected(Player player)
    {
        player.ChangeRestartPosition(Position);
    }
}