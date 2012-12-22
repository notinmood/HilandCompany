using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XQYC.Web.Models.Test;

namespace XQYC.Web.Areas.APIs.Controllers
{
    public class ContactController : ApiController
    {
        Contact[] contacts = new Contact[] 
        { 
            new Contact(){ ID=1, Age=23, Birthday=Convert.ToDateTime("1977-05-30"), Name="情缘", Sex="男"},
            new Contact(){ ID=2, Age=55, Birthday=Convert.ToDateTime("1937-05-30"), Name="令狐冲", Sex="男"},
            new Contact(){ ID=3, Age=12, Birthday=Convert.ToDateTime("1987-05-30"), Name="郭靖", Sex="男"},
            new Contact(){ ID=4, Age=18, Birthday=Convert.ToDateTime("1997-05-30"), Name="黄蓉", Sex="女"},
        };

        /// <summary>
        /// /api/Contact
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Contact> ListAll()
        {
            return contacts;
        }

        /// <summary>
        /// /api/Contact/id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contact GetContactByID(int id)
        {
            Contact contact = contacts.FirstOrDefault<Contact>(item => item.ID == id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return contact;
        }

        /// <summary>
        /// 根据性别查询
        /// /api/Contact?sex=女
        /// </summary>
        /// <param name="sex"></param>
        /// <returns></returns>
        public IEnumerable<Contact> GetListBySex(string sex)
        {
            return contacts.Where(item => item.Sex == sex);
        }
    }
}
