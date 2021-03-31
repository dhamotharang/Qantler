using Core.Model;
using Dapper;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Request.API.Repository.DataTables
{
  public class DataConverter
  {
    public static DataTable ToRequestData(Model.Request req)
    {
      DataTable dt = new DataTable();

      dt.Columns.Add("Step", typeof(int));
      dt.Columns.Add("RequestorID", typeof(Guid));
      dt.Columns.Add("RequestorName", typeof(string));
      dt.Columns.Add("AgentID", typeof(Guid));
      dt.Columns.Add("AgentName", typeof(string));
      dt.Columns.Add("CustomerID", typeof(Guid));
      dt.Columns.Add("CustomerCode", typeof(string));
      dt.Columns.Add("CustomerName", typeof(string));
      dt.Columns.Add("Type", typeof(int));
      dt.Columns.Add("Status", typeof(int));
      dt.Columns.Add("StatusMinor", typeof(int));
      dt.Columns.Add("OldStatus", typeof(int));
      dt.Columns.Add("RefID", typeof(string));
      dt.Columns.Add("ParentId", typeof(long));
      dt.Columns.Add("Expedite", typeof(bool));
      dt.Columns.Add("EscalateStatus", typeof(int));
      dt.Columns.Add("AssignedTo", typeof(Guid));
      dt.Columns.Add("AssignedToName", typeof(string));
      dt.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dt.Columns.Add("TargetCompletion", typeof(DateTimeOffset));
      dt.Columns.Add("DueOn", typeof(DateTimeOffset));
      dt.Columns.Add("ModifiedOn", typeof(DateTimeOffset));
      dt.Columns.Add("SubmittedOn", typeof(DateTimeOffset));


      dt.Rows.Add(
          req.Step,
          req.RequestorID,
          req.RequestorName,
          req.AgentID,
          req.AgentName,
          req.CustomerID,
          req.CustomerCode,
          req.CustomerName,
          req.Type,
          req.Status,
          req.StatusMinor,
          req.OldStatus,
          req.RefID,
          req.ParentID,
          req.Expedite,
          req.EscalateStatus,
          req.AssignedTo,
          req.AssignedToName,
          req.CreatedOn?.ToUniversalTime(),
          req.TargetCompletion?.ToUniversalTime(),
          req.DueOn?.ToUniversalTime(),
          req.ModifiedOn?.ToUniversalTime(),
          req.SubmittedOn.ToUniversalTime());

      return dt;
    }

    public static DataTable ToHalalTeamData(Model.Request req)
    {
      DataTable dh = new DataTable();

      dh.Columns.Add("AltID", typeof(string));
      dh.Columns.Add("Name", typeof(string));
      dh.Columns.Add("Designation", typeof(string));
      dh.Columns.Add("Role", typeof(string));
      dh.Columns.Add("IsCertified", typeof(bool));
      dh.Columns.Add("JoinedOn", typeof(DateTimeOffset));
      dh.Columns.Add("ChangeType", typeof(int));
      dh.Columns.Add("RequestID", typeof(long));
      dh.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dh.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (req.Teams != null)
      {
        foreach (HalalTeam ht in req.Teams)
        {
          dh.Rows.Add(
            ht.AltID,
            ht.Name,
            ht.Designation,
            ht.Role,
            ht.IsCertified,
            ht.JoinedOn,
            ht.ChangeType,
            ht.RequestID,
            ht.CreatedOn,
            ht.ModifiedOn);
        }
      }
      return dh;
    }

    public static DataTable ToCharacteristicData(params Characteristic[] chars)
    {
      DataTable dc = new DataTable();

      dc.Columns.Add("Type", typeof(int));
      dc.Columns.Add("Value", typeof(string));
      dc.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dc.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (chars != null)
      {
        foreach (Characteristic ct in chars)
        {
          dc.Rows.Add(
            ct.Type,
            ct.Value,
            ct.CreatedOn,
            ct.ModifiedOn);
        }
      }
      return dc;
    }

    public static DataTable ToIngredientData(Model.Request req)
    {
      DataTable di = new DataTable();
      di.Columns.Add("ID", typeof(long));
      di.Columns.Add("Text", typeof(string));
      di.Columns.Add("SubText", typeof(string));
      di.Columns.Add("RiskCategory", typeof(int));
      di.Columns.Add("Approved", typeof(bool));
      di.Columns.Add("Remarks", typeof(string));
      di.Columns.Add("ReviewedBy", typeof(Guid));
      di.Columns.Add("ReviewedByName", typeof(string));
      di.Columns.Add("ReviewedOn", typeof(DateTimeOffset));
      di.Columns.Add("ChangeType", typeof(int));
      di.Columns.Add("RequestID", typeof(long));
      di.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      di.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (req.Ingredients != null)
      {
        foreach (Ingredient ing in req.Ingredients)
        {
          di.Rows.Add(
            ing.ID,
            ing.Text,
            ing.SubText,
            ing.RiskCategory,
            ing.Approved,
            ing.Remarks,
            ing.ReviewedBy,
            ing.ReviewedByName,
            ing.ReviewedOn,
            ing.ChangeType,
            ing.RequestID,
            ing.CreatedOn,
            ing.ModifiedOn);
        }
      }
      return di;
    }

    public static DataTable ToIngredientData(IList<Ingredient> ingredients)
    {
      DataTable di = new DataTable();

      di.Columns.Add("ID", typeof(long));
      di.Columns.Add("Text", typeof(string));
      di.Columns.Add("SubText", typeof(string));
      di.Columns.Add("RiskCategory", typeof(int));
      di.Columns.Add("Approved", typeof(bool));
      di.Columns.Add("Remarks", typeof(string));
      di.Columns.Add("ReviewedBy", typeof(Guid));
      di.Columns.Add("ReviewedByName", typeof(string));
      di.Columns.Add("ReviewedOn", typeof(DateTimeOffset));
      di.Columns.Add("ChangeType", typeof(int));
      di.Columns.Add("RequestID", typeof(long));
      di.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      di.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (ingredients != null)
      {
        foreach (Ingredient ing in ingredients)
        {
          di.Rows.Add(
            ing.ID,
            ing.Text,
            ing.SubText,
            ing.RiskCategory,
            ing.Approved,
            ing.Remarks,
            ing.ReviewedBy,
            ing.ReviewedByName,
            ing.ReviewedOn,
            ing.ChangeType,
            ing.RequestID,
            ing.CreatedOn,
            ing.ModifiedOn);
        }
      }
      return di;
    }

    public static DataTable ToMenuData(Model.Request req)
    {

      DataTable dm = new DataTable();

      dm.Columns.Add("ID", typeof(long));
      dm.Columns.Add("Scheme", typeof(int));
      dm.Columns.Add("Text", typeof(string));
      dm.Columns.Add("SubText", typeof(string));
      dm.Columns.Add("Approved", typeof(bool));
      dm.Columns.Add("Remarks", typeof(string));
      dm.Columns.Add("ReviewdBy", typeof(Guid));
      dm.Columns.Add("ReviewedByName", typeof(string));
      dm.Columns.Add("ReviewedOn", typeof(DateTimeOffset));
      dm.Columns.Add("ChangeType", typeof(int));
      dm.Columns.Add("RequestID", typeof(long));
      dm.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dm.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (req.Menus != null)
      {
        foreach (Menu me in req.Menus)
        {
          dm.Rows.Add(
            me.ID,
            me.Scheme,
            me.Text,
            me.SubText,
            me.Approved,
            me.Remarks,
            me.ReviewedBy,
            me.ReviewedByName,
            me.ReviewedOn,
            me.ChangeType,
            me.RequestID,
            me.CreatedOn,
            me.ModifiedOn);
        }
      }
      return dm;
    }

    public static DataTable ToMenuData(IList<Menu> menus)
    {

      DataTable dm = new DataTable();
      dm.Columns.Add("ID", typeof(long));
      dm.Columns.Add("Scheme", typeof(int));
      dm.Columns.Add("Text", typeof(string));
      dm.Columns.Add("SubText", typeof(string));
      dm.Columns.Add("Approved", typeof(bool));
      dm.Columns.Add("Remarks", typeof(string));
      dm.Columns.Add("ReviewdBy", typeof(Guid));
      dm.Columns.Add("ReviewedByName", typeof(string));
      dm.Columns.Add("ReviewedOn", typeof(DateTimeOffset));
      dm.Columns.Add("ChangeType", typeof(int));
      dm.Columns.Add("RequestID", typeof(long));
      dm.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dm.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (menus != null)
      {
        foreach (Menu me in menus)
        {
          dm.Rows.Add(
            me.ID,
            me.Scheme,
            me.Text,
            me.SubText,
            me.Approved,
            me.Remarks,
            me.ReviewedBy,
            me.ReviewedByName,
            me.ReviewedOn,
            me.ChangeType,
            me.RequestID,
            me.CreatedOn,
            me.ModifiedOn);
        }
      }
      return dm;
    }

    public static DataTable ToPremiseData(Model.Request req)
    {
      DataTable dp = new DataTable();

      dp.Columns.Add("IsLocal", typeof(bool));
      dp.Columns.Add("Name", typeof(string));
      dp.Columns.Add("Type", typeof(int));
      dp.Columns.Add("Area", typeof(string));
      dp.Columns.Add("Schedule", typeof(string));
      dp.Columns.Add("BlockNo", typeof(string));
      dp.Columns.Add("UnitNo", typeof(string));
      dp.Columns.Add("FloorNo", typeof(string));
      dp.Columns.Add("BuildingName", typeof(string));
      dp.Columns.Add("Address1", typeof(string));
      dp.Columns.Add("Address2", typeof(string));
      dp.Columns.Add("City", typeof(string));
      dp.Columns.Add("Province", typeof(string));
      dp.Columns.Add("Country", typeof(string));
      dp.Columns.Add("Postal", typeof(string));
      dp.Columns.Add("Longitude", typeof(double));
      dp.Columns.Add("Latitude", typeof(double));
      dp.Columns.Add("IsPrimary", typeof(bool));
      dp.Columns.Add("ChangeType", typeof(int));
      dp.Columns.Add("RequestID", typeof(long));
      dp.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dp.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (req.Premises != null)
      {
        foreach (Premise pm in req.Premises)
        {
          dp.Rows.Add(
            pm.IsLocal,
            pm.Name,
            pm.Type,
            pm.Area,
            pm.Schedule,
            pm.BlockNo,
            pm.UnitNo,
            pm.FloorNo,
            pm.BuildingName,
            pm.Address1,
            pm.Address2,
            pm.City,
            pm.Province,
            pm.Country,
            pm.Postal,
            pm.Longitude,
            pm.Latitude,
            pm.IsPrimary,
            pm.ChangeType,
            pm.RequestID,
            pm.CreatedOn,
            pm.ModifiedOn);
        }
      }
      return dp;
    }

    public static DataTable ToPremiseData(Model.Premise premise)
    {
      DataTable dp = new DataTable();

      dp.Columns.Add("IsLocal", typeof(bool));
      dp.Columns.Add("Name", typeof(string));
      dp.Columns.Add("Type", typeof(int));
      dp.Columns.Add("Area", typeof(string));
      dp.Columns.Add("Schedule", typeof(string));
      dp.Columns.Add("BlockNo", typeof(string));
      dp.Columns.Add("UnitNo", typeof(string));
      dp.Columns.Add("FloorNo", typeof(string));
      dp.Columns.Add("BuildingName", typeof(string));
      dp.Columns.Add("Address1", typeof(string));
      dp.Columns.Add("Address2", typeof(string));
      dp.Columns.Add("City", typeof(string));
      dp.Columns.Add("Province", typeof(string));
      dp.Columns.Add("Country", typeof(string));
      dp.Columns.Add("Postal", typeof(string));
      dp.Columns.Add("Longitude", typeof(double));
      dp.Columns.Add("Latitude", typeof(double));
      dp.Columns.Add("IsPrimary", typeof(bool));
      dp.Columns.Add("ChangeType", typeof(int));
      dp.Columns.Add("RequestID", typeof(long));
      dp.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dp.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (premise != null)
      {        
          dp.Rows.Add(
            premise.IsLocal,
            premise.Name,
            premise.Type,
            premise.Area,
            premise.Schedule,
            premise.BlockNo,
            premise.UnitNo,
            premise.FloorNo,
            premise.BuildingName,
            premise.Address1,
            premise.Address2,
            premise.City,
            premise.Province,
            premise.Country,
            premise.Postal,
            premise.Longitude,
            premise.Latitude,
            premise.IsPrimary,
            premise.ChangeType,
            premise.RequestID,
            premise.CreatedOn,
            premise.ModifiedOn);        
      }
      return dp;
    }

    public static DataTable ToAttachmentData(Model.Request req)
    {
      DataTable da = new DataTable();

      da.Columns.Add("FileID", typeof(Guid));
      da.Columns.Add("FileName", typeof(string));
      da.Columns.Add("Extension", typeof(string));
      da.Columns.Add("Size", typeof(long));
      da.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      da.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (req.Attachments != null)
      {
        foreach (Attachment at in req.Attachments)
        {
          da.Rows.Add(
            at.FileID,
            at.FileName,
            at.Extension,
            at.Size,
            at.CreatedOn,
            at.ModifiedOn);
        }
      }
      return da;
    }

    public static DataTable ToAttachmentData(Review review)
    {
      DataTable da = new DataTable();

      da.Columns.Add("FileID", typeof(Guid));
      da.Columns.Add("FileName", typeof(string));
      da.Columns.Add("Extension", typeof(string));
      da.Columns.Add("Size", typeof(long));
      da.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      da.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (review.Attachments != null)
      {
        foreach (Attachment at in review.Attachments)
        {
          da.Rows.Add(
            at.FileID,
            at.FileName,
            at.Extension,
            at.Size,
            at.CreatedOn,
            at.ModifiedOn);
        }
      }
      return da;
    }

    public static DataTable ToRequestLineItemData(Model.Request req,
      out DataTable lineItemCharacteristic)
    {
      lineItemCharacteristic = new DataTable();

      lineItemCharacteristic.Columns.Add("Type", typeof(int));
      lineItemCharacteristic.Columns.Add("Value", typeof(string));
      lineItemCharacteristic.Columns.Add("Index", typeof(string));
      lineItemCharacteristic.Columns.Add("RefIndex", typeof(string));
      lineItemCharacteristic.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      lineItemCharacteristic.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      DataTable dl = new DataTable();
      dl.Columns.Add("Scheme", typeof(int));
      dl.Columns.Add("SubScheme", typeof(int));
      dl.Columns.Add("Index", typeof(int));
      dl.Columns.Add("ComplianceHistoryID", typeof(long));
      dl.Columns.Add("RequestID", typeof(long));
      dl.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dl.Columns.Add("ModifiedOn", typeof(DateTimeOffset));
      int i = 0;
      if (req.LineItems != null)
      {
        foreach (RequestLineItem rl in req.LineItems)
        {
          dl.Rows.Add(
            rl.Scheme,
            rl.SubScheme,
            i = i + 1,
            rl.ChecklistHistoryID,
            rl.RequestID,
            rl.CreatedOn,
            rl.ModifiedOn);
          if (rl.Characteristics != null)
          {
            ToRequestLineItemCharacteristicData(rl, i, lineItemCharacteristic);
          }
        }
      }
      return dl;

    }

    public static void ToRequestLineItemCharacteristicData(
      RequestLineItem requestLineItem, int requestLineItemIndex,
      DataTable lineItemCharacteristic)
    {
      if (requestLineItem.Characteristics != null)
      {
        int j = 0;
        foreach (Characteristic ch in requestLineItem.Characteristics)
        {
          lineItemCharacteristic.Rows.Add(
            ch.Type,
            ch.Value,
            j = j + 1,
            requestLineItemIndex,
            ch.CreatedOn,
            ch.ModifiedOn
            );
        }
      }
    }

    public static DataTable ToRFAAttachmentData(List<Attachment> attachments, int Index)
    {
      var attachmentItems = new DataTable();

      attachmentItems.Columns.Add("Index", typeof(int));
      attachmentItems.Columns.Add("FileID", typeof(Guid));
      attachmentItems.Columns.Add("FileName", typeof(string));
      attachmentItems.Columns.Add("Extension", typeof(string));
      attachmentItems.Columns.Add("Size", typeof(long));
      attachmentItems.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      attachmentItems.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (attachments != null && attachments.Count > 0)
      {
        foreach (Attachment at in attachments)
        {
          attachmentItems.Rows.Add(
            Index,
            at.FileID,
            at.FileName,
            at.Extension,
            at.Size,
            at.CreatedOn,
            at.ModifiedOn);
        }
      }
      return attachmentItems;
    }


    public static DataTable ToRFAReplyAttachmentData(IList<Attachment> attachments,
      long LineItemID)
    {
      var attachmentItems = new DataTable();

      attachmentItems.Columns.Add("LineItemID", typeof(long));
      attachmentItems.Columns.Add("FileID", typeof(Guid));
      attachmentItems.Columns.Add("FileName", typeof(string));
      attachmentItems.Columns.Add("Extension", typeof(string));
      attachmentItems.Columns.Add("Size", typeof(long));
      attachmentItems.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      attachmentItems.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      if (attachments != null && attachments.Count > 0)
      {
        foreach (Attachment at in attachments)
        {
          attachmentItems.Rows.Add(
            LineItemID,
            at.FileID,
            at.FileName,
            at.Extension,
            at.Size,
            at.CreatedOn,
            at.ModifiedOn);
        }
      }
      return attachmentItems;
    }


    public static DataTable ToRFAData(RFA rfa)
    {
      DataTable dr = new DataTable();

      dr.Columns.Add("Status", typeof(int));
      dr.Columns.Add("RaisedBy", typeof(Guid));
      dr.Columns.Add("RaisedByName", typeof(string));
      dr.Columns.Add("RequestID", typeof(long));
      dr.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dr.Columns.Add("DueOn", typeof(DateTimeOffset));
      dr.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      dr.Rows.Add(
        rfa.Status,
        rfa.RaisedBy,
        rfa.RaisedByName,
        rfa.RequestID,
        rfa.CreatedOn?.ToUniversalTime(),
        rfa.DueOn?.ToUniversalTime(),
        rfa.ModifiedOn?.ToUniversalTime()
        );

      return dr;
    }

    public static DataTable ToReviewData(Review review,
      out DataTable lineitem)
    {
      lineitem = new DataTable();
      lineitem.Columns.Add("Scheme", typeof(int));
      lineitem.Columns.Add("Recommendation", typeof(string));
      lineitem.Columns.Add("Remarks", typeof(string));
      lineitem.Columns.Add("ReviewID", typeof(long));

      DataTable dr = new DataTable();
      dr.Columns.Add("Step", typeof(int));
      dr.Columns.Add("Grade", typeof(int));
      dr.Columns.Add("ReviewerID", typeof(Guid));
      dr.Columns.Add("ReviewerName", typeof(string));
      dr.Columns.Add("RequestID", typeof(long));
      dr.Columns.Add("CreatedOn", typeof(DateTimeOffset));

      dr.Rows.Add(
        review.Step,
        review.Grade,
        review.ReviewerID,
        review.ReviewerName,
        review.RequestID,
        review.CreatedOn?.ToUniversalTime()
        );

      if (lineitem != null)
      {
        foreach (ReviewLineItem rl in review.LineItems)
        {
          lineitem.Rows.Add(
            rl.Scheme,
            rl.Approved,
            rl.Remarks,
            rl.ReviewID);
        }
      }

      return dr;
    }

    public static DataTable ToRFALineItemData(
  RFA rfa,
  out DataTable lineitemattachment, out DataTable lineitemreply,
  out DataTable lineitemreplyattachments)
    {
      lineitemattachment = new DataTable();
      lineitemreply = new DataTable();
      lineitemreplyattachments = new DataTable();

      DataTable replyItems = new DataTable();
      DataTable lineitemattachments = new DataTable();
      DataTable lineReplyAttachments = new DataTable();

      DataTable dl = new DataTable();

      dl.Columns.Add("Scheme", typeof(int));
      dl.Columns.Add("Index", typeof(int));
      dl.Columns.Add("ComplianceCategoryID", typeof(long));
      dl.Columns.Add("ComplianceCategoryText", typeof(string));
      dl.Columns.Add("ComplianceID", typeof(long));
      dl.Columns.Add("ComplianceText", typeof(string));
      dl.Columns.Add("Remarks", typeof(string));
      dl.Columns.Add("RFAID", typeof(long));
      dl.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      dl.Columns.Add("ModifiedOn", typeof(DateTimeOffset));
      dl.Columns.Add("Text", typeof(string));

      if (rfa.LineItems != null)
      {
        foreach (RFALineItem item in rfa.LineItems)
        {
          dl.Rows.Add(
            item.Scheme,
            item.Index,
            item.ChecklistCategoryID,
            item.ChecklistCategoryText,
            item.ChecklistID,
            item.ChecklistText,
            item.Remarks,
            item.RFAID,
            item.CreatedOn,
            item.ModifiedOn);

          if (item.Attachments != null)
          {
            var attachments = ToRFAAttachmentData(item.Attachments.ToList(), item.Index);
            if (lineitemattachments != null && lineitemattachments.Rows.Count > 0
              && attachments != null && attachments.Rows.Count > 0)
            {
              lineitemattachments.Merge(attachments);
            }
            else
            {
              lineitemattachments = attachments;
            }
          }
          if (item.Replies != null && item.Replies.Count > 0)
          {
            var reply = ToRFALineItemReplyData(item, out lineReplyAttachments);
            if (reply != null && reply.Rows.Count > 0)
            {
              replyItems.Merge(reply);
            }
            if (lineReplyAttachments != null && lineReplyAttachments.Rows.Count > 0)
            {
              if (lineitemreplyattachments != null && lineitemreplyattachments.Rows.Count > 0)
              {
                lineitemreplyattachments.Merge(lineReplyAttachments);
              }
              else
              {
                lineitemreplyattachments = lineReplyAttachments;
              }
            }

          }
        }
      }
      lineitemreply = replyItems;
      lineitemattachment = lineitemattachments;
      return dl;
    }

    public static DataTable ToRFALineItemReplyData(
      RFALineItem rfalineitem,
       out DataTable lineItemReplyAttachment)
    {
      var lineItemReply = new DataTable();

      lineItemReply.Columns.Add("RFAID", typeof(long));
      lineItemReply.Columns.Add("LineItemID", typeof(long));
      lineItemReply.Columns.Add("Scheme", typeof(int));
      lineItemReply.Columns.Add("ComplianceCategoryID", typeof(long));
      lineItemReply.Columns.Add("ComplianceID", typeof(long));
      lineItemReply.Columns.Add("Text", typeof(string));
      lineItemReply.Columns.Add("CreatedOn", typeof(DateTimeOffset));
      lineItemReply.Columns.Add("ModifiedOn", typeof(DateTimeOffset));

      lineItemReplyAttachment = new DataTable();
      if (rfalineitem.Replies != null && rfalineitem.Replies.Count > 0)
      {
        foreach (RFAReply rep in rfalineitem.Replies)
        {
          lineItemReply.Rows.Add(
            rfalineitem.RFAID,
            rfalineitem.ID,
            rfalineitem.Scheme,
            rfalineitem.ChecklistCategoryID,
            rfalineitem.ChecklistID,
            rep.Text,
            rep.CreatedOn,
            rep.ModifiedOn);
          //if (rep.Attachments != null)
          //{
          lineItemReplyAttachment = ToRFAReplyAttachmentData(rep.Attachments,
            rfalineitem.ID);
          //}
        }
      }

      return lineItemReply;
    }


    public static DynamicParameters Convert(Attachment entity)
    {
      var param = new DynamicParameters();
      param.Add("@FileID", entity.FileID);
      param.Add("@FileName", entity.FileName);
      param.Add("@Extension", entity.Extension);
      param.Add("@Size", entity.Size);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);
      return param;
    }
  }
}
