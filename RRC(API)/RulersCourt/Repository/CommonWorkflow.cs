using RulersCourt.Translators;
using System.Data.SqlClient;
using Workflow;

namespace RulersCourt.Repository
{
    public class CommonWorkflow
    {
        public static Actor GetActor(int? userID, string conn)
        {
            if (userID == 0 || userID is null)
            {
                Actor actor = new Actor();
                actor.Email = "WrdUser";
                actor.Name = "WrdUser";
                return actor;
            }

            SqlParameter[] param = { new SqlParameter("@P_UserID", userID), new SqlParameter("@P_Department", null) };
            var temp = SqlHelper.ExecuteProcedureReturnData<Actor>(conn, "Get_User_Workflow", r => r.TranslateAsGetActors(), param);
            return temp;
        }
    }
}
