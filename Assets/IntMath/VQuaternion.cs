using System;

public struct VQuaternion : IEquatable<VQuaternion>
{
    public const int Precision = 0x3e8;//1000
    public const float FloatPrecision = 1000f;
    public const float PrecisionFactor = 0.001f;
    public int x;
    public int y;
    public int z;
    public int w;

    public static readonly VQuaternion identity;

    public VInt3 xyz
    {
        get
        {
            return new VInt3(x, y, z);
        }
    }

    static VQuaternion()
    {
        identity = new VQuaternion(0, 0, 0, Precision);
    }

    private VQuaternion(int pitch, int yaw, int roll)
    {
        yaw /= 2;
        pitch /= 2;
        roll /= 2;

        VFactor c1, c2, c3, s1, s2, s3;
        IntMath.sincos(out s1, out c1, new VFactor(yaw, Precision) * VFactor.pi / 180L);
        IntMath.sincos(out s2, out c2, new VFactor(pitch, Precision) * VFactor.pi / 180L);
        IntMath.sincos(out s3, out c3, new VFactor(roll, Precision) * VFactor.pi / 180L);

        w = ((c1 * c2 * c3 - s1 * s2 * s3) * Precision).integer;
        x = ((s1 * s2 * c3 + c1 * c2 * s3) * Precision).integer;
        y = ((s1 * c2 * c3 + c1 * s2 * s3) * Precision).integer;
        z = ((c1 * s2 * c3 - s1 * c2 * s3) * Precision).integer;
    }

    public VQuaternion(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static VQuaternion Euler(VInt3 eulerAngles)
    {
        return new VQuaternion(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}, {2}, {3})", x, y, z, w);
    }

    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        VQuaternion other = (VQuaternion)o;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = w.GetHashCode();
            hashCode = (hashCode * 397) ^ x.GetHashCode();
            hashCode = (hashCode * 397) ^ y.GetHashCode();
            hashCode = (hashCode * 397) ^ z.GetHashCode();
            return hashCode;
        }
    }

    public bool Equals(VQuaternion other)
    {
        return (((this.x == other.x) && (this.y == other.y)) && (this.z == other.z) && (this.w == other.w));
    }

    /// <summary>
    /// Adds two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    public static VQuaternion operator +(VQuaternion left, VQuaternion right)
    {
        left.x += right.x;
        left.y += right.y;
        left.z += right.z;
        left.w += right.w;

        return left;
    }

    /// <summary>
    /// Subtracts two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    public static VQuaternion operator -(VQuaternion left, VQuaternion right)
    {
        left.x -= right.x;
        left.y -= right.y;
        left.z -= right.z;
        left.w -= right.w;

        return left;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>The result of the calculation.</returns>
    public static VQuaternion operator *(VQuaternion left, VQuaternion right)
    {
        Multiply(ref left, ref right, out left);
        return left;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    public static VQuaternion operator *(VQuaternion quaternion, VFactor scale)
    {
        Multiply(ref quaternion, scale, out quaternion);
        return quaternion;
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    public static VQuaternion operator *(VFactor scale, VQuaternion quaternion)
    {
        return new VQuaternion(quaternion.x * scale, quaternion.y * scale, quaternion.z * scale, quaternion.w * scale);
    }

    /// <summary>
    /// Compares two instances for equality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left equals right; false otherwise.</returns>
    public static bool operator ==(VQuaternion left, VQuaternion right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two instances for inequality.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>True, if left does not equal right; false otherwise.</returns>
    public static bool operator !=(VQuaternion left, VQuaternion right)
    {
        return !left.Equals(right);
    }

    public static implicit operator UnityEngine.Quaternion(VQuaternion ob)
    {
        return new UnityEngine.Quaternion(ob.x * PrecisionFactor, ob.y * PrecisionFactor, ob.z * PrecisionFactor, ob.w * PrecisionFactor);
    }

    //GG Add
    public static long Dot(VQuaternion a, VQuaternion b)
    {
        return a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
    }

    //GG Add
    public static VQuaternion Inverse(VQuaternion rotation)
    {
        //VFactor invNorm = 1000 / ((rotation.x * rotation.x) + (rotation.y * rotation.y) + (rotation.z * rotation.z) + (rotation.w * rotation.w));
        VFactor invNorm = new VFactor(1000, ((rotation.x * rotation.x) + (rotation.y * rotation.y) + (rotation.z * rotation.z) + (rotation.w * rotation.w)));
        return VQuaternion.Multiply(VQuaternion.Conjugate(rotation), invNorm);
    }

    //GG Add
    public static VQuaternion Conjugate(VQuaternion value)
    {
        VQuaternion quaternion;
        quaternion.x = -value.x;
        quaternion.y = -value.y;
        quaternion.z = -value.z;
        quaternion.w = value.w;
        return quaternion;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    public static VQuaternion Multiply(VQuaternion left, VQuaternion right)
    {
        VQuaternion result;
        Multiply(ref left, ref right, out result);
        return result;
    }

    /// <summary>
    /// Multiplies two instances.
    /// </summary>
    /// <param name="left">The first instance.</param>
    /// <param name="right">The second instance.</param>
    /// <param name="result">A new instance containing the result of the calculation.</param>
    public static void Multiply(ref VQuaternion left, ref VQuaternion right, out VQuaternion result)
    {
        int ci = ((left.x * right.w) + (left.y * right.z) - (left.z * right.y) + (left.w * right.x) / Precision);
        int cj = ((-left.x * right.z) + (left.y * right.w) + (left.z * right.x) + (left.w * right.y) / Precision);
        int ck = ((left.x * right.y) - (left.y * right.x) + (left.z * right.w) + (left.w * right.z) / Precision);
        int cr = ((-left.x * right.x) - (left.y * right.y) - (left.z * right.z) + (left.w * right.w) / Precision);
        result = new VQuaternion(cr, ci, cj, ck);
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <param name="result">A new instance containing the result of the calculation.</param>
    public static void Multiply(ref VQuaternion quaternion, VFactor scale, out VQuaternion result)
    {
        result = new VQuaternion(quaternion.x * scale, quaternion.y * scale, quaternion.z * scale, quaternion.w * scale);
    }

    /// <summary>
    /// Multiplies an instance by a scalar.
    /// </summary>
    /// <param name="quaternion">The instance.</param>
    /// <param name="scale">The scalar.</param>
    /// <returns>A new instance containing the result of the calculation.</returns>
    public static VQuaternion Multiply(VQuaternion quaternion, VFactor scale)
    {
        return new VQuaternion(quaternion.x * scale, quaternion.y * scale, quaternion.z * scale, quaternion.w * scale);
    }
}
