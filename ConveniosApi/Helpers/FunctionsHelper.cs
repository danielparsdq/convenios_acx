using ConveniosApi.Controllers;
using ConveniosApi.Models;
using DocumentFormat.OpenXml.Vml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace ConveniosApi.Helpers
{
    public class FunctionsHelper
    {
        public const byte PageSize = 20;
        /// <summary>
        /// Genera los links en una respuesta
        /// </summary>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static Links? GetLinks(int page, int total, HttpRequest Request)
        {
            string? Next = (page * 20) + 20 < total ? $"{Request.Scheme}://{Request.Host}{Request.Path}?page={page + 1}" : null;
            string? Prev = page > 0 ? $"{Request.Scheme}://{Request.Host}{Request.Path}?page={page - 1}" : null;
            if(Next!=null || Prev != null)
            {
                return new Links()
                {
                    Next = Next,
                    Prev = Prev
                };
            }
            return null;
        }

        /// <summary>
        /// Genera el SHA1 de una cadena
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Sha1(string text)
        {
            // Create a SHA256   
            using (SHA1 sha256Hash = SHA1.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Genera la respuesta con excepciones
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="_localizer"></param>
        /// <returns></returns>
        public static object BadRequest(Exception ex, IStringLocalizer _localizer)
        {
#if DEBUG
            return new
            {
                Message = _localizer[ex.Message].Value,
                Exeption = ex
            };
#else
            return new
            {
                Message = _localizer[ex.Message].Value
            };
#endif
        }

        /// <summary>
        /// Genera la paginación de un query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static async Task<Pagination<T>> Paginate<T>(IQueryable<T> query, int page)
        {
            var Total = await query.CountAsync();
            query = query.Skip(page * PageSize).Take(PageSize);
            var Data = await query.ToListAsync();
            return new Pagination<T>()
            {
                Total = Total,
                Data = Data
            };
        }
    }

    public class Pagination<T>
    {
        public int Total { get; set; }
        public List<T> Data { get;  set; }
    }
}
