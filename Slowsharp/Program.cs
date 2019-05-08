﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Slowsharp
{
    enum KeyCode
    {
        Space
    }
    interface IFoo { }
    class Foo
    {
        public int value;
        public static Foo operator +(Foo a, Foo b)
        {
            return new Foo() { value = a.value + b.value };
        }
    }
    class Input
    {
        public static bool GetKeyDown(KeyCode k)
        {
            return true;
        }
    }
    struct Vector3
    {
        public int x, y, z;
        public Vector3(int x, int y, int z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 operator +(Vector3 a, Vector3 b) { return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z); }
    }
    class Transform
    {
        public Vector3 position { get; set; }
    }

    class Resources
    {
        public static void Load<T>(string path)
        {
            Console.WriteLine(typeof(T) + " / " + path);
        }
    }
    class GameObject
    {
    }

    class Bar
    {
        public Transform transform = new Transform();

        protected int ff = 122;

        public Bar()
        {
            Console.WriteLine("I AM INIT");
        }

        public static void Opt(int n = 1)
        {
            Console.WriteLine("Opt" + n);
        }

        public void SayHello()
        {
            Console.WriteLine("hello from BAR");
        }
        public static void SayHelloStatic()
        {
            Console.WriteLine("hello");
        }
    }
    public class RefOrOutTest
    {
        public static void MakeDoubleOut(int input, out int value)
        {
            value = input * 2;
        }
        public static void MakeDoubleRef(ref int value)
        {
            value *= 2;
        }
    }
    struct St
    {
        public static object a;
    }

    public class SSDebugger
    {
        public static CScript runner;

        public static void Stop()
        {
            var dump = runner.GetDebuggerDump();

            while (true)
            {
                Console.WriteLine("<<");
                var src = Console.ReadLine();
                Console.WriteLine(">>");
                Console.WriteLine(runner.Eval(src));
            }

            ;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var src = @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slowsharp;
 
namespace HelloWorld
{

    class Fooo : Bar {
        public static int aa = 1;

        public int FF { get { return ff; } }        
        
        public void Foo() {
            Console.WriteLine(1);
        }
        public void Foo(int b) {
            Console.WriteLine(11);
        }
    }

class Boor : Bar {


public void MoveForward() {  
    transform.position += new Vector3(11,1,1);
}
}
class MyList : List<int> { }
    class Program : Bar, IFoo
    {
static int bb = 1;
static int Foo() { return 5; }
static int Foo(int n) { return 15; }

public static int Booo(int n = 5) => 5;

static IEnumerator Bbb() {
yield return 1;
yield return 2;
yield return 3;
}

private static int acac = 123;

        static int Main(int n) {

Console.WriteLine(BA.A);
Console.WriteLine(BA.B);


return v;

return 0;
//return ""asdf"";
        }

static void Bo() {
var aa = 1234;
Console.WriteLin(aa);
}
    }
}
";

            var hotReloadSrc = @"
class Fooo : Bar {
    public void Foo() {
        Console.WriteLine(12341234);
    }
}
";
            Console.WriteLine(CScript.RunSimple("\"hello from inception\""));

            Console.WriteLine(src);

            CSharpParseOptions options = new CSharpParseOptions(LanguageVersion.Default, kind: SourceCodeKind.Script);
            var tree = CSharpSyntaxTree.ParseText(src);
            var root = tree.GetCompilationUnitRoot();

            Dump(root);

            var config = ScriptConfig.Default;

            config.PrewarmTypes = new Type[] { typeof(Console) };

            var run = CScript.CreateRunner(src, config);
            SSDebugger.runner = run;
            var myList = run.RunMain();
            Console.WriteLine(myList.Is<List<int>>());

            var vd = new Validator();
            vd.Visit(root);

            var r = new Runner(config, new RunConfig() {
            });
            r.Run(root);

            Console.WriteLine(r.RunMain(5));
            Console.WriteLine(Goo());

            return;

            tree = CSharpSyntaxTree.ParseText(hotReloadSrc);
            root = tree.GetCompilationUnitRoot();
            r.UpdateMethodsOnly(root);

            r.RunMain(5);

            return;

            var bar = new Bar();
            //dynamic d = new DynamicHybInstance(r.Override("Boor", bar));
            //d.Boo();
            //d.SayHello();
            var boor = r.Override("Boor", bar);
            boor.Invoke("MoveForward");

            Console.WriteLine(bar.transform.position.x);

            //Console.WriteLine(r.Instantiate("Fooo").Invoke("Foo", 1));

            var a = new List<int>() { };
            a.Any();
            Any(a, (int x) => true);
        }

        private static void Any<T1, T2>(IEnumerable<T1> f, Func<T2, bool> ff)
        {
            
        }

        private static void Dump(SyntaxNode syntax, int depth = 0)
        {
            for (int i = 0; i < depth; i++) Console.Write("  ");
            Console.WriteLine(syntax.GetType() + " " + syntax);

            foreach (var child in syntax.ChildNodes())
                Dump(child, depth + 1);
        }

        private static int Goo()
        {
            var count = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    count += 10;
                    //if (j == 2) break;
                }
                count++;
                //if (i == 2) break;
            }
            return count;
        }
    }
}
