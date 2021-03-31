using Core.Model;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Models
{
  public class TransactionCodeOptions
  {
    public long? ID { get; set; }

    public string Code { get; set; }

    public string GLEntry { get; set; }

    public string Text { get; set; }

    public string Currency { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}