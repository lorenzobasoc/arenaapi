using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ArenaApi.Models;
using Microsoft.Extensions.Logging;

namespace ArenaApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ArenaController : Controller
    {
        private readonly Arena _arena;
        private readonly ILogger<ArenaController> _logger;

        public ArenaController(Arena arena, ILogger<ArenaController> logger) {
            _logger = logger;
            _arena = arena;
        }

        [HttpGet]
        public void Start() {
            if (_arena.endGame){
                throw new InvalidOperationException("L'arena è già stata avviata.");
            } else {
                _ = _arena.Start();
            }
        }

        [HttpPost]
        public void Stop() {
            if (_arena.endGame){
                _arena.endGame = false;
            } else {
                throw new InvalidOperationException("L'arena è già stata stoppata.");
            }
        }
        // _logger.LogError()   _logger.LogInformation

        [HttpGet]
        public string Status() {
            return _arena.Status();
        }
        [HttpGet]
        public List<string> GetActions() {
            return Arena.Actions
                    .Select(a => a.ToString())
                    .ToList();
        }

        [HttpGet("{n:int}")]
        public List<string> GetActions(int n) {   
            if (n <= 0 || n > Arena.Actions.Count){
                return GetActions();
            } else {
                return Arena.Actions
                        .Take(n)
                        .Select(a => a.ToString())
                        .ToList();
            }
        }

        [HttpGet]
        public List<Fighter> GetAliveFighters() {
            return _arena.Fighters
                        .Where(f => f.Pv > 0)
                        .ToList();
        }
    }
}
