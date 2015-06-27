using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNWF.Utill
{
    static public class WorkDistribution
    {
        static public int getRowCyclicWorkload(int worker, int workers, int workload)
        {
            if (workload % workers > worker)
                return workload / workers + 1;
            return workload / workers;
        }
    }
}
