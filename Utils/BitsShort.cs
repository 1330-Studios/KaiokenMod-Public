namespace KaiokenMod.Utils;

/// <summary>
/// Allows easy ways to set bits of a Short
/// </summary>
internal struct BitsShort {
    private short value;

#pragma warning disable IDE0251
    public bool this[int key] {
        get => (value & (1 << key)) != 0;
        set {
            if (value) {
                this.value |= (short)(1 << key);
            } else {
                this.value &= (short)(~(1 << key));
            }
        }
    }

    public BitsShort(bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false,
        bool b6 = false, bool b7 = false, bool b8 = false, bool b9 = false, bool b10 = false, bool b11 = false,
        bool b12 = false, bool b13 = false, bool b14 = false, bool b15 = false, bool b16 = false) {
        value = 0;
        this[0] = b1;
        this[1] = b2;
        this[2] = b3;
        this[3] = b4;
        this[4] = b5;
        this[5] = b6;
        this[6] = b7;
        this[7] = b8;
        this[8] = b9;
        this[9] = b10;
        this[10] = b11;
        this[11] = b12;
        this[12] = b13;
        this[13] = b14;
        this[14] = b15;
        this[15] = b16;
    }

    public override string ToString() {
        var chars = new char[16];
        for (var i = 0; i < chars.Length; i++) {
            chars[i] = this[i] ? '1' : '0';
        }

        return new string(chars);
    }

    public static implicit operator short(BitsShort bb) {
        return bb.value;
    }

    public static implicit operator BitsShort(short b) {
        var result = default(BitsShort);
        result.value = b;
        return result;
    }
}