using System;
using System.Runtime.InteropServices;

[Serializable, StructLayout(LayoutKind.Sequential)]
public struct VInt
{
    public int i;
    public VInt(int i)
    {
        this.i = i;
    }

    public VInt(float f)
    {
        this.i = MMGame_Math.RoundToInt((double) (f * VInt3.Precision));
    }

    public override bool Equals(object o)
    {
        if (o == null)
        {
            return false;
        }
        VInt num = (VInt) o;
        return (this.i == num.i);
    }

    public override int GetHashCode()
    {
        return this.i.GetHashCode();
    }

    public static VInt Min(VInt a, VInt b)
    {
        return new VInt(Math.Min(a.i, b.i));
    }

    public static VInt Max(VInt a, VInt b)
    {
        return new VInt(Math.Max(a.i, b.i));
    }

    public override string ToString()
    {
        return this.scalar.ToString();
    }

    public float scalar
    {
        get
        {
            return (this.i * VInt3.PrecisionFactor);
        }
    }
    public static explicit operator VInt(float f)
    {
        return new VInt(MMGame_Math.RoundToInt((double) (f * VInt3.Precision)));
    }

    public static implicit operator VInt(int i)
    {
        return new VInt(i);
    }

    public static explicit operator float(VInt ob)
    {
        return (ob.i * VInt3.PrecisionFactor);
    }

    public static explicit operator long(VInt ob)
    {
        return (long) ob.i;
    }

    public static VInt operator +(VInt a, VInt b)
    {
        return new VInt(a.i + b.i);
    }

    public static VInt operator -(VInt a, VInt b)
    {
        return new VInt(a.i - b.i);
    }

    public static bool operator ==(VInt a, VInt b)
    {
        return (a.i == b.i);
    }

    public static bool operator !=(VInt a, VInt b)
    {
        return (a.i != b.i);
    }
}

