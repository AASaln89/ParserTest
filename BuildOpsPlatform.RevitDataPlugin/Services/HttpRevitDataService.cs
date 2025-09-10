using BuildOpsPlatform.RevitDataCommon.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BuildOpsPlatform.RevitDataPlugin.Services
{
    public class HttpRevitDataService
    {
        public bool Get()
        {
            var client = new HttpClient();
            var result = client.GetAsync("https://localhost:7175/api/revit/categ").Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public List<CategoryDto> GetProjectCategories()
        {
            throw new NotImplementedException();
        }
    }
}
