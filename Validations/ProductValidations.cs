using DamianGonzalezCSharp.Handlers;
using DamianGonzalezCSharp.Models;
using System.Data;
using System.Net;

namespace DamianGonzalezCSharp.Validations
{
    public class ProductValidations
    {
        private ProductResponse response = new ProductResponse();
        public ProductResponse ValidateCreation(Product data)
        {
            response.Success = true;

            if (response.Success == true && data.Id != 0)
            {
                response.GenerateResponse(false, "Can't assign Id to a new Product", HttpStatusCode.Conflict);
            }

            if (response.Success == true && data.UserId == 0)
            {
                response.GenerateResponse(false, "Can't create a product without an user id", HttpStatusCode.Conflict);
            }

            return response;
        }

        public ProductResponse ValidateUpdate(Product data)
        {
            response.Success = true;

            if (response.Success == true && data.Id == 0)
            {
                response.GenerateResponse(false, "Product Id required for update", HttpStatusCode.Conflict);
            }

            if (response.Success == true && data.UserId == 0)
            {
                response.GenerateResponse(false, "Can't create a product without an user id", HttpStatusCode.Conflict);
            }

            return response;
        }

        public ProductResponse ValidateDelete(Int32 id)
        {
            response.Success = true;
            DataSet ds = new DataSet();
            ProductSaleHandler handler = new ProductSaleHandler();

            if (response.Success == true && id == 0)
            {
                response.GenerateResponse(false, "Product Id required to delete", HttpStatusCode.Conflict);
            }

            response.Success = handler.HandleGetProductSales(ds, id, IdColumns.product);

            if (response.Success && ds.Tables["DAT"].Rows.Count > 0)
            {
                response.GenerateResponse(false, "Can't delete Product, first delete ProductSales ", HttpStatusCode.Conflict);
                foreach (DataRow dr in ds.Tables["DAT"].Rows)
                {
                    response.Message = response.Message + " id:" + Convert.ToString(dr["Id"]);
                }
            }

            ds.Dispose();
            return response;
        }
    }
}
