using System.Threading.Tasks;
using CQRSTest.Application.Customers.Commands.CreateCustomer;
using CQRSTest.Application.Customers.Commands.DeleteCustomer;
using CQRSTest.Application.Customers.Commands.UpdateCustomer;
using CQRSTest.Application.Customers.Queries.GetCustomerDetail;
using CQRSTest.Application.Customers.Queries.GetCustomersList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<CustomersListViewModel>> GetAll()
        {
            return Ok(await _mediator.Send(new GetCustomersListQuery()));
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _mediator.Send(new GetCustomerDetailQuery { Id = id }));
        }

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCustomerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]UpdateCustomerCommand command)
        {
            if (command == null || command.Id != id)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(command));
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return Ok(await _mediator.Send(new DeleteCustomerCommand { Id = id }));
        }        
    }
}