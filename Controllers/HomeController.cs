using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StreamCapture;

namespace StreamCaptureWeb
{
    public class HomeController : Controller
    {
        //Holds context
        public Recordings recordings;

        public HomeController(Recordings _recordings)
        {
            recordings = _recordings;
        }

        [HttpGet("/api/schedule")]
        public IActionResult GetSchedule()
        {
            Console.WriteLine("API called!");
            List<RecordInfo> recordingsList = recordings.GetRecordInfoList();
            return Json(recordingsList);
            //return Json(new { Result = "OK", Records = recordingsList });
        }

        [HttpPost("/api/edit")]
        public IActionResult EditSchedule()
        {
            Console.WriteLine("EDIT API called!");
            foreach (string key in this.Request.Form.Keys)
            {
                Console.WriteLine($"{key} : {this.Request.Form[key]}");
            }

            //If Delete  (really means set ignore flag)
            if(this.Request.Form["oper"]=="del")
            {
               Console.WriteLine("Deleting!"); 
               foreach(RecordInfo recordInfo in recordings.GetRecordInfoList())
               {
                   if(recordInfo.id == this.Request.Form["id"])
                   {
                        Console.WriteLine("Found {recordInfo.description}");
                        //
                        // put this into a member function
                        //
                        // If not queued, then just set ignore flag
                        // If queued, then take it out
                        // If process started, then kill it and update ignore flag (clean up files)
                   }
               }
            }

            //If Edit 
            if(this.Request.Form["oper"]=="edit")
            {
               Console.WriteLine("Editing!"); 
               foreach(RecordInfo recordInfo in recordings.GetRecordInfoList())
               {
                   if(recordInfo.id == this.Request.Form["id"])
                   {
                       Console.WriteLine("Found {recordInfo.description}");
                        //
                        // If ignore flag is set (and it's different than the object), call delete member function
                        // If time has changed, but it's queued - make sure we're doing the right thing
                        // If duration changed, and the process started, make sure the right things happen 
                   }
               }
            }            


            return Json(new { Result = "OK"});
        }

        [HttpGet("home")]
        public IActionResult MainGrid()
        {
            //ViewBag.Message = "Hello world!";
            //ViewBag.Time = DateTime.Now;

            RecordInfo recordInfo = recordings.GetRecordInfoList()[0];
            ViewData["Message"] = recordInfo.description;            

            return View();
        }
    }
}