using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.stch111.Database
{
    public class Stack
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class StackDTO
    {
        public string Name { get; set; }
    }

    public class FlashCard
    {
        public int ID { get; set; }
        public string Front { get; set; }
        public string Back { get; set; }
        public int StackID { get; set; }
    }

    public class FlashCardDTO
    {
        public string Front { get; set; }
        public string Back { get; set; }
    }
}
