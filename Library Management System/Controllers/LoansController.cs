using AutoMapper;
using Library_Management_System.DTOs;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;

 [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly IBookService _bookService;
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        private readonly ILogger<LoansController> _logger;

        public LoansController(
            ILoanService loanService,
            IBookService bookService,
            IMemberService memberService,
            IMapper mapper,
            ILogger<LoansController> logger)
        {
            _loanService = loanService;
            _bookService = bookService;
            _memberService = memberService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os empréstimos.
        /// </summary>
        /// <returns>Lista de empréstimos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans()
        {
            var loans = await _loanService.GetAllLoansAsync();
            var loansDto = _mapper.Map<IEnumerable<LoanDto>>(loans);
            return Ok(loansDto);
        }

        /// <summary>
        /// Obtém um empréstimo pelo ID.
        /// </summary>
        /// <param name="id">ID do empréstimo.</param>
        /// <returns>Empréstimo correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoan(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                _logger.LogWarning("Empréstimo com ID {Id} não encontrado.", id);
                return NotFound();
            }

            var loanDto = _mapper.Map<LoanDto>(loan);
            return Ok(loanDto);
        }

        /// <summary>
        /// Cria um novo empréstimo.
        /// </summary>
        /// <param name="loanDto">Dados do empréstimo a ser criado.</param>
        /// <returns>Empréstimo criado.</returns>
        [HttpPost]
        public async Task<ActionResult<LoanDto>> CreateLoan([FromBody] LoanDto loanDto)
        {
            // Verificar se o livro existe
            var book = await _bookService.GetBookByIdAsync(loanDto.BookId);
            if (book == null)
            {
                return BadRequest($"Livro com ID {loanDto.BookId} não encontrado.");
            }

            // Verificar se o membro existe
            var member = await _memberService.GetMemberByIdAsync(loanDto.MemberId);
            if (member == null)
            {
                return BadRequest($"Membro com ID {loanDto.MemberId} não encontrado.");
            }

            // Verificar se o livro já está emprestado
            var activeLoans = await _loanService.GetActiveLoansByBookIdAsync(loanDto.BookId);
            if (activeLoans != null && activeLoans.Any())
            {
                return BadRequest("O livro já está emprestado e não pode ser emprestado novamente.");
            }

            var loan = _mapper.Map<Loan>(loanDto);
            await _loanService.CreateLoanAsync(loan);
            var createdLoanDto = _mapper.Map<LoanDto>(loan);

            return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, createdLoanDto);
        }

        /// <summary>
        /// Atualiza um empréstimo existente.
        /// </summary>
        /// <param name="id">ID do empréstimo a ser atualizado.</param>
        /// <param name="loanDto">Dados atualizados do empréstimo.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] LoanDto loanDto)
        {
            if (id != loanDto.Id)
            {
                return BadRequest("ID do empréstimo na URL não corresponde ao ID no corpo.");
            }

            var existingLoan = await _loanService.GetLoanByIdAsync(id);
            if (existingLoan == null)
            {
                return NotFound();
            }

            // Verificar se o livro existe
            var book = await _bookService.GetBookByIdAsync(loanDto.BookId);
            if (book == null)
            {
                return BadRequest($"Livro com ID {loanDto.BookId} não encontrado.");
            }

            // Verificar se o membro existe
            var member = await _memberService.GetMemberByIdAsync(loanDto.MemberId);
            if (member == null)
            {
                return BadRequest($"Membro com ID {loanDto.MemberId} não encontrado.");
            }

            // Se o livro foi alterado, verificar se o novo livro está disponível
            if (existingLoan.BookId != loanDto.BookId)
            {
                var activeLoans = await _loanService.GetActiveLoansByBookIdAsync(loanDto.BookId);
                if (activeLoans != null && activeLoans.Any())
                {
                    return BadRequest("O novo livro já está emprestado e não pode ser emprestado novamente.");
                }
            }

            _mapper.Map(loanDto, existingLoan);
            await _loanService.UpdateLoanAsync(existingLoan);

            return NoContent();
        }

        /// <summary>
        /// Deleta um empréstimo.
        /// </summary>
        /// <param name="id">ID do empréstimo a ser deletado.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            await _loanService.DeleteLoanAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Registra a devolução de um empréstimo.
        /// </summary>
        /// <param name="id">ID do empréstimo a ser devolvido.</param>
        /// <returns>Status da operação.</returns>
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnLoan(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            if (loan.ReturnDate != null)
            {
                return BadRequest("Este empréstimo já foi devolvido.");
            }

            loan.ReturnDate = System.DateTime.UtcNow;
            await _loanService.UpdateLoanAsync(loan);

            return NoContent();
        }
    }
