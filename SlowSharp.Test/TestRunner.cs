﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Slowsharp.Test
{
    public class TestRunner
    {
        public static RunConfig config = new RunConfig();

        public static object RunRaw(string code)
        {
            return CScript.Run(@"
using System;
" + code,
null, config);
        }
        public static object Run(string code)
        {
            return CScript.Run(@"
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Slowsharp.Test;

public class FooTest__ {

public static object Main() {
"
+ code +
@"
}
}",
null, config);
        }

        public static object Run(string classBody, string body)
        {
            return CScript.Run(@"
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Slowsharp.Test;

public class FooTest__ {"
+ classBody +
@"
public static object Main() {
"
+ body +
@"
}
}",
null, config);
        }

        public static object Run(string external, string classBody, string body)
        {
            return CScript.Run(@"
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Slowsharp.Test;

"
+ external +
@"
public class FooTest__ {"
+ classBody +
@"
public static object Main() {
"
+ body +
@"
}
}",
null, config);
        }
    }
}
