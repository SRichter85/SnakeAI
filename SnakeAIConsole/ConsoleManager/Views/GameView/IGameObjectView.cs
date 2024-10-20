using SnakeAICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeAIConsole {
    public interface IGameObjectView {
        IGameObject GameObject { get; }

        void Refresh();

        void Cleanup();
    }
}
