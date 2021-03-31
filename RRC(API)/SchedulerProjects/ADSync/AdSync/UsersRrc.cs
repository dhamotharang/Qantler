using System;
using System.Collections.Generic;
using System.Text;

namespace AdSync
{
  class UsersRrc
  {
    public string ADUserName { get; set; }
    public string ADEmployeeName { get; set; }
    public string EmployeeName { get; set; }
    public string EmployeePhoneNumber { get; set; }
    public string OfficialMailId { get; set; }
    public string EmployeePosition { get; set; }
    public string AREmployeePosition { get; set; }
    public string AREmployeeName { get; set; }


    public UsersRrc()
    {
    }

    public Boolean Eq(UsersRrc obj)
    {
      if (obj.EmployeeName != this.EmployeeName || obj.EmployeePhoneNumber != this.EmployeePhoneNumber || obj.OfficialMailId != this.OfficialMailId ||
          obj.EmployeePosition != this.EmployeePosition || obj.AREmployeePosition != this.AREmployeePosition || obj.AREmployeeName != this.AREmployeeName)
      {
        return false;
      }

      return true;
    }
  }
}
