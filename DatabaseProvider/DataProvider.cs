using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DatabaseProvider
{
    /// <summary>
    /// Hỗ trợ kết nối với cơ sở dữ liệu, được xây dựng dựa trên ADO.NET
    /// </summary>
    public class DataProvider
    {
        private string ConnectionString = "Server=.;Database=ToDoList;Trusted_Connection=True;TrustServerCertificate=True;";


        private static DataProvider instance;
        /// <summary>
        /// Design pattern Singleton
        /// </summary>
        public static DataProvider Instance
        {
            get => instance == null ? instance = new DataProvider() : instance;
            private set => instance = value;
        }



        /// <summary>
        /// Truy vấn, xử lý dữ liệu
        /// </summary>
        /// <param name="query"></param>
        /// <param name="Parameter"></param>
        /// <returns>Trả về kết quả là 1 bảng hoặc danh sách</returns>
        public DataTable ExecuteQuery(string query, object[] Parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (Parameter != null)
                {
                    string[] tok = query.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    int cnt = 0;
                    foreach (string item in tok)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, Parameter[cnt++]);
                        }
                    }
                }
                SqlDataAdapter sqlData = new SqlDataAdapter(cmd);
                sqlData.Fill(data);
            }
            return data;
        }



        /// <summary>
        /// Cập nhật dữ liệu, đưa dữ liệu vào cơ sở dữ liệu
        /// </summary>
        /// <param name="query"></param>
        /// <param name="Parameter"></param>
        /// <returns>Số dòng thay đổi</returns>
        public int ExecuteNonQuery(string query, object[] Parameter = null)
        {
            int Count = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (Parameter != null)
                {
                    int cnt = 0;
                    string[] tok = query.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in tok)
                    {
                        if (item.Contains('@')) cmd.Parameters.AddWithValue(item, Parameter[cnt++]);
                    }
                }
                try
                {
                    Count = cmd.ExecuteNonQuery();
                }
                catch { }
            }
            return Count;
        }



        /// <summary>
        ///  Thống kê dữ liệu
        /// </summary>
        /// <param name="query"></param>
        /// <param name="Parameter"></param>
        /// <returns>1 cột hoặc 1 dòng duy nhất</returns>
        public object ExecuteScalar(string query, object[] Parameter = null)
        {
            object Count = null;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (Parameter != null)
                {
                    int cnt = 0;
                    string[] tok = query.Split(new char[] {' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in tok)
                    {
                        if (item.Contains('@')) cmd.Parameters.AddWithValue(item, Parameter[cnt++]);
                    }
                }
                Count = cmd.ExecuteScalar();
            }
            return Count;
        }


    }
}
