using Core.EventBus;
using Request.Model;
using System;
using System.Collections.Generic;

namespace Request.Events
{
  public class NewIngredientEvent : Event
  {
    public IList<Item> Ingredients { get; set; }
  }

  public class Item
  {
    public string Name { get; set; }

    public string Brand { get; set; }

    public RiskCategory RiskCategory { get; set; }

    public IngredientStatus? Status { get; set; }

    public string SupplierName { get; set; }

    public string CertifyingBodyName { get; set; }

    public Guid? ReviewedBy { get; set; }
  }
}
