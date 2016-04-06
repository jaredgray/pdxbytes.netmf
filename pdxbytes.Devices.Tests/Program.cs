using System;
using System.Reflection;
using MFUnitTest;

namespace pdxbytes.Devices.Tests
{
    class Program
    {
        public static void Main()
        {
            // Run all tests in current assembly
            TestManager.RunTests(Assembly.GetExecutingAssembly());

            // Run all tests for specified Test Class
            //TestManager.RunTest(typeof(PIDControllerTest));

            // Run specified test for specified Test Class
            //TestManager.RunTest(typeof(UnitTest1), "TestMethod1");
        }
    }
}
