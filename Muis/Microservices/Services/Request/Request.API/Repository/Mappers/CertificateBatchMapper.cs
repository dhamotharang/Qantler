using System;
using System.Collections.Generic;
using Request.Model;

namespace Request.API.Repository.Mappers
{
  public class CertificateBatchMapper
  {
    readonly IDictionary<long, CertificateBatch> _batchCache =
      new Dictionary<long, CertificateBatch>();

    readonly IDictionary<long, Certificate> _certificateCache =
      new Dictionary<long, Certificate>();

    readonly IDictionary<long, Comment> _commentCache =
     new Dictionary<long, Comment>();

    readonly IDictionary<long, Menu> _menuCache =
     new Dictionary<long, Menu>();

    public CertificateBatch Map(CertificateBatch batch,
      Certificate certificate = null,
      Premise premise = null,
      Premise mailingPremise = null,
      Comment comment = null,
      Menu menu = null)
    {
      if (!_batchCache.TryGetValue(batch.ID, out CertificateBatch result))
      {
        _batchCache[batch.ID] = batch;
        result = batch;
      }

      Certificate outCertificate = null;
      if (   (certificate?.ID ?? 0) > 0
          && !_certificateCache.TryGetValue(certificate.ID, out outCertificate))
      {
        outCertificate = certificate;
        _certificateCache[certificate.ID] = certificate;

        certificate.Premise = premise;
        certificate.MailingPremise = mailingPremise;

        if (result.Certificates == null)
        {
          result.Certificates = new List<Certificate>();
        }
        result.Certificates.Add(certificate);
      }

      if(   (comment?.ID ?? 0) > 0
         && !_commentCache.ContainsKey(comment.ID))
      {
        _commentCache[comment.ID] = comment;

        if (result.Comments == null)
        {
          result.Comments = new List<Comment>();
        }
        result.Comments.Add(comment);
      }

      if (   (menu?.ID ?? 0) > 0
          && !_menuCache.ContainsKey(menu.ID))
      {
        _menuCache[menu.ID] = menu;

        if (outCertificate.Menus == null)
        {
          outCertificate.Menus = new List<Menu>();
        }
        outCertificate.Menus.Add(menu);
      }

      return batch;
    }
  }
}
