using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using WebApi.Models;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace WebApi.Controllers
{
    [Route("customers")]
    //[Route("api/[controller]")]
    //[ApiController]

    public class CustomerController : Controller
    {
        private readonly WebApiContext _context;

        public CustomerController(WebApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        //“акже, должен быть метод получени€ пользовател€ по идентификатору, а сервер должен отдавать статус-код
        //200 с информацией о пользователе, если он есть, либо
        //404, если пользователь не был найден.
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Customer>> GetCustomer([FromRoute] long id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


        //—ервер, в свою очередь, при добавлении пользовател€ должен провер€ть наличие пользовател€ в Ѕƒ и возвращать коды ответов:
        //200, если пользователь добавлен без ошибок;
        //409, если пользователь с таким Id уже существует в базе.
        [HttpPost("")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                //¬озвращмть стоило бы не просто коды ответов, как сказано в услови€х задачи, а Id созданного пользовател€
                //var result = CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);

                return StatusCode(statusCode: 200);

            }
            catch (ArgumentException ex)
            {
                return StatusCode(409);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        //[HttpPost("")]
        //public async Task<ActionResult<Customer>> CreateCustomerAsync([FromBody] Customer customer)
        //{

        //    _context.Customers.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        //}

    }
}