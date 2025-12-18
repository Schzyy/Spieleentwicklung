using Godot;
using System;

public static class HexMath
{
    // Flat-top, axial coordinates (q, r)
    // size = hex radius (center to corner)

    public const float SQRT3 = 1.7320508075688772f;

    // Axial -> Weltposition (y = 0). Returns Vector3(x, 0, z).
    public static Vector3 AxialToWorld(int q, int r, float size)
    {
        float x = size * (1.5f * q);
        float z = size * (SQRT3 * (r + q * 0.5f));
        return new Vector3(x, 0f, z);
    }

    // Same but with float axial coords (useful when working with fractional positions)
    public static Vector3 AxialToWorld(float q, float r, float size)
    {
        float x = size * (1.5f * q);
        float z = size * (SQRT3 * (r + q * 0.5f));
        return new Vector3(x, 0f, z);
    }

    // World -> axial (returns integer axial coordinates, rounded correctly)
    // worldPosition.Y wird ignoriert (wir arbeiten auf XZ Ebene)
    public static Vector2I WorldToAxial(Vector3 worldPos, float size)
    {
        // convert world x,z to fractional axial (qf, rf)
        float xf = worldPos.X / size;
        float zf = worldPos.Z / size;

        // inverse transform for flat-top:
        // q = (2/3) * x / size
        // r = (-1/3) * x/size + (sqrt(3)/3) * z/size
        float qf = (2f / 3f) * xf;
        float rf = (-1f / 3f) * xf + (1f / SQRT3) * zf; // sqrt(3)/3 == 1/sqrt(3)

        // convert to cube fractional, round, convert back to axial ints
        Vector3 cubeFrac = AxialFractionalToCube(qf, rf);
        Vector3 cubeRounded = RoundCube(cubeFrac);

        int q = (int)cubeRounded.X; // cube x -> axial q
        int r = (int)cubeRounded.Z; // cube z -> axial r

        return new Vector2I(q, r);
    }

    // Return fractional cube from fractional axial qf, rf
    private static Vector3 AxialFractionalToCube(float qf, float rf)
    {
        float x = qf;
        float z = rf;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    // Round cube coordinates to nearest integer cube coordinate (standard algorithm)
    // returns a Vector3 with integer components stored as floats (X, Y, Z)
    private static Vector3 RoundCube(Vector3 cube)
    {
        int rx = Mathf.RoundToInt(cube.X);
        int ry = Mathf.RoundToInt(cube.Y);
        int rz = Mathf.RoundToInt(cube.Z);

        float xDiff = Mathf.Abs(rx - cube.X);
        float yDiff = Mathf.Abs(ry - cube.Y);
        float zDiff = Mathf.Abs(rz - cube.Z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3(rx, ry, rz);
    }

    // Utility: axial neighbor offsets (flat-top)
    // neighbors: 0..5
    private static readonly Vector2I[] axialNeighbors = new Vector2I[] {
        new Vector2I(+1,  0),
        new Vector2I(+1, -1),
        new Vector2I( 0, -1),
        new Vector2I(-1,  0),
        new Vector2I(-1, +1),
        new Vector2I( 0, +1)
    };

    // Get neighbor axial coordinate
    public static Vector2I AxialNeighbor(int q, int r, int direction)
    {
        if (direction < 0 || direction > 5) throw new ArgumentOutOfRangeException(nameof(direction));
        var off = axialNeighbors[direction];
        return new Vector2I(q + off.X, r + off.Y);
    }

    // Distance between two axial hexes (in steps)
    public static int AxialDistance(int q1, int r1, int q2, int r2)
    {
        // convert to cube and compute max delta
        int x1 = q1;
        int z1 = r1;
        int y1 = -x1 - z1;

        int x2 = q2;
        int z2 = r2;
        int y2 = -x2 - z2;

        return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2) + Math.Abs(z1 - z2)) / 2;
    }
}
