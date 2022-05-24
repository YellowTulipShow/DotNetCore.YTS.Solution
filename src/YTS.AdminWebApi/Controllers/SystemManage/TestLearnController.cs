using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YTS.Tools;
using YTS.WebApi;
using YTS.Shop;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace YTS.AdminWebApi.Controllers
{
    [AllowAnonymous]
    public class TestLearnController : BaseApiController
    {
        public class TestLearnName
        {
            public string Name { get; set; }
        }
        public class TestLearnModel
        {
            public bool? IsMan { get; set; }
            public int? Age { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public object SubmitIdAndName(int ID, string Name)
        {
            return ID.ToString() + ": =>" + Name;
        }

        [AllowAnonymous]
        [HttpPost]
        public object SubmitModel(TestLearnName model)
        {
            return JsonHelper.ToString(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public object SubmitIdAndModel(int ID, TestLearnName model)
        {
            return ID.ToString() + ": =>" + JsonHelper.ToString(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public object SubmitArray(int[] ids)
        {
            return JsonHelper.ToString(ids);
        }

        [AllowAnonymous]
        [HttpPost]
        public object SubmitStringAndArray(string name, int[] ids)
        {
            return name + ": =>" + JsonHelper.ToString(ids);
        }

        // [AllowAnonymous]
        // [HttpPost]
        // public object SubmitArrayAndModel(int[] ids, TestLearnName model)
        // {
        //     return JsonHelper.ToString(ids) + " | " + JsonHelper.ToString(model);
        // }
    }
}
