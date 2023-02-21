using DamianGonzalezCSharp.Handlers;
using DamianGonzalezCSharp.Models;
using System.Data;
using System.Net;

namespace DamianGonzalezCSharp.Validations
{
    public class UserValidations
    {
        private UserResponse response = new UserResponse();
        public UserResponse ValidateCreation(User data)
        {
            response.Success = true;
            DataSet ds = new DataSet();
            UserHandler handler = new UserHandler();

            if (response.Success == true && data.Id != 0)
            {
                response.GenerateResponse(false, "Can't assign Id to a new User", HttpStatusCode.Conflict);
            }

            if (response.Success == true && (data.Name == "" || data.Name == null))
            {
                response.GenerateResponse(false, "Can't create an User without Name", HttpStatusCode.Conflict);
            }

            if (response.Success == true && (data.Surname == "" || data.Surname == null))
            {
                response.GenerateResponse(false, "Can't create an User without Surname", HttpStatusCode.Conflict);
            }

            if (response.Success == true && (data.UserName == "" || data.UserName == null))
            {
                response.GenerateResponse(false, "Can't create an User without Username", HttpStatusCode.Conflict);
            }

            if (response.Success == true && (data.Password == "" || data.Password == null))
            {
                response.GenerateResponse(false, "Can't create an User without password", HttpStatusCode.Conflict);
            }

            if (response.Success == true && (data.Email == "" || data.Email == null))
            {
                response.GenerateResponse(false, "Can't create an User without Email", HttpStatusCode.Conflict);
            }

            if (response.Success == true) response.Success = handler.HandleGetUsers(ds, data.UserName);

            if (response.Success && ds.Tables["DAT"].Rows.Count > 0)
            {
                response.GenerateResponse(false, "Username already exists", HttpStatusCode.Conflict);
            }

            ds.Dispose();
            return response;
        }

        public UserResponse ValidateUpdate(User data)
        {
            response.Success = true;

            if (response.Success == true && (data.UserName == "" || data.UserName == null))
            {
                response.GenerateResponse(false, "Username is required to update", HttpStatusCode.Conflict);
            }

            if (response.Success == true && data.Password != "" && data.Password != null)
            {
                response.GenerateResponse(false, "Use EditPassword to update the user password", HttpStatusCode.Conflict);
            }

            return response;
        }

        public UserResponse ValidateDelete(Int32 id)
        {
            response.Success = true;
            DataSet ds = new DataSet();
            SaleHandler saleHandler = new SaleHandler();
            ProductHandler productHandler = new ProductHandler();

            if (response.Success == true && id == 0)
            {
                response.GenerateResponse(false, "UserId required to delete", HttpStatusCode.Conflict);
            }

            response.Success = saleHandler.HandleGetSalesByUserId(ds, id);

            if (response.Success && ds.Tables["DAT"].Rows.Count > 0)
            {
                response.GenerateResponse(false, "Can't delete User, first delete Sale ", HttpStatusCode.Conflict);
                foreach (DataRow dr in ds.Tables["DAT"].Rows)
                {
                    response.Message = response.Message + " id:" + Convert.ToString(dr["Id"]);
                }
            }

            response.Success = productHandler.HandleGetProducts(ds, id, IdColumns.user);

            if (response.Success && ds.Tables["DAT"].Rows.Count > 0)
            {
                response.GenerateResponse(false, "Can't delete User, first delete ProductSales ", HttpStatusCode.Conflict);
                foreach (DataRow dr in ds.Tables["DAT"].Rows)
                {
                    response.Message = response.Message + " id:" + Convert.ToString(dr["Id"]);
                }
            }

            ds.Dispose();
            return response;
        }

        public UserResponse ValidatePassword(UserPassword data)
        {
            response.Success = true;
            DataSet ds = new DataSet();
            LoginHandler handler = new LoginHandler();

            Login loginData = new Login(data.UserName, data.Password);

            response.Success = handler.HandleLoginUser(ds, loginData);

            if (response.Success && ds.Tables["DAT"].Rows.Count == 0)
            {
                response.GenerateResponse(false, "Wrong Username or Password", HttpStatusCode.Unauthorized);
            }

            ds.Dispose();
            return response;
        }
    }
}
