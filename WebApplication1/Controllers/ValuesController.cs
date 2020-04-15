using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/phonebook")]
    public class ValuesController : ApiController
    {
        // Cases cases = new Cases(new LiquidTrackCrmIntegrationEntities(), new LiquidTrackEntities2());

        PhoneBook phoneBook = new PhoneBook(new DAL.PhoneBookEntities());

        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}


        [ResponseType(typeof(PhoneBookEntryResponse))]
        [Route("CreatePhoneBookEntry")]
        [HttpPost]
        public PhoneBookEntryResponse CreatePhoneBookEntry(string Name, string Surname, string ContactNumber)
        {
            var phoneBookEntry = new PhoneBookEntry() { Name = Name, Surname = Surname, ContactNumber = ContactNumber };

            return phoneBook.CreateEntry(phoneBookEntry);
        }


        [ResponseType(typeof(PhoneBookEntryResponse))]
        [Route("UpdatePhoneBookEntry")]
        [HttpPost]
        public PhoneBookEntryResponse UpdatePhoneBookEntry(string Name, string Surname, string ContactNumber)
        {
            var phoneBookEntry = new PhoneBookEntry() { Name = Name, Surname = Surname, ContactNumber = ContactNumber };

            return phoneBook.UpdateEntry(phoneBookEntry);
        }

        [ResponseType(typeof(PhoneBookEntry))]
        [Route("LookupPhoneBookEntry")]
        [HttpPost]
        public List<PhoneBookEntry> LookupPhoneBookEntry(bool retrieveAll, string Name = "", string Surname = "", string ContactNumber = "")
        {

            if (retrieveAll)
            {
                return phoneBook.GetAllEntries();
            }

            var phoneBookEntry = new PhoneBookEntry() { Name = Name, Surname = Surname, ContactNumber = ContactNumber };

            return phoneBook.LookupEntry(phoneBookEntry);

        }




    }
}
