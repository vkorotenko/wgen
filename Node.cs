namespace wgen;

public class Node
{
    public int  Num {
        get;
        set;
    }
    public int Start {
        get;
        set;
    }

    public int End { get; set; }
    public override string ToString()
    {
        return Num == 1 ? "if (vol >= 0 && vol <= 143) host = '01'" : $"else if (vol >= {Start} && vol <= {End}) host = '{Num:D2}'";
    }
}