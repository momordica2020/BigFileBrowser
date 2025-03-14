using System;
using System.Threading;
using System.Threading.Tasks;


namespace BigFileBrowser.Providers
{
    public class Debouncer
    {
        private Timer _timer;
        private readonly int _delay;
        private Action _action;

        public Debouncer(int delay)
        {
            _delay = delay;
        }

        public void Debounce(Action action)
        {
            _action = action;
            if (_timer == null)
            {
                _timer = new Timer(_ => Execute(), null, _delay, Timeout.Infinite);
            }
            else
            {
                _timer.Change(_delay, Timeout.Infinite); // 重置计时器
            }
        }

        private void Execute()
        {
            _timer?.Dispose();
            _timer = null;
            _action?.Invoke(); // 执行回调函数
        }
    }
}
