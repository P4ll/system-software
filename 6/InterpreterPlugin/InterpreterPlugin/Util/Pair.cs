using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class Pair<T1, T2>
    {
        public T1 val1;
        public T2 val2;

        public Pair()
        {
        }

        public Pair(T1 val1, T2 val2)
        {
            this.val1 = val1;
            this.val2 = val2;
        }
    }
}
