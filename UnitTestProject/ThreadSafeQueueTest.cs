using KaspTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class ThreadSafeQueueTest
    {
        [Timeout(4000)]
        [TestMethod]
        public void Push_Pop_Single_Thread_Test()
        {
            var queue = new ThreadSafeQueue<int>();
            queue.Push(1);
            queue.Push(2);
            Assert.AreEqual(queue.Pop(), 1);
            Assert.AreEqual(queue.Pop(), 2);
        }

        [Timeout(100000)]
        [TestMethod]
        public void Push_And_Pop_Multi_Task_Test()
        {
            var popTaskAmount = 100;
            var pushNumberAmount = 60;
            var pushTaskAmount = 10;

            var queue = new ThreadSafeQueue<int>();
            var rand = new Random();
            var actualRange = new ConcurrentQueue<int>();
            var expectedRange = new ConcurrentQueue<int>();

            for (int i = 0; i < popTaskAmount; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        var sw = Stopwatch.StartNew();
                        var res = queue.Pop();
                        actualRange.Enqueue(res);
                        Thread.Sleep(10);
                    }
                });
            }

            var pushTasks = new List<Task>();

            for (int i = 0; i < pushTaskAmount; i++)
            {
                pushTasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < pushNumberAmount; ++j)
                    {
                        var randValue = rand.Next(0, 1000);
                        queue.Push(randValue);
                        expectedRange.Enqueue(randValue);
                        Thread.Sleep(10);
                    }
                }));
            }

            Task.WaitAll(pushTasks.ToArray());

            CollectionAssert.AreEquivalent(expectedRange, actualRange);
        }
    }
}
