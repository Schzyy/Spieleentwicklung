using Godot;
using System.Collections.Generic;

public partial class BulletPoolComponent : Node
{
    [Export] private PackedScene bulletScene;
    [Export] private int poolSize = 50;

    private readonly List<Bullet> _bullets = new();

    public override void _Ready()
    {
        if (bulletScene == null)
        {
            GD.PushError("BulletPool: Keine BulletScene gesetzt!");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            var bullet = bulletScene.Instantiate<Bullet>();
            bullet.Visible = false;
            bullet.SetProcess(false);
            AddChild(bullet);
            _bullets.Add(bullet);
        }
    }

    public Bullet GetBullet()
    {
        foreach (var b in _bullets)
        {
            if (!b.IsActive)
                return b;
        }

        GD.Print("⚠️ BulletPool leer – evtl. Poolgröße erhöhen!");
        return null;
    }
}
