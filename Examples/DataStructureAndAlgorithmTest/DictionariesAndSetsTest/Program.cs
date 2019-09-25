using System;
using System.Collections;

namespace DictionariesAndSetsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("name", "Tuan Nguyen");
            hashtable["age"] = 27;
            hashtable.Add("age", 20);
            foreach(var key in hashtable.Keys)
            {
                Console.WriteLine(key);
            }
        }
    }
}
