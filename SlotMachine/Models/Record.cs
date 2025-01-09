using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachine.Models
{
    public class Record
    {
        public string Category { get; set; } = string.Empty;
        public double Value { get; set; } = 0;
        public DateTime Date { get; set; } = new DateTime();

    }
}
