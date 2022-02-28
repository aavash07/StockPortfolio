using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortfolioManagementSystem.Models;

namespace StockPortfolio.Controllers
{
    public class StockDetailsController : Controller
    {
        private readonly StockDbContext _context;

        public StockDetailsController(StockDbContext context)
        {
            _context = context;
        }

        // GET: StockDetails
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var stockDbContext = _context.StockDetails.Include(s => s.Stocks);
            return View(await stockDbContext.ToListAsync());
        }

        // GET: StockDetails/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockDetails = await _context.StockDetails
                .Include(s => s.Stocks)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (stockDetails == null)
            {
                return NotFound();
            }

            return View(stockDetails);
        }

        [Authorize]
        // GET: StockDetails/Create
        public IActionResult Create()
        {
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockName");
            return View();
        }

        // POST: StockDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,TransactionType,Quantity,Amount,TransactionDate,StockId")] StockDetails stockDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stockDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockName", stockDetails.StockId);
            return View(stockDetails);
        }

        [Authorize]
        // GET: StockDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockDetails = await _context.StockDetails.FindAsync(id);
            if (stockDetails == null)
            {
                return NotFound();
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockName", stockDetails.StockId);
            return View(stockDetails);
        }

        // POST: StockDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,TransactionType,Quantity,Amount,TransactionDate,StockId")] StockDetails stockDetails)
        {
            if (id != stockDetails.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stockDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StockDetailsExists(stockDetails.TransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StockId"] = new SelectList(_context.Stocks, "StockId", "StockName", stockDetails.StockId);
            return View(stockDetails);
        }

        // GET: StockDetails/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockDetails = await _context.StockDetails
                .Include(s => s.Stocks)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (stockDetails == null)
            {
                return NotFound();
            }

            return View(stockDetails);
        }

        // POST: StockDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stockDetails = await _context.StockDetails.FindAsync(id);
            _context.StockDetails.Remove(stockDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StockDetailsExists(int id)
        {
            return _context.StockDetails.Any(e => e.TransactionId == id);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProfit()
        {
            var dbStockDetails = await _context.StockDetails.ToListAsync();
            var stockProfit = new StockProfitViewModel();
            var boughtUnits = 0;
            var soldUnits = 0;

            foreach (var stockDetails in dbStockDetails)
            {
                if (stockDetails.TransactionType == "buy")
                {
                    stockProfit.TotalInvestment += (stockDetails.Amount * stockDetails.Quantity);
                    boughtUnits += stockDetails.Quantity;
                }
                else
                {
                    stockProfit.SoldAmount += (stockDetails.Amount * stockDetails.Quantity);
                    soldUnits += stockDetails.Quantity;
                }
                stockProfit.TotalUnits += stockDetails.Quantity;
            }

            stockProfit.CurrentAmount = (boughtUnits - soldUnits) * dbStockDetails[dbStockDetails.Count - 1].Amount;
            stockProfit.Profit = stockProfit.SoldAmount - stockProfit.TotalInvestment;

            return View(stockProfit);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProfitSingle()
        {
            var stocks = await _context.Stocks.ToListAsync();
            var singleStockProfit = new List<StockProfitSingleViewModel>();


            foreach (var stock in stocks)
            {
                double investment = 0;
                double sales = 0;
                double currentAmount = 0;
                var totalUnits = 0;
                var boughtUnits = 0;
                var soldUnits = 0;
                double lastAmount = 0;
                var listDbStockDetails = await _context.StockDetails.Where(x => x.StockId == stock.StockId).ToListAsync();
                if (listDbStockDetails != null)
                {
                    foreach (var stockDetails in listDbStockDetails)
                    {
                        if (stockDetails.TransactionType == "buy")
                        {
                            investment += (stockDetails.Amount * stockDetails.Quantity);
                            boughtUnits += stockDetails.Quantity;
                        }
                        else
                        {
                            sales += (stockDetails.Amount * stockDetails.Quantity);
                            soldUnits += stockDetails.Quantity;
                        }
                        totalUnits += stockDetails.Quantity;
                        lastAmount = stockDetails.Amount;
                    }

                    currentAmount = (boughtUnits - soldUnits) * lastAmount;
                    singleStockProfit.Add(new StockProfitSingleViewModel
                    {
                        StockName = stock.StockName,
                        TotalUnits = totalUnits,
                        TotalInvestment = investment,
                        SoldAmount = sales,
                        CurrentAmount = currentAmount,
                        Profit = sales - investment
                    });
                }

            }
            return View(singleStockProfit);
        }
    }
}
