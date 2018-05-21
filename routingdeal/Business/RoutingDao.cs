using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using routingdeal.Models;

namespace routingdeal.Business
{
    public class RoutingDao
    {
        public string ConnectionString { get; set; }

        public RoutingDao(string Connection)
        {
            ConnectionString = Connection;
        }

        private MySqlConnection Get()
        {
            return new MySqlConnection(ConnectionString);
        }


        public async Task<ResponseDeal> AddRoutingDeal(RequestDeal data)
        {
            var model = new Deal();
            var result = new ResponseDeal();

            try
            {
                using (var conn = Get())
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"insert into tbl_deal (name, invoicekey, state, template, url) values('{data.Name}', '{data.InvoiceKey}',1, '{data.Template}', '{data.Url}')", conn);
                    var reader = cmd.ExecuteNonQuery();

                    result.Code = 200;
                    result.Data = new Deal()
                    {
                        Id = cmd.LastInsertedId,
                        Name = data.Name,
                        InvoiceKey = data.InvoiceKey,
                        State = true,
                        Template = data.Template,
                        Url = data.Url,
                    };
                    result.Message = "OK";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                result.Data = null;
            }

            return await Task.Run(() => result);
        }
        public async Task<ResponseDeal> GetRoutingDeal(string invoicekey)
        {
            var model = new Deal();
            var result = new ResponseDeal();

            try
            {
                using (var conn = Get())
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"select id, name, invoicekey, state, template, url, type, requesttemplate, numrequest from tbl_deal where invoicekey = '{invoicekey}'", conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Id = reader.GetInt64("id");
                            model.Name = reader.GetString("name");
                            model.InvoiceKey = reader.GetString("invoicekey");
                            model.State = reader.GetBoolean("state");
                            model.Template = reader.GetString("template");
                            model.Url = reader.GetString("url");
                            model.Type = reader.GetString("type");
                            model.RequestTemplate = reader.GetString("requesttemplate");
                            model.NumRequest = reader.GetInt32("numrequest");
                        }
                    }
                    result.Code = 200;
                    result.Data = model;
                    result.Message = "OK";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                result.Data = null;
            }

            return await Task.Run(() => result);
        }

        public async Task<ResponseUpdate> UpdateRoutingDeal(RequestUpdate data)
        {
            var model = new Deal();
            var result = new ResponseUpdate();

            try
            {
                using (var conn = Get())
                {
                    conn.Open();
                    var cmd = new MySqlCommand($"update tbl_deal set state={data.State}, url='{data.Url}', template='{data.Template}' where invoicekey = '{data.InvoiceKey}'", conn);
                    var reader = cmd.ExecuteNonQuery();

                    result.Code = 200;
                    result.Data = new UpdateDeal()
                    {
                        InvoiceKey = data.InvoiceKey,
                        State = true,
                    };
                    result.Message = "OK";
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
                result.Data = null;
            }

            return await Task.Run(() => result);
        }
    }
}