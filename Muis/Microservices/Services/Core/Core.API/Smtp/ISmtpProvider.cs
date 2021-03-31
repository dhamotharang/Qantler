using System;

namespace Core.API.Smtp
{
  public interface ISmtpProvider
  {
    void Send(Mail mail);
  }
}
