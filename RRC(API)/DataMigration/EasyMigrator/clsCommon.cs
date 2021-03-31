using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Security;
using Microsoft.SharePoint.Client;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Net;

namespace EasyMiger
{
    public static class clsCommon
    {
        public static string xmlFilePath = "Configuration.xml";
        public static string[] xmlSettingsFieldsList = new string[] { "SPServerType", "SPServerURL", "SPSiteURL", "SPDomain", "SPUserID", "SPPassword", "SQLConnectionString", "AttachmentPath", "RunFromCache", "RecordProcessingCount" };
        public static string[] supportedFileds = new string[] { "boolean", "datetime", "fileleafref", "integer", "lookup", "number", "counter", "user", "text", "note", "url", "choice" };
        public static DataTable dtSettings = new DataTable();
        public static DataTable dtContentList = new DataTable();
        public static DataTable dtContentSchema = new DataTable();
        public static ClientContext cliCXT = null;
        public static Web oWeb = null;
        public static SqlConnection sqlConn = null;
        public static string oServerURL = null;
        public static string oSiteURL = null;
        public static string attachmentPath = null;
        public static bool runFromCache = false;
        public static SharePointOnlineCredentials spoCredentails = null;
        public static NetworkCredential spOnPremCredentails = null;
        public static DataRow[] dtListColumn;
        public static string sqlQry = null;
        public static string serverType = null;

        public static clsReturn validateXMLFile()
        {
            createLog("validateXMLFile", "Information", "Started");

            dtContentList.Columns.Add("Name", typeof(string));
            dtContentList.Columns.Add("Type", typeof(string));
            dtContentList.Columns.Add("SubType", typeof(string));
            dtContentList.Columns.Add("IsMasterList", typeof(string));
            dtContentList.Columns.Add("IsListAvailable", typeof(bool));
            dtContentList.Columns.Add("IsAllFiledsAvailable", typeof(bool));
            dtContentList.Columns.Add("ActualItemCount", typeof(Int64));
            dtContentList.Columns.Add("ProcessedItemCount", typeof(Int64));
            dtContentList.Columns.Add("LastItemID", typeof(Int64));


            dtContentSchema.Columns.Add("ListName", typeof(string));
            dtContentSchema.Columns.Add("FieldName", typeof(string));
            dtContentSchema.Columns.Add("SPFieldType", typeof(string));
            dtContentSchema.Columns.Add("SQLDataType", typeof(string));
            dtContentSchema.Columns.Add("AllowNull", typeof(string));
            dtContentSchema.Columns.Add("IsSQLColumn", typeof(bool));
            dtContentSchema.Columns.Add("Order", typeof(Int64));

            //Common fileds for both list and library
            dtContentSchema.Rows.Add("1000_General", "ID", "Counter", "Bigint Identity", "No", false, 1);
            dtContentSchema.Rows.Add("1000_General", "AttachmentPath", "Path", "nvarchar(max)", "Yes", true, 4999);
            dtContentSchema.Rows.Add("1000_General", "Created", "Datetime", "Datetime", "Yes", false, 5000);
            dtContentSchema.Rows.Add("1000_General", "Author", "User", "nvarchar(100)", "Yes", false, 5001);
            dtContentSchema.Rows.Add("1000_General", "Modified", "Datetime", "Datetime", "Yes", false, 5002);
            dtContentSchema.Rows.Add("1000_General", "Editor", "User", "nvarchar(100)", "Yes", false, 5003);

            //List fields

            dtContentSchema.Rows.Add("1000_List", "Title", "Text", "nvarchar(500)", "Yes", false, 2);
            //Library fields
            dtContentSchema.Rows.Add("1000_Library", "FileLeafRef", "Text", "nvarchar(500)", "Yes", false, 2);
            dtContentSchema.Rows.Add("1000_Library", "Type", "Type", "nvarchar(20)", "Yes", true, 4001);
            dtContentSchema.Rows.Add("1000_Library", "Size", "Size", "nvarchar(20)", "Yes", true, 4002);
            //dtContentSchema.Rows.Add("1000_Library", "AttachmentPath", "", "nvarchar(max)", "Yes", true, 4003);

            dtSettings.Columns.Add("Name", typeof(string));
            dtSettings.Columns.Add("Value", typeof(string));


            bool isAllValidationOk, isFiledExists, isValueExists;
            isAllValidationOk = true;
            clsReturn isRet = new clsReturn();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlFilePath);
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name.ToLower() == "settings")
                    {

                        for (int i = 0; i < xmlSettingsFieldsList.Length; i++)
                        {
                            isFiledExists = isValueExists = false;
                            foreach (XmlNode item in node.ChildNodes)
                            {
                                if (xmlSettingsFieldsList[i].ToLower() == Convert.ToString(item.Attributes["name"].InnerText).ToLower())
                                {
                                    isFiledExists = true;
                                    if (!string.IsNullOrEmpty(item.Attributes["value"].InnerText))
                                    {
                                        isValueExists = true;
                                        dtSettings.Rows.Add(Convert.ToString(item.Attributes["name"].InnerText), item.Attributes["value"].InnerText);
                                    }
                                    break;
                                }
                            }
                            if (!isFiledExists || !isValueExists)
                            {
                                isAllValidationOk = isRet.result = false;
                                if (!isFiledExists)
                                {
                                    isRet.message.AppendLine("Settings filed : " + xmlSettingsFieldsList[i] + " not exists");
                                    createLog("validateXMLFile", "ERROR", "Settings filed : " + xmlSettingsFieldsList[i] + " not exists");
                                }
                                else if (!isValueExists)
                                {
                                    isRet.message.AppendLine("Settings filed : " + xmlSettingsFieldsList[i] + " VALUE not exists");
                                    createLog("validateXMLFile", "", "");
                                }
                            }

                        }
                    }
                    else if (node.Name.ToLower() == "content")
                    {
                        Int64 filedOrder = 100;
                        dtContentList.Rows.Add(node.Attributes["name"].InnerText, node.Attributes["type"].InnerText, node.Attributes["subtype"].InnerText, node.Attributes["ismasterlist"].InnerText, false, false, 0, 0, 0);
                        foreach (XmlNode field in node.ChildNodes[0])
                        {
                            //field.Attributes["allownull"].InnerText
                            dtContentSchema.Rows.Add(node.Attributes["name"].InnerText, field.Attributes["name"].InnerText, field.Attributes["fieldtype"].InnerText, field.Attributes["datatype"].InnerText, "Yes", false, filedOrder);
                            filedOrder++;
                        }
                    }
                }
                if (dtContentList.Rows.Count == 0)
                {
                    isAllValidationOk = false;
                    isRet.message.AppendLine("No List / Library configured for migration");
                    createLog("validateXMLFile", "ERROR", "No List / Library configured for migration");
                }

            }
            catch (Exception ex)
            {
                createLog("validateXMLFile", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
            }
            createLog("validateXMLFile", "Result", (isAllValidationOk ? "SUCCESS" : "FAILED"));
            createLog("validateXMLFile", "Information", "*** Completed ***");
            isRet.result = isAllValidationOk;
            return isRet;
        }
        public static clsReturn validateSharePointSite()
        {
            createLog("validateSharePointSite", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                getClientContext();
                createLog("validateSharePointSite", "Information", "SharePoint Site Validation : " + (cliCXT != null ? "Success" : "Failed"));
                if (cliCXT != null)
                {
                    //processLibrary();
                    //getListColumn();
                    isRet.result = false;
                    clsReturn isGet = new clsReturn();
                    isGet = listAvailabilityValidation();
                    if (isGet.result)
                    {
                        isGet = listSchemaAvailabilityValidation();
                        if (isGet.result)
                        {
                            //isGet = anyNonSupportedColumnsPresents();
                            isRet.result = isGet.result;
                            isRet.message.AppendLine(Convert.ToString(isGet.message));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("validateSharePointSite", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
            }
            createLog("validateSharePointSite", "Result", (isRet.result ? "SUCCESS" : "FAILED"));
            createLog("validateSharePointSite", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn listAvailabilityValidation()
        {
            createLog("listAvailabilityValidation", "Information", "Started");
            clsReturn isRet = new clsReturn();
            bool isAllListAvailable = false;
            try
            {
                getClientContext();
                if (cliCXT == null)
                {
                    createLog("listAvailabilityValidation", "ERROR", "Client Context is NULL");
                    return isRet;
                }
                ListCollection targetlListCollection = cliCXT.Web.Lists;
                cliCXT.Load(targetlListCollection);
                cliCXT.ExecuteQuery();
                // Iterate through each list object
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    foreach (List list in targetlListCollection)

                        if (Convert.ToString(dtContentList.Rows[i]["Name"]).ToLower() == Convert.ToString(list.Title).ToLower() && Convert.ToString(dtContentList.Rows[i]["SubType"]).ToLower() == Convert.ToString(list.BaseType).ToLower())
                        {
                            dtContentList.Rows[i]["IsListAvailable"] = true;
                            break;
                        }
                }
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    if (!Convert.ToBoolean(dtContentList.Rows[i]["IsListAvailable"]))
                    {
                        isAllListAvailable = false;
                        isRet.message.AppendLine(Convert.ToString(dtContentList.Rows[i]["SubType"]) + " : " + Convert.ToString(dtContentList.Rows[i]["Name"]) + " doesnot exists");
                    }
                    else if (i == dtContentList.Rows.Count - 1)
                    {
                        isAllListAvailable = true;
                    }
                }
                isRet.result = isAllListAvailable;
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("listAvailabilityValidation", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("listAvailabilityValidation", "Result", "SharePoint List And Library Availability Validation : " + (isAllListAvailable ? "Success" : "Failed"));
            if (!isRet.result)
            {
                createLog("listAvailabilityValidation", "Information", Convert.ToString(isRet.message));
            }
            createLog("listAvailabilityValidation", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn listSchemaAvailabilityValidation()
        {
            createLog("listSchemaAvailabilityValidation", "Information", "Started");
            clsReturn isRet = new clsReturn();
            bool isAllListSchemaAvailable = false;
            try
            {
                getClientContext();
                if (cliCXT == null)
                {
                    createLog("listSchemaAvailabilityValidation", "ERROR", "Client Context is NULL");
                    return isRet;
                }
                // Iterate through each list object
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    List oList = cliCXT.Web.Lists.GetByTitle(Convert.ToString(dtContentList.Rows[i]["Name"]));
                    cliCXT.Load(oList);
                    cliCXT.ExecuteQuery();
                    Int64 itemCount = oList.ItemCount;
                    dtContentList.Rows[i]["ActualItemCount"] = itemCount;
                    if (itemCount > 0)
                    {
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = "<View><Query><OrderBy><FieldRef Name='ID' Ascending='False'/></OrderBy></Query><RowLimit>1</RowLimit></View>";
                        ListItemCollection collListItem = oList.GetItems(camlQuery);
                        cliCXT.Load(collListItem);
                        cliCXT.ExecuteQuery();
                        dtContentList.Rows[i]["LastItemID"] = Convert.ToInt64(collListItem[0]["ID"]);
                    }
                    DataRow[] drSchemaRow = dtContentSchema.Select("ListName='" + dtContentList.Rows[i]["Name"] + "' and IsSQLColumn=" + false);
                    if (drSchemaRow != null && drSchemaRow.Length > 0)
                    {
                        FieldCollection fieldColl = oList.Fields;
                        cliCXT.Load(fieldColl);
                        cliCXT.ExecuteQuery();
                        bool anyFieldMissing = false;
                        foreach (DataRow schema in drSchemaRow)
                        {
                            for (int j = 0; j < fieldColl.Count; j++)
                            {
                                if (Convert.ToString(fieldColl[j].InternalName).ToLower() == Convert.ToString(schema["FieldName"]).ToLower() && Convert.ToString(fieldColl[j].FieldTypeKind).ToLower() == Convert.ToString(schema["SPFieldType"]).ToLower())
                                {
                                    break;
                                }
                                else if (j == fieldColl.Count - 1)
                                {
                                    anyFieldMissing = true;
                                    dtContentList.Rows[i]["IsAllFiledsAvailable"] = false;
                                    isRet.message.AppendLine("List : " + Convert.ToString(dtContentList.Rows[i]["Name"]) + ", Field : " + Convert.ToString(schema["FieldName"]) + " is missing / filed type is mismatch");
                                }
                            }
                        }
                        if (!anyFieldMissing)
                        {
                            dtContentList.Rows[i]["IsAllFiledsAvailable"] = true;
                        }
                    }
                    else
                    {
                        dtContentList.Rows[i]["IsAllFiledsAvailable"] = true;
                    }
                }
                isAllListSchemaAvailable = true;
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    if (!Convert.ToBoolean(dtContentList.Rows[i]["IsAllFiledsAvailable"]))
                    {
                        isAllListSchemaAvailable = false;
                    }
                }
                isRet.result = isAllListSchemaAvailable;
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("listSchemaAvailabilityValidation", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("listSchemaAvailabilityValidation", "Result", "SharePoint List And Library Schema Availability Validation : " + (isAllListSchemaAvailable ? "Success" : "Failed"));
            if (!isRet.result)
            {
                createLog("listSchemaAvailabilityValidation", "Information", Convert.ToString(isRet.message));
            }
            createLog("listSchemaAvailabilityValidation", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn anyNonSupportedColumnsPresents()
        {
            createLog("anyNonSupportedColumnsPresents", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                DataTable dtCurrent = dtContentSchema.Select("IsSQLColumn=0").CopyToDataTable();
                DataView dvCurrent = new DataView(dtCurrent);
                dtCurrent = dvCurrent.ToTable(true, "SPFieldType");
                for (int i = 0; i < dtCurrent.Rows.Count; i++)
                {
                    for (int j = 0; j < supportedFileds.Length; j++)
                    {
                        if (Convert.ToString(dtCurrent.Rows[i]["SPFieldType"]).ToLower() == Convert.ToString(supportedFileds[j]).ToLower())
                        {
                            break;
                        }
                        else if (j == supportedFileds.Length - 1)
                        {
                            isRet.result = false;
                            isRet.message.AppendLine("Field Type : " + Convert.ToString(dtCurrent.Rows[i]["SPFieldType"]).ToLower() + " is not supported");
                        }
                    }
                }
                isRet.result = (isRet.message.Length == 0 ? true : false);
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("anyNonSupportedColumnsPresents", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("anyNonSupportedColumnsPresents", "Result", "Supported Field Validation : " + (isRet.result ? "Success" : "Failed"));
            if (!isRet.result)
            {
                createLog("anyNonSupportedColumnsPresents", "Information", Convert.ToString(isRet.message));
            }
            createLog("anyNonSupportedColumnsPresents", "Information", "*** Completed ***");
            return isRet;
        }
        public static void getListColumn()
        {
            createLog("getListColumn", "Information", "Started");
            List oList = cliCXT.Web.Lists.GetByTitle(Convert.ToString("MigrationList"));
            cliCXT.Load(oList);
            cliCXT.ExecuteQuery();
            FieldCollection fieldColl = oList.Fields;
            cliCXT.Load(fieldColl);
            cliCXT.ExecuteQuery();
            for (int j = 0; j < fieldColl.Count; j++)
            {
                Console.WriteLine(Convert.ToString(fieldColl[j].InternalName).ToLower() + "," + Convert.ToString(fieldColl[j].FieldTypeKind).ToLower());
            }
            Console.ReadLine();
            createLog("getListColumn", "Information", "*** Completed ***");
        }
        public static clsReturn sqlServerValidation()
        {
            createLog("sqlServerValidation", "Information", "Started");


            clsReturn isRet = new clsReturn();
            try
            {
                getSQLConnection();


                SqlCommand cmd = new SqlCommand("select * from sys.objects", sqlConn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                createLog("sqlServerValidation", "Information", "SQL Server Validation : Test connection Succeed");


                string tableName = "test_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt");
                cmd = new SqlCommand("create table " + tableName + "(Id bigint identity, Name nvarchar(500))", sqlConn);
                cmd.ExecuteNonQuery();
                createLog("sqlServerValidation", "Information", "SQL Server Validation :  Test Table creation Succeed");


                cmd = new SqlCommand("drop table " + tableName, sqlConn);
                cmd.ExecuteNonQuery();
                sqlConn.Close();
                sqlConn = null;
                isRet.result = true;

            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("sqlServerValidation", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));

            }
            createLog("sqlServerValidation", "Result", "SQL Server Validation : " + (isRet.result ? "Success" : "Failed"));
            if (!isRet.result)
            {
                createLog("sqlServerValidation", "Information", Convert.ToString(isRet.message));
            }
            createLog("sqlServerValidation", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn attachmentPathValidation()
        {
            createLog("attachmentPathValidation", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                attachmentPath = Convert.ToString(dtSettings.Select("Name='AttachmentPath'")[0]["Value"]);
                if (System.IO.Directory.Exists(attachmentPath))
                {
                    createLog("attachmentPathValidation", "Information", "File System Validation : Attachment Path Directory Exists (" + attachmentPath + ")");
                    string folderName = "test_" + DateTime.Now.ToString("ddMMyyyyhhmmsstt");
                    folderName = Path.Combine(attachmentPath, folderName);
                    System.IO.Directory.CreateDirectory(folderName);
                    createLog("attachmentPathValidation", "Information", "File System Validation : Test folder created");
                    string sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestDocument.txt");
                    string destinationPath = Path.Combine(folderName, "TestDocument.txt");
                    System.IO.File.Copy(sourcePath, destinationPath);
                    createLog("attachmentPathValidation", "Information", "File System Validation : Test file moved to :" + destinationPath);
                    System.IO.Directory.Delete(folderName, true);
                    createLog("attachmentPathValidation", "Information", "File System Validation : Test folder deleted");
                    isRet.result = true;
                }
                else
                {
                    isRet.result = false;
                    createLog("attachmentPathValidation", "Information", "File System Validation : Failed : Attachment Path Directory NOT Exists (" + attachmentPath + ")");
                }
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("attachmentPathValidation", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
            }
            createLog("attachmentPathValidation", "Result", "File System Validation : " + (isRet.result ? "Success" : "Failed"));
            if (!isRet.result)
            {
                createLog("attachmentPathValidation", "Information", Convert.ToString(isRet.message));
            }
            createLog("attachmentPathValidation", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn validateRunFromCache()
        {
            createLog("validateRunFromCache", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                getSQLConnection();
                runFromCache = (Convert.ToString(dtSettings.Select("Name='RunFromCache'")[0]["Value"]).ToLower() == "yes" ? true : false);
                if (runFromCache)
                {
                    sqlQry = "if exists (select * from sys.tables where name='tempTable') select * from tempTable";
                    DataSet ds = executeDataset(sqlQry);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        deleteNotRequiredMigrationList(ds);
                        verifyExistingSQLTableSchemaWithXMLSchema();
                        updateExistingMismatchMigrationListCount();
                        addNewMigrationListToTempTable();
                    }
                    else
                    {
                        sqlQry = "if exists (select * from sys.tables where name='tempTable') drop table tempTable";
                        executeNonQuery(sqlQry);
                        createTempTableInSQL();
                    }
                }
                else
                {
                    sqlQry = "if exists (select * from sys.tables where name='tempTable') drop table tempTable";
                    for (int i = 0; i < dtContentList.Rows.Count; i++)
                    {
                        sqlQry += " if exists (select * from sys.tables where name='" + Convert.ToString(dtContentList.Rows[i]["Name"]) + "') drop table " + Convert.ToString(dtContentList.Rows[i]["Name"]);
                    }
                    executeNonQuery(sqlQry);
                    System.IO.DirectoryInfo di = new DirectoryInfo(attachmentPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    createTempTableInSQL();
                }
                isRet.result = true;
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("validateRunFromCache", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("validateRunFromCache", "Information", "*** Completed ***");
            return isRet;
        }
        public static void deleteNotRequiredMigrationList(DataSet ds)
        {
            createLog("deleteNotRequiredMigrationList", "Information", "Started");
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                for (int k = 0; k < dtContentList.Rows.Count; k++)
                {
                    if (Convert.ToString(dtContentList.Rows[k]["Name"]).ToLower() == Convert.ToString(ds.Tables[0].Rows[j]["Name"]).ToLower())
                    {
                        break;
                    }
                    else if (k == dtContentList.Rows.Count - 1)
                    {
                        sqlQry = "delete from tempTable where ID=" + ds.Tables[0].Rows[j]["ID"];
                        executeNonQuery(sqlQry);
                    }
                }
            }
            createLog("deleteNotRequiredMigrationList", "Information", "*** Completed ***");
        }
        public static void verifyExistingSQLTableSchemaWithXMLSchema()
        {
            createLog("verifyExistingSQLTableSchemaWithXMLSchema", "Information", "Started");
            bool isTableDeleted;
            for (int l = 0; l < dtContentList.Rows.Count; l++)
            {
                isTableDeleted = false;
                DataRow[] drSchemaRow = null;
                if (Convert.ToString(dtContentList.Rows[l]["Type"]).ToLower() == "list")
                {
                    drSchemaRow = dtContentSchema.Select("ListName='" + dtContentList.Rows[l]["Name"] + "' or ListName='1000_General' or ListName='1000_List'");
                }
                else if (Convert.ToString(dtContentList.Rows[l]["Type"]).ToLower() == "library")
                {
                    drSchemaRow = dtContentSchema.Select("ListName='" + dtContentList.Rows[l]["Name"] + "' or ListName='1000_General' or ListName='1000_Library'");
                }
                if (drSchemaRow != null && drSchemaRow.Length > 0)
                {
                    sqlQry = "if exists (select * from sys.tables where name='" + dtContentList.Rows[l]["Name"] + "') select * from " + dtContentList.Rows[l]["Name"];
                    DataSet dsCurrent = executeDataset(sqlQry);
                    if (dsCurrent != null && dsCurrent.Tables.Count > 0 && dsCurrent.Tables[0] != null)
                    {
                        foreach (DataRow drCu in drSchemaRow)
                        {
                            for (int m = 0; m < dsCurrent.Tables[0].Columns.Count; m++)
                            {
                                if (Convert.ToString(drCu["FieldName"]).ToLower() == Convert.ToString(dsCurrent.Tables[0].Columns[m].ColumnName).ToLower())
                                {
                                    break;
                                }
                                else if (m == dsCurrent.Tables[0].Columns.Count - 1)
                                {
                                    //Console.WriteLine("Table : " + dtContentList.Rows[l]["Name"] + ", Field Name :" + Convert.ToString(drCu["FieldName"]) + " does not exists. Table will be dropped");

                                    sqlQry = "drop table " + dtContentList.Rows[l]["Name"] + " delete from tempTable where Name'=" + dtContentList.Rows[l]["Name"] + "'";
                                    executeNonQuery(sqlQry);

                                    if (System.IO.Directory.Exists(Path.Combine(attachmentPath, Convert.ToString(dtContentList.Rows[l]["Name"]))))
                                    {
                                        System.IO.Directory.Delete(Path.Combine(attachmentPath, Convert.ToString(dtContentList.Rows[l]["Name"])));
                                    }
                                    isTableDeleted = true;
                                    break;
                                }
                            }
                            if (isTableDeleted)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            createLog("verifyExistingSQLTableSchemaWithXMLSchema", "Information", "*** Completed ***");
        }
        public static void updateExistingMismatchMigrationListCount()
        {
            createLog("updateExistingMismatchMigrationListCount", "Information", "Started");
            sqlQry = "if exists (select * from sys.tables where name='tempTable') select * from tempTable";
            DataSet ds = executeDataset(sqlQry);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                {
                    Int64 actItem = Convert.ToInt64(dtContentList.Select("Name='" + ds.Tables[0].Rows[l]["Name"] + "'")[0]["ActualItemCount"]);
                    if (Convert.ToInt64(ds.Tables[0].Rows[l]["Total"]) < actItem)
                    {
                        sqlQry = "update tempTable set Total=" + actItem + ",IsFullyMigrated=0 where ID=" + ds.Tables[0].Rows[l]["ID"];
                        executeNonQuery(sqlQry);
                    }
                }
            }
            createLog("updateExistingMismatchMigrationListCount", "Information", "*** Completed ***");
        }
        public static void addNewMigrationListToTempTable()
        {
            createLog("addNewMigrationListToTempTable", "Information", "Started");
            sqlQry = "if exists (select * from sys.tables where name='tempTable') select * from tempTable";
            DataSet ds = executeDataset(sqlQry);
            sqlQry = "";
            DataRow[] cuRow;
            for (int n = 0; n < dtContentList.Rows.Count; n++)
            {
                cuRow = ds.Tables[0].Select("Name='" + dtContentList.Rows[n]["Name"] + "'");
                if (!(cuRow != null && cuRow.Length > 0))
                {
                    sqlQry += "insert into tempTable(Name,Type,Total,Processed,IsFullyMigrated,LastItemID) values('" + Convert.ToString(dtContentList.Rows[n]["Name"]) + "','" + Convert.ToString(dtContentList.Rows[n]["Type"]) + "'," + Convert.ToString(dtContentList.Rows[n]["ActualItemCount"]) + ",0,0,0)";
                }
            }
            executeNonQuery(sqlQry);
            createLog("addNewMigrationListToTempTable", "Information", "*** Completed ***");
        }
        public static clsReturn createTempTableInSQL()
        {
            createLog("createTempTableInSQL", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                sqlQry = "create table tempTable(ID bigint identity,Name nvarchar(100),Type nvarchar(20),Total bigint,Processed bigint,IsFullyMigrated bit,LastItemID bigint)";
                executeNonQuery(sqlQry);
                sqlQry = "";
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    sqlQry += " insert into tempTable(Name,Type,Total,Processed,IsFullyMigrated,LastItemID) values ('" + Convert.ToString(dtContentList.Rows[i]["Name"]) + "','" + Convert.ToString(dtContentList.Rows[i]["Type"]) + "'," + Convert.ToString(dtContentList.Rows[i]["ActualItemCount"]) + ",0,0,0)";
                }

                executeNonQuery(sqlQry);
            }
            catch (Exception ex)
            {
                createLog("createTempTableInSQL", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("createTempTableInSQL", "Information", "*** Completed ***");
            return isRet;
        }
        public static clsReturn processList()
        {
            createLog("processList", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                for (int i = 0; i < dtContentList.Rows.Count; i++)
                {
                    createLog("processList", "Information", "List : " + Convert.ToString(dtContentList.Rows[i]["Name"]) + " processing started");

                    getClientContext();
                    if (cliCXT == null)
                    {
                        createLog("processList", "Information", "Client Context is NULL, Exit from processing");
                        return isRet;
                    }
                    string listName = Convert.ToString(dtContentList.Rows[i]["Name"]);
                    string listType = Convert.ToString(dtContentList.Rows[i]["Type"]);
                    string subType = Convert.ToString(dtContentList.Rows[i]["SubType"]);

                    DataSet dsCurrentInfo = null;
                    if (runFromCache)
                    {
                        sqlQry = "select * from temptable where name='" + listName + "'";
                        dsCurrentInfo = executeDataset(sqlQry);
                        if (dsCurrentInfo != null && dsCurrentInfo.Tables.Count > 0 && dsCurrentInfo.Tables[0].Rows.Count > 0 && Convert.ToBoolean(dsCurrentInfo.Tables[0].Rows[0]["IsFullyMigrated"]))
                        {
                            createLog("processList", "Information", "List : " + listName + " is already migrated");
                            continue;
                        }
                    }
                    //if (listType == "List")
                    //{
                    dtListColumn = null;

                    if (listType.ToLower() == "list")
                    {
                        dtListColumn = dtContentSchema.Select("ListName='1000_General' or ListName='1000_List' or ListName='" + listName + "'", "Order ASC");
                    }
                    else if (listType.ToLower() == "library")
                    {
                        dtListColumn = dtContentSchema.Select("ListName='1000_General' or ListName='1000_Library' or ListName='" + listName + "'", "Order ASC");
                    }
                    sqlQry = "";
                    if (runFromCache)
                    {
                        sqlQry += "if not exists(select * from sys.tables where name='" + listName + "') begin ";
                    }
                    sqlQry += "create table " + listName + "(";
                    for (int j = 0; j < dtListColumn.Length; j++)
                    {
                        sqlQry += Convert.ToString(dtListColumn[j]["FieldName"]) + " " + Convert.ToString(dtListColumn[j]["SQLDataType"]) + " " + (Convert.ToString(dtListColumn[j]["AllowNull"]).ToLower() == "yes" ? "NULL" : "NOT NULL");
                        if (j != dtListColumn.Length - 1)
                        {
                            sqlQry += ",";
                        }
                    }
                    sqlQry += ")";
                    if (runFromCache)
                    {
                        sqlQry += " end ";
                    }
                    getSQLConnection();
                    executeNonQuery(sqlQry);
                    sqlQry = "";
                    int rowLimit = Convert.ToInt32(dtSettings.Select("Name='RecordProcessingCount'")[0]["Value"]);
                    Int64 startIndex = 0;
                    Int64 endIndex = 0;
                    if (runFromCache && (dsCurrentInfo != null && dsCurrentInfo.Tables.Count > 0 && dsCurrentInfo.Tables[0].Rows.Count > 0))
                    {
                        endIndex = Convert.ToInt64(dsCurrentInfo.Tables[0].Rows[0]["LastItemID"]);
                    }
                    Int64 processCount = 0;
                    Int64 lastItemID = Convert.ToInt64(dtContentList.Rows[i]["LastItemID"]);
                    List oList = cliCXT.Web.Lists.GetByTitle(Convert.ToString(dtContentList.Rows[i]["Name"]));
                    cliCXT.Load(oList);
                    cliCXT.ExecuteQuery();
                    while (endIndex < lastItemID)
                    {
                        startIndex = endIndex + 1;
                        if (startIndex < lastItemID)
                        {
                            if (startIndex + (rowLimit - 1) > lastItemID)
                            {
                                endIndex = lastItemID;
                            }
                            else
                            {
                                endIndex = startIndex + (rowLimit - 1);
                            }
                        }
                        else
                        {
                            endIndex = lastItemID;
                        }
                        createLog("processList", "Information", "List : " + listName + " Start Index : " + Convert.ToString(startIndex) + ", End Index : " + Convert.ToString(endIndex));
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = "<View><Query><Where><And><Geq><FieldRef Name='ID' /><Value Type='Number'>" + Convert.ToString(startIndex) + "</Value></Geq>" +
                            "<Leq><FieldRef Name='ID' /><Value Type='Number'>" + Convert.ToString(endIndex) + "</Value></Leq></And></Where><OrderBy><FieldRef Name='ID' Ascending='True'/></OrderBy></Query><RowLimit>" + Convert.ToString(rowLimit) + "</RowLimit></View>";
                        ListItemCollection collListItem = oList.GetItems(camlQuery);
                        cliCXT.Load(collListItem);
                        cliCXT.ExecuteQuery();

                        
                        bool containsMULField = false;
                        DataRow[] drCur = dtContentSchema.Select("ListName='"+listName+"' and SPFieldType='Note'");
                        if(drCur!=null && drCur.Length>0)
                        {
                            containsMULField = true;
                        }
                        FieldStringValues fieldVal=null;
                        foreach (ListItem item in collListItem)
                        {
                            if(containsMULField)
                            {
                                ListItem mulItem = oList.GetItemById(Convert.ToString(item["ID"]));
                                fieldVal = mulItem.FieldValuesAsText;
                                cliCXT.Load(fieldVal);
                                cliCXT.ExecuteQuery();
                            }                           
                            createLog("processList", "Information", "List : " + listName + " : Processing#" + item["ID"].ToString() + " started");
                            if (listType.ToLower() == "list" || (listType.ToLower() == "library" && item.FileSystemObjectType == FileSystemObjectType.File))
                            {
                                insertIntoSQLTable(listName, item, fieldVal);
                            }
                            if (listType.ToLower() == "list")
                            {
                                Folder folder = oWeb.GetFolderByServerRelativeUrl(oSiteURL + "Lists/" + listName + "/Attachments/" + item["ID"]);
                                cliCXT.Load(folder);
                                try
                                {
                                    cliCXT.ExecuteQuery();
                                }
                                catch (ServerException ex)
                                {
                                    processCount++;
                                    updateTempTableData(item, listName);
                                    continue;
                                }
                                FileCollection oColl = folder.Files;
                                cliCXT.Load(oColl);
                                cliCXT.ExecuteQuery();
                                clsReturn isGetAttach = new clsReturn();
                                isGetAttach = processAttachment(oColl, listName, item, null, false, null);
                                if (isGetAttach.result)
                                {
                                    processCount++;
                                    updateTempTableData(item, listName);
                                }
                            }
                            else if (listType.ToLower() == "library" && item.FileSystemObjectType == FileSystemObjectType.File)
                            {
                                Microsoft.SharePoint.Client.File oFile = item.File;
                                cliCXT.Load(oFile);
                                try
                                {
                                    cliCXT.ExecuteQuery();
                                }
                                catch (ServerException ex)
                                {
                                    processCount++;
                                    updateTempTableData(item, listName);
                                    //Console.WriteLine("No Attachment for ID " + item["ID"].ToString());
                                    createLog("processList", "WARNING", "List : " + listName + " : Processing ID " + item["ID"].ToString() + " NO ATTACHMENT Found for library");
                                    continue;
                                }
                                clsReturn isGetAttach = new clsReturn();
                                isGetAttach = processAttachment(null, listName, item, oFile, false, null);
                                if (isGetAttach.result)
                                {
                                    processCount++;
                                    updateTempTableData(item, listName);
                                }
                            }
                            else if (listType.ToLower() == "library" && item.FileSystemObjectType == FileSystemObjectType.Folder)
                            {
                                Microsoft.SharePoint.Client.Folder oFi = item.Folder;
                                cliCXT.Load(oFi);
                                cliCXT.ExecuteQuery();
                                string fileName = "";
                                fileName = attachmentPath + "\\" + listName;
                                if (!Directory.Exists(fileName))
                                {
                                    Directory.CreateDirectory(fileName);
                                }
                                fileName = fileName + "\\" + Convert.ToString(oFi.Name);
                                if (!Directory.Exists(fileName))
                                {
                                    Directory.CreateDirectory(fileName);
                                }
                                updateTempTableData(item, listName);
                                GetFoldersAndFiles(oFi, fileName, listName, fieldVal);
                            }
                            dtContentList.Rows[i]["ProcessedItemCount"] = Convert.ToString(Convert.ToInt64(dtContentList.Rows[i]["ProcessedItemCount"]) + 1);
                            dtContentList.Rows[i]["LastItemID"] = Convert.ToString(item["ID"]);
                            createLog("processList", "Information", "List : " + listName + " : Processing ID " + item["ID"].ToString() + " completed");
                        }
                    }
                    sqlQry = "if exists (select * from sys.tables where name='tempTable') begin update temptable set IsFullyMigrated=1 where Total=Processed and IsFullyMigrated=0  end";
                    executeNonQuery(sqlQry);
                    sqlConn.Close();
                    sqlConn = null;
                }
            }
            catch (Exception ex)
            {
                createLog("processList", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("processList", "Information", "*** Completed ***");
            createLog("processList", "Information", "");
            createLog("processList", "Information", "*********** PROCESSING COMPLETED *****************");
            return isRet;
        }
        public static void updateTempTableData(ListItem item, string listName)
        {
            createLog("updateTempTableData", "Information", "Started");
            sqlQry = " Update temptable set Processed=Processed+1,LastItemID=" + Convert.ToString(item["ID"]) + " where name='" + listName + "'";
            executeNonQuery(sqlQry);
            createLog("updateTempTableData", "Information", "*** Completed ***");
        }
        public static clsReturn processAttachment(FileCollection oFiles, string listName, ListItem item, Microsoft.SharePoint.Client.File cuFile, bool isFolderCreated, string pathString)
        {
            createLog("processAttachment", "Information", "Started");
            clsReturn isRet = new clsReturn();
            try
            {
                List<string> attachmentList = new List<string>();
                if (oFiles != null && oFiles.Count > 0)
                {
                    foreach (Microsoft.SharePoint.Client.File oFile in oFiles)
                    {
                        var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(cliCXT, oFile.ServerRelativeUrl);
                        string fileName = getAttachmentFileName(item, oFile, listName, isFolderCreated, pathString);
                        using (var fileStream = System.IO.File.Create(fileName))
                        {
                            fileInfo.Stream.CopyTo(fileStream);
                        }
                        attachmentList.Add(fileName);
                    }
                }
                else if (cuFile != null)
                {
                    var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(cliCXT, cuFile.ServerRelativeUrl);
                    string fileName = getAttachmentFileName(item, cuFile, listName, isFolderCreated, pathString);
                    using (var fileStream = System.IO.File.Create(fileName))
                    {
                        fileInfo.Stream.CopyTo(fileStream);
                    }
                    attachmentList.Add(fileName);
                }
                sqlQry = "update " + listName + " set AttachmentPath='" + string.Join(",", attachmentList.ToArray()).Replace(attachmentPath,"") + "' where ID=" + Convert.ToString(item["ID"]);
                executeNonQuery(sqlQry);
                isRet.result = true;
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                //throw;
                createLog("processAttachment", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));

            }
            createLog("processAttachment", "Result", (isRet.result ? "SUCCESS" : "FAILED"));
            createLog("processAttachment", "Information", "*** Completed ***");
            return isRet;
        }
        public static string getAttachmentFileName(ListItem item, Microsoft.SharePoint.Client.File oFile, string listName, bool isFolderCreated, string pathString)
        {
            createLog("getAttachmentFileName", "Information", "Started");
            string fileName = "";
            if (!isFolderCreated)
            {
                fileName = attachmentPath + "\\" + listName;
                if (!Directory.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                }
                fileName = fileName + "\\" + Convert.ToString(item["ID"]);
                if (!Directory.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                }
                fileName = fileName + "\\" + oFile.Name;
            }
            else
            {
                fileName = pathString + "\\" + oFile.Name;
            }
            createLog("getAttachmentFileName", "Information", "*** Completed ***");
            return fileName;

        }
        public static string getValueFromListItem(string colName, string colType, ListItem item, bool isSQLColumn, FieldStringValues fieldVal)
        {
            //createLog("getValueFromListItem", "Information", "Started : Column Name : " + colName);

            string isret = "NULL";
            if (!isSQLColumn)
            {
                if (item[colName] != null)
                {
                    switch (colType.ToLower())
                    {
                        case "boolean":
                            isret = (Convert.ToBoolean(item[colName]) ? "1" : "0");
                            break;
                        case "datetime":
                            isret = "'" + Convert.ToString(Convert.ToDateTime(item[colName])) + "'";
                            break;
                        case "integer":
                            isret = "'" + Convert.ToString(Convert.ToInt64(item[colName])) + "'";
                            break;
                        case "lookup":
                            FieldLookupValue fuv = (FieldLookupValue)item[colName];
                            if (fuv != null)
                                isret = "'" + fuv.LookupValue + "'";
                            break;
                        case "attachments":

                            break;
                        case "number":
                            isret = Convert.ToString(Convert.ToInt64(item[colName]));
                            break;
                        case "counter":
                            isret = Convert.ToString(Convert.ToInt64(item[colName]));
                            break;
                        case "user":
                            FieldUserValue fuv1 = (FieldUserValue)item[colName];
                            if (fuv1 != null)
                                isret = "'" + Convert.ToString(fuv1.LookupValue) + "'";
                            break;
                        case "text":
                            isret = "'" + Convert.ToString(Convert.ToString(item[colName])) + "'";
                            break;
                        case "note":
                            isret = "'" + Convert.ToString(Convert.ToString(fieldVal[colName])) + "'";
                            break;
                        case "url":
                            FieldUrlValue fuv2 = (FieldUrlValue)item[colName];
                            if (fuv2 != null)
                                isret = "'" + Convert.ToString(fuv2.Url) + "'";
                            break;
                        case "choice":
                            isret = "'" + Convert.ToString(Convert.ToString(item[colName])) + "'";
                            break;
                        default:
                            isret = "'" + Convert.ToString(Convert.ToString(item[colName])) + "'";
                            break;
                    }
                }
            }
            else
            {
                switch (colType.ToLower())
                {
                    case "path":
                        isret = "NULL";
                        break;
                    case "type":
                        isret = "NULL";
                        break;
                    case "size":
                        isret = "NULL";
                        break;
                    default:
                        isret = "NULL";
                        break;
                }
            }
            //createLog("getValueFromListItem", "Information", "*** Completed ***");
            return isret;
        }
        public static string getValueFromMULFiled(ListItemCollection mulColl, ListItem item, string colName)
        {
            string isRet = "";

            foreach (ListItem cuItem in mulColl)
            {
                if (Convert.ToString(cuItem["ID"]) == Convert.ToString(item["ID"]))
                {
                    isRet = Convert.ToString(cuItem[colName]);
                    break;
                }
            }
            return isRet;
        }
        public static clsReturn executeNonQuery(string query)
        {
            //createLog("executeNonQuery", "Information", "Started");

            clsReturn isRet = new clsReturn();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.ExecuteNonQuery();
                isRet.result = true;
            }
            catch (Exception ex)
            {
                isRet.result = false;
                isRet.message.AppendLine(ex.Message);
                createLog("executeNonQuery", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));

            }
            createLog("executeNonQuery", "Result", "executeNonQuery : " + (isRet.result ? "Success" : "Failed"));
            //createLog("executeNonQuery", "Information", "*** Completed ***");
            return isRet;
        }
        public static DataSet executeDataset(string query)
        {
            createLog("executeDataset", "Information", "Started");
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                createLog("executeDataset", "EXCEPTION", ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
            }
            createLog("executeDataset", "Result", "executeDataset : " + (ds != null && ds.Tables.Count > 0 ? "Success" : "Failed"));
            createLog("executeDataset", "Information", "*** Completed ***");
            return ds;
        }
        public static void getClientContext()
        {
            createLog("getClientContext", "Information", "Started");
            try
            {
                if (cliCXT == null || oWeb == null)
                {
                    serverType = Convert.ToString(dtSettings.Select("Name='SPServerType'")[0]["Value"]);
                    string domain = Convert.ToString(dtSettings.Select("Name='SPDomain'")[0]["Value"]);
                    string userName = Convert.ToString(dtSettings.Select("Name='SPUserID'")[0]["Value"]);
                    oServerURL = Convert.ToString(dtSettings.Select("Name='SPServerURL'")[0]["Value"]);
                    oSiteURL = Convert.ToString(dtSettings.Select("Name='SPSiteURL'")[0]["Value"]);
                    oSiteURL = oServerURL + oSiteURL;
                    string password = Convert.ToString(dtSettings.Select("Name='SPPassword'")[0]["Value"]);
                    SecureString securePassword = new SecureString();
                    foreach (char c in password)
                        securePassword.AppendChar(c);
                    cliCXT = new ClientContext(oSiteURL);
                    if (serverType.ToLower() == "online")
                    {
                        spoCredentails = new SharePointOnlineCredentials(userName, securePassword);
                        cliCXT.Credentials = spoCredentails;
                    }
                    else if (serverType.ToLower() == "onPrem")
                    {
                        spOnPremCredentails = new NetworkCredential(userName, password, domain);
                        cliCXT.Credentials = spOnPremCredentails;
                    }
                    oWeb = cliCXT.Web;
                    cliCXT.Load(oWeb);
                    cliCXT.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                cliCXT = null;
                createLog("getClientContext", "EXCEPTION", "Authentication Error : " + ex.Message + (ex.InnerException != null ? " : " + ex.InnerException.Message : ""));
                throw;
            }
            createLog("getClientContext", "Information", "*** Completed ***");
            //return cliCXT;
        }
        public static void getSQLConnection()
        {
            createLog("getSQLConnection", "Information", "Started");
            if (sqlConn == null)
            {
                sqlConn = new SqlConnection(Convert.ToString(dtSettings.Select("Name='SQLConnectionString'")[0]["Value"]));
            }
            if (sqlConn != null && sqlConn.State != ConnectionState.Open)
            {
                sqlConn.Open();
            }
            createLog("getSQLConnection", "Information", "*** Completed ***");
        }
        public static void insertIntoSQLTable(string listName, ListItem item, FieldStringValues fieldVal)
        {
            createLog("insertIntoSQLTable", "Information", "Started");
            string colNameList = "";
            sqlQry = "SET IDENTITY_INSERT " + listName + " ON ";
            sqlQry += " if exists(select * from " + listName + " where ID=" + Convert.ToString(item["ID"]) + ") begin delete from " + listName + " where ID=" + Convert.ToString(item["ID"]) + " end";
            sqlQry += "  insert into " + listName + "(AA_COL__NMAE__LIST_BB) values(";
            for (int j = 0; j < dtListColumn.Length; j++)
            {
                colNameList += Convert.ToString(dtListColumn[j]["FieldName"]);

                sqlQry += getValueFromListItem(Convert.ToString(dtListColumn[j]["FieldName"]), Convert.ToString(dtListColumn[j]["SPFieldType"]), item, Convert.ToBoolean(dtListColumn[j]["IsSQLColumn"]), fieldVal);
                if (j != dtListColumn.Length - 1)
                {
                    colNameList += ",";
                    sqlQry += ",";
                }
            }
            sqlQry += ")";
            sqlQry += " SET IDENTITY_INSERT " + listName + " OFF ";
            sqlQry = sqlQry.Replace("AA_COL__NMAE__LIST_BB", colNameList);
            executeNonQuery(sqlQry);
            createLog("insertIntoSQLTable", "Information", "*** Completed ***");
        }
        public static void processLibrary()
        {
            createLog("processLibrary", "Information", "Started");
            var list = cliCXT.Web.Lists.GetByTitle("MigrationLibrary");
            var rootFolder = list.RootFolder;
            cliCXT.Load(list);
            string pathString = Convert.ToString(dtSettings.Select("Name='AttachmentPath'")[0]["Value"]);
            GetFoldersAndFiles(rootFolder, pathString, "", null);
            createLog("processLibrary", "Information", "*** Completed ***");
        }
        private static void GetFoldersAndFiles(Folder mainFolder, string pathString, string listName, FieldStringValues fieldVal)
        {
            createLog("GetFoldersAndFiles", "Information", "Started");
            cliCXT.Load(mainFolder, k => k.Files, k => k.Folders);
            cliCXT.ExecuteQuery();
            foreach (var folder in mainFolder.Folders)
            {
                string folderPath = string.Format(@"{0}\{1}", pathString, folder.Name);
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                ListItem item = folder.ListItemAllFields;
                cliCXT.Load(item);
                cliCXT.ExecuteQuery();
                insertIntoSQLTable(listName, item, fieldVal);
                updateTempTableData(item, listName);

                GetFoldersAndFiles(folder, folderPath, listName, fieldVal);
            }
            foreach (var file in mainFolder.Files)
            {
                ListItem item = file.ListItemAllFields;
                cliCXT.Load(item);
                cliCXT.ExecuteQuery();
                insertIntoSQLTable(listName, item, fieldVal);
                processAttachment(null, listName, item, file, true, pathString);
                updateTempTableData(item, listName);
            }
            createLog("GetFoldersAndFiles", "Information", "*** Completed ***");
        }
        public static void createLog(string location, string type, string message)
        {
            string logFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Log.txt");
            string formatString = "                                        ";
            location = (location + formatString).Substring(0, 25);
            type = (type + formatString).Substring(0, 20);
            if (!System.IO.File.Exists(logFileName))
            {
                System.IO.File.Create(logFileName).Dispose();

            }
            string logMessage = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss:tt") + "\t" + location + "\t" + type + "\t" + message;
            using (var tw = new StreamWriter(logFileName, true))
            {
                tw.WriteLine(logMessage);
            }
            message = DateTime.Now.ToString("hh:mm:ss:tt") + "  " + location + "  " + message;
            Console.WriteLine(message);
        }
    }
}
