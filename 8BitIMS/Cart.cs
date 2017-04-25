using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8BitIMS
{
    // To use class type Cart.Instance.myFunction()
    class Cart
    {
        // item will contain all needed item information
        public List<Item> iList = new List<Item>();

        

        public Cart()
        {
        }

       

        // adds an item to the cart
        public void addItem(Item i)
        {
            iList.Add(i);
        }

        
    }
}
