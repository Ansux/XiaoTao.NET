using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xiaotao.Models
{
  public class CartItems
  {
    public int cid { get; set; }

    public int proId { get; set; }

    public string proName { get; set; }

    public int number { get; set; }

    public decimal price { get; set; }

    public decimal subPrice { get; set; }

    public int stock { get; set; }

    public bool isCheck { get; set; }

   public string thumb { get; set; }


  }

  public class CartStores
  {
    public int sid { get; set; }

    public string sname { get; set; }

    public bool isCheck { get; set; }

    public virtual ICollection<CartItems> proItems { get; set; }

  }

  public class SettleStores
  {
    public int sid { get; set; }

    public string sname { get; set; }
  }
}