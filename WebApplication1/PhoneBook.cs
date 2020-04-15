using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1
{
    public class PhoneBook
    {
        private readonly PhoneBookEntities db;

        public PhoneBook() { }

        public PhoneBook(PhoneBookEntities entities)
        {
            db = entities;
        }

        public PhoneBookEntryResponse CreateEntry(PhoneBookEntry phoneBookEntry)
        {

            try
            {

                if (string.IsNullOrEmpty(phoneBookEntry.Name) || string.IsNullOrEmpty(phoneBookEntry.Surname) || string.IsNullOrEmpty(phoneBookEntry.ContactNumber))
                {
                    return new PhoneBookEntryResponse { status = "Failed", message = "Please make sure all the details are captured" };
                }

                if (!IsNumber(phoneBookEntry.ContactNumber))
                {
                    return new PhoneBookEntryResponse { status = "Failed", message = "Contact Number should only contain numeric values" };
                }

                db.USP_Insert(phoneBookEntry.Name, phoneBookEntry.Surname, phoneBookEntry.ContactNumber);

                return new PhoneBookEntryResponse { status = "Success", message = "Phonebook entry created for " + phoneBookEntry.Name + " " + phoneBookEntry.Surname };
            }
            catch (Exception ex)
            {

                return new PhoneBookEntryResponse { status = "Failed", message = ex.Message };
            }

        }


        public PhoneBookEntryResponse UpdateEntry(PhoneBookEntry phoneBookEntry)
        {

            try
            {
                if (string.IsNullOrEmpty(phoneBookEntry.ContactNumber))
                {
                    return new PhoneBookEntryResponse { status = "Failed", message = "Contact Number needed to update the Phone book entry" };
                }

                if (!IsNumber(phoneBookEntry.ContactNumber))
                {
                    return new PhoneBookEntryResponse { status = "Failed", message = "Contact Number should only contain numeric values" };
                }

                var existingContact = db.Database.SqlQuery<PhoneBookEntry>("Select Name, Surname, ContactNumber from dbo.Phonebook where Name = @Name and Surname = @Surname", new SqlParameter("@Name", phoneBookEntry.Name), new SqlParameter("@Surname", phoneBookEntry.Surname)).ToList();

                if (existingContact.Count > 0)
                {
                    db.USP_Update(phoneBookEntry.Name, phoneBookEntry.Surname, phoneBookEntry.ContactNumber);
                }
                else
                {
                    return new PhoneBookEntryResponse { status = "Failed", message = "Could not find an entry to update for Name: " + phoneBookEntry.Name + " and Surname: " + phoneBookEntry.Surname };
                }


                return new PhoneBookEntryResponse { status = "Success", message = "Phonebook entry updated for " + phoneBookEntry.Name + " " + phoneBookEntry.Surname };

            }
            catch (Exception ex)
            {

                return new PhoneBookEntryResponse { status = "Failed", message = ex.Message };
            }
        }

        public List<PhoneBookEntry> LookupEntry(PhoneBookEntry phoneBookEntry)
        {

            try
            {

                var PhoneBookEntries = new List<PhoneBookEntry>();

                if (string.IsNullOrEmpty(phoneBookEntry.Name) && string.IsNullOrEmpty(phoneBookEntry.Surname) && string.IsNullOrEmpty(phoneBookEntry.ContactNumber))
                {
                    var phoneBookEntryError = new PhoneBookEntry() { Name = "No Name Captured", Surname = "No Surname Captured", ContactNumber = "No Contact Number Captured" };
                    var errorList = new List<PhoneBookEntry>();
                    errorList.Add(phoneBookEntryError);

                    return errorList;
                }

                //var phoneBookEntryList = db.USP_Search(phoneBookEntry.Name, phoneBookEntry.Surname, phoneBookEntry.ContactNumber);

                var ExistingEntry = new DAL.PhoneBook();

                using (var ctx = new PhoneBookEntities())
                {
                    ExistingEntry = ctx.PhoneBooks.Where(s => s.Name.Contains(phoneBookEntry.Name))
                                                  .Where(s => s.Surname.Contains(phoneBookEntry.Surname))
                                                  .Where(s => s.ContactNumber.Contains(phoneBookEntry.ContactNumber))
                        .FirstOrDefault();
                }

                if (ExistingEntry != null)
                {
                    var entry = new PhoneBookEntry();
                    entry.Name = ExistingEntry.Name;
                    entry.Surname = ExistingEntry.Surname;
                    entry.ContactNumber = ExistingEntry.ContactNumber;

                    PhoneBookEntries.Add(entry);


                }

                return PhoneBookEntries;
            }
            catch (Exception ex)
            {

                return new List<PhoneBookEntry>();
            }

          

        }

        public List<PhoneBookEntry> GetAllEntries()
        {

            try
            {


                var AllEntries = db.Database.SqlQuery<PhoneBookEntry>("Select Name, Surname, ContactNumber from dbo.Phonebook ").ToList();

                return AllEntries;

            }
            catch (Exception ex)
            {

                return new List<PhoneBookEntry>();
            }

           

        }
        public Boolean IsNumber(String value)
        {
            return value.All(Char.IsDigit);
        }

    }



}