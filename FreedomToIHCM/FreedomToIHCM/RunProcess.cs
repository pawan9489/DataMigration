using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreedomToIHCM
{
    class RunProcess
    {
        static readonly int ConsumerCount_ = Environment.ProcessorCount / 4;
        static BlockingCollection<Table> bc = new BlockingCollection<Table>(ConsumerCount_);
        static int n = Table.Tables.Count;

        static void Producer()
        {
            for (int i = 0; i < n; i++)
            {
                foreach(var table in Table.Tables)
                {
                    if (table.isParentsCompleted())
                    {
                        // Remove the table from list and add it to a blocking collection
                        Table.Tables.Remove(table);
                        bc.Add(table);
                        break;
                    }
                }           
            }
            bc.CompleteAdding();
        }

        static void Consumer()
        {
            foreach (var table in bc.GetConsumingEnumerable())
            {
                // Execute SQL for the table based on the Dictionary<Table, SQL>
            }
        }

        public static void Run(List<FromToTableModel> rows)
        {
            Task.Factory.StartNew(() => Producer());

            for (int i = 0; i < ConsumerCount_; i++)
            {
                Task.Factory.StartNew(() => Consumer());
            }
        }
    }
}
