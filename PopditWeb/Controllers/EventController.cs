﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using PopditWebApi;

namespace PopditWeb.Controllers
{
    public class EventController : Controller
    {
        // GET: Event
        public async Task<ActionResult> Index()
        {
            try
            {
                Stream json = await WebApi(WebApiMethod.Get, "api/Event");
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<EventInterop>));
                return View((List<EventInterop>)serializer.ReadObject(json));
            }
            // Authentication failure?
            catch (Exception e) { return RedirectToAction("SignOut", "Home"); }
        }
    }
}
