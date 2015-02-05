using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroAssistant.Data;
using System.Data;
using System.Data.SqlClient;


namespace MicroAssistant.Lib
{
   public class RegdUser
    {
       MicroAssistantEntities objcontext = new MicroAssistantEntities();
       public void RegUser(long UserID,string UserName,string usrPassword,string usrEmail,string usrImage,string Website)
       {
            objcontext.SP_Resitered_Users_Edition(UserID, UserName, usrPassword, usrEmail, usrImage, Website);
        }
       public void LoginHistory(string UserName, string IPAddress)
       {
           objcontext.SP_Login_Histories(UserName, IPAddress, DateTime.UtcNow);
       }
       public void UserDetail(long userId)
       {
           objcontext.SP_Resitered_User_Detail(userId).ToList();
       }
       public void UserActivation(string stremail,string stractval)
       {
           objcontext.SP_Registered_User_Activation(stremail, stractval);
       }
       public DataSet validUser(string stremail)
       {
          
           DataSet dsresumeSearch = new DataSet();
           SqlConnection sqlcon = new SqlConnection(objcontext.Database.Connection.ConnectionString);
           SqlDataAdapter adpviewjobtrack = new SqlDataAdapter("SP_Verify_Registered_User", sqlcon);
           adpviewjobtrack.SelectCommand.CommandType = CommandType.StoredProcedure;

           adpviewjobtrack.SelectCommand.Parameters.AddWithValue("@email", stremail);
           adpviewjobtrack.Fill(dsresumeSearch);
           sqlcon.Close();
           return dsresumeSearch;
       }

    }
   public enum UserRoles
   {
       Host,
       Admin,
       SiteUser,
       UnverifiedUser
   }
}
