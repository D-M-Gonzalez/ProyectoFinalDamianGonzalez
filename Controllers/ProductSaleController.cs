using DamianGonzalezCSharp.Models;
using System.Data;
using DamianGonzalezCSharp.Handlers;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalezCSharp.Validations;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductSaleController : ControllerBase
    {
        private static readonly ProductSaleValidations validations = new();
        private LoginHandler loginHandler = new LoginHandler();

        [HttpGet("GetProductSales")]
        public ProductSaleResponse GetProductSales(string? descrip, string? order)
        {
            List<SoldProduct> products = new List<SoldProduct>();
            DataSet dsSales = new DataSet();
            ProductSaleHandler handler = new ProductSaleHandler();
            ProductSaleResponse response = new ProductSaleResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (handler.HandleGetProductSales(dsSales, descrip, order))
            {
                if (dsSales.Tables.Contains("DAT") && dsSales.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsSales.Tables["DAT"];

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        SoldProduct sale = new SoldProduct();

                        sale.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["Cantidad"] != DBNull.Value) sale.Amount = Convert.ToInt32(dr["Cantidad"]);
                        if (dr["IdVenta"] != DBNull.Value) sale.SaleId = Convert.ToInt32(dr["IdVenta"]);
                        if (dr["IdProducto"] != DBNull.Value) sale.ProductId = Convert.ToInt32(dr["IdProducto"]);
                        if (dr["Descripciones"] != DBNull.Value) sale.Product = Convert.ToString(dr["Descripciones"]).Trim();
                        if (dr["Costo"] != DBNull.Value) sale.Cost = Convert.ToDecimal(dr["Costo"]);
                        if (dr["PrecioVenta"] != DBNull.Value) sale.Price = Convert.ToDecimal(dr["PrecioVenta"]);
                        if (dr["Stock"] != DBNull.Value) sale.Stock = Convert.ToInt32(dr["Stock"]);

                        products.Add(sale);

                    }

                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Products = products;
                    
                }
                else
                {
                    response.GenerateResponse(false, "Can't obtain requested data", HttpStatusCode.Conflict);
                }
            }

            dsSales.Dispose();
            return response;
        }

        [HttpGet("GetProductSale")]
        public ProductSaleResponse GetProductSale(Int32 productSaleId)
        {
            SoldProduct sale = new SoldProduct();
            List<SoldProduct> productList = new List<SoldProduct>();
            DataSet dsProduct = new DataSet();
            ProductSaleHandler handler = new ProductSaleHandler();
            ProductSaleResponse response = new ProductSaleResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (handler.HandleGetProductSales(dsProduct, productSaleId))
            {
                if (dsProduct.Tables.Contains("DAT") && dsProduct.Tables["DAT"].Rows.Count > 0)
                {
                    DataRow dr = dsProduct.Tables["DAT"].Rows[0];

                    sale.Id = Convert.ToInt32(dr["Id"]);
                    if (dr["Cantidad"] != DBNull.Value) sale.Amount = Convert.ToInt32(dr["Cantidad"]);
                    if (dr["IdVenta"] != DBNull.Value) sale.SaleId = Convert.ToInt32(dr["IdVenta"]);
                    if (dr["IdProducto"] != DBNull.Value) sale.ProductId = Convert.ToInt32(dr["IdProducto"]);
                    if (dr["Descripciones"] != DBNull.Value) sale.Product = Convert.ToString(dr["Descripciones"]).Trim();
                    if (dr["Costo"] != DBNull.Value) sale.Cost = Convert.ToDecimal(dr["Costo"]);
                    if (dr["PrecioVenta"] != DBNull.Value) sale.Price = Convert.ToDecimal(dr["PrecioVenta"]);
                    if (dr["Stock"] != DBNull.Value) sale.Stock = Convert.ToInt32(dr["Stock"]);
                    productList.Add(sale);

                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Products = productList;
                }
                else
                {
                    response.GenerateResponse(false, "Product Sale not found", HttpStatusCode.Conflict);
                }

            }

            dsProduct.Dispose();
            return response;
        }

        [HttpDelete("DeleteProductSale")]
        public ProductSaleResponse DeleteProductSale(Int32 id)
        {
            ProductSaleResponse response = validations.ValidateDelete(id);
            ProductSaleHandler productHandler = new ProductSaleHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }
            if (response.Success == true && productHandler.HandleDeleteProductSale(id))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}
