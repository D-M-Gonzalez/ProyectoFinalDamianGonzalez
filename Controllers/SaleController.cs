using System.Data;
using DamianGonzalezCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalezCSharp.Handlers;
using System.Net;
using DamianGonzalezCSharp.Validations;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {
        private static readonly SaleValidations validations = new();
        private LoginHandler loginHandler = new LoginHandler();

        [HttpGet("GetSales")]
        public SaleResponse GetSales(string? descrip, string? order)
        {
            List<Sale> sales = new List<Sale>();
            DataSet dsSales = new DataSet();
            SaleHandler saleHandler = new SaleHandler();
            SaleResponse response = new SaleResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (saleHandler.HandleGetSales(dsSales, descrip, order))
            {
                if (dsSales.Tables.Contains("DAT") && dsSales.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsSales.Tables["DAT"];

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        Sale sale = new Sale();
                        DataSet dsProductSale = new DataSet();
                        ProductSaleHandler productSaleHandler = new ProductSaleHandler();

                        sale.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["IdUsuario"] != DBNull.Value) sale.UserId = Convert.ToInt32(dr["IdUsuario"]);
                        if (dr["Comentarios"] != DBNull.Value) sale.SaleComments = Convert.ToString(dr["Comentarios"]).Trim();

                        if (productSaleHandler.HandleGetProductSales(dsProductSale, sale.Id, IdColumns.sale))
                        {
                            if (dsProductSale.Tables.Contains("DAT") && dsProductSale.Tables["DAT"].Rows.Count > 0)
                            {
                                List<SaleProduct> saleProductList = new List<SaleProduct>();
                                DataTable dtPS = dsProductSale.Tables["DAT"];
                                foreach (DataRow dr2 in dtPS.Rows)
                                {
                                    SaleProduct saleProduct = new SaleProduct();

                                    saleProduct.SaleProductId = Convert.ToInt32(dr2["Id"]);
                                    if (dr2["Cantidad"] != DBNull.Value) saleProduct.SaleAmount = Convert.ToInt32(dr2["Cantidad"]);
                                    if (dr2["IdProducto"] != DBNull.Value) saleProduct.ProductId = Convert.ToInt32(dr2["IdProducto"]);
                                    if (dr2["Descripciones"] != DBNull.Value) saleProduct.Product = Convert.ToString(dr2["Descripciones"]).Trim();
                                    if (dr2["Costo"] != DBNull.Value) saleProduct.Cost = Convert.ToInt32(dr2["Costo"]);
                                    if (dr2["PrecioVenta"] != DBNull.Value) saleProduct.Price = Convert.ToInt32(dr2["PrecioVenta"]);
                                    if (dr2["Stock"] != DBNull.Value) saleProduct.Stock = Convert.ToInt32(dr2["Stock"]);

                                    saleProductList.Add(saleProduct);
                                }
                                sale.SaleProducts = saleProductList;
                            }
                        }

                        sales.Add(sale);

                    }
                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Sales = sales;
                    
                }
                else
                {
                    response.GenerateResponse(false, "Can't obtain requested data", HttpStatusCode.Conflict);
                }
            }

            dsSales.Dispose();
            return response;
        }

        [HttpGet("GetSale")]
        public SaleResponse GetSale(Int32 saleId)
        {
            Sale sale = new Sale();
            List < Sale > saleList = new List<Sale>();
            DataSet dsSale = new DataSet();
            SaleHandler SaleHandler = new SaleHandler();
            SaleResponse response = new SaleResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (SaleHandler.HandleGetSales(dsSale, saleId))
            {
                if (dsSale.Tables.Contains("DAT") && dsSale.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsSale.Tables["DAT"];

                    DataRow dr = dtDAT.Rows[0];

                        DataSet dsProductSale = new DataSet();
                        ProductSaleHandler productSaleHandler = new ProductSaleHandler();

                        sale.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["IdUsuario"] != DBNull.Value) sale.UserId = Convert.ToInt32(dr["IdUsuario"]);
                        if (dr["Comentarios"] != DBNull.Value) sale.SaleComments = Convert.ToString(dr["Comentarios"]).Trim();

                        if (productSaleHandler.HandleGetProductSales(dsProductSale,sale.Id, IdColumns.sale))
                        {
                            if (dsProductSale.Tables.Contains("DAT") && dsProductSale.Tables["DAT"].Rows.Count > 0)
                            {
                                List<SaleProduct> saleProductList = new List<SaleProduct>();
                                DataTable dtPS = dsProductSale.Tables["DAT"];
                                foreach (DataRow dr2 in dtPS.Rows)
                                {
                                    SaleProduct saleProduct = new SaleProduct();

                                    saleProduct.SaleProductId = Convert.ToInt32(dr2["Id"]);
                                    if (dr2["Cantidad"] != DBNull.Value) saleProduct.SaleAmount = Convert.ToInt32(dr2["Cantidad"]);
                                    if (dr2["IdProducto"] != DBNull.Value) saleProduct.ProductId = Convert.ToInt32(dr2["IdProducto"]);
                                    if (dr2["Descripciones"] != DBNull.Value) saleProduct.Product = Convert.ToString(dr2["Descripciones"]).Trim();
                                    if (dr2["Costo"] != DBNull.Value) saleProduct.Cost = Convert.ToInt32(dr2["Costo"]);
                                    if (dr2["PrecioVenta"] != DBNull.Value) saleProduct.Price = Convert.ToInt32(dr2["PrecioVenta"]);
                                    if (dr2["Stock"] != DBNull.Value) saleProduct.Stock = Convert.ToInt32(dr2["Stock"]);

                                    saleProductList.Add(saleProduct);
                                }

                                sale.SaleProducts = saleProductList;
                            }
                        }

                    saleList.Add(sale);
                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Sales = saleList;
                    
                }
                else
                {
                    response.GenerateResponse(false, "Sale not found", HttpStatusCode.Conflict);
                }

            }

            dsSale.Dispose();
            return response;
        }

        [HttpPost("CreateSale")]
        public SaleResponse CreateSale(Sale data)
        {
            SaleResponse response = validations.ValidateCreation(data);
            SaleHandler SaleHandler = new SaleHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && SaleHandler.HandleCreateSale(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }
            
            return response;
        }


        [HttpPut("EditSale")]
        public SaleResponse ModifySale(Sale data)
        {
            SaleResponse response = validations.ValidateUpdate(data);
            SaleHandler SaleHandler = new SaleHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && data.Id != 0 && SaleHandler.HandleUpdateSale(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpDelete("DeleteSale")]

        public SaleResponse DeleteSale(Int32 id)
        {
            SaleResponse response = validations.ValidateDelete(id);
            SaleHandler SaleHandler = new SaleHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && SaleHandler.HandleDeleteSale(id))
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
