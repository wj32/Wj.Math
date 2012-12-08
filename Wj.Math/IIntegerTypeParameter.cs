using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wj.Math
{
    public interface TIntegerType
    {
        int GetValue();
    }

    public class TInteger0 : TIntegerType { public int GetValue() { return 0; } }
    public class TInteger1 : TIntegerType { public int GetValue() { return 1; } }
    public class TInteger2 : TIntegerType { public int GetValue() { return 2; } }
    public class TInteger3 : TIntegerType { public int GetValue() { return 3; } }
    public class TInteger4 : TIntegerType { public int GetValue() { return 4; } }
    public class TInteger5 : TIntegerType { public int GetValue() { return 5; } }
    public class TInteger6 : TIntegerType { public int GetValue() { return 6; } }
    public class TInteger7 : TIntegerType { public int GetValue() { return 7; } }
    public class TInteger8 : TIntegerType { public int GetValue() { return 8; } }
    public class TInteger9 : TIntegerType { public int GetValue() { return 9; } }
}
