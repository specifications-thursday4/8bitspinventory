using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8BitIMS
{
    // Use this to create an item object to add to cart
    // use like a struct

    /* ex:  Item it = new Item();
            it.price = 100; */
    class Item
    {
        public int gameID { get; set; }

        public int platID { get; set; }

        public int quantity { get; set; }

        public int price { get; set; }

        public bool inBox { get; set; }
    }
}
