using System;
using System.Collections.Generic;

namespace BTL_ASP_Web_Sport.Data;

public partial class Product
{
    public int ProId { get; set; }

    public int? ProCategoryId { get; set; }

    public string? ProDescription { get; set; }

    public string? ProImage { get; set; }

    public string? ProName { get; set; }

    public double? ProPrice { get; set; }

    public double? ProSalePrice { get; set; }

    public int? ProStatus { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Category? ProCategory { get; set; }
}
