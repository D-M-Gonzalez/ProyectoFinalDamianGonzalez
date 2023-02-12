using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DamianGonzalezCSharp.Models;
using DamianGonzalezCSharp;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalesCSharp.Models;
using DamianGonzalesCSharp.Handlers;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class SaleController : ControllerBase
    {
        [HttpGet("GetSales")]
        public SaleResponse GetSales(string? descrip, string? order)
        {
            List<Sale> sales = new List<Sale>();
            DataSet dsSales = new DataSet();
            SaleHandler saleHandler = new SaleHandler();
            SaleResponse response = new SaleResponse();

            if (saleHandler.HandleGetSales(dsSales, 0, descrip, order))
            {
                if (dsSales.Tables.Contains("DAT") && dsSales.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsSales.Tables["DAT"];
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        Sale sale = new Sale();
                        DataSet dsProductSale = new DataSet();
                        ProductSaleHandler productSaleHandler = new ProductSaleHandler();

                        sale.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["IdUsuario"] != DBNull.Value) sale.UserId = Convert.ToInt32(dr["IdUsuario"]);
                        if (dr["Comentarios"] != DBNull.Value) sale.SaleComments = Convert.ToString(dr["Comentarios"]).Trim();

                        if (productSaleHandler.HandleGetProductSales(dsProductSale, 0, sale.Id, descrip, order))
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

                    response.Sales = sales;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpGet("GetSale")]
        public Response GetSale(Int32 saleId)
        {
            Sale sale = new Sale();
            List < Sale > saleList = new List<Sale>();
            DataSet dsSale = new DataSet();
            SaleHandler SaleHandler = new SaleHandler();
            SaleResponse response = new SaleResponse();

            if (SaleHandler.HandleGetSales(dsSale, saleId, "", ""))
            {
                if (dsSale.Tables.Contains("DAT") && dsSale.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsSale.Tables["DAT"];
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;

                    DataRow dr = dtDAT.Rows[0];

                        DataSet dsProductSale = new DataSet();
                        ProductSaleHandler productSaleHandler = new ProductSaleHandler();

                        sale.Id = Convert.ToInt32(dr["Id"]);
                        if (dr["IdUsuario"] != DBNull.Value) sale.UserId = Convert.ToInt32(dr["IdUsuario"]);
                        if (dr["Comentarios"] != DBNull.Value) sale.SaleComments = Convert.ToString(dr["Comentarios"]).Trim();

                        if (productSaleHandler.HandleGetProductSales(dsProductSale, 0, sale.Id, "", ""))
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

                    response.Sales = saleList;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }

            } else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpPost("CreateSale")]
        public SaleResponse CreateSale(Sale data)
        {
            SaleResponse response = new SaleResponse();
            SaleHandler SaleHandler = new SaleHandler();

            if(SaleHandler.HandleSaleData(data, true))
            {
                response.Success = true;
                response.Message = "Sale created successfully";
                response.StatusCode = HttpStatusCode.OK;
            } else
            {
                response.Message = "An error has ocurred when inserting the Sale";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            
            return response;
        }


        /*[HttpPut("EditSale")]
        public SaleResponse ModifySale(Sale data)
        {
            SaleResponse response = new SaleResponse();
            SaleHandler SaleHandler = new SaleHandler();

            if (data.Id != 0 && SaleHandler.HandleSaleData(data, false))
            {
                response.Success = true;
                response.Message = "Sale modified successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when editing the Sale";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpDelete("DeleteSale")]

        public SaleResponse DeleteSale(Int32 id)
        {
            SaleResponse response = new SaleResponse();
            SaleHandler SaleHandler = new SaleHandler();

            if (id != 0 && SaleHandler.HandleDeleteSale(id))
            {
                response.Success = true;
                response.Message = "Sale deleted successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when deleting the Sale";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }*/
    }
}
