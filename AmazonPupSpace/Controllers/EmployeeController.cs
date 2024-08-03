using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AmazonPupSpace.Models;
using AmazonPupSpace.Models.ViewModels;

namespace AmazonPupSpace.Controllers
{
    public class EmployeeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static EmployeeController()
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
        /// This method communicate with the employee data api and get the list of employees and show them on the webpage 
        /// </summary>
        /// <returns>
        /// Returns  a view with the list of employees
        /// </returns>
        /// <example>  GET: Employee/List => List View (with the list of employees)
        /// </example>

        [HttpGet]
        public ActionResult List()
        {
            string url = "EmployeeData/ListEmployees";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<EmployeeDto> employees = response.Content.ReadAsAsync<IEnumerable<EmployeeDto>>().Result;
            //Debug.WriteLine("Number of employees received : ");
            //Debug.WriteLine(employees.Count());



            return View(employees);
        }

        /// <summary>
        /// This method communicate with the FindEmployee method in the employee data api , get the infomartion about the particular employee and show it on the webpage 
        /// </summary>
        /// <param name="id">The id of an employee whose information  is requested </param>
        /// <returns>
        /// Returns  a view with the information about a particular employee
        /// </returns>
        /// <example>  GET: Employee/Details/5 => Details View( The details of a requested employee)
        /// </example>



        // GET: /Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            DetailsEmployee viewModel = new DetailsEmployee();

            // Fetch employee data
            string employeeUrl = "employeeData/findemployee/" + id;
            HttpResponseMessage employeeResponse = client.GetAsync(employeeUrl).Result;

            if (employeeResponse.IsSuccessStatusCode)
            {
                viewModel.SelectedEmployee = employeeResponse.Content.ReadAsAsync<EmployeeDto>().Result;
            }
            else
            {
                return HttpNotFound("Employee not found.");
            }

            // Fetch posts related to the employee
            string postsUrl = "postdata/listpostsbyemployee/" + id; // Ensure this endpoint exists
            HttpResponseMessage postsResponse = client.GetAsync(postsUrl).Result;

            if (postsResponse.IsSuccessStatusCode)
            {
                viewModel.RelatedPosts = postsResponse.Content.ReadAsAsync<IEnumerable<PostDto>>().Result;
            }
            else
            {
                viewModel.RelatedPosts = new List<PostDto>();
            }

            // Fetch dogs owned by the employee
            string dogsUrl = "dogdata/listdogsbyemployee/" + id;
            HttpResponseMessage dogsResponse = client.GetAsync(dogsUrl).Result;

            if (dogsResponse.IsSuccessStatusCode)
            {
                viewModel.RelatedDogs = dogsResponse.Content.ReadAsAsync<IEnumerable<DogDto>>().Result;
            }
            else
            {
                viewModel.RelatedDogs = new List<DogDto>();
            }

            return View(viewModel);
        }



        /// <summary>
        /// This method communicate with the employee data api and get the departments options and position options  and  show them a webpage where a new employee can be created
        /// </summary>
        /// <returns>
        /// Returns  a view which prompts to create a new employee
        /// </returns>
        /// <example>  GET: Employee/New => New View => (this webpage gives a form with an empty input fields where new user's information can be filled)
        /// </example>
        public ActionResult New()
        {
            AddEmployee ViewModel = new AddEmployee();
            //information about all departments in the system.
            //GET api/DepartmentData/ListDepartments
            string url = "DepartmentData/ListDepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = departmentOptions;
            return View(ViewModel);
        }

        /// <summary>
        /// This method communicate with the AddEmployee method in the employee data api , pass on the new employer's infomation to this method to create a new employee in the database
        /// </summary>
        /// <param name="employee">The  Employee object with new employee's information </param>
        /// <returns>
        /// if the infromation is processed successfully redirects to the List View 
        /// else directs to the Error View page 
        /// </returns>
        /// <example>  POST: Employee/Create
        /// FORM DATA: Employee JASON Object
        /// </example>
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            string url = "EmployeeData/AddEmployee";


            string jsonpayload = jss.Serialize(employee);

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
        // GET : Employee/Error - > Directs to the Error View showing the Error message
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// This method communicates with the employee data api ,  fetch the  stored informtaion of an employee and  directs  it to the edit page  where it can be updated
        /// </summary>
        /// <param name="id"> The id of an employee </param>
        /// <returns>
        ///  GET: Employee/Edit/5 => Edit View with the previous information of an employee showing on the page 
        /// </returns>

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateEmployee ViewModel = new UpdateEmployee();

            //the existing employee information
            string url = "EmployeeData/FindEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto SelectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            ViewModel.SelectedEmployee = SelectedEmployee;
            SelectedEmployee.PreviousDepartmentId = SelectedEmployee.DepartmentId;
            

            // Departments to choose from when updating this employee
            url = "DepartmentData/ListDepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        /// <summary>
        /// This method communicates with UpdateEmployee  method in the employee data api, pass on the updatd information about an employee and update it in the database
        /// </summary>
        /// <param name="id"> The id of an employee </param>
        ///<param name="employee"> The employee object with the updated information </param>
        ///<returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        ///  POST: Employee/Update/5   - > update an employee with employee id of 5 with the updated information by communicating wih the UpdateEmployee method in the Employee data Api and redirects it to the List view 
        /// FORM DATA - Employee JASON  Object
        /// </example>
        
        // POST: Employee/Update/5
        [HttpPost]
        public ActionResult Update(int id, Employee employee)
        {
            string url = "EmployeeData/UpdateEmployee/" + id;
            //Debug.WriteLine("id :" +id);
            //Debug.WriteLine("employeeid :" + employee.EmployeeId);
            string jsonpayload = jss.Serialize(employee);
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


        /// <summary>
        /// This method communicates with the Employee Data Api and redirects to the DeleteConfirm View where it confirms with user  before deleting an employee
        /// </summary>
        /// <param name="id">The id of en employee who is requested to be deleted from te database</param>
        /// <returns>
        ///  Directs to DeleteConfrim View  prompting user to confirm the deletion of an employee
        /// </returns>
        /// <example>
        /// GET: Employee/Delete/5 - >   Directs to DeleteConfrim View 
        /// </example>

        // GET: Employee/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "EmployeeData/FindEmployee/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            EmployeeDto selectedEmployee = response.Content.ReadAsAsync<EmployeeDto>().Result;
            return View(selectedEmployee);
        }

        /// <summary>
        /// This method communicates with the DeleteEmployee mehtod in the  Employee Data Api and  Deletes the particular  employee from the database
        /// </summary>
        /// <param name="id">The id of an employee to be deleted </param>
        /// <returns>
        ///if the informtion is processed succssfully redircts to the List View 
        ///else it  directs to the Error View
        /// </returns>
        /// <example>
        /// POST: Employee/Delete/5 => deletes an employee with employee id 5 by communicating  with the DeleteEmployee method in the Employee data api and redirects to the List View  
        /// </example>

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "EmployeeData/DeleteEmployee/" + id;
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
