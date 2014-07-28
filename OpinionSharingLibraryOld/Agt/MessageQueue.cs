using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpinionSharing.Agt
{
    public class MessageQueue
    {
        protected Queue<BWMessage> messageQueue;
        protected Queue<BWMessage> processQueue;

        public Object MessageQueueLock = new Object();

        //コンストラクタで初期化された直後の状態に戻す
        public virtual void Initialize()
        {
            messageQueue = new Queue<BWMessage>();
            processQueue = new Queue<BWMessage>();
        }

        public virtual void Enqueue(BWMessage message)
        {
            lock (MessageQueueLock)
            {
                messageQueue.Enqueue(message);
            }
        }

        public virtual int Count
        {
            get
            {
                return messageQueue.Count;
            }
        }

        public virtual void Clear()
        {
            lock (MessageQueueLock)
            {
                messageQueue.Clear();
                processQueue.Clear();
            }
        }

        public virtual void Flip()
        {
            lock (MessageQueueLock)
            {
                processQueue = messageQueue;
                messageQueue = new Queue<BWMessage>();
            }
        }

        public virtual IEnumerable<BWMessage> ProcessQueue
        {
            get
            {
                return processQueue;
            }
        }

        public virtual void ProcessMessage(Action<BWMessage> action)
        {
            //nullであってはいけない
            if (processQueue == null)
            {
                throw new Exception("process queueはnullです");
            }

            lock (MessageQueueLock)
            {
                foreach(var message in processQueue){
                    action(message);
                }
            }
        }

        public virtual void ClearProcessQueue()
        {
            lock (MessageQueueLock)
            {
                processQueue.Clear();
            }
        }



    }
}
