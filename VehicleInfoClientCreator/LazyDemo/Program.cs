using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LazyDemo
{
    class Program
    {
        static Lazy<LargeObject> lazyLargeObject = null;

        static LargeObject InitLargeObject()
        {
            var large = new LargeObject(Thread.CurrentThread.ManagedThreadId);
            // Perform additional initialization here.
            return large;
        }


        static void Main()
        {
            // The lazy initializer is created here. LargeObject is not created until the 
            // ThreadProc method executes.
            lazyLargeObject = new Lazy<LargeObject>(InitLargeObject,true);

            // The following lines show how to use other constructors to achieve exactly the
            // same result as the previous line: 
            //lazyLargeObject = new Lazy<LargeObject>(InitLargeObject, true);
            //lazyLargeObject = new Lazy<LargeObject>(InitLargeObject, 
            //                               LazyThreadSafetyMode.ExecutionAndPublication);


            Console.WriteLine(
                "\r\nLargeObject is not created until you access the Value property of the lazy" +
                "\r\ninitializer. Press Enter to create LargeObject.");
            Console.ReadLine();
            Console.WriteLine(DateTime.Now.Second+"-"+DateTime.Now.Millisecond);
            // Create and start 3 threads, each of which uses LargeObject.
            var threads = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                threads[i] = new Thread(ThreadProc);
                threads[i].Start();
            }

            // Wait for all 3 threads to finish. 
            foreach (var t in threads)
            {
                t.Join();
            }
            Console.WriteLine(DateTime.Now.Second+"-"+DateTime.Now.Millisecond);
            Console.WriteLine("\r\nPress Enter to end the program");
            Console.ReadLine();
        }


        static void ThreadProc(object state)
        {
            var large = lazyLargeObject.Value;

            // IMPORTANT: Lazy initialization is thread-safe, but it doesn't protect the  
            //            object after creation. You must lock the object before accessing it,
            //            unless the type is thread safe. (LargeObject is not thread safe.)
            lock (large)
            {
                large.Data[0] = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Initialized by thread {0}; last used by thread {1}.",
                    large.InitializedBy, large.Data[0]);
            }
        }
    }
   
    class LargeObject
    {
        public int InitializedBy { get; } = 0;

        public LargeObject(int initializedBy)
        {
            InitializedBy = initializedBy;
            Console.WriteLine("LargeObject was created on thread id {0}.", InitializedBy);
        }

        public long[] Data = new long[200000000];
     
    }

    /* This example produces output similar to the following:

    LargeObject is not created until you access the Value property of the lazy
    initializer. Press Enter to create LargeObject.

    LargeObject was created on thread id 3.
    Initialized by thread 3; last used by thread 3.
    Initialized by thread 3; last used by thread 4.
    Initialized by thread 3; last used by thread 5.

    Press Enter to end the program
     */
}
