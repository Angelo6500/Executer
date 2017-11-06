using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyAttribute;
//ctrl + barra spaziatrice 
namespace MyLibrary
{
    public class Roo
    {
        [ExecuteMe(1)]
        public void M10(ref int x)
        {
            Console.WriteLine("M10");
            Console.WriteLine("the parameter ref is "+x);

        }


        //il parametro out deve essere assegnato prima ce il controllo lasci il metodo
        //public void M11(out int x)
        //{
        //    Console.WriteLine("M11");
        //    Console.WriteLine("the parameter ref is " + x);
        //}

        


    }


    public class Foo
    {

        [ExecuteMe()]
        public void M1 ()
        {
            Console.WriteLine("M1");

        }

        [ExecuteMe(45)]
        [ExecuteMe(0)]
        [ExecuteMe(3)]
        public void M2(int a)
        {
            Console.WriteLine("M2 a={0}", a);

        }
        [ExecuteMe("hello", "reflection")]
        public void M3(string s1, string s2)
        {
            Console.WriteLine("M3 s1={0} s2={1}", s1, s2);

        }
        [ExecuteMe]
        public void M4()
        {
            Console.WriteLine("M4");
            Console.ReadLine();

        }


        [ExecuteMe(new string[]{"ciao","mamma"})]
        [ExecuteMe(0, 1, 2,3)]
        [ExecuteMe(new int[]{0,1,2,3})]
        public void M5(params object[] parameters)
        {
            Console.WriteLine("M5");

            //genera una classe a compile time
            parameters.ToList().ForEach(i => Console.WriteLine(i.ToString()));
            //foreach (var p in parameters)
            //{
            //    Console.WriteLine(p.ToString());

            //}
            Console.ReadLine();

        }




    }


    public class Moo
    {
        [ExecuteMe]
        public void M1024()
        {
            Console.WriteLine("M1024");
            Console.ReadLine();
        }

        [ExecuteMe("tre")]
        public void M6(int n)
        {
            Console.WriteLine("M6 {0}", n);
            Console.ReadLine();
        }
        [ExecuteMe(0, 2)]
        public void M7(int x)
        {
            Console.WriteLine("M7 {0}", x);
            Console.ReadLine();

        }


    }



    public class Goo
    {
        private string X { get; }
        private int Y { get; }

        [ExecuteMe2("ciao")]
        public Goo(string x)
        {
            Console.WriteLine("Constructor of G {0}", x);
            Console.ReadLine();
            X = x;
            Y = 0;
        }

        [ExecuteMe]
        public void G()
        {
            Console.WriteLine("Constructor of G without parameters");
            Console.ReadLine();
        }
    }


    

    public class Boo
    {

        [ExecuteMe(0, "c", "puffo", "e", 2.3)]
        public void Abc(int a, int b = 1, string c = "2", int d = 3, double e = 5.5)
        {
            Console.WriteLine("Abc");
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
            Console.WriteLine(e);
            Console.ReadLine();
        }




        [ExecuteMe(0)]
        [ExecuteMe(0, "name", "minnie")]
        [ExecuteMe(0, "surname", "topolino")]
        [ExecuteMe(0, "name", "pippo", "surname", "pluto")]
        [ExecuteMe(0, "name", "pippo", "surname", "pluto", 1, 2, 3, 4)]
        [ExecuteMe()]
        [ExecuteMe("name")]
        [ExecuteMe("pippo")]
        [ExecuteMe("pippo", "name")]
        [ExecuteMe("name", "pippo")]
        [ExecuteMe("name", "surname")]
        [ExecuteMe(0, 1)]
        public void M8(int r, string name = "alice", string surname = "bob", params object[] p)
        {
            Console.WriteLine("M8");
            Console.WriteLine(r);
            Console.WriteLine(name);
            Console.WriteLine(surname);
            if (p != null)
            {
                foreach (var e in p)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine("p nullo");

            }

            Console.ReadLine();
        }

        [ExecuteMe]
        [ExecuteMe(0)]
        [ExecuteMe(0,1)]
        [ExecuteMe(0, 1,2)]
        [ExecuteMe(0, 1,2,3)]
        [ExecuteMe(0,1,2,3,4)]
        [ExecuteMe(0, 1,2,3,4,5)]
        [ExecuteMe(0,1,2,3,4,5,6,7,8,9,10,11)]
        public void M9(int a, int b, int c = 2, int d=3,int e =4, params object[] f)
        {
            Console.WriteLine("Mandatory: " + a + " " +b );
            
            Console.WriteLine("Optional: "+c + " " + d + " " + e);
           
            Console.Write("Params: ");
            f.ToList().ForEach(i => Console.Write(i.ToString() + " "));
            Console.WriteLine();
            Console.ReadLine();
        }



    }

}
