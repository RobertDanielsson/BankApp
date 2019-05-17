//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BankApp.WebUI.Attributes
//{
//    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
//    {
//        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
//    }

//    public class ExportModelStateToTempData : ModelStateTempDataTransfer
//    {
//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            //Only export when ModelState is not valid
//            if (!filterContext.Controller.ViewData.ModelState.IsValid)
//            {
//                //Export if we are redirecting
//                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
//                {
//                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
//                }
//            }
//            // Added to pull message from ViewBag
//            if (!string.IsNullOrEmpty(filterContext.Controller.ViewBag.Message))
//            {
//                filterContext.Controller.TempData["Message"] = filterContext.Controller.ViewBag.Message;
//            }

//            base.OnActionExecuted(filterContext);
//        }
//    }

//    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
//    {
//        public override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            ModelStateDictionary modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;

//            if (modelState != null)
//            {
//                //Only Import if we are viewing
//                if (filterContext.Result is ViewResult)
//                {
//                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
//                }
//                else
//                {
//                    //Otherwise remove it.
//                    filterContext.Controller.TempData.Remove(Key);
//                }
//            }
//            // Restore Viewbag message
//            if (!string.IsNullOrEmpty((string)filterContext.Controller.TempData["Message"]))
//            {
//                filterContext.Controller.ViewBag.Message = filterContext.Controller.TempData["Message"];
//            }

//            base.OnActionExecuted(filterContext);
//        }
//    }
//}
