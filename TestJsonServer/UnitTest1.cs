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
            Assert.AreEqual(2, res.Count);
            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //printing the data in console
            foreach (var i in res)
            {
                Console.WriteLine("FirstName:{0}\t LastName:{1}\t PhoneNumber:{2}\t, Address:{3}\t City:{4}\t,State:{5}\t ZipCode:{6}\t EmailId:{7}\t", i.FirstName, i.LastName, i.PhoneNumber, i.Addresses, i.City, i.State, i.ZipCode, i.EmailId);
            }
        }
    }
}
