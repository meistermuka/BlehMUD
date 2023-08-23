using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlehMUD.Core
{
    public class TickSystem
    {
        private readonly Timer _timer;
        public event EventHandler Tick;

        public TickSystem(int tickIntervalMilliseconds)
        {
            _timer = new Timer(TickCallback, null, 0, tickIntervalMilliseconds);
        }

        private void TickCallback(object state)
        {
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}
