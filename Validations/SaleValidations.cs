using DamianGonzalezCSharp.Handlers;
using DamianGonzalezCSharp.Models;
using System.Data;
using System.Net;

namespace DamianGonzalezCSharp.Validations
{
    public class SaleValidations
    {
        private SaleResponse response = new SaleResponse();

        public SaleResponse ValidateCreation(Sale data)
        {
            response.Success = true;

            if (response.Success == true && data.Id != 0)
            {
                response.GenerateResponse(false, "Can't assign Id to a new Sale", HttpStatusCode.Conflict);
            }
            if (response.Success == true && data.UserId == 0)
            {
                response.GenerateResponse(false, "Can't create a sale without an user id", HttpStatusCode.Conflict);
            }
            if (response.Success == true && (data.SaleProducts == null || data.SaleProducts.Count == 0))
            {
                response.GenerateResponse(false, "Can't create a sale without products", HttpStatusCode.Conflict);
            }
            if (response.Success == true)
            {
                foreach (SaleProduct saleProduct in data.SaleProducts)
                {
                    if (saleProduct.ProductId == 0 || saleProduct.SaleAmount == 0)
                    {
                        response.GenerateResponse(false, "Can't create a sale with products having ID or Amount zero", HttpStatusCode.Conflict);
                        break;
                    }

                    DataSet ds = new DataSet();
                    ProductHandler handler = new ProductHandler();

                    if (handler.HandleGetProducts(ds, saleProduct.ProductId))
                    {
                        if (ds.Tables.Contains("DAT") && ds.Tables["DAT"].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables["DAT"].Rows[0];
                            if (dr["stock"] != DBNull.Value && Convert.ToInt32(dr["stock"]) < saleProduct.SaleAmount)
                            {
                                response.GenerateResponse(false, "Product: " + Convert.ToInt32(dr["id"]) + " hasn't enough stock", HttpStatusCode.Conflict);
                                break;
                            }
                        }
                        else
                        {
                            response.GenerateResponse(false, "Product: " + saleProduct.ProductId + " not found", HttpStatusCode.Conflict);
                        }

                    }
                }
            }

            return response;
        }

        public SaleResponse ValidateUpdate(Sale data)
        {
            response.Success = true;

            if (response.Success == true && data.Id == 0)
            {
                response.GenerateResponse(false, "Sale Id required to update", HttpStatusCode.Conflict);
            }

            if (response.Success == true && data.UserId == 0)
            {
                response.GenerateResponse(false, "Can't create a sale without an user id", HttpStatusCode.Conflict);
            }
            return response;
        }

        public SaleResponse ValidateDelete(Int32 id)
        {
            response.Success = true;
            DataSet ds = new DataSet();
            ProductSaleHandler handler = new ProductSaleHandler();

            if (response.Success == true && id == 0)
            {
                response.GenerateResponse(false, "Sale Id required to delete", HttpStatusCode.Conflict);
            }

            response.Success = handler.HandleGetProductSales(ds, id, IdColumns.sale);

            if (response.Success && ds.Tables["DAT"].Rows.Count > 0)
            {
                response.GenerateResponse(false, "Can't delete Sale, first delete ProductSales ", HttpStatusCode.Conflict);
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
