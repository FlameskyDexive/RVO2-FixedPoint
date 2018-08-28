using System;

// 分数类, 维护一个分子和一个分母
public struct VFactor
{
    public long nom;
    public long den;

    public static VFactor zero;
    public static VFactor one;
    public static VFactor pi;
    public static VFactor twoPi;

    private static long mask_;
    private static long upper_;
    public VFactor(long n, long d)
    {
        this.nom = n;
        this.den = d;
    }

    static VFactor()
    {
        zero = new VFactor(0L, 1L);
        one = new VFactor(1L, 1L);
        pi = new VFactor(0x7ab8L, 0x2710L);
        twoPi = new VFactor(0xf570L, 0x2710L);
        mask_ = 0x7fffffffffffffffL;
        upper_ = 0xffffffL;
    }

    public int roundInt
    {
        get
        {
            return (int)IntMath.Divide(this.nom, this.den);
        }
    }
    public int integer
    {
        get
        {
            return (int)(this.nom / this.den);
        }
    }
    public float single
    {
        get
        {
            double num = ((double)this.nom) / ((double)this.den);
            return (float)num;
        }
    }
    public bool IsPositive
    {
        get
        {
#if WLOG
            bool check = (this.den != 0L);
            if(!check)
            {
                Debug.LogError("VFactor(IsPositive): denominator is zero !");
            }
#endif
            if (this.nom == 0)
            {
                return false;
            }
            bool flag = this.nom > 0L;
            bool flag2 = this.den > 0L;
            return !(flag ^ flag2);
        }
    }
    public bool IsNegative
    {
        get
        {
#if WLOG
            bool check = (this.den != 0L);
            if(!check)
            {
                Debug.LogError("VFactor(IsNegative): denominator is zero !");
            }
#endif

            if (this.nom == 0)
            {
                return false;
            }
            bool flag = this.nom > 0L;
            bool flag2 = this.den > 0L;
            return (flag ^ flag2);
        }
    }

    public VFactor Abs()
    {
        return new VFactor(Math.Abs(this.nom), Math.Abs(this.den));
    }

    public bool IsZero
    {
        get
        {
            return (this.nom == 0L);
        }
    }
    public override bool Equals(object obj)
    {
        return (((obj != null) && (base.GetType() == obj.GetType())) && (this == ((VFactor)obj)));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public VFactor Inverse
    {
        get
        {
            return new VFactor(this.den, this.nom);
        }
    }
    public override string ToString()
    {
        return this.single.ToString() + ",nom:" + nom + ",den:" + den;
    }

    public void strip()
    {
        while (((this.nom & mask_) > upper_) && ((this.den & mask_) > upper_))
        {
            this.nom = this.nom >> 1;
            this.den = this.den >> 1;
        }
    }

    public static bool operator <(VFactor a, VFactor b)
    {
        long num = a.nom * b.den;
        long num2 = b.nom * a.den;
        return (!((b.den > 0L) ^ (a.den > 0L)) ? (num < num2) : (num > num2));
    }

    public static bool operator >(VFactor a, VFactor b)
    {
        long num = a.nom * b.den;
        long num2 = b.nom * a.den;
        return (!((b.den > 0L) ^ (a.den > 0L)) ? (num > num2) : (num < num2));
    }

    public static bool operator <=(VFactor a, VFactor b)
    {
        long num = a.nom * b.den;
        long num2 = b.nom * a.den;
        return (!((b.den > 0L) ^ (a.den > 0L)) ? (num <= num2) : (num >= num2));
    }

    public static bool operator >=(VFactor a, VFactor b)
    {
        long num = a.nom * b.den;
        long num2 = b.nom * a.den;
        return (!((b.den > 0L) ^ (a.den > 0L)) ? (num >= num2) : (num <= num2));
    }

    public static bool operator ==(VFactor a, VFactor b)
    {
        return ((a.nom * b.den) == (b.nom * a.den));
    }

    public static bool operator !=(VFactor a, VFactor b)
    {
        return ((a.nom * b.den) != (b.nom * a.den));
    }

    public static bool operator <(VFactor a, long b)
    {
        long nom = a.nom;
        long num2 = b * a.den;
        return ((a.den <= 0L) ? (nom > num2) : (nom < num2));
    }

    public static bool operator >(VFactor a, long b)
    {
        long nom = a.nom;
        long num2 = b * a.den;
        return ((a.den <= 0L) ? (nom < num2) : (nom > num2));
    }

    public static bool operator <=(VFactor a, long b)
    {
        long nom = a.nom;
        long num2 = b * a.den;
        return ((a.den <= 0L) ? (nom >= num2) : (nom <= num2));
    }

    public static bool operator >=(VFactor a, long b)
    {
        long nom = a.nom;
        long num2 = b * a.den;
        return ((a.den <= 0L) ? (nom <= num2) : (nom >= num2));
    }

    public static bool operator ==(VFactor a, long b)
    {
        return (a.nom == (b * a.den));
    }

    public static bool operator !=(VFactor a, long b)
    {
        return (a.nom != (b * a.den));
    }

    // notify by 松:需要约分下 发现很多情况下分子或分母溢出了
    public static VFactor operator +(VFactor a, VFactor b)
    {
        return new VFactor { nom = (a.nom * b.den) + (b.nom * a.den), den = a.den * b.den }.FractionReduction();
    }

    public static VFactor operator +(VFactor a, long b)
    {
        a.nom += b * a.den;
        return a;
    }

    // notify by 松:需要约分下 发现很多情况下分子或分母溢出了
    public static VFactor operator -(VFactor a, VFactor b)
    {
        return new VFactor { nom = (a.nom * b.den) - (b.nom * a.den), den = a.den * b.den }.FractionReduction();
    }

    public static VFactor operator -(VFactor a, long b)
    {
        a.nom -= b * a.den;
        return a;
    }

    public static VFactor operator -(long b, VFactor a)
    {
        a.nom = b * a.den - a.nom;
        return a;
    }

    public static VFactor operator *(VFactor a, long b)
    {
        a.nom *= b;
        return a;
    }

    public static VFactor operator *(VInt b, VFactor a)
    {
        a.nom *= (long)b;
        return a;
    }

    public static VFactor operator /(VFactor a, long b)
    {
        a.den *= b;
        return a;
    }

    public static VInt3 operator *(VInt3 v, VFactor f)
    {
        return IntMath.Divide(v, f.nom, f.den);
    }

    /*public static VInt3 operator *(VInt3 v, VFactor f)
    {
        return IntMath.Divide(v, f.nom, f.den);
    }*/

    public static VInt2 operator *(VInt2 v, VFactor f)
    {
        return IntMath.Divide(v, f.nom, f.den);
    }

    // add by 焕松:为什么VFactor没提供 VFactor * VFactor 和 VFactor / VFactor?? 有可能是怕分子分母太大？超过long储存范围？怕外面滥用吗？
    // -------------------
    public static VFactor operator *(VFactor a, VFactor b)
    {
        a.nom *= b.nom;
        a.den *= b.den;
        return a.FractionReduction();
    }

    public static VFactor operator /(VFactor a, VFactor b)
    {
        a.nom *= b.den;
        a.den *= b.nom;
        return a.FractionReduction();
    }

    public VFactor FractionReduction()
    {
        // 先求出最大公约数
        long m = nom;
        long d = den;

        while (d != 0)
        {
            long temp = m % d;
            m = d;
            d = temp;
        }

        nom /= m;
        den /= m;

        return this;
    }
    // -------------------

    public static VInt3 operator /(VInt3 v, VFactor f)
    {
        return IntMath.Divide(v, f.den, f.nom);
    }

    public static VInt2 operator /(VInt2 v, VFactor f)
    {
        return IntMath.Divide(v, f.den, f.nom);
    }

    public static int operator *(int i, VFactor f)
    {
        return (int)IntMath.Divide((long)(i * f.nom), f.den);
    }

    public static VFactor operator -(VFactor a)
    {
        a.nom = -a.nom;
        return a;
    }

    public static explicit operator int(VFactor a)
    {
        return (int)(a.nom * 1000 / a.den);
    }
}

