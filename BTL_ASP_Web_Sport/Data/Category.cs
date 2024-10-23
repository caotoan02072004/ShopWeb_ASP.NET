using System;
using System.Collections.Generic;

namespace BTL_ASP_Web_Sport.Data;

public partial class Category
{
    public int CateId { get; set; }

    public string? CateName { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
