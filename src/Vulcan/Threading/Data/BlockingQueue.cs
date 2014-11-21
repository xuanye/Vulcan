using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Vulcan.Threading.Data
{
    /// <summary>
    /// 比较好的阻塞队列
    /// </summary>
    public class BlockingQueue<T>
    {
        private Queue<T> queue = new Queue<T>();
        private int waitingConsumers = 0; //等待的消费者数量 这个变量可以减少Monitor.Pulse的调用

        public int Count
        {
            get
            {
                lock (queue) //这里弄个object也是一样的
                {
                    return queue.Count;
                }
            }
        }

        public void Clear()
        {
            lock (queue)
            {
                queue.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (queue)
            {
                return queue.Contains(item);
            }
        }

        public void Enqueue(T item)
        {
            lock (queue)
            {
                queue.Enqueue(item);
                if (waitingConsumers > 0) //唤醒等待新元素的消费者
                    Monitor.Pulse(queue);
            }
        }

        public T Dequeue()
        {
            lock (queue)
            {

                while (queue.Count == 0)
                {
                    waitingConsumers++;
                    try
                    {
                        Monitor.Wait(queue);
                    }
                    finally
                    {
                        waitingConsumers--;
                    }
                }
                return queue.Dequeue();
            }
        }
    }
}
