//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;

public class Char
{
    public static char[] zalgoUp =
    {
        (char) 0x030d, (char) 0x030e, (char) 0x0304, (char) 0x0305,
        (char) 0x033f, (char) 0x0311, (char) 0x0306, (char) 0x0310,
        (char) 0x0352, (char) 0x0357, (char) 0x0351, (char) 0x0307,
        (char) 0x0308, (char) 0x030a, (char) 0x0342, (char) 0x0343,
        (char) 0x0344, (char) 0x034a, (char) 0x034b, (char) 0x034c,
        (char) 0x0303, (char) 0x0302, (char) 0x030c, (char) 0x0350,
        (char) 0x0300, (char) 0x0301, (char) 0x030b, (char) 0x030f,
        (char) 0x0312, (char) 0x0313, (char) 0x0314, (char) 0x033d,
        (char) 0x0309, (char) 0x0363, (char) 0x0364, (char) 0x0365,
        (char) 0x0366, (char) 0x0367, (char) 0x0368, (char) 0x0369,
        (char) 0x036a, (char) 0x036b, (char) 0x036c, (char) 0x036d,
        (char) 0x036e, (char) 0x036f, (char) 0x033e, (char) 0x035b,
    };

    public static char[] zalgoDown =
    {
        (char) 0x0316, (char) 0x0317, (char) 0x0318, (char) 0x0319,
        (char) 0x031c, (char) 0x031d, (char) 0x031e, (char) 0x031f,
        (char) 0x0320, (char) 0x0324, (char) 0x0325, (char) 0x0326,
        (char) 0x0329, (char) 0x032a, (char) 0x032b, (char) 0x032c,
        (char) 0x032d, (char) 0x032e, (char) 0x032f, (char) 0x0330,
        (char) 0x0331, (char) 0x0332, (char) 0x0333, (char) 0x0339,
        (char) 0x033a, (char) 0x033b, (char) 0x033c, (char) 0x0345,
        (char) 0x0347, (char) 0x0348, (char) 0x0349, (char) 0x034d,
        (char) 0x034e, (char) 0x0353, (char) 0x0354, (char) 0x0355,
        (char) 0x0356, (char) 0x0359, (char) 0x035a, (char) 0x0323
    };

    public static char[] zalgoMid =
    {
        (char) 0x0315, (char) 0x031b, (char) 0x0340, (char) 0x0341,
        (char) 0x0358, (char) 0x0321, (char) 0x0322, (char) 0x0327,
        (char) 0x0328, (char) 0x0334, (char) 0x0335, (char) 0x0336,
        (char) 0x034f, (char) 0x035c, (char) 0x035d, (char) 0x035e,
        (char) 0x035f, (char) 0x0360, (char) 0x0362, (char) 0x0338,
        (char) 0x0337, (char) 0x0361, (char) 0x0489
    };
}

public class ZalgoGenerator
{
    static uint m_zalgoUpLen;
    static uint m_zalgoDownLen;
    static uint m_zalgoMidLen;

    static Random rnd = new Random();

    static bool isZalgoChar(char c)
    {
        uint i = 0;
        for (i = 0; i < m_zalgoUpLen; i++)
            if (c == Char.zalgoUp[i])
                return true;
        for (i = 0; i < m_zalgoDownLen; i++)
            if (c == Char.zalgoDown[i])
                return true;
        for (i = 0; i < m_zalgoMidLen; i++)
            if (c == Char.zalgoMid[i])
                return true;
        return false;
    }

    static uint randomNumber(int max)
    {
        return (uint) rnd.Next(max);
    }

    static char getZalgoChar(char[] arr, uint n)
    {
        int index = rnd.Next((int) n);
        return arr[index];
    }

    public static string generateString(string str, ZalgoIntensity intensity, bool up, bool mid, bool down)
    {
        string ret = "";
        int strLen = str.Length;
        char[] s = str.ToCharArray();
        m_zalgoUpLen = (uint) Char.zalgoUp.Length;
        m_zalgoDownLen = (uint) Char.zalgoDown.Length;
        m_zalgoMidLen = (uint) Char.zalgoMid.Length;

        for (int i = 0; i < strLen; i++)
        {
            if (isZalgoChar(s[i]))
                continue;

            uint numUp = 0;
            uint numMid = 0;
            uint numDown = 0;

            ret += s[i];

            if (s[i] == '\r' || s[i] == '\n')
                continue;
            switch (intensity)
            {
                case ZalgoIntensity.MINI:
                    numUp = randomNumber(8);
                    numMid = randomNumber(2);
                    numDown = randomNumber(8);
                    break;
                case ZalgoIntensity.NORMAL:
                    numUp = randomNumber(16) / 2 + 1;
                    numMid = randomNumber(6) / 2;
                    numDown = randomNumber(16) / 2 + 1;
                    break;
                case ZalgoIntensity.MAXI:
                    numUp = randomNumber(64) / 4 + 3;
                    numMid = randomNumber(16) / 4 + 1;
                    numDown = randomNumber(64) / 4 + 3;
                    break;
            }

            if (up)
                for (int j = 0; j < numUp; j++)
                    ret += getZalgoChar(Char.zalgoUp, m_zalgoUpLen);
            if (mid)
                for (int j = 0; j < numMid; j++)
                    ret += getZalgoChar(Char.zalgoMid, m_zalgoMidLen);
            if (down)
                for (int j = 0; j < numDown; j++)
                    ret += getZalgoChar(Char.zalgoDown, m_zalgoDownLen);
        }

        return ret;
    }

    public enum ZalgoIntensity
    {
        MINI,
        NORMAL,
        MAXI
    }
}