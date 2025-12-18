using Godot;
using System.Collections.Generic;

public partial class HexMathTest : Node3D
{
    [Export] public int Radius = 3;
    [Export] public float TileSize = 1.0f;
    [Export] public float MarkerHeight = 0.05f;

    public override void _Ready()
    {
        GenerateDebugGrid();
        TestRoundTrip();
    }

    private void GenerateDebugGrid()
    {
        for (int q = -Radius; q <= Radius; q++)
        {
            for (int r = -Radius; r <= Radius; r++)
            {
                int s = -q - r;
                if (Mathf.Abs(s) <= Radius)
                {
                    Vector3 pos = HexMath.AxialToWorld(q, r, TileSize);
                    // create a small cylinder marker and a Label3D showing "q,r"
                    var mesh = new CylinderMesh();
                    mesh.TopRadius = 0.1f * TileSize;
                    mesh.BottomRadius = 0.1f * TileSize;
                    mesh.Height = MarkerHeight;
                    var mi = new MeshInstance3D();
                    mi.Mesh = mesh;
                    mi.Position = pos + new Vector3(0, MarkerHeight * 0.5f, 0);
                    AddChild(mi);

                    var label = new Label3D();
                    label.Text = $"{q},{r}";
                    label.Position = pos + new Vector3(0, MarkerHeight + 0.02f, 0);
                    labelBillboard(label);
                    AddChild(label);
                }
            }
        }
    }

    // set label to billboard so it faces camera in editor/runtime
    private void labelBillboard(Label3D label)
    {
        label.Billboard = BaseMaterial3D.BillboardModeEnum.Enabled;    }

    private void TestRoundTrip()
    {
        var tests = new List<Vector3> {
            new Vector3(0,0,0),
            new Vector3(1.5f * TileSize, 0, HexMath.SQRT3 * 0.5f * TileSize),
            new Vector3(2.0f * TileSize, 0, 0.5f * TileSize)
        };

        GD.Print("=== HexMath roundtrip tests ===");
        foreach (var w in tests)
        {
            var axial = HexMath.WorldToAxial(w, TileSize);
            var back = HexMath.AxialToWorld(axial.X, axial.Y, TileSize);
            GD.Print($"World {w} -> Axial {axial.X},{axial.Y} -> World {back}");
        }
    }
}
