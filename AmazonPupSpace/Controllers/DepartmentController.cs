using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;
using AmazonPupSpace.Models.ViewModels;

namespace AmazonPupSpace.Controllers
{
    public class DepartmentController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44351/api/");
        }


        /// <summary>
        /// This method communicate with the  department data api and get the list of departments and show them on the webpage 
        /// </summary>
        /// <returns>
        /// Returns  a view with the list of departments
        /// </returns>
        /// <example>  GET: Department/List => List View (with the list of departments)
        /// </example>

        // GET: Department/List
        [HttpGet]
        public ActionResult List()
        {
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            //Debug.WriteLine("Number of  departments received : ");
            //Debug.WriteLine(departments.Count());
            return View(departments);
        }
        /// <summary>
        /// This method communicate with the FindDepartment method in the department data api , get the infomartion about the particular  department and show it on the webpage 
        /// </summary>
        /// <param name="id">The id of a department whose information  is requested </param>
        /// <returns>
        /// Returns  a view with the information about a particular department
        /// </returns>
        /// <example>  GET: Department/Details/5 => Details View( The details of a requested  department with the department  id of 5)
        /// </example>

        // GET: Department/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            // fetching an information about the particular department
            DetailsDepartment ViewModel = new DetailsDepartment();
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            DepartmentDto selectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            Debug.WriteLine("employee received : ");
            Debug.WriteLine(selectedDepartment.DepartmentName);
            ViewModel.SelectedDepartment = selectedDepartment;

            //showcase information about  employees related to this  department
            url = "EmployeeData/ListEmployeesForDepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> relatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.RelatedEmployees = relatedEmployees;




            return View(ViewModel);
        }

        ///GET: Department/New => New View => (this webpage gives a form with an empty input fields where new department's information can be filled)
       
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// This method communicate with the AddDepartment method in the department data api , pass on the new  department's infomation to this method to create a new  department in the database
        /// </summary>
        /// <param name="department">The  Department object with new  department's information </param>
        /// <returns>
        /// if the infromation is processed successfully redirects to the List View 
        /// else directs to the Error View page 
        /// </returns>
        /// <example>  POST: Department/Create
        /// FORM DATA: Department JASON Object
        /// </example>

        // GET: Department/Create
        [HttpPost]
        //[Authorize]
        public ActionResult Create(Department department)
        {
           

            string url = "DepartmentData/AddDepartment";


            string jsonpayload = jss.Serialize(department);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }
        //---------Change Department of a employee to associate it with the new Department--------------  

        /// <summary>
        /// This method communicates with the AssociateEmployeeToDepartment Method in the Employee data  api  and add the employees into the particular deprtment by changing their current department Id
        /// </summary>
        /// <param name="id">The Department Id </param>
        /// <param name="EmployeeId">The employee Id of an employee who is added into the department</param>
        /// <returns>Directs it  to the Same Edit page for the Same Department wtih the Employee being upadted/Added into the Department </returns>
        /// <example>
        /// POST : Deparment/Associate/5/6=> The employee with the employee id of  6 being added into the department with department id of 5
        /// </example>
        //POST: Department/Associate/{DepartmentId}{EmployeeId}

        [HttpPost]
            //[Authorize]
        public ActionResult Associate(int id, int EmployeeId)
        {
            //GetApplicationCookie();
            Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Department Id" + id);
            //call to FindEmployee Function
            // string URL = "EmployeeData/FindEmployee/" + EmployeeId;
            // HttpResponseMessage Response = client.GetAsync(URL).Result;
            // EmployeeDto SelectedEmployee = Response.Content.ReadAsAsync<EmployeeDto>().Result;

            // SelectedEmployee.PreviousDepartmentId = SelectedEmployee.DepartmentId;


            //call our api to associate employee with department
            string url = "EmployeeData/AssociateEmployeeToDepartment/" + EmployeeId + "/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Edit/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }


        }
        //---------Change Department of a employee to unassociate it  with the  current Department and assign it again with the previous department--------------  

        /// <summary>
        /// This method communicates with the UnAssociateEmployeeToDepartment Method in the Employee data api  and  remove the employees  from the particular deprtment by changing their current department Id to it's stored previous department id 
        /// </summary>
        /// <param name="id">The Department Id </param>
        /// <param name="EmployeeId">The employee Id of an employee who is removed from  the department</param>
        /// <returns>Directs it  to the Same Edit page for the Same Department wtih the Employee being upadted/ removed  from the Department </returns>
        /// <example>
        /// GET : Deparment/UnAssociate/5/6=> The employee with the employee id of  6 being removed from the department with department id of 5
        /// </example>
        //Get: Department/UnAssociate/{id}?EmployeeId={EmployeeId}
        [HttpGet]
        //[Authorize]
        public ActionResult UnAssociate(int id, int EmployeeId)
        {
           // GetApplicationCookie();
            // Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Employee Id" + EmployeeId);
            Debug.WriteLine("Department Id" + id);
            //call our api to unassociate  employee with department
            string url = "EmployeeData/UnAssociateEmployeeToDepartment/" + EmployeeId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Edit/" + id);
            }
            else
            {
                return RedirectToAction("Error");
            }


        }



        /// <summary>
        /// This method communicates with the DepartmentData api ,  fetch the  stored informtaion of a  Department and  directs  it to the edit page  where it can be updated
        /// </summary>
        /// <param name="id"> The id of a department </param>
        /// <returns>
        ///  GET: Department/Edit/5 => dircets it to the Edit View with the previous information of an department showing on the page 
        /// </returns>

        // GET: Department/Edit/5
        //[Authorize]
        public ActionResult Edit(int id)
        {

            UpdateDepartment ViewModel = new UpdateDepartment();
            //the existing department information
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;

            //showcase information about the related employees in the department
            url = "EmployeeData/ListEmployeesForDepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> relatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.RelatedEmployees = relatedEmployees;


            // showcase infomation about the employees not related to the department
            url = "EmployeeData/ListEmployeesNotInDepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<EmployeeDto> UnrelatedEmployees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            ViewModel.UnrelatedEmployees = UnrelatedEmployees;

            return View(ViewModel);
        }

        /// <summary>
        /// This method communicates with UpdateDepartment  method in the DepartmentData api, pass on the updatd information about an  department and update it in the database
        /// </summary>
        /// <param name="id"> The id of a  Department </param>
        ///<param name="department"> The Department object with the updated information </param>
        ///<returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        ///  POST: Department/Update/5   - > update the department with department id of 5 with the updated information by communicating wih the UpdateDepartment method in the DepartmentData Api and redirects it to the List view 
        /// FORM DATA - Department JASON  Object
        /// </example>

        // POST: Department/Udate/5
        [HttpPost]
        //[Authorize]
        public ActionResult Update(int id, Department department)
        {
            //GetApplicationCookie();
            // Debug.WriteLine("Update function accessed");
            string url = "DepartmentData/UpdateDepartment/" + id;
            //Debug.WriteLine("id :" +id);
            //Debug.WriteLine("departmentid :" + department.DepartmentId);
            string jsonpayload = jss.Serialize(department);
            //Debug.WriteLine(jsonpayload);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            // Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Department/Error => Directs to the Error View showing the Error message
        public ActionResult Error()
        {
            return View();
        }

        
        /// <summary>
        /// This method communicates with the DepartmentData Api and redirects to the DeleteConfirm View where it confirms with user  before deleting a department
        /// </summary>
        /// <param name="id">The id of  department which  is requested to be deleted from te database</param>
        /// <returns>
        ///  Directs to DeleteConfrim View  prompting user to confirm the deletion of a department
        /// </returns>
        /// <example>
        /// GET: Department/DeleteConfirm/5 - >   Directs to DeleteConfrim View prompting to confirm the deletion of department with department id of 5
        /// </example>

        // GET: Department/DeleteConfirm/5
       // [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(SelectedDepartment);
        }
        /// <summary>
        /// This method communicates with the DeleteDepartment mehtod in the  Department Data Api and  Deletes the particular  department from the database
        /// </summary>
        /// <param name="id">The id of a Department to be deleted </param>
        /// <returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        /// POST: Department/Delete/5 => deletes a department with Department id 5 by communicating  with the DeleteDepartment method in the Department data api and redirects to the List View  
        /// </example>

        // POST: Department/Delete/5
        [HttpPost]
       // [Authorize]
        public ActionResult Delete(int id)
        {
            //GetApplicationCookie();
            string url = "DepartmentData/DeleteDepartment/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
