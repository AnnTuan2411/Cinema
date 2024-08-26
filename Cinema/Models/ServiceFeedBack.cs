using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class ServiceFeedBack
{
    public int SeviceFeedBackId { get; set; }

    public string? Comment { get; set; }

    public int Star { get; set; }

    public int? CustomerId { get; set; }

    public DateTime FeedBackDate { get; set; }

    public virtual Customer? Customer { get; set; }
}
