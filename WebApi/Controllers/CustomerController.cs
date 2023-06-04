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

        //�����, ������ ���� ����� ��������� ������������ �� ��������������, � ������ ������ �������� ������-���
        //200 � ����������� � ������������, ���� �� ����, ����
        //404, ���� ������������ �� ��� ������.
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


        //������, � ���� �������, ��� ���������� ������������ ������ ��������� ������� ������������ � �� � ���������� ���� �������:
        //200, ���� ������������ �������� ��� ������;
        //409, ���� ������������ � ����� Id ��� ���������� � ����.
        [HttpPost("")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                //���������� ������ �� �� ������ ���� �������, ��� ������� � �������� ������, � Id ���������� ������������
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