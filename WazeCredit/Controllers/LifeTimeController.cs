﻿
using Microsoft.AspNetCore.Mvc;
using WazeCredit.Services.LifeTimeExample;

namespace WazeCredit.Controllers
{
    public class LifeTimeController : Controller
    {
        private readonly TransientService _transientService;

        private readonly ScopedService _scopedService;

        private readonly SingletonService _singletonService;
        public LifeTimeController(TransientService transientService , ScopedService scopedService ,SingletonService singletonService )
        {
            _transientService = transientService;

            _scopedService = scopedService;

            _singletonService = singletonService;
        }
        public IActionResult Index()
        {
            var messages = new List<string>
            {
                HttpContext.Items["CustomMiddlewareTransient"].ToString(),
                $"Transient Controller - {_transientService.GetGuid()}",
                HttpContext.Items["CustomMiddlewareScoped"].ToString(),
                $"Scoped Controller - {_scopedService.GetGuid()}",
                HttpContext.Items["CustomMiddlewareSingleton"].ToString(),
                $"Singleton Controller - {_singletonService.GetGuid()}",

            };
         
            return View(messages);
        }
    }
}