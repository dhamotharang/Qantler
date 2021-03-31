﻿using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public enum JobOrderStatus
  {
    Draft = 100,
    Pending = 200,
    WIP = 300,
    Done = 400,
    Cancelled = 500,
    Expired = 600
  }

  public enum JobOrderType
  {
    Audit,
    Periodic,
    Enforcement,
    Reinstate
  }

  public class JobOrder
  {
    public long ID { get; set; }

    public long? RefID { get; set; }

    public JobOrderType? Type { get; set; }

    public JobOrderStatus? Status { get; set; }

    public string Notes { get; set; }

    public long ChecklistHistoryID { get; set; }

    public Guid? CustomerID { get; set; }

    public Guid? AssignedTo { get; set; }

    public DateTimeOffset? TargetDate { get; set; }

    public DateTimeOffset? ScheduledOn { get; set; }

    public DateTimeOffset? ScheduledOnTo { get; set; }

    public DateTimeOffset? CompletedOn { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool? IsDeleted { get; set; }

    #region Generated Properties

    public string TypeText { get; set; }

    public string StatusText { get; set; }

    public Person ContactPerson { get; set; }

    public Customer Customer { get; set; }

    public Officer Officer { get; set; }

    public IList<Premise> Premises { get; set; }

    public IList<Log> Logs { get; set; }

    public IList<JobOrderLineItem> LineItems { get; set; }

    public IList<Attendee> Attendees { get; set; }

    public IList<Findings> Findings { get; set; }

    public IList<Attachment> Attachment { get; set; }

    public IList<Officer> Invitees { get; set; }

    #endregion
  }
}
