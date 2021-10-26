using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ArenaApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArenaApi.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class ArenaController : Controller
    {
        private readonly Arena _arena;

        public ArenaController(Arena arena) {
            _arena = arena;
        }

        [HttpGet]
        public void Start() {
            if (_arena.running){
                throw new InvalidOperationException("L'arena è già stata avviata.");
            } else {
                _ = _arena.Start();
            }
        }

        [HttpPost]
        public void Stop() {
            if (_arena.running){
                _arena.running = false;
            } else {
                throw new InvalidOperationException("L'arena è già stata stoppata.");
            }
        }

        [HttpGet]
        public string Status() {
            if (_arena.Status()){
                 return "L'arena è in esecuzione";
            } else {
                return "L'arena non è in esecuzione";
            }
        }

        [HttpGet]
        public List<string> GetActions() {
            if (_arena.Fighters is null){
                return new List<string>();
            } else {
                return _arena.Actions
                        .Select(a => a.ToString())
                        .ToList();
            }
        }

        [HttpGet("{n:int}")]
        public List<string> GetActions(int n) {   
            if (_arena.Fighters is null){
                return new List<string>();
            } else if (n <= 0 || n > _arena.Actions.Count){
                return GetActions();
            } else {
                return _arena.Actions
                        .Take(n)
                        .Select(a => a.ToString())
                        .ToList();
            }
        }

        [HttpGet]
        public List<Fighter> GetAliveFighters() {
            if (_arena.Fighters is null){
                return new List<Fighter>();
            } else {
                return _arena.Fighters
                            .Where(f => !f.IsDead )
                            .ToList();
            }
        }
    }
}
