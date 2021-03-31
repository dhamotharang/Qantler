using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;

namespace AdSync
{
  class SyncTask
  {
    static public string searchBase = string.Empty;
    static public int searchScope = LdapConnection.SCOPE_SUB;
    static public string[] attributeList = { "sAMAccountName", "cn", "givenName", "displayName", "telephoneNumber", "mail", "title", "extensionAttribute1", "extensionAttribute2" };

    LdapConnection ldapConn;
    static public string ladpConnectionString;
    static public int ladpConnectionPort = 389;

    static public string ladpbindcn;
    static public string ladpbindpassword;

    SqlConnection sqlConnection;
    static public string sqlServerConnectionString;

    private int updatedCount = 0;
    private int createdCount = 0;

    static public byte[] key;
    static public byte[] iv;

    public SyncTask()
    {
      try
      {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json");
        var configuration = builder.Build();


       key = Convert.FromBase64String(configuration["EncryptionKeys:Key"]);
       iv = Convert.FromBase64String(configuration["EncryptionKeys:IV"]);

        configuration["exclude:0"] = Decrypt_Aes(configuration["exclude:0"].ToString());
        configuration["exclude:1"] = Decrypt_Aes(configuration["exclude:1"].ToString());
        configuration["exclude:2"] = Decrypt_Aes(configuration["exclude:2"].ToString());
        configuration["exclude:3"] = Decrypt_Aes(configuration["exclude:3"].ToString());                
        configuration["exclude:4"] = Decrypt_Aes(configuration["exclude:4"].ToString());                
        configuration["exclude:5"] = Decrypt_Aes(configuration["exclude:5"].ToString());                

        ladpConnectionString = Decrypt_Aes(configuration.GetSection("ldapConnectionString").Value);
        sqlServerConnectionString = Decrypt_Aes(configuration.GetSection("sqlServerConnectionString").Value);
        ladpbindcn = Decrypt_Aes(configuration.GetSection("binddn").Value);
        ladpbindpassword = Decrypt_Aes(configuration.GetSection("bindpassword").Value);
        searchBase = Decrypt_Aes(configuration.GetSection("searchBase").Value);
      }
      catch (Exception exc)
      {
        Console.WriteLine("Error reading config.json" + exc.StackTrace);
      }

      ldapConn = new LdapConnection();
      sqlConnection = new SqlConnection(sqlServerConnectionString);
    }

       public string Decrypt_Aes(string cipherTextStr)
       {
            byte[] cipherText = Convert.FromBase64String(cipherTextStr);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            string plaintext;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
       }

    public bool connectLadp()
    {
      try
      {
        ldapConn.Connect(ladpConnectionString, ladpConnectionPort);
        ldapConn.Bind(ladpbindcn, ladpbindpassword);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
      }

      return ldapConn.Connected;
    }

    public bool disconnectLadp()
    {
      try
      {
        ldapConn.Disconnect();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);

      }
      return ldapConn.Connected ? true : false;
    }

    public bool connectDB()
    {
      try
      {
        sqlConnection.Open();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);


      }
      return sqlConnection.State == ConnectionState.Open ? true : false;
    }

    public bool closeDB()
    {
      try
      {
        sqlConnection.Close();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);

      }
      return sqlConnection.State == ConnectionState.Open ? true : false;
    }


    public void syncDBwithAD()
    {
      LdapSearchResults lsc = (LdapSearchResults)ldapConn.Search(searchBase, searchScope, "objectCategory=Person", attributeList, false);
      while (lsc.hasMore())
      {
        LdapEntry nextEntry = null;
        try
        {
          nextEntry = lsc.next();
        }
        catch (LdapException e)
        {
          Console.WriteLine("Error: " + e.LdapErrorMessage);
          //Exception is thrown, go for next entry
          continue;
        }

        // Get the attribute set of the entry
        LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
        System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();

        // Parse through the attribute set to get the attributes and the corresponding values
        UsersRrc tempUser = new UsersRrc();
        while (ienum.MoveNext())
        {
          LdapAttribute attribute = (LdapAttribute)ienum.Current;
          string attributeName = attribute.Name;
          string attributeVal = attribute.StringValue;

          switch (attributeName)
          {
            case "sAMAccountName":
              tempUser.ADUserName = attributeVal;
              break;

            case "cn":
              tempUser.ADEmployeeName = attributeVal;
              break;

            case "displayName":
              tempUser.EmployeeName = attributeVal;
              break;

            case "telephoneNumber":
              tempUser.EmployeePhoneNumber = attributeVal;
              break;

            case "mail":
              tempUser.OfficialMailId = attributeVal;
              break;

            case "title":
              tempUser.EmployeePosition = attributeVal;
              break;

            case "extensionAttribute1":
              tempUser.AREmployeeName = attributeVal;
              break;

            case "extensionAttribute2":
              tempUser.AREmployeePosition = attributeVal;
              break;

          }
        }

        if (queryUser(tempUser))
        {
          updateUser(tempUser);
        }
        else
        {
          Console.WriteLine("Creating User : {0}", tempUser.ADUserName);
          createUser(tempUser);
        }
      }

      Console.WriteLine("Sync complete created : {0} users and updated : {1} users.", createdCount, updatedCount);
    }

    private bool queryUser(UsersRrc user)
    {
      Boolean dbresult = true;
      connectDB();

      SqlCommand command = new SqlCommand("Select COUNT(*) from [dbo].[UserProfile] where ADUserName = @adusername", sqlConnection);
      command.Parameters.AddWithValue("@adusername", user.ADUserName);

      var result = command.ExecuteScalar();
      closeDB();
      if (result != null)
      {
        return (int)result == 0 ? false : true;

      }
      return dbresult;
    }

    private bool createUser(UsersRrc user)
    {
      connectDB();
      SqlCommand insertcommand = new SqlCommand("insert into [dbo].[UserProfile] (ADUserName, ADEmployeeName, EmployeeName, OfficialMailId, EmployeePhoneNumber, CreatedDatetime, EmployeePosition, AREmployeePosition, AREmployeeName)  values (@adusername, @adempname, @empname, @mailid, @phone, @CreatedDateTime, @employeeposition, @aremployeeposition ,@aremployeename)", sqlConnection);

      insertcommand.Parameters.AddWithValue("@adusername", user.ADUserName);
      insertcommand.Parameters.AddWithValue("@adempname", user.ADEmployeeName);
      insertcommand.Parameters.AddWithValue("@empname", (object)user.EmployeeName ?? DBNull.Value);
      insertcommand.Parameters.AddWithValue("@mailid", (object)user.OfficialMailId ?? DBNull.Value);
      insertcommand.Parameters.AddWithValue("@phone", (object)user.EmployeePhoneNumber ?? DBNull.Value);
      insertcommand.Parameters.AddWithValue("@CreatedDateTime", DateTime.UtcNow);
      insertcommand.Parameters.AddWithValue("@employeeposition", (object)user.EmployeePosition ?? DBNull.Value);
      insertcommand.Parameters.AddWithValue("@aremployeename", (object)user.AREmployeeName ?? DBNull.Value);
      insertcommand.Parameters.AddWithValue("@aremployeeposition", (object)user.AREmployeePosition ?? DBNull.Value);
      int queryresult = 0;
      try
      {
        queryresult = insertcommand.ExecuteNonQuery();
        createdCount++;

        closeDB();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
        closeDB();
      }

      return (queryresult > 0) ? true : false;
    }

    private bool updateUser(UsersRrc user)
    {
      UsersRrc dbUser = new UsersRrc();
      connectDB();

      SqlCommand command = new SqlCommand("Select ADUserName,ADEmployeeName,EmployeeName,EmployeePhoneNumber,OfficialMailId,AREmployeeName,AREmployeePosition,EmployeePosition from [dbo].[UserProfile] where ADUserName=@adusername", sqlConnection);
      command.Parameters.AddWithValue("@adusername", user.ADUserName);

      using (SqlDataReader oReader = command.ExecuteReader())
      {
        while (oReader.Read())
        {
          dbUser.ADUserName = Convert.IsDBNull(oReader["ADUserName"]) ? null : oReader["ADUserName"].ToString();
          dbUser.ADEmployeeName = Convert.IsDBNull(oReader["ADEmployeeName"]) ? null : oReader["ADEmployeeName"].ToString();
          dbUser.EmployeeName = Convert.IsDBNull(oReader["EmployeeName"]) ? null : oReader["EmployeeName"].ToString();
          dbUser.EmployeePhoneNumber = Convert.IsDBNull(oReader["EmployeePhoneNumber"]) ? null : oReader["EmployeePhoneNumber"].ToString();
          dbUser.OfficialMailId = Convert.IsDBNull(oReader["OfficialMailId"]) ? null : oReader["OfficialMailId"].ToString();
          dbUser.AREmployeeName = Convert.IsDBNull(oReader["AREmployeeName"]) ? null : oReader["AREmployeeName"].ToString();
          dbUser.AREmployeePosition = Convert.IsDBNull(oReader["AREmployeePosition"]) ? null : oReader["AREmployeePosition"].ToString();
          dbUser.EmployeePosition = Convert.IsDBNull(oReader["EmployeePosition"]) ? null : oReader["EmployeePosition"].ToString();
        }

        closeDB();
      }

      if (!dbUser.Eq(user))
      {
        connectDB();

        SqlCommand updatecommand = new SqlCommand("UPDATE  [dbo].[UserProfile] Set ADEmployeeName=@adempname, EmployeeName=@empname, OfficialMailId=@mailid ,EmployeePhoneNumber=@phone, EmployeePosition=@employeeposition, AREmployeeName=@aremployeename,AREmployeePosition=@aremployeeposition where ADUserName=@adusername", sqlConnection);
        updatecommand.Parameters.AddWithValue("@adusername", user.ADUserName);
        updatecommand.Parameters.AddWithValue("@adempname", (object)user.ADEmployeeName ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@empname", (object)user.EmployeeName ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@mailid", (object)user.OfficialMailId ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@phone", (object)user.EmployeePhoneNumber ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@employeeposition", (object)user.EmployeePosition ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@aremployeename", (object)user.AREmployeeName ?? DBNull.Value);
        updatecommand.Parameters.AddWithValue("@aremployeeposition", (object)user.AREmployeePosition ?? DBNull.Value);
        int updateresult = updatecommand.ExecuteNonQuery();
        if (updateresult > 0)
        {
          updatedCount++;
          Console.WriteLine("Updated user : {0} , {1} , {2} , {3}", user.ADUserName, user.EmployeeName, user.EmployeePhoneNumber, user.OfficialMailId);
        }
        else
        {
          Console.WriteLine("Error updating  user : {0} , {1} , {2} , {3}", user.ADUserName, user.EmployeeName, user.EmployeePhoneNumber, user.OfficialMailId);
        }
        closeDB();
      }

      return true;
    }
  }
}
