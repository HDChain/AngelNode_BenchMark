using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthDataTest.DataTask
{
    public class DataTask<T>  where T : class {

        private readonly object _lock = new object();
        private readonly Queue<T> _tasks = new Queue<T>();
        private readonly List<Task> _threads = new List<Task>();
        private bool _isStop;

        public void AddTask(T task) {
            lock (_lock) {
                _tasks.Enqueue(task);
            }
        }

        private T GetTask() {
            lock (_lock) {
                if (_tasks.Count > 0) {
                    return _tasks.Dequeue();
                }

                return default(T);
            }
        }

        public void Start(Action<T> action,int threadCount = 1) {

            for (int i = 0; i < threadCount; i++) {
                var t = Task.Factory.StartNew(async () => {

                    while (!_isStop) {

                        var task = GetTask();
                        if (task == null) {

                            await Task.Delay(1);
                            continue;
                        }

                        action(task);
                    }
                });

                _threads.Add(t);
            }
        }

        public void Wait() {
            Task.WaitAll(_threads.ToArray());
        }

        public void Stop() {
            _isStop = true;

            Task.WaitAll(_threads.ToArray());
        }
    }
}
