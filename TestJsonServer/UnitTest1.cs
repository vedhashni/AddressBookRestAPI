using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace TestJsonServer
{
    [TestClass]
    public class UnitTest1
    {
        //Initializing the restclient as null
        RestClient client = null;
        [TestInitialize]
        //This method is calling evrytime to initialzie the restclient object
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        public IRestResponse ReadAddressBookData()
        {
            //Get request 
            RestRequest request = new RestRequest("/AddressBook", Method.GET);
            //Passing the request and execute 
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        [TestMethod]
        public void OnCallingGetAPI_ReturnPersonDetails()
        {
            IRestResponse response = ReadAddressBookData();
            //Convert the json object to list(deserialize)
            var res = JsonConvert.DeserializeObject<List<Person>>(response.Content);
            Assert.AreEqual(4, res.Count);
            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //printing the data in console
            foreach (var i in res)
            {
                Console.WriteLine("FirstName:{0}\t LastName:{1}\t PhoneNumber:{2}\t, Address:{3}\t City:{4}\t,State:{5}\t ZipCode:{6}\t EmailId:{7}\t", i.FirstName, i.LastName, i.PhoneNumber, i.Addresses, i.City, i.State, i.ZipCode, i.EmailId);
            }
        }

        //add data to json server
        public void AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/AddressBook", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

        }
        /// <summary>
        /// UC2--->Adding a contact details in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployee()
        {
            List<JsonObject> list = new List<JsonObject>();
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("FirstName", "Swetha");
            json.Add("LastName", "Raju");
            json.Add("PhoneNumber", "8939785641");
            json.Add("Addresses", "cross Road");
            json.Add("City", "Mysore");
            json.Add("State", "Karnataka");
            json.Add("ZipCode", "64345");
            json.Add("EmailId", "swetha@gmail.com");
            list.Add(json);
            JsonObject json1 = new JsonObject();
            json1.Add("FirstName", "Ashok");
            json1.Add("LastName", "Kumar");
            json1.Add("PhoneNumber", "9567323890");
            json1.Add("Addresses", "Park street");
            json1.Add("City", "Hyderabad");
            json1.Add("State", "Telangana");
            json1.Add("ZipCode", "65567");
            json1.Add("EmailId", "ashok@gmail.com");
            list.Add(json1);
            //convert the jsonobject to employee object
            foreach (var i in list)
            {
                AddingInJsonServer(i);
            }

            IRestResponse response = ReadAddressBookData();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// UC3-->Upadte the existing contact using(PUT)
        /// </summary>
        [TestMethod]
        public void OnCallingPutAPI_UpdateEmployeeDetails()
        {
            //Passing the method type as put(update existing employee details)
            RestRequest request = new RestRequest("/AddressBook/2", Method.PUT);
            //Creating a object
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("FirstName", "Vedhashni");
            json.Add("LastName", "V");
            json.Add("PhoneNumber", "70105543509");
            json.Add("Addresses", "sixth street");
            json.Add("City", "cochin");
            json.Add("State", "kerala");
            json.Add("ZipCode", "60345");
            json.Add("EmailId", "ved@gmail.com");
            //passing the type as json 
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //convert the jsonobject to employee object
            var res = JsonConvert.DeserializeObject<Person>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// UC4--->Delete the person details using the id
        /// </summary>

        [TestMethod]
        public void OnCallingDeleteAPI_DeleteEmployeeDetails()
        {

            RestRequest request = new RestRequest("/AddressBook/6", Method.DELETE);
            IRestResponse response = client.Execute(request);
            //Calling the read data address book 
            IRestResponse response1 = ReadAddressBookData();
            List<Person> result = JsonConvert.DeserializeObject<List<Person>>(response1.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }

}
